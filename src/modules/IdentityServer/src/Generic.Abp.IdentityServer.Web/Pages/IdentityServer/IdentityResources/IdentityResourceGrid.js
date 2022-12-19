function IdentityResourceGrid(config){
    config = config || {
        el: '#layout_layout_panel_main div.w2ui-panel-content',
        modal: {
            create: 'IdentityServer/IdentityResources/CreateModal',
            edit: 'IdentityServer/IdentityResources/EditModal'
        },
        url: '/api/identity-resources',
        api: window.generic.abp.identityServer.identityResources.identityResource,
        name: 'ApiResource',
        columns:[
            { field: 'name', text: "IdentityResource:Name", size: '15%', isMessage: true },
            { field: 'displayName', text: "IdentityResource:DisplayName", size: '15%' },
            { field: 'description', text: "IdentityResource:Description", size: '20%' },
            {
                field: 'enabled', text: "IdentityResource:Enabled", size: '10%', style: 'text-align: center',
                action: { check: 'disable', uncheck: 'enabled' },
                editable: { type: 'checkbox', style: 'text-align: center' }
            },
            {
                field: 'required', text: "IdentityResource:Required", size: '10%', style: 'text-align: center',
                action: { check: 'optional', uncheck: 'required' },
                editable: { type: 'checkbox', style: 'text-align: center' }
            },
            {
                field: 'emphasize', text: "IdentityResource:Emphasize", size: '10%', style: 'text-align: center',
                action: { check: 'understate', uncheck: 'emphasize' },
                editable: { type: 'checkbox', style: 'text-align: center' }
            },
            {
                field: 'showInDiscoveryDocument', text: "IdentityResource:ShowInDiscoveryDocument", size: '10%', style: 'text-align: center',
                action: { check: 'hide', uncheck: 'show' },
                editable: { type: 'checkbox', style: 'text-align: center' }
            },
            {
                size: '80px',
                text: `Details`,
                style: 'text-align: center;',
                isAction: true,
                render(record) {
                    let text = window.abp.localization.getResource('AbpIdentityServer')('Details');
                    return `<span style="cursor:pointer;" data-id="${record.recid}" class="action">${text}</span>`;
                },

            }
        ]
    };
    Grid.call(this, config);
}

inherits(IdentityResourceGrid, Grid);


IdentityResourceGrid.prototype.onActionClick = function(event){
    let me = this,
        target = event.target,
        id = target.getAttribute('data-id'),
        record = me.grid.get(id);
    w2ui.layout.currentRecord = record;
    w2ui.layout.toggle('main');
    w2ui.layout.toggle('right');
}

