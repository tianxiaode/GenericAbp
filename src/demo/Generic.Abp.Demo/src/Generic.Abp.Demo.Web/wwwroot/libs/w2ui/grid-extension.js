function Grid(config) {
    let me = this;
    me.initConfig = config;
    me.modal = config.modal;
    me.el = $(`#${config.el}`);
    me.localization = window.abp.localization.getResource(config.resourceName);

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
        header: true,
        toolbar: true,
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
            skipRecords: false,
            lineNumbers: true,
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
        onLoad: me.onReload.bind(me)
    };

}

Grid.prototype.getColumns = function () {
    let me = this,
        l = me.localization,
        columns = [];
    me.initConfig.columns.forEach(c => {
        if (c.isMessage) me.messageField = c.field;
        if (c.isAction) me.hasActionColumn = true;
        let config = Object.assign({}, c);
        config.text = l(c.text);
        config.tooltip = config.text;
        columns.push(config);
    })
    return columns;
}

Grid.prototype.initGrid = function () {
    let me = this,
        config = me.getGridDefaultConfig(),
        showConfig = config.showConfig;
    config = Object.assign(config, me.initConfig);
    if(config.header){
        showConfig.header = true;
    }
    config.columns = me.getColumns();
    me.grid = me.el.w2grid(config);

    if (me.hasActionColumn) {
        me.el.delegate('span.action', 'click', me.onActionClick.bind(me));
    }

}

Grid.prototype.initCreateAndEditModal = function () {
    let me = this,
        modal = me.modal;
    if (!modal) return;
    if (modal.create) {
        me.createModal = new window.abp.ModalManager(
            window.abp.appPath + me.modal.create
        );
        me.createModal.onResult(me.updateSuccess.bind(me));
    }

    if (modal.edit) {
        me.editModal = new window.abp.ModalManager(
            window.abp.appPath + me.modal.edit
        );

        me.editModal.onResult(me.updateSuccess.bind(me));
    }


}


Grid.prototype.onRequest = function (event) {
    let postData = event.postData;
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
        record = me.grid.get(event.recid);
    if (!record || !me.editModal) return;
    me.editModal.open({
        id: record.id
    });

}

Grid.prototype.onDelete = function (event) {
    let me = this,
        l = me.localization,
        grid = me.grid,
        data = grid.getSelection(),
        title = window.abp.utils.formatString(l('DeleteConfirmTitle'), l("Records")),
        message = [],
        ids = [];
    data.forEach(m => {
        let rec = grid.get(m);
        if (!rec) return;
        message.push(`${rec[me.messageField]}`);
        ids.push(rec.id);
    })
    window.abp.message.confirm(message.join(','), title, function (confirm) {
        if (!confirm) retrun;

        me.api.delete(ids)
            .then(me.updateSuccess.bind(me), me.ajaxFailure.bind(me));
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
    toastr.success(this.localization('SaveSuccessfully'));
    this.grid.reload();
}

Grid.prototype.deleteSuccess = function () {
    toastr.success(this.localization('DataDeleted'));
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
        abp.message.error(this.localization(error.code));
    }
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
}

Grid.prototype.clear = function (isRefresh) {
    let me = this;
    if (me.grid) me.grid.clear();
    me.data = null;
    if (isRefresh) me.onRefresh();
}

