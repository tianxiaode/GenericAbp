(function ($) {
    if (abp.localization.currentCulture.cultureName === 'zh-Hans') {
        w2utils.locale('/libs/w2ui/locale/zh-cn.json');
    }

    var l = window.abp.localization.getResource('AbpIdentityServer');

    var apiResourceAppService = window.generic.abp.identityServer.apiResources.apiResource;
    var editModal = new window.abp.ModalManager(
        window.abp.appPath + 'IdentityServer/ApiResources/EditModal'
    );
    var createModal = new window.abp.ModalManager(
        window.abp.appPath + 'IdentityServer/ApiResources/CreateModal'
    );

    var currentRecord;

    let layout = $("#layout").w2layout({
        name: 'layout',
        panels: [
            { type: 'main', title: l("ApiResources") },
            {
                type: 'right', size: 300, resizable: true,
                title: l("Detail"),
                tabs: {
                    active: 'detail',
                    name: 'tabs',
                    tabs: [
                        { id: 'detail', text: 'Detail' },
                        { id: 'tab2', text: 'Tab 2' },
                        { id: 'tab3', text: 'Tab 3' }
                    ],
                }
            },
        ]
    });


    layout.html('main', $().w2grid({
        dataType:'HTTP',
        name: 'apiResources',
        url: '/api/api-resources',
        limit: 25,
        header: true,
        toolbar: true,
        method: 'GET', // need this to avoid 412 error on Safari
        multiSelect: true,
        show: {
            selectColumn: true,
            toolbar: true,
            footer: true,
            toolbarSearch: false,
            toolbarColumns: false,
            toolbarInput: false,
            toolbarAdd: { text: null},   // indicates if toolbar add new button is visible
            toolbarEdit: true,   // indicates if toolbar edit button is visible
            toolbarDelete: true,   // indicates if toolbar delete button is visible
            skipRecords: false,    // indicates if skip records should be visible,
            lineNumbers: true,
         },
        columns: [
            { field: 'name', text: l("ApiResource:Name"), size: '30%' },
            { field: 'displayName', text: l("ApiResource:DisplayName"), size: '30%' },
            { field: 'description', text: l("ApiResource:Description"), size: '30%' },
            { field: 'enabled', text: l("ApiResource:Enabled"), size: '10%', style: 'text-align: center',
                editable: { type: 'checkbox', style: 'text-align: center' } 
            },
        ],
        onRequest(event) {
            let postData = event.postData;
            postData.skipCount = postData.offset;
            postData.MaxResultCount = postData.limit;
        },
        parser(data) {
            data.total = data.totalCount;
            data.records = data.items;
            return data;
        },
        onSelect(event) {
            let grid = w2ui.apiResources,
                recid = event.recid;
            currentRecord = getCurrentRecord(grid, recid);
            onRefreshDetail();
        },
        onUnselect(event){
            let grid = w2ui.apiResources,
                recid = event.recid,
                selections =  grid.getSelection(),
                ln = selections.length;
            if(ln <= 1){
                onClearDetail();
                return;
            }
            recid = selections[ln-2];
            currentRecord = getCurrentRecord(grid, recid);
            onRefreshDetail();
        },
        onAdd(event) {
            createModal.open();
        },
        onEdit(event) {
            let record = getCurrentRecord(w2ui.apiResources, event.recid);
            if(!record) return;
            editModal.open({
                id: record.id
            });
        },
        delete() {
            let grid = w2ui.apiResources,
                data = grid.getSelection(),
                title = window.abp.utils.formatString(l('DeleteConfirmTitle'), l('ApiResource')),
                message = [],
                ids = [];
            data.forEach(m => {
                let rec = grid.get(m);
                if (!rec) return;
                message.push(`${rec.name}`);
                ids.push(rec.id);
            })
            window.abp.message.confirm(message.join(','), title, function (confirm) {
                if (confirm) {
                    apiResourceAppService
                        .delete(ids)
                        .then(function () {
                            grid.clear();
                            grid.reload();
                            onClearDetail();
                        });
                }
            });
        },
        onChange(event){
            console.log('onChange', event);
        }
    }));

    createModal.onResult(function () {
        w2ui.apiResources.reload();
    });

    editModal.onResult(function () {
        w2ui.apiResources.reload();
    });

    function getCurrentRecord(grid, recid) {
        let record = grid.get(recid);
        return record;
    };

    function onRefreshDetail() {
        let tabs = layout.get('right').tabs,
            active = tabs.get(tabs.active);
        console.log(active);
        resetDetialTitle();
    };

    function onClearDetail() {
        currentRecord = null;
        let tabs = layout.get('right').tabs,
            active = tabs.get(tabs.active);
        resetDetialTitle();
    };

    function resetDetialTitle(){
        let title = l("Detail"),
            el = $('#layout_layout_panel_right > div.w2ui-panel-title');
        if(currentRecord) title = `${title} - ${currentRecord.name}`;
        el.html(title);
    }

    

    $(function() {

        //var $apiResourcesWrapper = $("#apiResourcesWrapper");
        //var allowEdit = window.abp.auth.isGranted('IdentityServer.ApiResources.Update') && `<button class="btn btn-info  btn-sm Edit ml-1 mr-1"><i class="fa fa-edit"></i></button>`;
        //var allowDelete = window.abp.auth.isGranted('IdentityServer.ApiResources.Delete') && `<button class="btn btn-danger btn-sm Delete ml-1 mr-1"><i class="fa fa-trash"></i></button>`;
        //var dropdown = `<div class="dropdown more">
        //    <a class="btn btn-primary dropdown-toggle btn-sm" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">...</a>
        //    <ul class="dropdown-menu" aria-labelledby="dropdownMenuLink">
        //        <li><a class="dropdown-item claims" href="#">${l("Claims")}</a></li>
        //        <li><a class="dropdown-item scopes" href="#">${l("Scopes")}</a></li>
        //        <li><a class="dropdown-item secrets" href="#">${l("Secrets")}</a></li>
        //    </ul>
        //</div>`;
        //var $currentEntity = null;
        //var dataTable =$apiResourcesWrapper.DataTable({
        //    order: [[0, "asc"]],
        //    processing: true,
        //    serverSide: true,
        //    searching: false,
        //    scrollX: true,
        //    paging: true,
        //    ajax: window.abp.libs.datatables.createAjax(apiResourceAppService.getList),
        //    pagingType: "full_numbers",
        //    columnDefs: [
        //        { data: 'name', title: l("ApiResource:Name"), width: 100, targets: 0 },
        //        { data: 'displayName', title: l("ApiResource:DisplayName"), width: 100, targets: 1},
        //        { data: "description", title: l("ApiResource:Description"), width: 100, targets: 2},
        //        { data: "enabled", width: 80, render: boolValueRender, className: "text-center", title: l("ApiResource:Enabled"), width: 60, targets: 3 },
        //        { data: "allowedAccessTokenSigningAlgorithms", title: l("ApiResource:AllowedAccessTokenSigningAlgorithms"), width: 60, targets: 4 },
        //        { data: "showInDiscoveryDocument", width: 60, render: boolValueRender, className: "text-center", title: l("ApiResource:ShowInDiscoveryDocument"), width: 60, targets: 4 },
        //        {
        //            data: null,
        //            orderable: false,
        //            targets: 5 ,
        //            className: "text-center",
        //            defaultContent: `
        //                <div class="d-flex">
        //                    <div class="flex-fill text-center">${allowEdit}</div>
        //                    <div class="flex-fill text-center">${allowDelete}</div>
        //                </div>
        //            `,
        //            width: 140
        //        }
        //    ],
        //    "oLanguage": {//国际语言转化
        //        "oAria": {
        //            "sSortAscending": " - click/return to sort ascending",
        //            "sSortDescending": " - click/return to sort descending"
        //        },
        //        "sLengthMenu": "显示 _MENU_ 记录",
        //        "sZeroRecords": "对不起，查询不到任何相关数据",
        //        "sEmptyTable": "未有相关数据",
        //        "sLoadingRecords": "正在加载数据-请等待...",
        //        "sInfo": "当前显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录。",
        //        "sInfoEmpty": "当前显示0到0条，共0条记录",
        //        "sInfoFiltered": "（数据库中共为 _MAX_ 条记录）",
        //        "sProcessing": "<img src='../resources/user_share/row_details/select2-spinner.gif'/> 正在加载数据...",
        //        "sSearch": "模糊查询：",
        //        "sUrl": "",
        //        //多语言配置文件，可将oLanguage的设置放在一个txt文件中，例：Javascript/datatable/dtCH.txt
        //        "oPaginate": {
        //            "sFirst": `<span class="angle-double-left></span>`,
        //            "sPrevious": " <",
        //            "sNext": " > ",
        //            "sLast": ""
        //        }
        //    }
        //});


    

    //    $apiResourcesWrapper.find('button[name=CreateApiResource]').click(function(e) {
    //        e.preventDefault();
    //        createModal.open();
    //    });

    //    $apiResourcesWrapper.delegate('button.Edit', 'click', (e) => {
    //        e.preventDefault();
    //        var data = getRowData(e.target,dataTable);
    //        if (!data) return;
    //        editModal.open({
    //            id: data.id
    //        });
    //    });

    //    $apiResourcesWrapper.delegate('button.Delete', 'click', (e) => {
    //        e.preventDefault();
    //        let data = getRowData(e.target,dataTable);
    //        if (!data) return;
    //        let title = window.abp.utils.formatString(l('DeleteConfirmTitle'), l('ApiResource'));
    //        let message = data.name;
    //        window.abp.message.confirm(message, title, function (confirm) {
    //            if (confirm) {
    //                apiResourceAppService
    //                    .delete([data.id])
    //                    .then(function () {
    //                        dataTable.ajax.reload();
    //                    });
    //            }
    //        });
    //    });


    //    var details = {
    //        Claims: {
    //            columnDefs: [
    //                {
    //                    data: 'type',
    //                    targets: 0
    //                },
    //                {
    //                    targets: 1,
    //                    data: null,
    //                    orderable: false,
    //                    //defaultContent: `${detail.delete}`,
    //                    width: 40
    //                }
    //                ]
    //        },
    //        Scopes: {
    //            columnDefs: [
    //                {
    //                    data: 'scope',
    //                    targets: 0
    //                },
    //                {
    //                    targets: 1,
    //                    data: null,
    //                    orderable: false,
    //                    //defaultContent: `${detail.delete}`,
    //                    width: 40
    //                }
    //            ]

    //        },
    //        Secrets: {
    //            columnDefs: [
    //                {
    //                    data: 'type',
    //                    targets: 0
    //                },
    //                {
    //                    data: 'value',
    //                    targets: 1
    //                },
    //                {
    //                    data: 'description',
    //                    targets: 2
    //                },
    //                {
    //                    data: 'expiration',
    //                    targets: 3
    //                },
    //                {
    //                    targets: 4,
    //                    data: null,
    //                    orderable: false,
    //                    //defaultContent: `${detail.delete}`,
    //                    width: 40
    //                }
    //            ]
    //        }
    //    };

    //    function initDetail(name, url) {
    //        let removes = {
    //            Claims: apiResourceAppService.removeClaim,
    //            Scopes: apiResourceAppService.removeScope,
    //            Secrets: apiResourceAppService.removeSecrets
    //        };
    //        let detail = details[name];            
    //        let wrapper = detail.wrapper = $(`#${name}Wrapper`);

    //        wrapper.show();
    //        detail.remove = removes[name];
    //        detail.tableDiv = wrapper.find('table');
    //        detail.delete = window.abp.auth.isGranted('IdentityServer.ApiResources.Update') && `<button class="btn btn-danger btn-sm Delete ml-1 mr-1"><i class="fa fa-trash"></i></button>`;
    //        detail.createModal = new window.abp.ModalManager(window.abp.appPath + `IdentityServer/ApiResources/Create${name}Modal`);
            

    //        let columns = detail.columnDefs;
    //        columns[columns.length - 1].defaultContent = detail.delete;

    //        detail.table = detail.tableDiv.DataTable({
    //            order: [[0, "asc"]],
    //            processing: true,
    //            serverSide: true,
    //            searching: false,
    //            scrollX: true,
    //            paging: false,
    //            ajax: url,
    //            columnDefs: columns,
    //            "language": {
    //                "info": "显示 _TOTAL_ 个条目中的 _START_ 到 _END_ 个.",
    //                "infoFiltered": "(从 _MAX_ 总条目中过滤掉)",
    //                "infoEmpty": "显示0个条目中的0到0",
    //                "search": "搜索",
    //                "processing": "处理中...",
    //                "loadingRecords": "加载中...",
    //                "lengthMenu": "显示 _MENU_ 条数据",
    //                "emptyTable": "表中没有数据",
    //                "paginate": {
    //                    "first": "首页",
    //                    "last": "尾页",
    //                    "previous": "上一页",
    //                    "next": "下一页"
    //                }
    //            }
    //        });

    //        wrapper.find(`button[name=returnButton]`).click(e => {
    //            $currentActivity = null;
    //            wrapper.hide();
    //            $apiResourcesWrapper.show();
    //        });


    //        wrapper.find('button[name=createButton]').click(e => {
    //            detail.createModal.open({
    //                id: $currentEntity.id
    //            });
    //        });

    //        detail.createModal.onResult(function () {
    //            detail.table.ajax.reload();
    //        });

    //        wrapper.delegate('button.Delete', 'click', (e) => {
    //            e.preventDefault();
    //            let data = getRowData(e.target, detail.table);
    //            if (!data) return;
    //            let title = window.abp.utils.formatString(l('DeleteConfirmTitle'), l(`${name}`));
    //            let message = data.type || data.scope;

    //            window.abp.message.confirm(message, title, function (confirm) {

    //                if (confirm) {
    //                    detail.remove($currentEntity.id, data)
    //                        .then(function () {
    //                            detail.table.ajax.reload();
    //                        });
    //                }
    //            });
    //        });


    //    }


    //    $apiResourcesWrapper.delegate('.more li', 'click', (e) => {
    //        e.preventDefault();
    //        var target = e.target,
    //            data = getRowData(target,dataTable),
    //            classname = target.className;
    //        if (!data) return;
    //        $currentEntity = data;
    //        var name = null;
    //        var url = null;
    //        if (classname.includes('claims')) {
    //            name = "Claims";
    //            url = window.abp.libs.datatables.createAjax(apiResourceAppService.getClaims,
    //                () => { return [$currentEntity.id] });
    //        }
    //        if (classname.includes('scopes')) {
    //            name = "Scopes";
    //            url = window.abp.libs.datatables.createAjax(apiResourceAppService.getScopes,
    //                () => { return [$currentEntity.id] });
    //        }
    //        if (classname.includes('secrets')) {
    //            name = "Secrets";
    //            url = window.abp.libs.datatables.createAjax(apiResourceAppService.getClientSecrets,
    //                () => { return [$currentEntity.id] });
    //        }

    //        if (name) {
    //            $apiResourcesWrapper.hide();
    //            var detail = details[name];
    //            if (!detail.table) {
    //                detail = initDetail(name, url);
    //                return;
    //            }
    //            var table = detail.table;
    //            table.ajax.url = url;
    //            table.ajax.reload();
    //        }
    //    });



    });

})(jQuery);
