(function ($) {

    var l = window.abp.localization.getResource('OpenIddict');

    let layout = new w2layout({
        box: '#layout',
        name: 'layout',
        panels: [
            { type: 'main',size: '100%',resizable: true, title: l("Applications") },
            {
                type: 'right',
                size: '100%',
                resizable: true,
                hidden: true,
                tabs: {
                    tabs: [
                        { id: 'detail', text: l('Details'), style: 'font-size:16px;height:36px;'  },
                        { id: 'permissions', text: l('Permission'), style: 'font-size:16px;height:36px;'  },
                        { id: 'redirectUris', text: l('RedirectUris'), style: 'font-size:16px;height:36px;'  },
                        { id: 'postLogoutRedirectUris', text: l('Application:PostLogoutRedirectUris'), style: 'font-size:16px;height:36px;'  },
                        { id: 'requirements', text: l('Requirements'), style: 'font-size:16px;height:36px;'  },
                        { id: 'property', text: l('Properties'), style: 'font-size:16px;height:36px;'  }
                    ],
                    onClick(event) {
                        let me = this,
                            record =  w2ui.layout.currentRecord,
                            extensions = w2ui.extensions,
                            panel = extensions[event.target];
                        panel && panel.refresh(record);
                        me.tabs.forEach(t => {
                            if (t.id === event.target) {
                                $(`#${t.id}_grid`).show();
                            } else {
                                $(`#${t.id}_grid`).hide();
                            }
                        })
                    }

                },
                title: `
                    <div class="d-flex flex-row">
                        <a id="backButton" class="px-2" href="javascript:void(0)"><span class="fas fa-arrow-left" st></span></a>
                        <div id="detailTitle"></div>
                        <div class="flex-fill"></div>
                    </div>
                `,
                html: `
                    <div id="detail_grid" class="w-100 h-100" ></div>
                    <div id="permissions_grid" class="w-100 h-100" style="display:none;"></div>
                    <div id="redirectUris_grid" class="w-100 h-100" style="display:none;"></div>
                    <div id="postLogoutRedirectUris_grid" class="w-100 h-100" style="display:none;"></div>
                    <div id="requirements" class="w-100 h-100" style="display:none;"></div>
                    <div id="property_grid" class="w-100 h-100" style="display:none;"></div>
                `
            }
        ]
    });

    $('#backButton').click(() => {
        w2ui.layout.toggle('main');
        w2ui.layout.toggle('right');
    })

    new Grid({
        el: 'layout_layout_panel_main .w2ui-panel-content',
        entityName: 'OpenIddict.Applications',
        api: generic.abp.identity.roles.role,
        resourceName: 'OpenIddict',
        //header: 'Applications',
        modal: {
            create: 'OpenIddict/Applications/CreateModal',
            edit: 'OpenIddict/Applications/EditModal'
        },
        url: '/api/applications',
        api: window.generic.abp.openIddict.applications.application,
        //detailPanels:[ 'detail', 'permissions', 'redirectUris', 'postLogoutRedirectUris', 'requirements', 'property'],
        columns: [
            { field: 'clientId', text: "Application:ClientId", size: '50%', isMessage: true, isEdit: true },
            { field: 'displayName', text: "Application:DisplayName", size: '50%'},
            { field: 'type', text: "Application:Type", size: '100px'},
            { field: 'consentType', text: "Application:ConsentType", size: '100px' },
            {
                size: '80px',
                text: `Details`,
                style: 'text-align: center;',
                isAction: true,
                render(record) {
                    let text = l('Details');
                    return `<a data-id="${record.recid}" class="action" href="javascript:void(0);">${text}</a>`;
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
            $('#detailTitle').html(`${l('Details')} - ${record.clientId}`);
            w2ui.layout_right_tabs.click('detail');
        }
    });

    new PropertyGrid({
        el: 'detail_grid',
        entityName: 'detail',
        resourceName: 'OpenIddict',
        resourcePrefix: 'Application',
        fields:['id', 'clientId', 'displayName', 'clientSecret', 'type', 'consentType', 'clientUri', 'logoUri', 'creatorId', 'creationTime', 'lastModifierId', 'lastModificationTime' ]
    })
 


})(m4q);
