
Metro.toastSetup({
    showTop:true,
    timeout: 5000
});

Metro.validatorSetup({
    onErrorForm: function(logs, data){
        console.log(logs)
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
    let span = $('<span>').addClass('invalid_feedback'),
        invalidInput = div.find(`input[name="__Invariant"]`),
        label = div.find('label').first(),
        labelWidth = label ? label.width() : 0;
    if(invalidInput){
        invalidInput.remove();
    }
    if(div.hasClass('d-flex')){
        span.addClass('pos-absolute');
    }
    span.css('left', labelWidth);
    span.css('bottom', 2);
    span.appendTo(div);

}

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

    $.extend({
        serializeToArray: function(form){
            var _form = $(form)[0];
            if (!_form || _form.nodeName !== "FORM") {
                console.warn("Element is not a HTMLFromElement");
                return;
            }
            var i, j, q = [], data= {};
            for (i = _form.elements.length - 1; i >= 0; i = i - 1) {
                if (_form.elements[i].name === "") {
                    continue;
                }
                switch (_form.elements[i].nodeName) {
                    case 'INPUT':
                        switch (_form.elements[i].type) {
                            case 'checkbox':
                            case 'radio':
                                if (_form.elements[i].checked) {
                                    data[_.camelCase(_form.elements[i].name)] = _form.elements[i].value;
                                }
                                break;
                            case 'file':
                                break;
                            default: data[_.camelCase(_form.elements[i].name)] = _form.elements[i].value;
                        }
                        break;
                    case 'TEXTAREA':
                        data[_.camelCase(_form.elements[i].name)] = _form.elements[i].value;
                        break;
                    case 'SELECT':
                        switch (_form.elements[i].type) {
                            case 'select-one':
                                data[_.camelCase(_form.elements[i].name)] = _form.elements[i].value;
                                break;
                            case 'select-multiple':
                                let name = _.camelCase(_form.elements[i].name);
                                data[name] = [];
                                for (j = _form.elements[i].options.length - 1; j >= 0; j = j - 1) {
                                    if (_form.elements[i].options[j].selected) {
                                        data[name].push(_form.elements[i].value);
                                    }
                                }
                                break;
                        }
                        break;
                    case 'BUTTON':
                        switch (_form.elements[i].type) {
                            case 'reset':
                            case 'submit':
                            case 'button':
                                data[_.camelCase(_form.elements[i].name)] = _form.elements[i].value;
                                break;
                        }
                        break;
                }
            }
            return data;
        },
        defer() {
            const deferred = {};
            const promise = new Promise((resolve, reject) => {
              deferred.resolve = resolve;
              deferred.reject = reject;
            });
          
            deferred.promise = () => {
              return promise;
            };
          
            return deferred;
          }
    })


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

    abp.message.confirm = function (message, title, callback) {
        let opts = { 
            title: title,
            clsDialog: 'primary',
            content: message,
            closeButton: false,
            actions:[
                {
                    caption: abp.localization.resources.AbpUi.texts.Yes,
                    cls: "js-dialog-close success",
                    onclick: callback
                },
                {
                    caption: abp.localization.resources.AbpUi.texts.No,
                    cls: 'js-dialog-close'
                }                
            ]
        };

        Metro.dialog.create(opts);

    };

})

