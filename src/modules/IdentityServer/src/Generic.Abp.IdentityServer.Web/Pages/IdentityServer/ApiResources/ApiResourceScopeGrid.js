function ApiResourceScopeGrid(config){
    config = config || {
        el: '#scopesTab',
        modal: {
            create: 'IdentityServer/ApiScopes/CreateModal',
            edit: 'IdentityServer/ApiScopes/EditModal'
        },
        url: '/api/api-scopes',
        api: window.generic.abp.identityServer.apiScopes.apiScope,
        name: 'ApiResourceScopes',
        columns:[
            { field: 'name', text:"ApiScope:Name", size: '15%', isMessage: true },
            { field: 'displayName', text:"ApiScope:DisplayName", size: '15%'},
            { field: 'description', text:"ApiScope:Description", size: '20%'},
            { 
                field: 'enabled', text:"ApiScope:Enabled", size: '10%', style: 'text-align: center',
                action:{ check: 'disable', uncheck: 'enabled' },
                editable: { type: 'checkbox', style: 'text-align: center' }                         
            },
            { 
                field: 'required', text:"ApiScope:Required", size: '10%', style: 'text-align: center',
                action:{ check: 'optional', uncheck: 'required' },
                editable: { type: 'checkbox', style: 'text-align: center' } 
            },
            { 
                field: 'emphasize', text:"ApiScope:Emphasize", size: '10%', style: 'text-align: center',
                action:{ check: 'understate', uncheck: 'emphasize' },
                editable: { type: 'checkbox', style: 'text-align: center' } 
            },
            { field: 'showInDiscoveryDocument', text:"ApiScope:ShowInDiscoveryDocument", size: '10%', style: 'text-align: center',
                action:{ check: 'hide', uncheck: 'show' },
                editable: { type: 'checkbox', style: 'text-align: center' } 
            },
            { 
                field: 'owned', text:"Owned", size: '10%', 
                action: "OwnedChange",
                editable: { type: 'checkbox', style: 'text-align: center' } 
            },
            {
                size: '80px',
                text: `Details`,
                style: 'text-align: center;',
                isAction: true,
                render(record){ 
                    let text = window.abp.localization.getResource('AbpIdentityServer')('Details');
                    return  `<span style="cursor:pointer;" data-id="${record.recid}" class="action">${text}</span>`;
                },

            }
        ]
    };
    Grid.call(this, config);
    this.apiResourceSevice = window.generic.abp.identityServer.apiResources.apiResource;
}

inherits(ApiResourceScopeGrid, Grid);

ApiResourceScopeGrid.prototype.OwnedChange = function(record , column){
    let me = this,
        name = record[column.field] ? 'remove' : 'add',
        fn = me.apiResourceSevice[`${name}Scope`];
    if(!isFunction(fn)) return;
    fn.call(null, me.currentRecord.id, { name: record.name })
        .then(me.mergeChanges.bind(me), me.rejectChanges.bind(me));
}

ApiResourceScopeGrid.prototype.onReload = function(){
    this.onRefresh();
}

ApiResourceScopeGrid.prototype.onRefresh = function(){
    let me = this,
        data = me.data || [],
        scopes = me.grid.records;

    if(scopes.length === 0){
        setTimeout(me.onRefresh.bind(me), 500);
        return;
    }
    scopes.forEach(c=>{
        let find = data.find(m=>m.scope == c.name); 
        c.owned = !!find;
    });
    me.grid.mergeChanges();

},


ApiResourceScopeGrid.prototype.refresh = function(record){
    let me= this;
    me.currentRecord = record;
    if(record && record.id){
        me.apiResourceSevice.getScopes(record.id).then((ret)=>{
            me.data = ret.items;
            me.onRefresh();
        })
    }else{
        me.onRefresh();
    }

}

ApiResourceScopeGrid.prototype.onActionClick = function(event){
    let me = this,
        l = me.localization,
        target = event.target,
        id = target.getAttribute('data-id'),
        record = me.grid.get(id),
        detail = me.detailModal;
    if(!detail){
        detail = me.detailModal = new ApiResourceScopeDetail();
    }
    detail.refresh(record);
    
}
