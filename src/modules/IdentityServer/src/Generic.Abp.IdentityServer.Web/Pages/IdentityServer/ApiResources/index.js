(function ($) {

    var l = window.abp.localization.getResource('AbpIdentityServer');

    // var apiResourceAppService = window.generic.abp.identityServer.apiResources.apiResource;
    // var editModal = new window.abp.ModalManager(
    //     window.abp.appPath + 'IdentityServer/ApiResources/EditModal'
    // );
    // var createModal = new window.abp.ModalManager(
    //     window.abp.appPath + 'IdentityServer/ApiResources/CreateModal'
    // );

    // var currentRecord;

    // var isRefreshScope;

    // var boolValueRender = function(v){
    // };

    let layout = $("#layout").w2layout({
        name: 'layout',
        panels: [
            { type: 'main', title: l("ApiResources") },
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
                content:`
                <ul class="nav nav-tabs" id="tabList" role="tablist">
                    <li class="nav-item" role="presentation">
                        <button class="nav-link active" data-bs-toggle="tab" data-bs-target="#detailTab" type="button" role="tab" aria-controls="detailTab" aria-selected="true">${l("Details")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#claimsTab" type="button" role="tab" aria-controls="claimsTab" aria-selected="false">${l("Claims")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#scopesTab" type="button" role="tab" aria-controls="scopesTab" aria-selected="false">${l("Scopes")}</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" data-bs-toggle="tab" data-bs-target="#secretsTab" type="button" role="tab" aria-controls="secretsTab" aria-selected="false" >${l("Secrets")}</button>
                    </li>
              </ul>
              <div class="tab-content" style="height: calc(100% - 41px);">
                <div class="tab-pane fade h-100 show active" id="detailTab" role="tabpanel" aria-labelledby="detailTab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="claimsTab" role="tabpanel" aria-labelledby="profile-tab" tabindex="0"></div>
                <div class="tab-pane fade h-100" id="scopesTab" role="tabpanel" aria-labelledby="contact-tab" tabindex="0">
                </div>
                <div class="tab-pane fade h-100" id="secretsTab" role="tabpanel" aria-labelledby="disabled-tab" tabindex="0"></div>
              </div>
              `,
              onShow(){
                  let record = w2ui.layout.currentRecord;
                  $('#detailTitle').html(`${l('Details')} - ${record.name}`);
                  $('#tabList button.active').removeClass("active");                  
                  $('#tabList button[data-bs-target="#detailTab"]').addClass('active');
                  switchTab();
              }
            },
        ]
    });

    $('#backButton').click(()=>{
        w2ui.layout.toggle('main');
        w2ui.layout.toggle('right');    
    })

    var apiResourceGrid = new ApiResourceGrid({
        el: '#layout_layout_panel_main div.w2ui-panel-content',
        modal: {
            create: 'IdentityServer/ApiResources/CreateModal',
            edit: 'IdentityServer/ApiResources/EditModal'
        },
        url: '/api/api-resources',
        api: window.generic.abp.identityServer.apiResources.apiResource,
        name: 'ApiResource',
        columns:[
            { field: 'name', text: "ApiResource:Name", size: '20%', isMessage: true },
            { field: 'displayName', text: "ApiResource:DisplayName", size: '20%'},
            { field: 'description', text: "ApiResource:Description", size: '20%'  },
            { field: 'allowedAccessTokenSigningAlgorithms', text: "ApiResource:AllowedAccessTokenSigningAlgorithms", size: '20%'  },
            { field: 'enabled', text: "ApiResource:Enabled", size: '10%', style: 'text-align: center',
                editable: { type: 'checkbox', style: 'text-align: center' } 
            },
            { field: 'showInDiscoveryDocument', text: "ApiResource:ShowInDiscoveryDocument", size: '10%', style: 'text-align: center',
                editable: { type: 'checkbox', style: 'text-align: center' }  
            },
            {
                size: '80px',
                text: `Details`,
                style: 'text-align: center;',
                isAction: true,
                render(record){ 
                    let text = window.abp.localization.getResource('AbpIdentityServer')('Details');
                    return  `<span style="cursor:pointer;" data-id="${record.recid}" class="action">${text}</span>`;
                },

            }
        ]
    })

    $('#tabList button[data-bs-toggle="tab"]').on('shown.bs.tab', event => {
        switchTab();
    });

    function switchTab() {
        let record = w2ui.layout.currentRecord,
            data = $('#tabList button.active').data(),
            active = data.bsTarget;
        if(active.includes('detail')){
            let grid = window.detailGrid;
            if(!grid){
                grid = window.detailGrid = new PropertyGrid({
                    name: 'detailGrid',
                    el: '#detailTab',
                    fields: [
                        { text: 'ApiResource:Name', field: 'name' },
                        { text: 'ApiResource:DisplayName', field: 'displayName' },
                        { text: 'ApiResource:Description', field: 'description' },
                        { text: 'ApiResource:AllowedAccessTokenSigningAlgorithms', field: 'allowedAccessTokenSigningAlgorithms' },
                        { text: 'ApiResource:Enabled', field: 'enabled', render: 'boolean' },
                        { text: 'ApiResource:ShowInDiscoveryDocument', field: 'showInDiscoveryDocument', render: 'boolean' },
                    ]
                })
            }
            grid.refresh(record);
            return;
        }
        if(active.includes('claims')){
            let grid = window.apiResourceClaimsGrid;
            if(!grid){
                grid = window.apiResourceClaimsGrid = new Claims('#claimsTab', 'resourceClaims', apiResourceAppService);
            }
            grid.refresh(record);
            return;
        }
        
        if(active.includes('scopes')){
            return;
        }
        
        if(active.includes('secrets')){
            return;
        }
    };


    // layout.html('main', $().w2grid({
    //     dataType:'HTTP',
    //     name: 'apiResources',
    //     url: '/api/api-resources',
    //     limit: 25,
    //     header: true,
    //     toolbar: true,
    //     method: 'GET', // need this to avoid 412 error on Safari
    //     multiSelect: true,
    //     show: {
    //         selectColumn: true,
    //         toolbar: true,
    //         footer: true,
    //         toolbarSearch: false,
    //         toolbarColumns: false,
    //         toolbarInput: false,
    //         toolbarAdd: { text: null},   // indicates if toolbar add new button is visible
    //         toolbarEdit: true,   // indicates if toolbar edit button is visible
    //         toolbarDelete: true,   // indicates if toolbar delete button is visible
    //         skipRecords: false,    // indicates if skip records should be visible,
    //         lineNumbers: true,
    //      },
    //     columns: [
    //         { field: 'name', text: l("ApiResource:Name"), size: '25%', tooltip: l("ApiResource:Name") },
    //         { field: 'displayName', text: l("ApiResource:DisplayName"), size: '25%' , tooltip: l("ApiResource:DisplayName") },
    //         { field: 'description', text: l("ApiResource:Description"), size: '30%' , tooltip: l("ApiResource:Description") },
    //         { field: 'enabled', text: l("ApiResource:Enabled"), size: '10%', style: 'text-align: center',
    //             editable: { type: 'checkbox', style: 'text-align: center' } , tooltip: l("ApiResource:Enabled") 
    //         },
    //         { field: 'showInDiscoveryDocument', text: l("ApiResource:ShowInDiscoveryDocument"), size: '10%', style: 'text-align: center',
    //             editable: { type: 'checkbox', style: 'text-align: center' } , tooltip: l("ApiResource:ShowInDiscoveryDocument") 
    //         },
    //     ],
    //     onRequest(event) {
    //         let postData = event.postData;
    //         postData.skipCount = postData.offset;
    //         postData.MaxResultCount = postData.limit;
    //     },
    //     parser(data) {
    //         data.total = data.totalCount;
    //         data.records = data.items;
    //         return data;
    //     },
    //     onSelect(event) {
    //         let grid = w2ui.apiResources,
    //             recid = event.recid;
    //         currentRecord = getCurrentRecord(grid, recid);
    //         onRefreshDetail();
    //     },
    //     onUnselect(event){
    //         let grid = w2ui.apiResources,
    //             recid = event.recid,
    //             selections =  grid.getSelection(),
    //             ln = selections.length;
    //         if(ln <= 1){
    //             currentRecord = null;
    //             onRefreshDetail();
    //             return;
    //         }
    //         for(let i=ln;i>=1;i--){
    //             let id = selections[i-1];
    //             if(id !== recid && currentRecord.recid !== id){
    //                 currentRecord = getCurrentRecord(grid, id);
    //                 break;
    //             }                
    //         }
    //         onRefreshDetail();
    //     },
    //     onAdd(event) {
    //         createModal.open();
    //     },
    //     onEdit(event) {
    //         let record = getCurrentRecord(w2ui.apiResources, event.recid);
    //         if(!record) return;
    //         editModal.open({
    //             id: record.id
    //         });
    //     },
    //     delete() {
    //         let grid = w2ui.apiResources,
    //             data = grid.getSelection(),
    //             title = window.abp.utils.formatString(l('DeleteConfirmTitle'), l('ApiResource')),
    //             message = [],
    //             ids = [];
    //         data.forEach(m => {
    //             let rec = grid.get(m);
    //             if (!rec) return;
    //             message.push(`${rec.name}`);
    //             ids.push(rec.id);
    //         })
    //         window.abp.message.confirm(message.join(','), title, function (confirm) {
    //             if (confirm) {
    //                 apiResourceAppService
    //                     .delete(ids)
    //                     .then(function () {
    //                         grid.clear();
    //                         grid.reload();
    //                         currentRecord = null;
    //                         onRefreshDetail();
    //                     });
    //             }
    //         });
    //     },
    //     onChange(event){
    //         let grid = this,
    //             column = grid.columns[event.column],
    //             record = grid.get(event.recid);
    //         if(column.field === 'enabled'){
    //             if(record[column.field]){
    //                 apiResourceAppService
    //                     .disable(record.id)
    //                     .then(function () {
    //                         grid.mergeChanges();
    //                         onRefreshDetail();
    //                     });
    //             }else{
    //                 apiResourceAppService
    //                     .enable(record.id)
    //                     .then(function () {
    //                         grid.mergeChanges();
    //                         onRefreshDetail();
    //                     });
    //             }
    //         }

    //         if(column.field === 'showInDiscoveryDocument'){
    //             if(record[column.field]){
    //                 apiResourceAppService
    //                     .hide(record.id)
    //                     .then(function () {
    //                         grid.mergeChanges();
    //                         onRefreshDetail();
    //                     }, function(){console.log("error", arguments)});
    //             }else{
    //                 apiResourceAppService
    //                     .show(record.id)
    //                     .then(function () {
    //                         grid.mergeChanges();
    //                         onRefreshDetail();
    //                     });
    //             }
    //         }

    //     }
    // }));

    // var currentClaim;

    // function onRefreshClaimsPanel(){
    //     let record = currentRecord || {};
        
    //     if(!currentClaim) currentClaim = new Claims('#claimsTab', 'resourceClaims', apiResourceAppService);
    //     currentClaim.refresh(record);
    // };

    // var currentScope;

    // function onRefreshScopePanel(){
    //     let record = currentRecord || {};
        
    //     if(!currentScope) currentScope = new ApiScope();
    //     currentScope.refresh(record, isRefreshScope);
    //     isRefreshScope = false;
       
    // }

    // function onRefreshSecretsPanel(){

    // }


})(jQuery);
