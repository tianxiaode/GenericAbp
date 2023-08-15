(function ($) {

    var l = window.abp.localization.getResource('OpenIddict');

        let layout = new w2layout({
        box: '#layout',
        name: 'layout',
        panels: [
            { type: 'main',size: '100%',resizable: true, title: l("Scopes") },
            {
                type: 'right',
                size: '100%',
                resizable: true,
                hidden: true,
                tabs: {
                    tabs: [
                        { id: 'detail', text: l('Details'), style: 'font-size:16px;height:36px;'  },
                        { id: 'property', text: l('Scope:Properties'), style: 'font-size:16px;height:36px;'  },
                        { id: 'resource', text: l('Scope:Resources'), style: 'font-size:16px;height:36px;'  }
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
                    <div id="property_grid" class="w-100 h-100" style="display:none;"></div>
                    <div id="resource_grid" class="w-100 h-100" style="display:none;"></div>
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
        entityName: 'OpenIddict.Scopes',
        resourceName: 'OpenIddict',
        modal: {
            create: 'OpenIddict/Scopes/CreateModal',
            edit: 'OpenIddict/Scopes/EditModal'
        },
        url: '/api/scopes',
        api: window.generic.abp.openIddict.scopes.scope,
        columns: [
            { field: 'name', text: "Scope:Name", size: '40%', isMessage: true, isEdit: true },
            { field: 'displayName', text: "Scope:DisplayName", size: '30%' },
            { field: 'description', text: "Scope:Description", size: '30%' },
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
            $('#detailTitle').html(`${l('Details')} - ${record.name}`);
            w2ui.layout_right_tabs.click('detail');
        }
    });

    new PropertyGrid({
        el: 'detail_grid',
        entityName: 'detail',
        resourceName: 'OpenIddict',
        resourcePrefix: 'Scope',
        fields:['id', 'name', 'displayName', 'description', 'creatorId', 'creationTime', 'lastModifierId', 'lastModificationTime' ]
    });


    new ItemGrid({
        el: 'property_grid',
        resourceName: 'OpenIddict',
        entityName: 'Property',
        apiGetName: 'getProperties',
        grantedPolicie: 'OpenIddict.Scopes.Update',
        api: generic.abp.openIddict.scopes.scope
    });

    new ItemGrid({
        el: 'resource_grid',
        resourceName: 'OpenIddict',
        entityName: 'Resource',
        apiGetName: 'getResources',
        grantedPolicie: 'OpenIddict.Scopes.Update',
        api: generic.abp.openIddict.scopes.scope
    });

})(m4q);
