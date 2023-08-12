function ItemGrid(config) {
    let me = this;
    me.inputId = `${config.entityName}Input`;

    config.columns = [
        { text: config.entityName, field: 'value', isMessage: true }
    ]

    Grid.call(me, config);


}

window.inherits(ItemGrid, Grid);


ItemGrid.prototype.initPermission = function () {
    let me = this;
    ['Create', 'Delete', 'Update'].forEach(p => {
        me[`allow${p}`] = abp.auth.isGranted(`${me.grantedPolicie}`);
    })

}

ItemGrid.prototype.initCreateAndEditModal = function () {
    let me = this,
        toolbar = me.grid.toolbar;
    toolbar.insert('w2ui-add', { type: 'html', html: `<input type="text" id="${me.inputId}" style="margin-top:-2px;" />`, id: me.inputId });
},


ItemGrid.prototype.onToolbar = function (event) {
    if(event.target !== 'w2ui-reload') return;
    this.clear();
    this.loadData();
},

ItemGrid.prototype.loadData = function () {
    let me = this,
        record = me.currentRecord,
        api = me.api,
        fn = api[me.apiGetName];
    if (!record || !_.isFunction(fn)) return;
    fn.call(null, record.id).then(me.loadDataSuccess.bind(me), me.ajaxFailure.bind(me));
}

ItemGrid.prototype.loadData = function () {
    let me = this,
        record = me.currentRecord,
        api = me.api,
        fn = api[me.apiGetName];
    if (!record || !_.isFunction(fn)) return;
    fn.call(null, record.id).then(me.loadDataSuccess.bind(me));
}

ItemGrid.prototype.loadDataSuccess = function (response) {
    let me = this,
        grid = me.grid
        data = response.responseJson,
        records = [];
    data.forEach((m, i) => {
        let rec = { value: m };
        rec.recid = `recid-${i}`;
        records.push(rec);
    })
    grid.records = records;
    grid.refresh()
}


ItemGrid.prototype.refresh = function (record) {
    let me = this;
    me.currentRecord = record;
    me.clear();
    me.loadData();
}


ItemGrid.prototype.onAdd = function (event) {
    let me = this,
        apiName = `add${me.entityName}`,
        value = $(`#${me.inputId}`).val();
    if (!value) {
        return abp.message.error(abp.localization.resources.AbpValidation.texts["ThisFieldIsRequired."]);
    }

    let fn = me.api[apiName],
        id = me.currentRecord.id;
    if (!id || !_.isFunction(fn)) return;
    fn.call(null, id, { value: value }).then(me.updateSuccess.bind(me));

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
        let apiName = `remove${me.entityName}`,
            fn = me.api[apiName];
    if (!id || !_.isFunction(fn)) return;
        fn.call(null, id, { items: message }).then(me.deleteSuccess.bind(me));
    });

}

ItemGrid.prototype.updateSuccess = function () {
    let me = this;
    abp.notify.success(me.globalLocalization('UpdateSuccess'));
    me.clear();
    me.loadData();
}

ItemGrid.prototype.deleteSuccess = function () {
    let me = this;
    abp.notify.success(me.globalLocalization('DeleteSuccess'));
    me.clear();
    me.loadData();
}

