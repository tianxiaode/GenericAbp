function MultilingualGrid(config) {
    config.show ={
        selectColumn: false,
        toolbar: true,
        toolbarReload:false,
        toolbarAdd: false,
        toolbarDelete: false,
        toolbarSave: true,
        footer: true
    };
    config.multiSelect = false;
    config.entityName = 'Multilingual';

    Grid.call(this, config);
}

window.inherits(MultilingualGrid, Grid);

MultilingualGrid.prototype.getColumns = function () {
    let l = abp.localization.resources.ExtResource.texts;
    return [
        { 
            field: 'displayName', text: l.Language, size: '50%'
        },
        { 
            field: 'value', text: l.Value, size: '50%' ,
            editable: { type: 'text' }
        }
    ];
}

MultilingualGrid.prototype.onRefresh = function () {
    let me = this,
        languages = {},
        record = me.currentRecord,
        grid = me.grid,
        index = 1;
    abp.localization.languages.forEach(l => {
        languages[l.cultureName] = Object.assign({ value: '', recid: index}, l);
        index++;
    })
    me.api.getTranslation(record.id).then((response) => {
        response.responseJson.items.forEach(i => {
            let l = languages[i.language];
            if (l) {
                l.value = i.name;
            }
        })
        grid.records = Object.values(languages);
        grid.refresh();
        $(grid.toolbar.box).height(52);
    })

}
