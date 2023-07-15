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
        columns: [
            { field: 'name', text: 'RoleName', size: '10%', isMessage: true, isEdit: true, editable: { type: 'text' }  },
            { field: 'isDefault', text: 'IsDefault', size: '60px', sortable: true, resizable: true, style: 'text-align: center',
                action: 'setDefault',
                editable: { type: 'checkbox', style: 'text-align: center' }
            },
            { field: 'isStatic', text: 'IsStatic', size: '60px', sortable: true, resizable: true, style: 'text-align: center',
                editable: { type: 'checkbox', style: 'text-align: center' }
            },
            { field: 'isPublic', text: 'IsPublic', size: '60px', sortable: true, resizable: true, style: 'text-align: center',
                editable: { type: 'checkbox', style: 'text-align: center' }
            }
        ],
        onSelect(event) {
            let grid = event.owner,
                selection = grid.getSelection(),
                ln = selection.length + 1,
                rec = null;
            if(ln === 1) rec = event.detail.clicked.recid;
            console.log(event, selection, ln, rec)
            if(rec) rec = grid.get(rec);
            permissionGrid.refresh(rec);
            multilingualGrid.refresh(rec);
        }
    });

    let permissionGrid = new Grid({
        el: 'permissions_grid',
        entityName: 'Permissions',
        resourceName: 'AbpPermissionManagement',
        //url: '/api/permission-management/permissions',
        autoLoad: false,
        show: {
            toolbar: true,
            toolbarReload:false,
            toolbarAdd: false,
            toolbarDelete: false,
            toolbarSave: true,
            footer: true
        },
        columns: [
            { field: 'displayName', text: 'Permissions', size: '100%', isMessage: true, isEdit: true }
        ],
        onRequest(event) {
            let postData = event.detail.postData;
            postData.providerName = 'R';
            postData.MaxResultCount = 10000;
        },
        onParser(data) {
            let groups = data.groups,
                recs = { records: [] },
                index = 1;
            groups.forEach(g => {
                let parents = {};
                g.permissions.forEach(p => {
                    let parentName = p.parentName;
                    index++;
                    if (!parentName) {
                        parents[p.name] = Object.assign({ w2ui: { children: [] }, recid: index}, p);
                    } else {                        
                        parents[parentName].w2ui.children.push(Object.assign({ recid: index }, p));
                    }
                })

                g.w2ui = { children: Object.values(parents) };
                index++;
                recs.records.push(Object.assign({recid: index}, g));
            })
            return recs;
        }
    })

    let multilingualGrid = new MultilingualGrid({
        el: 'multilingual_grid',
        api: generic.abp.identity.roles.role
    })


//    var _identityRoleAppService = volo.abp.identity.identityRole;
//    var _permissionsModal = new abp.ModalManager(
//        abp.appPath + 'AbpPermissionManagement/PermissionManagementModal'
//    );
//    var _editModal = new abp.ModalManager(
//        abp.appPath + 'Identity/Roles/EditModal'
//    );
//    var _createModal = new abp.ModalManager(
//        abp.appPath + 'Identity/Roles/CreateModal'
//    );

//    var _dataTable = null;

//    abp.ui.extensions.entityActions.get('identity.role').addContributor(
//        function(actionList) {
//            return actionList.addManyTail(
//                [
//                    {
//                        text: l('Edit'),
//                        visible: abp.auth.isGranted(
//                            'AbpIdentity.Roles.Update'
//                        ),
//                        action: function (data) {
//                            _editModal.open({
//                                id: data.record.id,
//                            });
//                        },
//                    },
//                    {
//                        text: l('Permissions'),
//                        visible: abp.auth.isGranted(
//                            'AbpIdentity.Roles.ManagePermissions'
//                        ),
//                        action: function (data) {
//                            _permissionsModal.open({
//                                providerName: 'R',
//                                providerKey: data.record.name,
//                                providerKeyDisplayName: data.record.name
//                            });
//                        },
//                    },
//                    {
//                        text: l('Delete'),
//                        visible: function (data) {
//                            return (
//                                !data.isStatic &&
//                                abp.auth.isGranted(
//                                    'AbpIdentity.Roles.Delete'
//                                )
//                            ); //TODO: Check permission
//                        },
//                        confirmMessage: function (data) {
//                            return l(
//                                'RoleDeletionConfirmationMessage',
//                                data.record.name
//                            );
//                        },
//                        action: function (data) {
//                            _identityRoleAppService
//                                .delete(data.record.id)
//                                .then(function () {
//                                    _dataTable.ajax.reload();
//                                    abp.notify.success(l('SuccessfullyDeleted'));
//                                });
//                        },
//                    }
//                ]
//            );
//        }
//    );

//    abp.ui.extensions.tableColumns.get('identity.role').addContributor(
//        function (columnList) {
//            columnList.addManyTail(
//                [
//                    {
//                        title: l("Actions"),
//                        rowAction: {
//                            items: abp.ui.extensions.entityActions.get('identity.role').actions.toArray()
//                        }
//                    },
//                    {
//                        title: l('RoleName'),
//                        data: 'name',
//                        render: function (data, type, row) {
//                            var name = '<span>' + $.fn.dataTable.render.text().display(data) + '</span>'; //prevent against possible XSS
//                            if (row.isDefault) {
//                                name +=
//                                    '<span class="badge rounded-pill bg-success ms-1">' +
//                                    l('DisplayName:IsDefault') +
//                                    '</span>';
//                            }
//                            if (row.isPublic) {
//                                name +=
//                                    '<span class="badge rounded-pill bg-info ms-1">' +
//                                    l('DisplayName:IsPublic') +
//                                    '</span>';
//                            }
//                            return name;
//                        },
//                    }
//                ]
//            );
//        },
//        0 //adds as the first contributor
//    );
    
//    $(function () {
//        var _$wrapper = $('#IdentityRolesWrapper');
//        var _$table = _$wrapper.find('table');

//        _dataTable = _$table.DataTable(
//            abp.libs.datatables.normalizeConfiguration({
//                order: [[1, 'asc']],
//                searching: false,
//                processing: true,
//                serverSide: true,
//                scrollX: true,
//                paging: true,
//                ajax: abp.libs.datatables.createAjax(
//                    _identityRoleAppService.getList
//                ),
//                columnDefs: abp.ui.extensions.tableColumns.get('identity.role').columns.toArray()
//            })
//        );

//        _createModal.onResult(function () {
//            _dataTable.ajax.reload();
//        });

//        _editModal.onResult(function () {
//            _dataTable.ajax.reload();
//        });

//        $('#AbpContentToolbar button[name=CreateRole]').click(function (e) {
//            e.preventDefault();
//            _createModal.open();
//        });
//    });
})(m4q);
