function ApiScope(){
    let me = this;
    me.service = window.generic.abp.identityServer.apiScopes.apiScope;
    me.localization = window.abp.localization.getResource('AbpIdentityServer');

    me.initLayout();
    me.initGrid();
    me.initCreateAndEditModal();
    me.initClaimGrid();
} 

ApiScope.prototype = {

   initLayout(){
       let me = this,
            l = me.localization;
        $('#scopesTab').w2layout({
            name: 'scopesLayout',
            panels: [
                { type: 'main', },
                { type: 'bottom', title: l("Claims"), size: '45%', resizable: true}
            ]
        });
    },

    initGrid(){
        let me = this,
            l = me.localization,
            grid;
        gird = me.grid = $('#layout_scopesLayout_panel_main div.w2ui-panel-content').w2grid({
            name: 'scopeGrid',
            toolbar: true,
            multiSelect: true,
            url: '/api/api-scopes',
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
                { field: 'name', text: l("ApiScope:Name"), size: '15%', tooltip: l("ApiScope:Name"), },
                { field: 'displayName', text: l("ApiScope:DisplayName"), size: '15%',tooltip: l("ApiScope:DisplayName"), },
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
                { field: 'owned', text: l("Owned"), size: '10%', 
                    editable: { type: 'checkbox', style: 'text-align: center' } 
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
            onSelect: me.onScopeSelect.bind(me),
            onUnselect: me.onScopeUnselect.bind(me),
            onAdd: me.onScopeAdd.bind(me),
            onEdit: me.onScopeEdit.bind(me) ,
            delete: me.onScopeDelete.bind(me),
            onChange: me.onScopeChange.bind(me)
        });
    },

    onScopeSelect(event){
        let me = this;
        me.current = me.grid.get(event.recid);
        me.onRefreshClaim();
    },

    onScopeUnselect(event){
        let me = this,
            grid = me.grid,
            recid = event.recid,
            selections =  grid.getSelection(),
            ln = selections.length;
        if(ln <= 1){
            me.current = null;
            me.onRefreshClaim();
            return;
        }
        for(let i=ln;i>=1;i--){
            let id = selections[i-1];
            if(id !== recid && currentRecord.recid !== id){
                me.current = grid.get(id);
                break;
            }                
        }
        me.onRefreshClaim();

    },

    onScopeAdd(event){
        this.createModal.open();
    },

    onScopeEdit(event){
        let me = this,
            record = me.grid.get(event.recid);
        if(!record) return;
        me.editModal.open({
            id: record.id
        });
    },

    onScopeDelete(event){
        let me = this,
            l = me.localization,
            grid = me.grid,
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
            if (!confirm) retrun ;
            me.apiResource
                .delete(ids)
                .then(function () {
                    grid.clear();
                    grid.reload();
                    me.current = null;
                    me.onRefreshClaim();
                });
        });

    },

    onScopeChange(event){
        let me = this,
            grid = me.grid,
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


    },
    
    initClaimGrid(){
        let me = this,
            l = me.localization;
        me.claimGrid =  $('#layout_scopesLayout_panel_bottom div.w2ui-panel-content').w2grid({
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
                    apiScopeAppService
                        .removeClaim(currentRecord.id, { type: claim.name } )
                        .then(function () {
                            grid.mergeChanges();
                        });
                }else{
                    apiScopeAppService
                        .addClaim(currentRecord.id, { type: claim.name })
                        .then(function () {
                            grid.mergeChanges();
                        });
                }        
    
            }        
        });     
    },

    initCreateAndEditModal(){
        let me = this;
        me.createModal = new window.abp.ModalManager(
            window.abp.appPath + 'IdentityServer/ApiScopes/CreateModal'
        );
        me.editModal = new window.abp.ModalManager(
            window.abp.appPath + 'IdentityServer/ApiScopes/EditModal'
        );
    
        me.createModal.onResult(function () {
            me.grid.reload();
        });
    
        me.editModal.onResult(function () {
            me.grid.reload();
        });
    
    },

    onRefreshClaim(){

    },

    onRefreshClaimTitle(){
        let me = this,
            current = me.current,
            l = this.localization,
            title = l("Claims"),
            el = $('#layout_scopesLayout_panel_bottom div.w2ui-panel-title');
        if(current) title = `${title} - ${current.name}`;
        el.html(title);

    },


    refresh(apiResource){
        let me = this;
        me.apiResource = apiResource;
        console.log(apiResource.id);
        if(apiResource.id){

        }else{
            me.current = null;
            me.grid.clear(true);
            me.claimGrid.clear();
            me.onRefreshClaimTitle();
        }
    }

}