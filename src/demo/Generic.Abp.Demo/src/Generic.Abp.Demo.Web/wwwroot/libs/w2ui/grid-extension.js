function Grid(config) {
    let me = this;
    me.initialConfig = config;
    me.autoLoad = true;
    for (i in config) {
        me[i] = config[i];
    }

    me.policies = config.policies || ['Create', 'Delete', 'Update'];
    ['Create', 'Delete', 'Update'].forEach(p => {
        me[`allow${p}`] = abp.auth.isGranted(`${me.entityName}.${p}`);
    })
    me.el = `#${me.el}`;

    me.localization = window.abp.localization.getResource(me.resourceName);
    me.globalLocalization = window.abp.localization.getResource('ExtResource');
    me.initGrid();
    me.initCreateAndEditModal();
    let extensions = w2ui.extensions;
    if (!extensions) extensions = w2ui.extensions = {};
    extensions[me.entityName] = me;
}

Grid.prototype.render = {
    boolean(record, extra) {
        let value = extra.value,
            cls = value ? 'fa-check-square' : 'fa-square';
        return `<i class="far ${cls}"></i>`;
    },
    editColumn(record, extra) {
        let value = extra.value;
        return `<a class='edit' data-id='${record.recid}' href="javascript:void(0);">${value}</a>`;
    }

}

Grid.prototype.getGridDefaultConfig = function () {
    let me = this;        
    return {
        //dataType: 'HTTP',
        //limit: 25,
        //method: 'GET',
        multiSelect: true,
        show: {
            selectColumn: true,
            toolbar: true,
            footer: true,
            toolbarSearch: false,
            toolbarColumns: false,
            toolbarInput: false,
            toolbarAdd: me.allowCreate,
            //toolbarEdit: true,
            toolbarDelete: me.allowDelete,
            //skipRecords: false,
            lineNumbers: true,
        },
        onRequest: me.onRequest.bind(me),
        parser: me.onParser.bind(me),
        onSelect: me.onSelect.bind(me),
        onAdd: me.onAdd.bind(me),
        onEdit: me.onEdit.bind(me),
        delete: me.onDelete.bind(me),
        onChange: me.onChange.bind(me),
        onColumnClick: me.onColumnClick.bind(me),
        onRelaod: me.onReload.bind(me),
        onLoad: me.onReload.bind(me),
        onDestroy: me.onDestroy.bind(me),
        onToolbar: me.onToolbar.bind(me)
    };

}

Grid.prototype.getColumns = function () {
    let me = this,
        columns = [];
    me.columns.forEach(c => {
        if (c.isMessage) me.messageField = c.field;
        if (c.isAction) me.hasActionColumn = true;
        let config = Object.assign({}, c),
            text = config.text || config.field;
        if (c.isEdit && me.allowUpdate) {
            config.render = me.render.editColumn;
            me.hasEditColumn = true;
        };
        config.text = me.getColumnTitle(text);
        config.tooltip = config.text;
        columns.push(config);
    })
    return columns;
}

Grid.prototype.getColumnTitle = function(text){
    text = _.upperFirst(text);
    let me = this,
        source = abp.localization.resources[me.resourceName],
        displayName = `DisplayName:${text}`,
        title = source.texts[displayName];
     if (title) {
            return title;
     }

     title = source.texts[text];
     return title || text;
}

Grid.prototype.initGrid = function () {
    let me = this,
        config = me.getGridDefaultConfig(),
        defaultShowConfig = config.show;
    config.name = (me.entityName || 'grid'). replace('.', '_');
    if (me.url) {
        config.url = me.url;
        config.dataType = 'HTTP';
        config.limit = 25;
        config.method = 'GET';
        config.autoLoad = me.autoLoad;
    }
    if(me.header){
        config.header = me.localization(me.header);
        defaultShowConfig.header = true;
    }
    if (me.toolbar) {
        config.toolbar = Object.assign({}, me.toolbar);
    }
    config.show = Object.assign(defaultShowConfig, me.show);
    config.columns = me.getColumns();
    if (me.multiSelect) {
        config.multiSelect = me.multiSelect;
    }
    config.box = me.el;
    if (me.advanceOnEdit) {
        config.advanceOnEdit = true;
    }
    me.el = $(me.el);
    me.grid = new w2grid(config);
    if (me.hasEditColumn) {
        me.el.on('click','a.edit', me.onEdit.bind(me));
    }

    if (me.hasActionColumn) {
        me.el.on('click','span.action',  me.onActionClick.bind(me));
    }

}

Grid.prototype.initCreateAndEditModal = function () {
    let me = this,
        modal = me.modal;
    if (!modal) return;
    if (modal.create) {
        me.createModal = new ModalManager(
            window.abp.appPath + me.modal.create
        );
        me.createModal.onResult(me.updateSuccess.bind(me, 'SaveSuccess'));
    }

    if (modal.edit) {
        me.editModal = new ModalManager(
            window.abp.appPath + me.modal.edit
        );

        me.editModal.onResult(me.updateSuccess.bind(me, 'SaveSuccess'));
    }


}

