function Grid(config) {
    let me = this;
    me.initConfig = config;
    me.modal = config.modal;
    me.el = `#${config.el}`;
    delete config.el;
    me.localization = window.abp.localization.getResource(config.resourceName);
    me.globalLocalization = window.abp.localization.getResource('ExtResource');
    me.initGrid();
    me.initCreateAndEditModal();
}

Grid.prototype.render = {
    boolean(value) {
        let cls = value ? 'fa-check-square' : 'fa-square';
        return `<i class="far ${cls}"></i>`;
    }

}

Grid.prototype.getGridDefaultConfig = function () {
    let me = this;
    return {
        dataType: 'HTTP',
        limit: 25,
        method: 'GET',
        multiSelect: true,
        show: {
            selectColumn: true,
            toolbar: true,
            footer: true,
            toolbarSearch: false,
            toolbarColumns: false,
            toolbarInput: false,
            toolbarAdd: { text: null },
            toolbarEdit: true,
            toolbarDelete: true,
            //skipRecords: false,
            //lineNumbers: true,
        },
        onRequest: me.onRequest.bind(me),
        parser: me.onParser.bind(me),
        onSelect: me.onSelect.bind(me),
        onUnselect: me.onDeselect.bind(me),
        onAdd: me.onAdd.bind(me),
        onEdit: me.onEdit.bind(me),
        delete: me.onDelete.bind(me),
        onChange: me.onChange.bind(me),
        onColumnClick: me.onColumnClick.bind(me),
        onRelaod: me.onReload.bind(me),
        onLoad: me.onReload.bind(me),
        onDestroy: me.onDestroy.bind(me)
    };

}

Grid.prototype.getColumns = function () {
    let me = this,
        l = me.localization,
        columns = [];
    me.initConfig.columns.forEach(c => {
        if (c.isMessage) me.messageField = c.field;
        if (c.isAction) me.hasActionColumn = true;
        let config = Object.assign({}, c),
            text = config.text || config.field;
        if (text) {
            text = _.upperFirst(text);
            config.text = l(text) || l(`DisplayName:${text}`);
            config.tooltip = config.text;
        }
        columns.push(config);
    })
    return columns;
}

Grid.prototype.initGrid = function () {
    let me = this,
        config = me.getGridDefaultConfig(),
        showConfig = config.show;
    config = Object.assign(config, me.initConfig);
    if (!config.name) {
        config.name = 'grid';
    }
    if(config.header){
        if(config.name === 'grid') config.name = config.header;
        config.header = me.localization(config.header);
        showConfig.header = true;
    }
    config.show = showConfig;
    config.columns = me.getColumns();
    config.box = me.el;
    me.el = $(me.el);
    me.grid = new w2grid(config);

    if (me.hasActionColumn) {
        me.el.delegate('span.action', 'click', me.onActionClick.bind(me));
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
        me.createModal.onResult(me.updateSuccess.bind(me));
    }

    if (modal.edit) {
        me.editModal = new ModalManager(
            window.abp.appPath + me.modal.edit
        );

        me.editModal.onResult(me.updateSuccess.bind(me));
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
    console.log(arguments)
    let me = this,
        record = me.grid.get(event.detail.recid);
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

        let config = me.initConfig,
            api = config.api;
        if (api) {
            return api.delete(ids).then(me.deleteSuccess.bind(me), me.ajaxFailure.bind(me));
        }
        fetch(config.url,{ 
            method: 'DELETE',
            body: JSON.stringify(ids),
            headers: { 
                "Content-type": "application/json;charset=utf-8",
                RequestVerificationToken: abp.security.antiForgery.getToken()
                }
        })
        .then(response => response.json())
        .then(me.deleteSuccess.bind(me))
        .catch(me.ajaxFailure.bind(me));
    });

}

Grid.prototype.onChange = function (event) {
    let me = this,
        grid = me.grid,
        column = grid.columns[event.column],
        record = grid.get(event.recid),
        action = column.action;

    if (!action) return;
    if (typeof action === 'string') {
        let fn = me[action];
        if (isFunction(fn)) fn.call(me, record, column);
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

Grid.prototype.updateSuccess = function () {
    abp.notify.success(this.globalLocalization('SavedAndExit'));
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
    console.log(arguments)
    if (error.code) {
        return abp.message.error(this.localization(error.code));
    }
    abp.message.error(error.message);
}

Grid.prototype.onReload = function () { }

Grid.prototype.onSelect = function () { }

Grid.prototype.onDeselect = function () { }


Grid.prototype.onColumnClick = function () { }

Grid.prototype.onActionClick = function () { }

Grid.prototype.onDestroy=  function() {
    let me = this;
    me.initConfig = null;
    me.el = null;
    me.localization = null;
    me.globalLocalization = null;
}

Grid.prototype.clear = function (isRefresh) {
    let me = this;
    if (me.grid) me.grid.clear();
    me.data = null;
    if (isRefresh) me.onRefresh();
}

