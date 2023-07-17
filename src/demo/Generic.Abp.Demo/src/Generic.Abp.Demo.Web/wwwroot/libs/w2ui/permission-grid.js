function PermissionGrid(config) {
    let me = this;
    config.show ={
        selectColumn: false,
        toolbar: true,
        toolbarReload:false,
        toolbarAdd: false,
        toolbarDelete: false,
        toolbarSave: true,
        footer: true
    };
    config.multiSelect = false;
    config.entityName = 'Permissions';
    config.resourceName = 'AbpPermissionManagement';
    config.toolbar = {
        items:[
            { 
                type: 'html',
                html: `
                    <div style="padding: 3px 10px;">
                        <input type="checkbox" id="selectAllInput" disabled="true">${abp.localization.resources.ExtResource.texts.SelectAll}
                    </div>
                `
            }
        ]
    }
    me.providerName = 'R';
    Grid.call(this, config);
    $('#selectAllInput').on('change', me.onSelectAllChange.bind(me));
}

window.inherits(PermissionGrid, Grid);

PermissionGrid.prototype.getColumns = function () {
    let l = abp.localization.resources.ExtResource.texts;
    return [
        { field: 'displayName', text: l.Permissions, size: '70%'},
        { 
            field: 'isGranted', text: l.Granted, size: '30%' , style: 'text-align: center',
            editable: { type: 'checkbox', style: 'text-align: center' }
        }
    ];
}

PermissionGrid.prototype.onRefresh = function () {
    let me = this,
        record = me.currentRecord,
        providerName = me.providerName,
        providerKey = providerName === 'R' ? record.name : record.id,
        url = `/api/permission-management/permissions?providerName=${providerName}&providerKey=${providerKey}`;
    abp.ajax({
        url: url,
        type: 'GET'
    }).then(me.onLoadDataSuccess.bind(me))

}

PermissionGrid.prototype.onLoadDataSuccess = function(response){
    let me = this,
        grid = me.grid,
        data = response.responseJson;
        groups = data.groups,
        records = [],
        index = 1;
    groups.forEach(g => {
        let parents = {};
        g.permissions.forEach(p => {
            let parentName = p.parentName;
            index++;
            if (!parentName) {
                parents[p.name] = Object.assign({ w2ui: { children: [] }, recid: index}, p);
            } else {                        
                parents[parentName].w2ui.children.push(Object.assign({ recid: index }, p));
            }
        })

        index++;
        records= [...records, ...Object.values(parents)];
    });
    grid.records = records;
    grid.refresh();
    $('#selectAllInput').toggleAttr('disabled');
    $(grid.toolbar.box).height(52);

}

PermissionGrid.prototype.onChange = function(event){
    let me = this;
    event.complete.then(me.onChangeComplete.bind(me));
}

PermissionGrid.prototype.onChangeComplete = function(event){
    let me = this,
        grid = me.grid,
        detail = event.detail,
        recid = detail.recid,
        value = detail.value.new,
        record = grid.get(recid),
        children = record.w2ui && record.w2ui.children;
    record.isGranted = value;
    if(!children) return;
    children.forEach(c => {
        c.isGranted = value;
    })
    grid.mergeChanges();
    grid.toolbar.enable('w2ui-save')
}

PermissionGrid.prototype.onSelectAllChange = function(event){
    let target = event.target,
        grid = this.grid,
        value = target.checked,
        records = grid.records;
    records.forEach(r => {
        let children = r.w2ui && r.w2ui.children;
        r.isGranted = value;
        if (children) {
            children.forEach(c => {
                c.isGranted = value;
            })
        }
    })

    grid.mergeChanges();
    grid.toolbar.enable('w2ui-save')
}

PermissionGrid.prototype.clear = function (isRefresh) {
    let me = this;
    me.constructor.super_.prototype.clear.call(me, isRefresh);
    $('#selectAllInput').toggleAttr('disabled', true);
}
