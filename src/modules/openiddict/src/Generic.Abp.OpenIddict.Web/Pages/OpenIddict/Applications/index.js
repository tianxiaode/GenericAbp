(function ($) {

    var l = window.abp.localization.getResource('OpenIddict');

    let layout = $("#layout").w2layout({
        name: 'layout',
        panels: [
            { type: 'main', title: l("Applications") },
            {
                type: 'right',
                size: '100%',
                hidden: true,
                title: `
                    <div class="d-flex flex-row">
                        <div id="backButton" class="px-2"><span id="backButton" style="cursor:pointer;" class="fas fa-arrow-left" st></span></div>
                        <div id="detailTitle"></div>
                        <div class="flex-fill"></div>
                    </div>
                `,
                content: `
                <ul class="nav nav-tabs" id="tabList" role="tablist">
                    <li class="nav-item" role="presentation">
                        <button class="nav-link active" data-bs-toggle="tab" data-bs-target="#detailTab" type="button" role="tab" aria-controls="detailTab" aria-selected="true">${l("Details")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#permissionsTab" type="button" role="tab" aria-controls="permissionsTab" aria-selected="false">${l("Permissions")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#redirectUrisTab" type="button" role="tab" aria-controls="redirectUrisTab" aria-selected="false">${l("RedirectUris")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#postLogoutRedirectUrisTab" type="button" role="tab" aria-controls="postLogoutRedirectUrisTab" aria-selected="false">${l("PostLogoutRedirectUris")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#requirementsTab" type="button" role="tab" aria-controls="requirementsTab" aria-selected="false" >${l("Requirements")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#propertyTab" type="button" role="tab" aria-controls="propertyTab" aria-selected="false" >${l("Properties")}</button>
                    </li>
              </ul>
              <div class="tab-content" style="height: calc(100% - 41px);">
                <div class="tab-pane fade h-100 show active" id="detailTab" role="tabpanel" aria-labelledby="detailTab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="permissionsTab" role="tabpanel" aria-labelledby="permissionsTab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="redirectUrisTab" role="tabpanel" aria-labelledby="redirectUrisTab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="postLogoutRedirectUrisTab" role="tabpanel" aria-labelledby="postLogoutRedirectUrisTab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="requirementsTab" role="tabpanel" aria-labelledby="requirementsTab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="propertyTab" role="tabpanel" aria-labelledby="propertyTab" tabindex="0"></div>
              </div>
              `,
                onShow() {
                    let record = w2ui.layout.currentRecord;
                    $('#detailTitle').html(`${l('Details')} - ${record.clientId}`);
                    switchTab();
                }
            },
        ]
    });

    $('#backButton').click(() => {
        w2ui.layout.toggle('main');
        w2ui.layout.toggle('right');
    })

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
            { field: 'applicationType', text: "Application:ApplicationType", size: '100px'},
            { field: 'clientType', text: "Application:ClientType", size: '100px', isMessage: true },
            { field: 'consentType', text: "Application:ConsentType", size: '100px' },
            {
                size: '80px',
                text: `Details`,
                style: 'text-align: center;',
                isAction: true,
                render(record) {
                    let text = l('Details');
                    return `<span style="cursor:pointer;" data-id="${record.recid}" class="action">${text}</span>`;
                },

            },
        ],
        onActionClick(event) {
            let me = this,
                target = event.target,
                id = target.getAttribute('data-id'),
                record = me.grid.get(id);
            w2ui.layout.currentRecord = record;
            w2ui.layout.toggle('main');
            w2ui.layout.toggle('right');
        }
    });

    $('#tabList button[data-bs-toggle="tab"]').on('shown.bs.tab', event => {
        switchTab();
    });

    function switchTab() {
        let record = w2ui.layout.currentRecord,
            data = $('#tabList button.active').data(),
            api = window.generic.abp.openIddict.applications.application,
            active = data.bsTarget;
        if (active.includes('detail')) {
            let grid = window.detailGrid;
            if (!grid) {
                grid = window.detailGrid = new PropertyGrid({
                    name: 'detailGrid',
                    el: '#detailTab',
                    fields: [
                        { text: 'Application:ClientId', field: 'clientId' },
                        { text: 'Application:DisplayName', field: 'displayName' },
                        { text: 'Application:Type', field: 'type' },
                        { text: 'Application:ClientSecret', field: 'clientSecret' },
                        { text: 'Application:ConsentType', field: 'consentType' },
                        //{ text: 'Application:Permissions', field: 'permissions' },
                        { text: 'Application:ClientUri', field: 'clientUri' },
                        { text: 'Application:LogoUri', field: 'logoUri' },
                        // { text: 'Application:PostLogoutRedirectUris', field: 'postLogoutRedirectUris' },
                        // { text: 'Application:RedirectUris', field: 'redirectUris' },
                        // { text: 'Application:Properties', field: 'properties' },
                        // { text: 'Application:Requirements', field: 'requirements' },
                    ]
                })
            }
            grid.refresh(record);
            return;
        }

        if (active.includes('permissions')) {
            let grid = window.applicationPermissionsGrid;
            if (!grid) {
                grid = window.applicationPermissionsGrid = new PermissionGrid({
                    el: '#permissionsTab',
                    api: api,
                    name: 'applicationPermissionsGrid',
                    apiGetName: 'getPermissions',
                });
            }
            grid.refresh(record);
            return;
        }

        if (active.includes('redirectUris')) {
            let grid = window.applicationRedirectUrisGrid;
            if (!grid) {
                grid = window.applicationRedirectUrisGrid = new ItemGrid({
                    el: '#redirectUrisTab',
                    api: api,
                    name: 'applicationRedirectUrisGrid',
                    apiGetName: 'getRedirectUris',
                    apiDeleteName: 'removeRedirectUri',
                    apiAddName: 'addRedirectUri',
                    columns: [
                        { text: 'RedirectUri', field: 'value', isMessage: true },
                    ]
                });
            }
            grid.refresh(record);
            return;
        }

        if (active.includes('postLogoutRedirectUris')) {
            let grid = window.applicationPostLogoutRedirectUrisGrid;
            if (!grid) {
                grid = window.applicationPostLogoutRedirectUrisGrid = new ItemGrid({
                    el: '#postLogoutRedirectUrisTab',
                    api: api,
                    name: 'applicationPostLogoutRedirectUrisGrid',
                    apiGetName: 'getPostLogoutRedirectUris',
                    apiDeleteName: 'removePostLogoutRedirectUri',
                    apiAddName: 'addPostLogoutRedirectUri',
                    columns: [
                        { text: 'PostLogoutRedirectUri', field: 'value', isMessage: true },
                    ]
                });
            }
            grid.refresh(record);
            return;
        }

        if (active.includes('requirements')) {
            let grid = window.applicationRequirementsGrid;
            if (!grid) {
                grid = window.applicationRequirementsGrid = new ItemGrid({
                    el: '#requirementsTab',
                    api: api,
                    name: 'applicationRequirementsGrid',
                    apiGetName: 'getRequirements',
                    apiDeleteName: 'removeRequirement',
                    apiAddName: 'addRequirement',
                    columns: [
                        { text: 'Requirement', field: 'value', isMessage: true },
                    ]
                });
            }
            grid.refresh(record);

            return;
        }

        if (active.includes('property')) {
            let grid = window.applicationPropertyGrid;
            if (!grid) {
                grid = window.applicationPropertyGrid = new ItemGrid({
                    el: '#propertyTab',
                    api: api,
                    name: 'applicationPropertyGrid',
                    apiGetName: 'getProperties',
                    apiDeleteName: 'removeProperty',
                    apiAddName: 'addProperty',
                    columns: [
                        { text: 'Property', field: 'value', isMessage: true },
                    ]
                });
            }
            grid.refresh(record);

            return;
        }

    };



})(jQuery);
