function ApiResourceScopeGrid(config){
    config = config || {
        el: '#layout_scopesLayout_panel_main div.w2ui-panel-content',
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
                action: {  },
                editable: { type: 'checkbox', style: 'text-align: center' } 
            },
    ]
    };
    Grid.call(this, config);
}

inherits(ApiResourceScopeGrid, Grid);



