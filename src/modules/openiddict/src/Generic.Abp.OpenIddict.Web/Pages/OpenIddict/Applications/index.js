(function ($) {

    var l = window.abp.localization.getResource('OpenIddict');

    $("#layout").w2layout({
        name: 'layout',
        panels: [
            { type: 'main', title: l("Applications") },
            {
                type: 'right',
                title: l("Details"),
                size: '30%',
            }
        ]
    });

    new Grid({
        el: '#layout_layout_panel_main div.w2ui-panel-content',
        modal: {
            create: 'OpenIddict/Applications/CreateModal',
            edit: 'OpenIddict/Applications/EditModal'
        },
        url: '/api/applications',
        api: window.generic.abp.openIddict.applications.application,
        name: 'Applications',
        columns: [
            { field: 'clientId', text: "Application:ClientId", size: '50%', isMessage: true },
            { field: 'displayName', text: "Application:DisplayName", size: '50%', isMessage: true },
            { field: 'type', text: "Application:Type", size: '100px', isMessage: true },
            { field: 'consentType', text: "Application:ConsentType", size: '100px' },
        ],
        onSelect(event) {
            let grid = w2ui.Applications,
                rec = grid.get(event.recid);
            detailGrid.refresh(rec);
        },
    });

    let detailGrid = new PropertyGrid({
        name: 'details',
        el: '#layout_layout_panel_right div.w2ui-panel-content',
        fields: [
            { text: 'Application:ClientId', field: 'clientId' },
            { text: 'Application:DisplayName', field: 'displayName' },
            { text: 'Application:Type', field: 'type' },
            { text: 'Application:ClientSecret', field: 'clientSecret' },
            { text: 'Application:ConsentType', field: 'consentType' },
            { text: 'Application:Permissions', field: 'permissions' },
            { text: 'Application:ClientUri', field: 'clientUri' },
            { text: 'Application:LogoUri', field: 'logoUri' },
            { text: 'Application:PostLogoutRedirectUris', field: 'postLogoutRedirectUris' },
            { text: 'Application:RedirectUris', field: 'redirectUris' },
            { text: 'Application:Properties', field: 'properties' },
            { text: 'Application:Requirements', field: 'requirements' },
        ]
    });

    detailGrid.refresh();


})(jQuery);