Grid.prototype.onRequest = function (event) {
    let postData = event.detail.postData;
    postData.skipCount = postData.offset;
    postData.MaxResultCount = postData.limit;
}

Grid.prototype.onParser = function (data) {
    data.total = data.totalCount;
    data.records = data.items;
    return data;
}

Grid.prototype.onAdd = function (event) {
    if (this.createModal) this.createModal.open();
}

Grid.prototype.onEdit = function (event) {
    let me = this,
        recId = event.srcElement.getAttribute('data-id'),
        record = me.grid.get(recId);
    if (!record || !me.editModal) return;
    me.editModal.open({
        id: record.id
    });

}

Grid.prototype.onDelete = function (event) {
    let me = this,
        l = me.globalLocalization,
        grid = me.grid,
        data = grid.getSelection(),
        title = l('DeleteConfirmMessageTitle'),
        message = [],
        ids = [];
    data.forEach(m => {
        let rec = grid.get(m);
        if (!rec) return;
        message.push(`${rec[me.messageField]}`);
        ids.push(rec.id);
    })
    window.abp.message.confirm(l('DeleteConfirmMessage').replace('{0}',message.join(',')), title, function (confirm) {
        if (!confirm) retrun;

        let api = me.api;
        if (api) {
            api.delete(ids).then(me.deleteSuccess.bind(me), (error)=>console.log('grid', error));
        }
    //    fetch(config.url,{ 
    //        method: 'DELETE',
    //        body: JSON.stringify(ids),
    //        headers: { 
    //            "Content-type": "application/json;charset=utf-8",
    //            RequestVerificationToken: abp.security.antiForgery.getToken()
    //            }
    //    })
    //    .then(response => response.json())
    //    .then(me.deleteSuccess.bind(me))
    //    .catch(me.ajaxFailure.bind(me));
    });

}

Grid.prototype.onChange = function (event) {
    let me = this,
        api = me.api,
        detail = event.detail,
        grid = me.grid,
        column = grid.columns[detail.column],
        record = grid.get(detail.recid),
        action = column.action;

    if (!action) return;
    if (typeof action === 'string') {
        let fn = api[action];
        if (_.isFunction(fn)) fn.call(me, record.id, detail.value.new).then(me.updateSuccess.bind(me, 'UpdateSuccess'), () => { });
        return
    }
    if (typeof action === 'object' && action.check) {
        me.updateCheckChange(record, column);
        return;
    }

}


Grid.prototype.updateCheckChange = function (record, column) {
    let me = this,
        id = record.id,
        apiName = uncapitalize(record[column.field] ? column.action.check : column.action.uncheck),
        fn = me.api[apiName];
    if (!isFunction(fn)) return;
    fn.call(null, id).then(me.mergeChanges.bind(me), me.rejectChanges.bind(me));
}

Grid.prototype.updateSuccess = function (message) {
    abp.notify.success(this.globalLocalization(message));
    this.grid.reload();
}

Grid.prototype.deleteSuccess = function () {
    abp.notify.success(this.globalLocalization('DeleteSuccess'));
    this.grid.reload();
}

Grid.prototype.mergeChanges = function () {
    this.grid.mergeChanges();
}

Grid.prototype.rejectChanges = function (error) {
    if (error) this.ajaxFailure(error);
    this.grid.rejectChanges();
}

Grid.prototype.ajaxFailure = function (error) {
    if (error.code) {
        return abp.message.error(this.localization(error.code));
    }
    abp.message.error(error.message);
}

Grid.prototype.onReload = function () { }

Grid.prototype.onSelect = function (event) { 
    let me = this;
    event.complete.then(me.onSelectComplete.bind(me));
}

Grid.prototype.onSelectComplete = function (event) {
    let me = this,
        grid = me.grid,
        extensions = w2ui.extensions,
        panels = me.detailPanels || [],
        selection = grid.getSelection(),
        rec;
    if (selection.length === 1) { 
        rec = grid.get(selection[0]);
    }
    panels.forEach(p => {
        let panel = extensions[p];
        panel && panel.refresh(rec);
    })
}

Grid.prototype.onRefresh = function () { }
Grid.prototype.refresh = function (record) {
    let me = this;
    me.currentRecord = record;
    if (!record) {
        return me.clear(false);
    }
    me.onRefresh();
}

Grid.prototype.onColumnClick = function () { }

Grid.prototype.onActionClick = function () { }

Grid.prototype.onToolbar = function () { }


Grid.prototype.onDestroy=  function() {
    let me = this;
    me.initConfig = null;
    me.el = null;
    me.localization = null;
    me.globalLocalization = null;
    me.policies = null;
    me.columns = null;
    me.show = null;
    me.currentRecord = null;
    me.data = null;
    me.toolbar = null;
}

Grid.prototype.clear = function (isRefresh) {
    let me = this;
    if (me.grid) me.grid.clear();
    me.data = null;
    if (isRefresh) me.onRefresh();
}

