function MultilingualGrid(config) {
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
    config.entityName = 'Multilingual';

    Grid.call(this, config);
}

window.inherits(MultilingualGrid, Grid);

MultilingualGrid.prototype.getColumns = function () {
    let l = abp.localization.resources.ExtResource.texts;
    return [
        { 
            field: 'displayName', text: l.Language, size: '50%'
        },
        { 
            field: 'value', text: l.Value, size: '50%' ,
            editable: { type: 'text' }
        }
    ];
}

MultilingualGrid.prototype.onRefresh = function () {
    let me = this,
        languages = {},
        record = me.currentRecord,
        grid = me.grid,
        index = 1;
    abp.localization.languages.forEach(l => {
        languages[l.cultureName] = Object.assign({ value: '', recid: index}, l);
        index++;
    })
    me.api.getTranslation(record.id).then((response) => {
        response.responseJson.items.forEach(i => {
            let l = languages[i.language];
            if (l) {
                l.value = i.name;
            }
        })
        grid.records = Object.values(languages);
        grid.refresh();
    })

}

MultilingualGrid.prototype.onSave = function(event){
    let me = this,
        grid = me.grid,
        current = me.currentRecord,
        changes = grid.getChanges(),
        records = grid.records,
        api = me.api,
        removes = {},
        data = [];
    changes.forEach(r => {
        let recid = r.recid,
            value = r.value;
        if (value) {
            let record = grid.get(recid);
            data.push({
                language: record.cultureName,
                name: value
            })
        } else {
            removes[recid] = true;
        }

    });
    records.forEach(r => {
        let recid = r.recid,
            value = r.value;
        if (value && !removes[recid]) {
            data.push({
                language: r.cultureName,
                name: value
            })
        }
    })
    if(data.length  === 0) return;
    api.updateTranslation(current.id, data).then(me.updateSuccess.bind(me, 'UpdateSuccess', false), () => { });
    return { isCancelled: true };
}

MultilingualGrid.prototype.updateSuccess = function (message, isReload) {
    let me = this,
        grid = me.grid;
    abp.notify.success(me.globalLocalization(message));
    grid.mergeChanges();
}
