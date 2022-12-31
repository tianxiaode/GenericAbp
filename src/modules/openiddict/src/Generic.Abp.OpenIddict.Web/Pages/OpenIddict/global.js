(function ($) {
    if (abp.localization.currentCulture.cultureName === 'zh-Hans') {
        w2utils.locale('/libs/w2ui/locale/zh-cn.json');
    }

    w2obj.grid.prototype.rejectChanges = function(){
        var changes = this.getChanges();
        for (var c = 0; c < changes.length; c++) {
            var record = this.get(changes[c][this.recid || 'recid']);
            if (record.w2ui) delete record.w2ui.changes;
        }
        this.refresh();

    }

})(jQuery);