
Metro.toastSetup({
    showTop:true,
    timeout: 5000
});

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
    }
})

Metro.selectSetup({
    onSelectCreate(input){
        addErrorSpan(input.parent().parent());
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

// let left = ['L', ''], 
//     right = ['R', ''],
//     borderTop =[ 'T', ''],
//     bottom = ['B' ,''],
//     style = ['solid', 'dashed', 'dotted', 'double',  'groove', 'inset' ,'outset' ,'ridge'],
//     text = [];
// left.forEach(l=>{
//     right.forEach(r=>{
//         borderTop.forEach(t=>{
//             bottom.forEach(b=>{
//                 // style.forEach(s=>{
//                 //     [1,2,3,4,5].forEach(size=>{
//                         let result = [];
//                         if(!_.isEmpty(l)) result.push(l);
//                         if(!_.isEmpty(r)) result.push(r);
//                         if(!_.isEmpty(t)) result.push(t);
//                         if(!_.isEmpty(b)) result.push(b);
//                         if(result.length === 0) return;
//                         result[0] = _.capitalize(result[0]);
//                         text.push(result.join(''));
//                 //     })
//                 // })
//             })
//         })
//     })
// })
// console.log(text.sort().join(',\n'));
// var colorList = ["black"," white"," dark"," light"," grayBlue"," grayWhite"," grayMouse"," brandColor1"," brandColor2","lime"," green"," emerald"," blue"," teal"," cyan"," cobalt"," indigo"," violet"," pink"," magenta"," crimson"," red"," orange"," amber"," yellow"," brown"," olive"," steel"," mauve"," taupe"," gray","lightLime"," lightGreen"," lightEmerald"," lightBlue"," lightTeal"," lightCyan"," lightCobalt"," lightIndigo"," lightViolet"," lightPink"," lightMagenta"," lightCrimson"," lightRed"," lightOrange"," lightAmber"," lightYellow"," lightBrown"," lightOlive"," lightSteel"," lightMauve"," lightTaupe"," lightGray"," lightGrayBlue","darkLime"," darkGreen"," darkEmerald"," darkBlue"," darkTeal"," darkCyan"," darkCobalt"," darkIndigo"," darkViolet"," darkPink"," darkMagenta"," darkCrimson"," darkRed"," darkOrange"," darkAmber"," darkYellow"," darkBrown"," darkOlive"," darkSteel"," darkMauve"," darkTaupe"," darkGray"," darkGrayBlue"];
// var result = [];
// colorList.forEach(c=>{
//     c = _.upperFirst(_.trim(c));
//     result.push(c);
// })
// console.log(result.sort().join(',\n'))


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

    var showNotification = function (type, message, options) {
        Metro.toast.create(message, null, null, type, options);
    };

    abp.notify.success = function (message, options) {
        showNotification('success', message);
    };

    abp.notify.info = function (message,  options) {
        showNotification('info', message,  options);
    };

    abp.notify.warn = function (message,  options) {
        showNotification('warning', message,  options);
    };

    abp.notify.error = function (message,  options) {
        showNotification('alert', message,  options);
    };

    abp.message.info = function (message, detail) {
        return showNotification('info', message, title);
    };

    abp.message.success = function (message, detail) {
        return showNotification('success', message, title);
    };

    abp.message.warn = function (message, detail) {
        return showNotification('warn', message, title);
    };

    abp.message.error = function (message, detail) {
        message = `<h4>${message}</h4>`;
        if(detail) message += `<p>${detail}</p>`;
        
        Metro.dialog.create({
            title: abp.localization.resources.AbpExceptionHandling.texts.Error,
            content: message,
            clsDialog: 'alert',
            closeButton: false
        });
    };

    abp.message.confirm = function (message, titleOrCallback, callback) {

        var userOpts = {
            text: message
        };

        if ($.isFunction(titleOrCallback)) {
            closeOnEsc = callback;
            callback = titleOrCallback;
        } else if (titleOrCallback) {
            userOpts.title = titleOrCallback;
        };

        var opts = $.extend(
            {},
            abp.libs.sweetAlert.config['default'],
            abp.libs.sweetAlert.config.confirm,
            userOpts
        );

        return $.Deferred(function ($dfd) {
            Swal.fire(opts).then(result  => {
                callback && callback(result.value);
                $dfd.resolve(result.value);
            })
        });
    };

})

