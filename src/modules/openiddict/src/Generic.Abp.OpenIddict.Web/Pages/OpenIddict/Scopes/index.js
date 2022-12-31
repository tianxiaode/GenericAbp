(function ($) {

    var l = window.abp.localization.getResource('OpenIddict');

    new Grid({
        el: '#grid',
        modal: {
            create: 'OpenIddict/Scopes/CreateModal',
            edit: 'OpenIddict/Scopes/EditModal'
        },
        url: '/api/scopes',
        api: window.generic.abp.openIddict.scopes,
        name: 'Scopes',
        columns: [
            { field: 'name', text: "Scope:Name", size: '100px', isMessage: true },
            { field: 'displayName', text: "Scope:DisplayName", size: '100px', isMessage: true },
            { field: 'description', text: "Scope:Description", size: '100px' },
            { field: 'properties', text: "Scope:Properties", size: '50%' },
            { field: 'resources', text: "Scope:Resources", size: '50%' },
        ]

    });



})(jQuery);
