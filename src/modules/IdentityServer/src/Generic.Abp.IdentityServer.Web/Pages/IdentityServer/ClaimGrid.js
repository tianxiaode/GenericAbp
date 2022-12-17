function ClaimGrid(config){
    config.url = "/api/claim-types";
    Grid.call(this, config);
}

inherits(ClaimGrid, Grid);

ClaimGrid.prototype.getGridConfig = function(){
    return {}
}

ClaimGrid.prototype.getColumns = function(){
    let l = this.localization;
    return [
        { 
            field: 'name', text: l("Claims"), size: '150px', 
            style: 'background-color: #efefef; border-bottom: 1px solid white; padding-right: 5px;', attr: "align=right" 
        },
        { field: 'value', text: l("OwnedClaims"), size: '100%', 
            editable: { type: 'checkbox', style: 'text-align: center' } 
        },
    ];
}

ClaimGrid.prototype.onChange = function(event){
    let me = this,            
        grid = me.grid,
        currentRecord = me.currentRecord,
        claim = grid.get(event.recid);
    if(claim.value){
        me.api.removeClaim(currentRecord.id, { type: claim.name } )
            .then(me.mergeChanges.bind(me), me.rejectChanges.bind(me));
    }else{
        me.api.addClaim(currentRecord.id, { type: claim.name })
            .then(me.mergeChanges.bind(me), me.rejectChanges.bind(me));
    }        

},


ClaimGrid.prototype.onRefresh = function(){
    let me = this,
        data = me.data || [],
        claims = me.grid.records;

    if(claims.length === 0){
        setTimeout(me.onRefresh.bind(me), 500);
        return;
    }
    claims.forEach(c=>{
        let find = data.find(m=>m.type == c.name); 
        c.value = !!find;
    });
    me.grid.mergeChanges();

},


ClaimGrid.prototype.refresh = function(record){
    let me= this;
    me.currentRecord = record;
    if(record && record.id){
        me.api.getClaims(record.id).then((ret)=>{
            me.data = ret.items;
            me.onRefresh();
        })
    }else{
        me.onRefresh();
    }

}
