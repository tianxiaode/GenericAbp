function ApiScope(){
    let me = this;
    me.service = window.generic.abp.identityServer.apiScopes.apiScope;
    me.apiResourceService = window.generic.abp.identityServer.apiResources.apiResource;
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
            onChange: me.onScopeChange.bind(me),
            onReload: me.onReoloadClick.bind(me)
        });
    },

    onReoloadClick(){
        let me = this;
        if(me.grid) me.grid.clear();
        me.scopes = null;
        me.onRefresh();
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
            ln = selections.length,
            currentRecord = me.current;
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

            me.service
                .delete(ids)
                .then(function () {
                    grid.clear();
                    me.scopes = null;
                    me.current = null;
                    me.onRefresh();
                    me.onRefreshClaim();
                },
                me.updateFailure.bind(me))
        });

    },

    onScopeChange(event){
        let me = this,
            grid = me.grid,
            column = grid.columns[event.column],
            record = grid.get(event.recid);
        if(column.field === 'enabled'){
            me.onUpdateValue(record[column.field] ? 'disbale': 'enable', record.id);
        }

        if(column.field === 'showInDiscoveryDocument'){
            me.onUpdateValue(record[column.field] ? 'hide': 'show', record.id);
        }

        if(column.field === 'emphasize'){
            me.onUpdateValue(record[column.field] ? 'understate': 'emphasize', record.id);
        }

        if(column.field === 'required'){
            me.onUpdateValue(record[column.field] ? 'optional': 'required', record.id);
        }

        if(column.field === 'owned'){
            let apiResource = me.apiResource,
                method = record[column.field] ? 'removeScope' :'addScope',
                fn = me.apiResourceService[method];
            if(!fn) return;
            fn.apply(me, [apiResource.id, { name: record.name }])
                .then( me.updateSuccess.bind(me) , me.updateFailure.bind(me));
        }
    },

    onUpdateValue(method, id){
        let me= this,
            fn = me.service[method];
        if(!fn) return;
        fn.apply(me, [id])
            .then( me.updateSuccess.bind(me) , me.updateFailure.bind(me));
    },

    updateSuccess(){
        this.grid.mergeChanges();
    },

    updateFailure(error){
        if(error.code){
            abp.message.error(this.localization(error.code));
        }
        this.grid.rejectChanges();
    },

    
    initClaimGrid(){
        let me = this;
        me.claimGrid =  new Claims('#layout_scopesLayout_panel_bottom div.w2ui-panel-content', 'scopeClaims', me.service);
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
            me.grid.clear();
            me.scopes = null;
            me.onRefresh();
        });
    
        me.editModal.onResult(function () {
            me.grid.clear();
            me.scopes = null;
            me.onRefresh();
        });
    
    },

    onRefreshClaim(){
        let me = this;
        me.claimGrid.refresh(me.current);
        me.onRefreshClaimTitle();

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

    getScopes(){
        window.generic.abp.identityServer.apiScopes.apiScope.getList()
        .then((d)=>{
            this.scopes = d.items;
            this.onRefresh();
        })

    },

    onRefresh(){
        let me = this,
            recs = {},
            data = me.data,
            scopes = me.scopes
            ln = 0;

        if(!scopes){
            me.getScopes();
            return;        
        }
        if(!data) return;
        scopes.forEach((c, index)=>{
            recs[c.name] = { 
                id: c.id,
                recid: index, 
                name: c.name,
                displayName: c.displayName,
                description: c.description,
                enabled: c.enabled,
                required: c.required,
                emphasize: c.emphasize,
                showInDiscoveryDocument: c.showInDiscoveryDocument,
                owned: false
            };
            ln++;
        });
        data.forEach(c=>{
            let exits = recs[c.scope];
            if(exits) {
                exits.owned = true;
            };
            
        })
        me.grid.add(Object.values(recs));

    },


    refresh(apiResource,isRefreshScope){
        let me = this;
        me.apiResource = apiResource;
        me.claimGrid.refresh();
        me.onRefreshClaimTitle();
        if(isRefreshScope) me.scopes = null;
        if(apiResource && apiResource.id){
            me.apiResourceService.getScopes(apiResource.id).then((ret)=>{
                me.data = ret.items;
                me.grid.clear();
                me.onRefresh();
            })
        }else{
            me.current = null;
            me.grid.clear();
        }
    }

}