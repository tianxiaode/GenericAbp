function RelationalGrid(config){
    Grid.call(this, config);
}

inherits(RelationalGrid, Grid);

RelationalGrid.prototype.getGridConfig = function(){
    let me = this;
    return {
        show: {
            selectColumn: true,
            toolbar: true,
            toolbarSearch: false,
            toolbarColumns: false,
            toolbarInput: false,
            toolbarAdd: { text: null},
            toolbarEdit: false, 
            toolbarDelete: true,
            skipRecords: false, 
            lineNumbers: true,
         },
         reload: me.reload.bind(me)
    }    
}

RelationalGrid.prototype.reload = function(){
    this.refresh(this.currentRecord);
},

RelationalGrid.prototype.loadData = function(){
    let me = this,
        record = me.currentRecord,
        api = me.api,
        fn = api[me.apiGetName] ;
    if(!record || !isFunction(fn)) return;
    fn.call(null, record.id).then(me.loadDataSuccess.bind(me), me.ajaxFailure.bind(me));
}

RelationalGrid.prototype.loadData = function(){
    let me = this,
        record = me.currentRecord,
        api = me.api,
        fn = api[me.apiGetName] ;
    if(!record || !isFunction(fn)) return;
    fn.call(null, record.id).then(me.loadDataSuccess.bind(me), me.ajaxFailure.bind(me));
}

RelationalGrid.prototype.loadDataSuccess= function(data){
    let me = this,
        records = [];
    data.items.forEach((m, i)=>{
        let rec = Object.assign({}, m);
        rec.recid = `recid-${i}`;
        records.push(rec);
    })
    me.grid.add(records);
}


RelationalGrid.prototype.refresh = function(record){
    let me= this;
    me.currentRecord = record;
    me.clear();
    me.loadData();
}


RelationalGrid.prototype.onAdd = function(event){
    let me = this;
    me.createModal.open({
        foreignKeyId: me.currentRecord.id
    });
}
