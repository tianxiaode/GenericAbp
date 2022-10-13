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


        var details = {
            Claims: {
                columnDefs: [
                    {
                        data: 'type',
                        targets: 0
                    },
                    {
                        targets: 1,
                        data: null,
                        orderable: false,
                        //defaultContent: `${detail.delete}`,
                        width: 40
                    }
                    ]
            },
            Scopes: {
                columnDefs: [
                    {
                        data: 'scope',
                        targets: 0
                    },
                    {
                        targets: 1,
                        data: null,
                        orderable: false,
                        //defaultContent: `${detail.delete}`,
                        width: 40
                    }
                ]

            },
            Secrets: {
                columnDefs: [
                    {
                        data: 'type',
                        targets: 0
                    },
                    {
                        data: 'value',
                        targets: 1
                    },
                    {
                        data: 'description',
                        targets: 2
                    },
                    {
                        data: 'expiration',
                        targets: 3
                    },
                    {
                        targets: 4,
                        data: null,
                        orderable: false,
                        //defaultContent: `${detail.delete}`,
                        width: 40
                    }
                ]
            }
        };

        function initDetail(name, url) {
            let removes = {
                Claims: apiResourceAppService.removeClaim,
                Scopes: apiResourceAppService.removeScope,
                Secrets: apiResourceAppService.removeSecrets
            };
            let detail = details[name];            
            let wrapper = detail.wrapper = $(`#${name}Wrapper`);

            wrapper.show();
            detail.remove = removes[name];
            detail.tableDiv = wrapper.find('table');
            detail.delete = window.abp.auth.isGranted('IdentityServer.ApiResources.Update') && `<button class="btn btn-danger btn-sm Delete ml-1 mr-1"><i class="fa fa-trash"></i></button>`;
            detail.createModal = new window.abp.ModalManager(window.abp.appPath + `IdentityServer/ApiResources/Create${name}Modal`);
            

            let columns = detail.columnDefs;
            columns[columns.length - 1].defaultContent = detail.delete;

            detail.table = detail.tableDiv.DataTable({
                order: [[0, "asc"]],
                processing: true,
                serverSide: true,
                searching: false,
                scrollX: true,
                paging: false,
                ajax: url,
                columnDefs: columns,
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

            wrapper.find(`button[name=returnButton]`).click(e => {
                $currentActivity = null;
                wrapper.hide();
                $apiResourcesWrapper.show();
            });


            wrapper.find('button[name=createButton]').click(e => {
                detail.createModal.open({
                    id: $currentEntity.id
                });
            });

            detail.createModal.onResult(function () {
                detail.table.ajax.reload();
            });

            wrapper.delegate('button.Delete', 'click', (e) => {
                e.preventDefault();
                let data = getRowData(e.target, detail.table);
                if (!data) return;
                let title = window.abp.utils.formatString(l('DeleteConfirmTitle'), l(`${name}`));
                let message = data.type || data.scope;

                window.abp.message.confirm(message, title, function (confirm) {

                    if (confirm) {
                        detail.remove($currentEntity.id, data)
                            .then(function () {
                                detail.table.ajax.reload();
                            });
                    }
                });
            });


        }


        $apiResourcesWrapper.delegate('.more li', 'click', (e) => {
            e.preventDefault();
            var target = e.target,
                data = getRowData(target,dataTable),
                classname = target.className;
            if (!data) return;
            $currentEntity = data;
            var name = null;
            var url = null;
            if (classname.includes('claims')) {
                name = "Claims";
                url = window.abp.libs.datatables.createAjax(apiResourceAppService.getClaims,
                    () => { return [$currentEntity.id] });
            }
            if (classname.includes('scopes')) {
                name = "Scopes";
                url = window.abp.libs.datatables.createAjax(apiResourceAppService.getScopes,
                    () => { return [$currentEntity.id] });
            }
            if (classname.includes('secrets')) {
                name = "Secrets";
                url = window.abp.libs.datatables.createAjax(apiResourceAppService.getClientSecrets,
                    () => { return [$currentEntity.id] });
            }

            if (name) {
                $apiResourcesWrapper.hide();
                var detail = details[name];
                if (!detail.table) {
                    detail = initDetail(name, url);
                    return;
                }
                var table = detail.table;
                table.ajax.url = url;
                table.ajax.reload();
            }
        });



    });

})(jQuery);
