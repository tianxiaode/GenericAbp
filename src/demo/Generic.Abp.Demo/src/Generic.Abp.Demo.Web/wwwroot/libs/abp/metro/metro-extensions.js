Metro.validatorSetup({
    onErrorForm: function(logs, data){
        logs.forEach(log=>{
            var input = $(log.input);
            var div = input.parent().parent();
            var span = div.find('.invalid_feedback');
            var texts =[];
            log.errors.forEach(error=>{
                let t = input.attr(`data-val-${error}`);
                if(error === 'min' || error === 'max'){
                    t = input.attr(`data-val-range`);
                }
                if(t)texts.push(t);
            })
            span.html(texts.join('<br/>'));
        })
    },
    onSubmit(){
        console.log('onSubmit', arguments);
    }
});

Metro.inputSetup({
    onInputCreate(input){
        addErrorSpan(input.parent().parent());
    }
})

Metro.textareaSetup({
    onTextareaCreate(input){
        addErrorSpan(input.parent().parent());
    }
})

Metro.calendarPickerSetup({
    onCalendarPickerCreate(input){
        addErrorSpan(input.parent().parent());
        // var me = this;
        // var calendar = input.parent().find('.calendar');
        // var id = $.uniqueId();
        // calendar.id(id);
        // this.calendarId = id;
        // var callback = function(){
        //     Metro.getPlugin(me, 'calendar-picker')._removeOverlay();
        //     calendar.removeClass("open open-up");
        // }
        // moveToFloatDiv(input, calendar , callback);
    },
    onCalendarShow(calendar){
        // console.log(calendar);
        // var inputDiv = $(this).parent();
        // var offset = inputDiv.offset();
        // calendar.attr({'style': `position:absolute;left:${offset.left}px !important;top:${offset.top + inputDiv.height() + 2}px !important;`});
        //calendar.cssText('top', `${offset.top + inputDiv.height() + 2} !important`);
        //calendar.css('position', 'absolute');
    }
})

Metro.selectSetup({
    onSelectCreate(input){
        addErrorSpan(input.parent().parent());
        // var dropContainer = input.parent().parent().find('.drop-container');
        // var id = $.uniqueId();
        // dropContainer.id(id);
        // this.dropContainerId = id;
        // console.log(this)
        // var callback = function(){
        //     Metro.getPlugin(dropContainer, "dropdown").close();
        // }
        // moveToFloatDiv(input, dropContainer , callback);
    },
    onDrop(){
        // var inputDiv = $(this).parent();
        // var id = this.dropContainerId;
        // var offset = inputDiv.offset();
        // var dropContainer = $(`#${id}`);
        // dropContainer.css('left', offset.left);
        // dropContainer.css('top', offset.top + inputDiv.height() + 2);
        // dropContainer.css('height', 234);
        // dropContainer.css('width', inputDiv.width());
    }
})

function addErrorSpan(div){
    var span = $('<span>').addClass('invalid_feedback pos-absolute');
    var invalidInput = div.find(`input[name="__Invariant"]`);
    var label = div.children().first('label');
    if(invalidInput){
        invalidInput.remove();
    }
    span.css('left', label.width());
    span.css('bottom', 2);
    span.appendTo(div);

}

function moveToFloatDiv(input, move, callback){
    var floatDiv = $('#global-floatWrap');    
    if(!floatDiv[0]){
        floatDiv = $('<div>');
        floatDiv.attr('id', 'global-floatWrap');
        floatDiv.addClass('float-wrap');
        floatDiv.addClass('root');
        floatDiv.attr('data-sticky', true);
        floatDiv.appendTo($('body'));
    }
    move.appendTo(floatDiv);
    var dialogContent = input.parents('.dialog-content');
    dialogContent.on('scroll', callback)
}


// Metro.checkboxSetup({
//     onCheckboxCreate(input){
//         var div = $(input.closest('div'));
//         var label = div.find('.checkbox-label');
//         var html = label.html();
//         var span = div.find('span.caption');
//         span.text(html);
//     }
// })

$(function () {

    $.extend(Metro.locales['zh-CN'],{
        "table": {
            "rowsCount": "显示实体:",
            "search": "搜索:",
            "info": "显示 $1 到 $2 的 $3 条目",
            "prev": "上一页",
            "next": "下一页",
            "all": "全部",
            "inspector": "查看",
            "skip": "转到页面",
            "empty": "没有数据"
        },
        "colorSelector": {
            addUserColorButton: "添加到颜色板",
            userColorsTitle: "用户颜色"
        },
        "switch": {
            on: "开",
            off: "关"
        }
    });


})

