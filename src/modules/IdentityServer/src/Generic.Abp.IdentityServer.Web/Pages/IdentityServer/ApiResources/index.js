(function ($) {
    var l = window.abp.localization.getResource('AbpIdentityServer');

    var apiResourceAppService = window.generic.abp.identityServer.apiResources.apiResource;
    var editModal = new window.abp.ModalManager(
        window.abp.appPath + 'IdentityServer/ApiResources/EditModal'
    );
    var createModal = new window.abp.ModalManager(
        window.abp.appPath + 'IdentityServer/ApiResources/CreateModal'
    );
    var boolValueRender = function(data) {
        let cls = data ? "fa-check-square" : "fa-square";
        return `<i class="far ${cls}"></i>`;

    }

    var getRowData = function (target, dataTable) {
        var row = dataTable.row($(target).closest('tr'));
        return row.data();
    }


    $(function() {

        var $apiResourcesWrapper = $("#apiResourcesWrapper");
        var allowEdit = window.abp.auth.isGranted('IdentityServer.ApiResources.Update') && `<button class="btn btn-info  btn-sm Edit ml-1 mr-1"><i class="fa fa-edit"></i></button>`;
        var allowDelete = window.abp.auth.isGranted('IdentityServer.ApiResources.Delete') && `<button class="btn btn-danger btn-sm Delete ml-1 mr-1"><i class="fa fa-trash"></i></button>`;
        var dropdown = `<div class="dropdown more">
            <a class="btn btn-primary dropdown-toggle btn-sm" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">...</a>
            <ul class="dropdown-menu" aria-labelledby="dropdownMenuLink">
                <li><a class="dropdown-item claims" href="#">${l("Claims")}</a></li>
                <li><a class="dropdown-item scopes" href="#">${l("Scopes")}</a></li>
                <li><a class="dropdown-item secrets" href="#">${l("Secrets")}</a></li>
            </ul>
        </div>`;
        var $table = $apiResourcesWrapper.find("table");
        var $currentEntity = null;
        var dataTable = $table.DataTable({
            order: [[0, "asc"]],
            processing: true,
            serverSide: true,
            searching: false,
            scrollX: true,
            paging: true,
            ajax: window.abp.libs.datatables.createAjax(apiResourceAppService.getList),
            columnDefs: [
                {
                    data: 'name',
                    targets: 0
                },
                {
                    data: 'displayName',
                    targets: 1
                },
                {
                    data: "description",
                    targets: 2
                },
                {
                    data: "enabled",
                    width: 80,
                    render: boolValueRender,
                    className: "text-center",
                    targets: 3
                },
                {
                    data: "allowedAccessTokenSigningAlgorithms", 
                    targets: 4
                },
                {
                    data: "showInDiscoveryDocument",
                    width: 80,
                    render: boolValueRender,
                    className: "text-center", 
                    targets: 5
                },
                {
                    data: null,
                    orderable: false,
                    targets: 6,
                    className: "text-center",
                    defaultContent: `
                        <div class="d-flex">
                            <div class="flex-fill text-center">${allowEdit}</div>
                            <div class="flex-fill text-center">${allowDelete}</div>
                            <div class="flex-fill text-center">${dropdown}</div>
                        </div>
                    `,
                    width: 140
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


        createModal.onResult(function() {
            dataTable.ajax.reload();
        });

        editModal.onResult(function() {
            dataTable.ajax.reload();
        });

        $apiResourcesWrapper.find('button[name=CreateApiResource]').click(function(e) {
            e.preventDefault();
            createModal.open();
        });

        $apiResourcesWrapper.delegate('button.Edit', 'click', (e) => {
            e.preventDefault();
            var data = getRowData(e.target,dataTable);
            if (!data) return;
            editModal.open({
                id: data.id
            });
        });

        $apiResourcesWrapper.delegate('button.Delete', 'click', (e) => {
            e.preventDefault();
            let data = getRowData(e.target,dataTable);
            if (!data) return;
            let title = window.abp.utils.formatString(l('DeleteConfirmTitle'), l('ApiResource'));
            let message = data.name;
            window.abp.message.confirm(message, title, function (confirm) {
                if (confirm) {
                    apiResourceAppService
                        .delete([data.id])
                        .then(function () {
                            dataTable.ajax.reload();
                        });
                }
            });
        });

        var $claimsWrapper = $("#claimsWrapper");
        var $claimsTable = $claimsWrapper.find('table');
        var allowClaimsDelete = window.abp.auth.isGranted('IdentityServer.ApiResources.Update') && `<button class="btn btn-danger btn-sm Delete ml-1 mr-1"><i class="fa fa-trash"></i></button>`;
        var claimCreateModal = new window.abp.ModalManager(window.abp.appPath + 'IdentityServer/ApiResources/CreateClaimsModal');

        var claimsTable = null;

        function initClaimsTable(url) {
            console.log(claimsTable);
            if (claimsTable) return;
            claimsTable = $claimsTable.DataTable({
                order: [[0, "asc"]],
                processing: true,
                serverSide: true,
                searching: false,
                scrollX: true,
                paging: false,
                ajax:url,
                columnDefs: [
                    {
                        data: 'type',
                        targets: 0
                    },
                    {
                        targets: 1,
                        data: null,
                        orderable: false,
                        defaultContent: `${allowClaimsDelete}`,
                        width: 40
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

        }

        $('button[name=ClaimsReturnButton]').click(e => {
            $currentActivity = null;
            $claimsWrapper.hide();
            $apiResourcesWrapper.show();
        });


        $('button[name=CreateApiResourceCalim]').click(e => {
            claimCreateModal.open({
                id: $currentEntity.id
            });
        });

        claimCreateModal.onResult(function () {
            claimsTable.ajax.reload();
        });



        $claimsWrapper.delegate('.more li', 'click', (e) => {
            e.preventDefault();
            var target = e.target,
                data = getRowData(target,dataTable),
                classname = target.className;
            if (!data) return;
            $currentEntity = data;
            if (classname.includes('claims')) {
                $apiResourcesWrapper.hide();
                $claimsWrapper.show();
                $('#claimsTitle').html(`${l('Claims')}:${data.name}`);
                var url = window.abp.libs.datatables.createAjax(apiResourceAppService.getClaims,
                    () => { return [$currentEntity.id] });
                if (claimsTable) {
                    claimsTable.ajax.url = url;
                        ;
                    claimsTable.ajax.reload();
                } else {
                    initClaimsTable(url);
                }
            }
        });



    });

})(jQuery);
