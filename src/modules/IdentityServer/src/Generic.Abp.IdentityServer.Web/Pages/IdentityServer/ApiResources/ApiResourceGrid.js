function ApiResourceGrid(config){
    config = config || {
        el: '#layout_layout_panel_main div.w2ui-panel-content',
        modal: {
            create: 'IdentityServer/ApiResources/CreateModal',
            edit: 'IdentityServer/ApiResources/EditModal'
        },
        url: '/api/api-resources',
        api: window.generic.abp.identityServer.apiResources.apiResource,
        name: 'ApiResource',
        columns:[
            { field: 'name', text: "ApiResource:Name", size: '20%', isMessage: true },
            { field: 'displayName', text: "ApiResource:DisplayName", size: '20%'},
            { field: 'description', text: "ApiResource:Description", size: '20%'  },
            { field: 'allowedAccessTokenSigningAlgorithms', text: "ApiResource:AllowedAccessTokenSigningAlgorithms", size: '20%'  },
            { field: 'enabled', text: "ApiResource:Enabled", size: '10%', style: 'text-align: center',
                action:{ check: 'disable', uncheck: 'enable' },
                editable: { type: 'checkbox', style: 'text-align: center' } 
            },
            { field: 'showInDiscoveryDocument', text: "ApiResource:ShowInDiscoveryDocument", size: '10%', style: 'text-align: center',
                action:{ check: 'hide', uncheck: 'show' },
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
}

inherits(ApiResourceGrid, Grid);


ApiResourceGrid.prototype.onActionClick = function(event){
    let me = this,
        target = event.target,
        id = target.getAttribute('data-id'),
        record = me.grid.get(id);
    w2ui.layout.currentRecord = record;
    w2ui.layout.toggle('main');
    w2ui.layout.toggle('right');
}

