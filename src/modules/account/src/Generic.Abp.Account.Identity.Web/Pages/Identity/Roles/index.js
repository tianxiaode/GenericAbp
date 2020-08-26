(function ($) {

    var l = abp.localization.getResource('AbpIdentity');
    var lDefault = abp.localization.getResource(abp.localization.defaultResourceName);

    var identityRoleAppService = volo.abp.identity.identityRole;
    var permissionsModal = new abp.ModalManager(abp.appPath + 'AbpPermissionManagement/PermissionManagementModal');
    var editModal = new abp.ModalManager(abp.appPath + 'Identity/Roles/EditModal');
    var createModal = new abp.ModalManager(abp.appPath + 'Identity/Roles/CreateModal');
    var allowEdit = abp.auth.isGranted('AbpIdentity.Roles.Update') && `<button class="btn btn-info  btn-sm Edit ml-1 mr-1"><i class="fa fa-edit"></i></button>`;
    var allowDelete = abp.auth.isGranted('AbpIdentity.Roles.Delete') && `<button class="btn btn-danger btn-sm Delete ml-1 mr-1"><i class="fa fa-trash"></i></button>`;
    var permissionsManage = abp.auth.isGranted('AbpIdentity.Roles.ManagePermissions') && `<button class="btn btn-warning btn-sm Permission ml-1 mr-1"><i class="fa fa-key"></i></button>`;
    $(function () {

        var $wrapper = $('#IdentityRolesWrapper');
        var $table = $wrapper.find('table');

        var dataTable = $table.DataTable({
            order: [[0, "asc"]],
            processing: true,
            serverSide: true,
            //select: { style: 'multi', info: false },
            scrollX: true,
            paging: true,
            ajax: abp.libs.datatables.createAjax(identityRoleAppService.getList),
            columnDefs: [
                {
                    data: "name",
                    render: function (data, type, row) {
                        var name = '<span>' + data + '</span>';
                        if (row.isDefault) {
                            name += '<span class="badge badge-pill text-success ml-1">' +
                                l('DisplayName:IsDefault') +
                                '</span>';
                        }
                        if (row.isPublic) {
                            name += '<span class="badge badge-pill text-info ml-1">' +
                                l('DisplayName:IsPublic') +
                                '</span>';
                        }
                        return name;
                    },
                    "targets": 0
                },
                {
                    "targets": 1,
                    "data": null,
                    "orderable": false,
                    "defaultContent": `${allowEdit}${permissionsManage}${allowDelete}`,
                    width: 80
                }
            ],
            "language": {
                "info": "显示 _TOTAL_ 个条目中的 _START_ 到 _END_ 个.",
                "infoFiltered": "(从 _MAX_ 总条目中过滤掉)",
                "infoEmpty": "显示0个条目中的0到0",
                "search": "搜索",
                "processing": "处理中...",
                "loadingRecords": "加载中...",
                "lengthMenu": "显示 _MENU_ 条数据",
                "emptyTable": "表中没有数据",
                "paginate": {
                    "first": "首页",
                    "last": "尾页",
                    "previous": "上一页",
                    "next": "下一页"
                }
            }
        });

        createModal.onResult(function () {
            dataTable.ajax.reload();
        });

        editModal.onResult(function () {
            dataTable.ajax.reload();
        });

        $wrapper.find('button[name=CreateRole]').click(function (e) {
            e.preventDefault();
            createModal.open();
        });

        var getRowData = function (target) {
            var row = dataTable.row($(target).closest('tr'));
            return row.data();
        }

        $wrapper.delegate('button.Edit', 'click', (e) => {
            e.preventDefault();
            var data = getRowData(e.target);
            if (!data) return;
            editModal.open({
                id: data.id
            });
        });

        $wrapper.delegate('button.Delete', 'click', (e) => {
            e.preventDefault();
            var data = getRowData(e.target);
            if (!data) return;
            let title = abp.utils.formatString(lDefault('DeleteConfirmTitle'), l('Roles'));
            let message = data.name;
            abp.message.confirm(message, title, function (confirm) {
                if (confirm) {
                    identityRoleAppService
                        .delete(data.id)
                        .then(function () {
                            dataTable.ajax.reload();
                        });
                }
            });

        });

        $wrapper.delegate('button.Permission', 'click', (e) => {
            e.preventDefault();
            var data = getRowData(e.target);
            if (!data) return;
            permissionsModal.open({
                providerName: 'R',
                providerKey: data.name
            });
        });

    });

})(jQuery);
