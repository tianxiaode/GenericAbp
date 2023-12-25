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
                        { id: 'redirectUri', text: l('RedirectUris'), style: 'font-size:16px;height:36px;'  },
                        { id: 'postLogoutRedirectUri', text: l('Application:PostLogoutRedirectUris'), style: 'font-size:16px;height:36px;'  },
                        { id: 'requirement', text: l('Requirements'), style: 'font-size:16px;height:36px;'  },
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
                    <div id="redirectUri_grid" class="w-100 h-100" style="display:none;"></div>
                    <div id="postLogoutRedirectUri_grid" class="w-100 h-100" style="display:none;"></div>
                    <div id="requirement_grid" class="w-100 h-100" style="display:none;"></div>
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
            { field: 'applicationType', text: "Application:ApplicationType", size: '100px'},
            { field: 'clientType', text: "Application:ClientType", size: '100px'},
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
    });

    new Grid({
        el: 'permissions_grid',
        entityName: 'permissions',
        resourceName: 'OpenIddict',
        resourcePrefix: 'Permission',
        api: generic.abp.openIddict.applications.application,
        show: {
            selectColumn: false,
            toolbar: false
        },
        columns:[
            { field: 'name', text: 'Permissions', size: '60%'},
            { 
                field: 'isGranted', text: 'Granted', size: '50%' , style: 'text-align: center',
                isCheckboxColumn: true,
                render: 'checkboxColumn',
                //editable: { type: 'checkbox', style: 'text-align: center' }
            }

        ],
        onRefresh() {
            let me = this,
                map = me.map;
            if (!map) {
                return abp.ajax({
                    url: '/api/applications/permissions',
                    type: 'GET'
                }).then(me.onLoadMapSuccess.bind(me))
            }
            me.loadData()
        },
        onLoadMapSuccess(response) {
            let me = this,
                map = me.map,                
                data = response.responseJson;
            if(!map) map = me.map = new Map();
            Object.keys(data).forEach(k => {
                let children = data[k];
                Object.keys(children).forEach(c => {
                    let value = children[c];
                    map.set(value, { name: c, group: k, value: value });
                })
            })
            me.loadData();
        },
        loadData() {
            let me = this,
                record = me.currentRecord;
            generic.abp.openIddict.applications.application.getPermissions(record.id).then(me.loadDataSuccess.bind(me));
        },
        loadDataSuccess(response) {
            let me = this,
                grid = me.grid,
                map = me.map,
                data = response.responseJson,
                index = 0;
                parentMap = {};
            map.forEach((value, key) => {
                let parentName = value.group,
                    parent = parentMap[parentName],
                    r;
                if (!parent) {
                    parent = parentMap[parentName] = {  w2ui: { children: [] }, recid: index , name: parentName};
                    index++;
                }
                r = Object.assign({ recid: index }, value);
                r.isGranted = data.includes(r.value);
                index++;
                parent.w2ui.children.push(r);
            })
            grid.records = Object.values(parentMap);
            grid.refresh();
        },
        onCheckboxColumnClick(event) {
            let me = this,
                api = me.api,
                grid = me.grid,
                el = event.srcElement,
                recid = el.getAttribute('data-id'),
                colIndex = el.getAttribute('data-col-index'),
                record = grid.get(recid),
                col = grid.columns[colIndex],
                applicationRecord = me.currentRecord,
                isGranted = record.isGranted,
                params = { value: record.value };
            if (isGranted) {
                api.removePermission(applicationRecord.id, params).then(me.updateSuccess.bind(me, record));
            } else {
                api.addPermission(applicationRecord.id, params).then(me.updateSuccess.bind(me, record));
            }
        },
        updateSuccess(record, response) {
            record.isGranted = !record.isGranted;
            abp.notify.success(this.globalLocalization('UpdateSuccess'));
            this.mergeChanges();
        }

    });
 
    new ItemGrid({
        el: 'redirectUri_grid',
        resourceName: 'OpenIddict',
        entityName: 'RedirectUri',
        apiGetName: 'getRedirectUris',
        grantedPolicie: 'OpenIddict.Applications.Update',
        api: generic.abp.openIddict.applications.application
    });

    new ItemGrid({
        el: 'postLogoutRedirectUri_grid',
        resourceName: 'OpenIddict',
        entityName: 'PostLogoutRedirectUri',
        apiGetName: 'getPostLogoutRedirectUris',
        grantedPolicie: 'OpenIddict.Applications.Update',
        api: generic.abp.openIddict.applications.application
    });

    new ItemGrid({
        el: 'requirement_grid',
        resourceName: 'OpenIddict',
        entityName: 'Requirement',
        apiGetName: 'getRequirements',
        grantedPolicie: 'OpenIddict.Applications.Update',
        api: generic.abp.openIddict.applications.application
    });

    new ItemGrid({
        el: 'property_grid',
        resourceName: 'OpenIddict',
        entityName: 'Property',
        apiGetName: 'getProperties',
        grantedPolicie: 'OpenIddict.Applications.Update',
        api: generic.abp.openIddict.applications.application
    });

})(m4q);
