function ItemGrid(config) {
    this.inputId = `${config.name}Input`;
    Grid.call(this, config);
}

inherits(ItemGrid, Grid);

ItemGrid.prototype.getGridConfig = function () {
    let me = this;
    return {
        show: {
            selectColumn: true,
            toolbar: true,
            toolbarSearch: false,
            toolbarColumns: false,
            toolbarInput: false,
            toolbarAdd: { text: null },
            toolbarEdit: false,
            toolbarDelete: true,
            skipRecords: false,
            lineNumbers: true,
        },
        reload: me.reload.bind(me)
    }
}

ItemGrid.prototype.initCreateAndEditModal = function () {
    let me = this,
        toolbar = me.grid.toolbar;
    toolbar.insert('w2ui-add', { type: 'html', html: `<input type="text" id="${me.inputId}"/>`, id: me.inputId });
},


    ItemGrid.prototype.reload = function () {
        this.refresh(this.currentRecord);
    },

    ItemGrid.prototype.loadData = function () {
        let me = this,
            record = me.currentRecord,
            api = me.api,
            fn = api[me.apiGetName];
        if (!record || !isFunction(fn)) return;
        fn.call(null, record.id).then(me.loadDataSuccess.bind(me), me.ajaxFailure.bind(me));
    }

ItemGrid.prototype.loadData = function () {
    let me = this,
        record = me.currentRecord,
        api = me.api,
        fn = api[me.apiGetName];
    if (!record || !isFunction(fn)) return;
    fn.call(null, record.id).then(me.loadDataSuccess.bind(me), me.ajaxFailure.bind(me));
}

ItemGrid.prototype.loadDataSuccess = function (data) {
    let me = this,
        records = [];
    data.forEach((m, i) => {
        let rec = { value: m };
        rec.recid = `recid-${i}`;
        records.push(rec);
    })
    me.grid.add(records);
}


ItemGrid.prototype.refresh = function (record) {
    let me = this;
    me.currentRecord = record;
    me.clear();
    me.loadData();
}


ItemGrid.prototype.onAdd = function (event) {
    let me = this,
        value = $(`#${me.inputId}`).val();
    if (!value) {
        return abp.message.error(window.abp.localization.getResource('AbpValidation')("ThisFieldIsRequired."));
    }

    let fn = me.api[me.apiAddName],
        id = me.currentRecord.id;
    fn.call(null, id, { value: value }).then(me.updateSuccess.bind(me), me.ajaxFailure.bind(me));

}

ItemGrid.prototype.onDelete = function (event) {
    let me = this,
        l = me.localization,
        grid = me.grid,
        data = grid.getSelection(),
        title = window.abp.utils.formatString(l('DeleteConfirmTitle'), l("Records")),
        messageField = me.messageField,
        message = [],
        id = me.currentRecord.id,
        records = [];
    data.forEach(m => {
        let rec = grid.get(m);
        if (!rec) return;
        records.push(rec);
        message.push(`${rec[messageField]}`);
    })
    window.abp.message.confirm(message.join(','), title, function (confirm) {
        if (!confirm) retrun;
        let fn = me.api[me.apiDeleteName];
        if (!isFunction(fn)) return;
        forEachAsync(records, (m) => {
            let data = {};
            data[messageField] = m[messageField];
            return fn.call(null, id, data);
        }).then(me.deleteSuccess.bind(me), me.ajaxFailure.bind(me));
    });

}

