(function ($) {
    var l = abp.localization.getResource('AbpIdentity');

    let layout = new w2layout({
        box: '#layout',
        name: 'layout',
        panels: [
            { type: 'main',  },
            { 
                type: 'right', size: 300,
                style: 'border-bottom: 1px solid #efefef;;border-right: 1px solid #efefef;',
                tabs: {
                    active: 'permissions',
                    style: 'border-top: 1px solid #efefef;;border-right: 1px solid #efefef;',
                    tabs: [
                        { id: 'permissions', text: abp.localization.resources.AbpPermissionManagement.texts.Permissions, style: 'font-size:16px;height:36px;'  },
                        { id: 'multilingual', text: abp.localization.resources.ExtResource.texts.Multilingual, style: 'font-size:16px;height:36px;'  },
                    ],
                    onClick(event) {
                        let me = this;
                        $('#grid_multilingual_toolbar').height(52);
                        me.tabs.forEach(t => {
                            if (t.id === event.target) {
                                $(`#${t.id}_grid`).show();
                            } else {
                                $(`#${t.id}_grid`).hide();
                            }
                        })
                    }
                },
                html: `
                    <div id="permissions_grid" class="w-100 h-100" ></div>
                    <div id="multilingual_grid" class="w-100 h-100" style="height:300px;display:none;"></div>
                `
            },
        ]
    })


    let roleGrid = new Grid({
        el: 'layout_layout_panel_main .w2ui-panel-content',
        entityName: 'AbpIdentity.Roles',
        api: generic.abp.identity.roles.role,
        resourceName: 'AbpIdentity',
        header: 'Roles',
        modal: {
            create: 'Identity/Roles/CreateModal',
            edit: 'Identity/Roles/EditModal'
        },
        url: '/api/roles',
        detailPanels:[ 'Permissions', 'Multilingual'],
        columns: [
            { field: 'name', text: 'RoleName', size: '10%',sortable: true, isMessage: true, isEdit: true},
            { field: 'isDefault', text: 'IsDefault', size: '60px', sortable: true,  style: 'text-align: center',
                action: 'setDefault',
                editable: { type: 'checkbox', style: 'text-align: center' }
            },
            { field: 'isStatic', text: 'IsStatic', size: '60px', sortable: true,  style: 'text-align: center',
                editable: { type: 'checkbox', style: 'text-align: center' }
            },
            { field: 'isPublic', text: 'IsPublic', size: '60px', sortable: true, style: 'text-align: center',
                editable: { type: 'checkbox', style: 'text-align: center' }
            }
        ]
    });

    new PermissionGrid({
        el: 'permissions_grid'
    })

    new MultilingualGrid({
        el: 'multilingual_grid',
        api: generic.abp.identity.roles.role
    })


})(m4q);
