(function ($) {
    if (abp.localization.currentCulture.cultureName === 'zh-Hans') {
        w2utils.locale('/libs/w2ui/locale/zh-cn.json');
    }

    var claimTypes;

    window.generic.abp.identityServer.claimTypes.claimType.getList().then((d)=>{
        claimTypes = d.items;
    })

    var l = window.abp.localization.getResource('AbpIdentityServer');

    var apiResourceAppService = window.generic.abp.identityServer.apiResources.apiResource;
    var editModal = new window.abp.ModalManager(
        window.abp.appPath + 'IdentityServer/ApiResources/EditModal'
    );
    var createModal = new window.abp.ModalManager(
        window.abp.appPath + 'IdentityServer/ApiResources/CreateModal'
    );

    var currentRecord;

    var boolValueRender = function(v){
        let cls = v ? 'fa-check-square' : 'fa-square';
        return `<i class="far ${cls}"></i>`;
    };

    let layout = $("#layout").w2layout({
        name: 'layout',
        panels: [
            { type: 'main', title: l("ApiResources") },
            {
                type: 'right', size: '45%', resizable: true,
                title: l("Detail"),
                content:`
                <ul class="nav nav-tabs" id="tabList" role="tablist">
                    <li class="nav-item" role="presentation">
                        <button class="nav-link active" data-bs-toggle="tab" data-bs-target="#detailTab" type="button" role="tab" aria-controls="detailTab" aria-selected="true">${l("Detail")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#claimsTab" type="button" role="tab" aria-controls="claimsTab" aria-selected="false">${l("Claims")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#scopesTab" type="button" role="tab" aria-controls="scopesTab" aria-selected="false">${l("Scopes")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#secretsTab" type="button" role="tab" aria-controls="secretsTab" aria-selected="false" >${l("Secrets")}</button>
                    </li>
              </ul>
              <div class="tab-content" style="height: calc(100% - 41px);">
                <div class="tab-pane fade h-100 show active" id="detailTab" role="tabpanel" aria-labelledby="detailTab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="claimsTab" role="tabpanel" aria-labelledby="profile-tab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="scopesTab" role="tabpanel" aria-labelledby="contact-tab" tabindex="0">
                </div>
                <div class="tab-pane fade h-100" id="secretsTab" role="tabpanel" aria-labelledby="disabled-tab" tabindex="0"></div>
              </div>
              `
            },
        ]
    });


    layout.html('main', $().w2grid({
        dataType:'HTTP',
        name: 'apiResources',
        url: '/api/api-resources',
        limit: 25,
        header: true,
        toolbar: true,
        method: 'GET', // need this to avoid 412 error on Safari
        multiSelect: true,
        show: {
            selectColumn: true,
            toolbar: true,
            footer: true,
            toolbarSearch: false,
            toolbarColumns: false,
            toolbarInput: false,
            toolbarAdd: { text: null},   // indicates if toolbar add new button is visible
            toolbarEdit: true,   // indicates if toolbar edit button is visible
            toolbarDelete: true,   // indicates if toolbar delete button is visible
            skipRecords: false,    // indicates if skip records should be visible,
            lineNumbers: true,
         },
        columns: [
            { field: 'name', text: l("ApiResource:Name"), size: '25%', tooltip: l("ApiResource:Name") },
            { field: 'displayName', text: l("ApiResource:DisplayName"), size: '25%' , tooltip: l("ApiResource:DisplayName") },
            { field: 'description', text: l("ApiResource:Description"), size: '30%' , tooltip: l("ApiResource:Description") },
            { field: 'enabled', text: l("ApiResource:Enabled"), size: '10%', style: 'text-align: center',
                editable: { type: 'checkbox', style: 'text-align: center' } , tooltip: l("ApiResource:Enabled") 
            },
            { field: 'showInDiscoveryDocument', text: l("ApiResource:ShowInDiscoveryDocument"), size: '10%', style: 'text-align: center',
                editable: { type: 'checkbox', style: 'text-align: center' } , tooltip: l("ApiResource:ShowInDiscoveryDocument") 
            },
        ],
        onRequest(event) {
            let postData = event.postData;
            postData.skipCount = postData.offset;
            postData.MaxResultCount = postData.limit;
        },
        parser(data) {
            data.total = data.totalCount;
            data.records = data.items;
            return data;
        },
        onSelect(event) {
            let grid = w2ui.apiResources,
                recid = event.recid;
            currentRecord = getCurrentRecord(grid, recid);
            onRefreshDetail();
        },
        onUnselect(event){
            let grid = w2ui.apiResources,
                recid = event.recid,
                selections =  grid.getSelection(),
                ln = selections.length;
            if(ln <= 1){
                currentRecord = null;
                onRefreshDetail();
                return;
            }
            for(let i=ln;i>=1;i--){
                let id = selections[i-1];
                if(id !== recid && currentRecord.recid !== id){
                    currentRecord = getCurrentRecord(grid, id);
                    break;
                }                
            }
            onRefreshDetail();
        },
        onAdd(event) {
            createModal.open();
        },
        onEdit(event) {
            let record = getCurrentRecord(w2ui.apiResources, event.recid);
            if(!record) return;
            editModal.open({
                id: record.id
            });
        },
        delete() {
            let grid = w2ui.apiResources,
                data = grid.getSelection(),
                title = window.abp.utils.formatString(l('DeleteConfirmTitle'), l('ApiResource')),
                message = [],
                ids = [];
            data.forEach(m => {
                let rec = grid.get(m);
                if (!rec) return;
                message.push(`${rec.name}`);
                ids.push(rec.id);
            })
            window.abp.message.confirm(message.join(','), title, function (confirm) {
                if (confirm) {
                    apiResourceAppService
                        .delete(ids)
                        .then(function () {
                            grid.clear();
                            grid.reload();
                            currentRecord = null;
                            onRefreshDetail();
                        });
                }
            });
        },
        onChange(event){
            let grid = this,
                column = grid.columns[event.column],
                record = grid.get(event.recid);
            if(column.field === 'enabled'){
                if(record[column.field]){
                    apiResourceAppService
                        .disable(record.id)
                        .then(function () {
                            grid.mergeChanges();
                            onRefreshDetail();
                        });
                }else{
                    apiResourceAppService
                        .enable(record.id)
                        .then(function () {
                            grid.mergeChanges();
                            onRefreshDetail();
                        });
                }
            }

            if(column.field === 'showInDiscoveryDocument'){
                if(record[column.field]){
                    apiResourceAppService
                        .hide(record.id)
                        .then(function () {
                            grid.mergeChanges();
                            onRefreshDetail();
                        });
                }else{
                    apiResourceAppService
                        .show(record.id)
                        .then(function () {
                            grid.mergeChanges();
                            onRefreshDetail();
                        });
                }
            }

        }
    }));

    $('#detailTab').w2grid({
        name: 'detailGrid',
        show: { header: false, columnHeaders: false },
        columns: [
            { field: 'name', text: 'Name', size: '150px', style: 'background-color: #efefef; border-bottom: 1px solid white; padding-right: 5px;', attr: "align=right" },
            { field: 'value', text: 'Value', size: '100%' }
        ],
        records:[
            { recid: 0, name: l('ApiResource:Name'), value: '' },
            { recid: 1, name: l('ApiResource:DisplayName'), value: '' },
            { recid: 2, name: l('ApiResource:Description'), value: '' },
            { recid: 3, name: l('ApiResource:AllowedAccessTokenSigningAlgorithms'), value: '' },
            { recid: 4, name: l('ApiResource:Enabled'),  value: `<i class="far fa-square"></i>` },
            { recid: 5, name: l('ApiResource:ShowInDiscoveryDocument'), value: `<i class="far fa-square"></i>` }
        ]
    });

    $('#tabList button[data-bs-toggle="tab"]').on('shown.bs.tab', event => {
        onRefreshDetail();
    });

    createModal.onResult(function () {
        w2ui.apiResources.reload();
    });

    editModal.onResult(function () {
        w2ui.apiResources.reload();
    });

    function getCurrentRecord(grid, recid) {
        let record = grid.get(recid);
        return record;
    };

    function onRefreshDetail() {
        let data = $('#tabList button.active').data(),
            active = data.bsTarget;
        if(active.includes('detail')){
            onRefreshDetailPanel();
        }else if(active.includes('claims')){
            onRefreshClaimsPanel();
        }else if(active.includes('scopes')){
            onRefreshScopePanel();
        }
        resetDetialTitle();
    };

    function resetDetialTitle(){
        let title = l("Detail"),
            el = $('#layout_layout_panel_right div.w2ui-panel-title');
        if(currentRecord) title = `${title} - ${currentRecord.name}`;
        el.html(title);
    }

    function onRefreshDetailPanel(){
        let record = currentRecord || {},
            grid = w2ui.detailGrid;
        grid.clear();
        grid.add([
            { recid: 0, name: l('ApiResource:Name'), value: record.name || '' },
            { recid: 1, name: l('ApiResource:DisplayName'), value: record.displayName || '' },
            { recid: 2, name: l('ApiResource:Description'), value: record.description || '' },
            { recid: 3, name: l('ApiResource:AllowedAccessTokenSigningAlgorithms'), value: record.allowedAccessTokenSigningAlgorithms || '' },
            { recid: 4, name: l('ApiResource:Enabled'), value: boolValueRender(record.enabled) },
            { recid: 5, name: l('ApiResource:ShowInDiscoveryDocument'), value: boolValueRender(record.showInDiscoveryDocument)}
        ])
    }

    function onRefreshClaimsPanel(){
        let record = currentRecord || {},
            grid = w2ui.claimsGrid;
        if(!grid) {            
            grid = $('#claimsTab').w2grid({
                name: 'claimsGrid',
                columns: [
                    { field: 'name', text: l("Claims"), size: '150px', style: 'background-color: #efefef; border-bottom: 1px solid white; padding-right: 5px;', attr: "align=right" },
                    { field: 'value', text: l("OwnedClaims"), size: '100%', 
                        editable: { type: 'checkbox', style: 'text-align: center' } 
                    },
                ],
                onChange(event){
                    let grid = this,
                        claim = grid.get(event.recid);
                    if(claim.value){
                        apiResourceAppService
                            .removeClaim(currentRecord.id, { type: claim.name } )
                            .then(function () {
                                grid.mergeChanges();
                            });
                    }else{
                        apiResourceAppService
                            .addClaim(currentRecord.id, { type: claim.name })
                            .then(function () {
                                grid.mergeChanges();
                            });
                    }        
        
                }        
            });
        }

        let processing = function(data){
            let recs = [],
                index = 0;
            claimTypes.forEach(c=>{
                let rec = { recid: index, name: c.name, value: data.find(d=>d.type === c.name) };
                index++;
                recs.push(rec);
            });
            grid.add(recs);

        };
        grid.clear();
        if(record.id){
            apiResourceAppService.getClaims(record.id).then((ret)=>{
                let data = ret.items;
                processing(data);
            })
        }else{
            processing([]);
        }
    };

    function onRefreshScopePanel(){
        let record = currentRecord || {},
            grid = w2ui.scopesGrid,
            claimGrid = w2ui.scopeClaimGrid;
        
        if(!grid) {
            $('#scopesTab').w2layout({
                name: 'scopesLayout',
                panels: [
                    { type: 'main', },
                    { type: 'bottom', title: l("Claims"), size: '45%', resizable: true}
                ]
            });

            grid= $('#layout_scopesLayout_panel_main div.w2ui-panel-content').w2grid({
                name: 'scopeGrid',
                toolbar: true,
                multiSelect: true,
                show: {
                    selectColumn: true,
                    toolbar: true,
                    toolbarSearch: false,
                    toolbarColumns: false,
                    toolbarInput: false,
                    toolbarAdd: { text: null},   // indicates if toolbar add new button is visible
                    toolbarEdit: true,   // indicates if toolbar edit button is visible
                    toolbarDelete: true,   // indicates if toolbar delete button is visible
                    skipRecords: false,    // indicates if skip records should be visible,
                    lineNumbers: true,
                 },
                columns: [
                    { field: 'name', text: l("ApiScope:Name"), size: '20%', tooltip: l("ApiScope:Name"), },
                    { field: 'displayName', text: l("ApiScope:DisplayName"), size: '20%',tooltip: l("ApiScope:DisplayName"), },
                    { field: 'description', text: l("ApiScope:Description"), size: '20%', tooltip: l("ApiScope:Description"), },
                    { field: 'enabled', text: l("ApiScope:Enabled"), size: '10%', style: 'text-align: center',
                        tooltip: l("ApiScope:Enabled"),
                        editable: { type: 'checkbox', style: 'text-align: center' }                         
                    },
                    { field: 'required', text: l("ApiScope:Required"), size: '10%', style: 'text-align: center',
                        tooltip: l("ApiScope:Required"),
                        editable: { type: 'checkbox', style: 'text-align: center' } 
                    },
                    { field: 'emphasize', text: l("ApiScope:Emphasize"), size: '10%', style: 'text-align: center',
                        tooltip: l("ApiScope:Emphasize"),
                        editable: { type: 'checkbox', style: 'text-align: center' } 
                    },
                    { field: 'showInDiscoveryDocument', text: l("ApiScope:ShowInDiscoveryDocument"), size: '10%', style: 'text-align: center',
                        tooltip: l("ApiScope:ShowInDiscoveryDocument"),
                        editable: { type: 'checkbox', style: 'text-align: center' } 
                    },
                ],
            });
            claimGrid = $('##layout_scopesLayout_panel_bottom div.w2ui-panel-content').w2grid({
                name: 'scopeClaimsGrid',
                header: true,
                columns: [
                    { field: 'name', text: l("Claims"), size: '150px', style: 'background-color: #efefef; border-bottom: 1px solid white; padding-right: 5px;', attr: "align=right" },
                    { field: 'value', text: l("OwnedClaims"), size: '100%', 
                        editable: { type: 'checkbox', style: 'text-align: center' } 
                    },
                ],
                onChange(event){
                    let grid = this,
                        claim = grid.get(event.recid);
                    if(claim.value){
                        apiResourceAppService
                            .removeClaim(currentRecord.id, { type: claim.name } )
                            .then(function () {
                                grid.mergeChanges();
                            });
                    }else{
                        apiResourceAppService
                            .addClaim(currentRecord.id, { type: claim.name })
                            .then(function () {
                                grid.mergeChanges();
                            });
                    }        
        
                }        
            });
        }

        let processing = function(data){
            let recs = [],
                index = 0;
            claimTypes.forEach(c=>{
                let rec = { recid: index, name: c.name, value: data.find(d=>d.type === c.name) };
                index++;
                recs.push(rec);
            });
            grid.add(recs);

        };
        grid.clear();
        claimGrid.clear();
        if(record.id){
            grid.url = 
        }else{
            processing([]);
        }
        
    }

    


})(jQuery);
