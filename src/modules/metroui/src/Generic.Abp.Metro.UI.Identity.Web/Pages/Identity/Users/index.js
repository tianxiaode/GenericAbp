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
                    active: 'roles',
                    style: 'border-top: 1px solid #efefef;;border-right: 1px solid #efefef;',
                    tabs: [
                        { id: 'roles', text: abp.localization.resources.AbpIdentity.texts.Roles, style: 'font-size:16px;height:36px;'  },
                        { id: 'permissions', text: abp.localization.resources.AbpPermissionManagement.texts.Permissions, style: 'font-size:16px;height:36px;'  },
                    ],
                    onClick(event) {
                        let me = this;
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
                    <div id="roles_grid" class="w-100 h-100" ></div>
                    <div id="permissions_grid" class="w-100 h-100" style="height:300px;display:none;"></div>
                `
            },
        ]
    })

    let userGrid = new Grid({
        el: 'layout_layout_panel_main .w2ui-panel-content',
        entityName: 'AbpIdentity.Users',
        api: generic.abp.identity.users.user,
        resourceName: 'AbpIdentity',
        header: 'Users',
        modal: {
            create: 'Identity/Users/CreateModal',
            edit: 'Identity/Users/EditModal'
        },
        url: '/api/users',
        detailPanels:['Roles', 'Permissions'],
        columns: [
            { field: 'userName', text: 'UserName', size: '10%',sortable: true, isMessage: true, isEdit: true},
            { field: 'name', text: 'Name', size: '10%',sortable: true  },
            { field: 'surname', text: 'Surname', size: '10%',sortable: true   },
            { field: 'email', text: 'Email', size: '10%',sortable: true   },
            { field: 'phoneNumber', text: 'PhoneNumber', size: '10%',sortable: true   },
            { field: 'isActive', text: 'IsActive', size: '60px', sortable: true,  style: 'text-align: center',
                action: 'setActive',
                editable: { type: 'checkbox', style: 'text-align: center' }
            },
            { field: 'lockoutEnabled', text: abp.localization.resources.ExtResource.texts.UserLocked, size: '60px', sortable: true,  style: 'text-align: center', 
                action: 'setLockoutEnabled',
                editable: { type: 'checkbox', style: 'text-align: center' }
            },
            { 
                field: 'lockoutEnd', text: abp.localization.resources.ExtResource.texts.LockoutEnd, size: '10%',sortable: true,
                render: 'datetime'
            }
        ]
    });

    new Grid({
        el: 'roles_grid',
        entityName: 'Roles',
        resourceName: 'AbpIdentity',
        multiSelect: false,
        api:generic.abp.identity.users.user,
        show: {
            selectColumn: false,
            toolbar: false,
            toolbarReload:false,
            toolbarAdd: false,
            toolbarDelete: false,
            toolbarSave: true,
            footer: true
        },
        columns:[
            { field: 'name', text: 'RoleName', size: '50%',sortable: true  },
            { field: 'isSelected', text: 'SelectedOrNot:Selected', size: '50%', sortable: true,  style: 'text-align: center',
                editable: { type: 'checkbox', style: 'text-align: center' }
            }
        ],
        onRefresh() {
            let me = this,
                grid = me.grid,
                record = me.currentRecord,
                url = `/api/users/${record.id}/roles`;
            grid.url = url;
            grid.reload();
        },
        onChangeComplete(event) {
            let me = this,
                grid = me.grid,
                api = me.api,
                detail = event.detail,
                role = grid.get(detail.recid),
                value = detail.value.new,
                record = me.currentRecord;
            if (value) {
                api.addRole(record.id, role.id).then(me.onUpdateSuccess.bind(me));
            } else {
                api.removeRole(record.id, role.id).then(me.onUpdateSuccess.bind(me));
            }
        },
        onUpdateSuccess(response) {
            abp.notify.success(this.globalLocalization('UpdateSuccess'));
            this.grid.mergeChanges();
        }
    });
    

    new PermissionGrid({
        el: 'permissions_grid',
        providerName: 'U'
    })


//    var _identityUserAppService = volo.abp.identity.identityUser;

//    var togglePasswordVisibility = function () {
//        $("#PasswordVisibilityButton").click(function (e) {
//            var button = $(this);
//            var passwordInput = button.parent().find("input");
//            if(!passwordInput) {
//                return;
//            }

//            if(passwordInput.attr("type") === "password") {
//                passwordInput.attr("type", "text");
//            }
//            else {
//                passwordInput.attr("type", "password");
//            }

//            var icon = button.find("i");
//            if(icon) {
//                icon.toggleClass("fa-eye-slash").toggleClass("fa-eye");
//            }
//        });
//    }
    
//    abp.modals.createUser = function () {
//        var initModal = function (publicApi, args) {
//            togglePasswordVisibility();
//        };
//        return { initModal: initModal };
//    }
    
//    abp.modals.editUser = function () {
//        var initModal = function (publicApi, args) {
//            togglePasswordVisibility();
//        };
//        return { initModal: initModal };
//    }
    
//    var _editModal = new abp.ModalManager({
//        viewUrl: abp.appPath + 'Identity/Users/EditModal',
//        modalClass: "editUser"
//    });
//    var _createModal = new abp.ModalManager({
//        viewUrl: abp.appPath + 'Identity/Users/CreateModal',
//        modalClass: "createUser"
//    });
//    var _permissionsModal = new abp.ModalManager(
//        abp.appPath + 'AbpPermissionManagement/PermissionManagementModal'
//    );

//    var _dataTable = null;

//    abp.ui.extensions.entityActions.get('identity.user').addContributor(
//        function(actionList) {
//            return actionList.addManyTail(
//                [
//                    {
//                        text: l('Edit'),
//                        visible: abp.auth.isGranted(
//                            'AbpIdentity.Users.Update'
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
//                            'AbpIdentity.Users.ManagePermissions'
//                        ),
//                        action: function (data) {
//                            _permissionsModal.open({
//                                providerName: 'U',
//                                providerKey: data.record.id,
//                                providerKeyDisplayName: data.record.userName
//                            });
//                        },
//                    },
//                    {
//                        text: l('Delete'),
//                        visible: function(data) {
//                            return abp.auth.isGranted('AbpIdentity.Users.Delete') && abp.currentUser.id !== data.id;
//                        },
//                        confirmMessage: function (data) {
//                            return l(
//                                'UserDeletionConfirmationMessage',
//                                data.record.userName
//                            );
//                        },
//                        action: function (data) {
//                            _identityUserAppService
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

//    abp.ui.extensions.tableColumns.get('identity.user').addContributor(
//        function (columnList) {
//            columnList.addManyTail(
//                [
//                    {
//                        title: l("Actions"),
//                        rowAction: {
//                            items: abp.ui.extensions.entityActions.get('identity.user').actions.toArray()
//                        }
//                    },
//                    {
//                        title: l('UserName'),
//                        data: 'userName',
//                        render: function (data, type, row) {
//                            row.userName = $.fn.dataTable.render.text().display(row.userName);
//                            if (!row.isActive) {
//                                return  '<i data-toggle="tooltip" data-placement="top" title="' +
//                                    l('ThisUserIsNotActiveMessage') +
//                                    '" class="fa fa-ban text-danger"></i> ' +
//                                    '<span class="opc-65">' + row.userName + '</span>';
//                            }

//                            return row.userName;
//                        }
//                    },
//                    {
//                        title: l('EmailAddress'),
//                        data: 'email',
//                    },
//                    {
//                        title: l('PhoneNumber'),
//                        data: 'phoneNumber',
//                    }
//                ]
//            );
//        },
//        0 //adds as the first contributor
//    );

//    $(function () {
//        var _$wrapper = $('#IdentityUsersWrapper');
//        var _$table = _$wrapper.find('table');
//        _dataTable = _$table.DataTable(
//            abp.libs.datatables.normalizeConfiguration({
//                order: [[1, 'asc']],
//                processing: true,
//                serverSide: true,
//                scrollX: true,
//                paging: true,
//                ajax: abp.libs.datatables.createAjax(
//                    _identityUserAppService.getList
//                ),
//                columnDefs: abp.ui.extensions.tableColumns.get('identity.user').columns.toArray()
//            })
//        );

//        _createModal.onResult(function () {
//            _dataTable.ajax.reload();
//        });

//        _editModal.onResult(function () {
//            _dataTable.ajax.reload();
//        });

//        $('#AbpContentToolbar button[name=CreateUser]').click(function (e) {
//            e.preventDefault();
//            _createModal.open();
//        });
//    });
})(m4q);
