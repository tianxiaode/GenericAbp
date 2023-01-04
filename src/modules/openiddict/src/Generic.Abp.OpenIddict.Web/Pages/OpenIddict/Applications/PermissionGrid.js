function PermissionGrid(config) {
    config.url = "/api/applications/permissions";
    Grid.call(this, config);
}

inherits(PermissionGrid, Grid);

PermissionGrid.prototype.onParser = function (data) {
    let l = this.localization,
        result = {},
        records = [],
        i = 0;
    for (let group in data) {
        let groupData = data[group];
        for (let d in groupData) {
            records.push({
                recid: `recid-${i}`,
                group: l(group),
                name: l(d),
                value: groupData[d],
                checked: false
            })
            i++;
        }
    }

    result.total = records.length;
    result.records = records;
    return result;
}

PermissionGrid.prototype.getGridConfig = function () {
    return {}
}

PermissionGrid.prototype.getColumns = function () {
    let l = this.localization;
    return [
        {
            field: 'name', text: l("Permission:Name"), size: '25%',
        },
        {
            field: 'group', text: l("Permission:Group"), size: '20%',
        },
        {
            field: 'value', text: l("Permission:Value"), size: '30%',
        },
        {
            field: 'checked', text: l("Permission:Checked"), size: '25%',
            editable: { type: 'checkbox', }, style: 'text-align:center;'
        },
    ];
}

PermissionGrid.prototype.onChange = function (event) {
    let me = this,
        grid = me.grid,
        currentRecord = me.currentRecord,
        record = grid.get(event.recid);
    if (record.checked) {
        me.api.removePermission(currentRecord.id, { value: record.value })
            .then(me.mergeChanges.bind(me), me.rejectChanges.bind(me));
    } else {
        me.api.addPermission(currentRecord.id, { value: record.value })
            .then(me.mergeChanges.bind(me), me.rejectChanges.bind(me));
    }

}


PermissionGrid.prototype.onRefresh = function () {
    let me = this,
        data = me.data || [],
        records = me.grid.records;

    if (records.length === 0) {
        setTimeout(me.onRefresh.bind(me), 500);
        return;
    }
    records.forEach(c => {
        c.checked = data[c.value];
    });
    me.grid.mergeChanges();

}


PermissionGrid.prototype.refresh = function (record) {
    let me = this;
    me.currentRecord = record;
    if (record && record.id) {
        me.api.getPermissions(record.id).then((ret) => {
            let data = {};
            ret.forEach(r => {
                data[r] = true;
            })
            me.data = data;
            me.onRefresh();
        })
    } else {
        me.onRefresh();
    }

}
