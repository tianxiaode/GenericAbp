function Claims(el, claims, services){
    let me = this;
    me.el = $(el);
    me.localization = window.abp.localization.getResource('AbpIdentityServer');
    me.services = services;
    me.claims = claims;

    me.init();
}

Claims.prototype = {

    init(){
        let me = this,
            l = me.localization;
        me.grid = me.el.w2grid({
            name: 'claimsGrid',
            columns: [
                { field: 'name', text: l("Claims"), size: '150px', style: 'background-color: #efefef; border-bottom: 1px solid white; padding-right: 5px;', attr: "align=right" },
                { field: 'value', text: l("OwnedClaims"), size: '100%', 
                    editable: { type: 'checkbox', style: 'text-align: center' } 
                },
            ],
            onChange: me.onChange.bind(me)
        })
    },

    onChange(event){
        let me = this,            
            grid = me.grid,
            currentRecord = me.currentRecord,
            claim = grid.get(event.recid);
        if(claim.value){
            me.services
                .removeClaim(currentRecord.id, { type: claim.name } )
                .then(function () {
                    grid.mergeChanges();
                });
        }else{
            me.services
            .addClaim(currentRecord.id, { type: claim.name })
            .then(function () {
                grid.mergeChanges();
            });
        }        

    },

    onRefresh(data){
        let me = this,
            recs = [],
            index = 0;
        me.claims.forEach(c=>{
            let rec = { recid: index, name: c.name, value: data.find(d=>d.type === c.name) };
            index++;
            recs.push(rec);
        });
        me.grid.add(recs);

    },

    refresh(record){
        let me= this,
            grid = me.grid;
        me.currentRecord = record;
        grid.clear();
        if(record.id){
            me.services.getClaims(record.id).then((ret)=>{
                let data = ret.items;
                me.onRefresh(data);
            })
        }else{
            me.onRefresh([]);
        }

    }
}