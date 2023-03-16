function ModalManager(config) {
    let me = this;
    if (_.isString(config)) {
        config = { viewUrl: config };
    }
    for (let i in config) {
        me[i] = config[i];
    }
    me.initConfig = config;
    me.callBacks = [];
}

ModalManager.prototype.open = function (data) {
    data = data || {};
    let me = this;

    $.get(me.viewUrl, data)
        .then(me.getModalSuccess.bind(me), me.getModalFailure.bind(me));

}

ModalManager.prototype.getModalSuccess = function (data) {
    let me = this;
    if (me.initConfig.scriptUrl) {
        //加载资源
        // abp.ResourceLoader.loadScript(options.scriptUrl, function () {
        //     _initAndShowDialog();
        // });
    } else {
        let id = 'Metro_Modal_' + $.uniqueId(),
            container = $('<div>');
        me.container = container;
        container.attr('id', id);
        me.containerId = id;
        let body = $('body');
        container.html(data);
        container.appendTo(body);
        me.initAndShowModal();
    }

}

ModalManager.prototype.getModalFailure = function () {
    console.log(arguments);
}

ModalManager.prototype.initAndShowModal = function () {
    let me = this,
        container = me.container,
        modal = container.find('.dialog'),
        form = container.find('form');
    modal = Metro.getPlugin(modal, 'modal');
    if (!modal) {
        _.delay(me.initAndShowModal.bind(me), 50);
        return;
    }
    me.modal = modal;
    me.form = form;

    form.find('.js-dialog-save').on('click', me.submit.bind(me));
    form.find('.js-dialog-close').on('click', me.onClose.bind(me));

    Metro.makePlugin(form, 'validator', {
        onSubmit: me.onSubmit.bind(me)
    });


    if (me.init) me.init.call(me);

    modal.open();

}

ModalManager.prototype.onClose = function () {
    let me = this;
    me.form.off(Metro.events.click, ".js-dialog-close");
    me.form.off(Metro.events.click, ".js-dialog-save");
    me.container.remove();
    delete me.validator;
    delete me.modal;
    delete me.form;
    delete me.container;
    delete me.callBacks;
    delete me.mask;
}


ModalManager.prototype.getModalId = function () {
    return this.containerId;
}

ModalManager.prototype.getModal = function () {
    return this.modal;
}

ModalManager.prototype.getForm = function () {
    return this.form;
}

ModalManager.prototype.getData = function () {
    return this.initConfig.data;
}

ModalManager.prototype.getConfig = function () {
    return this.initConfig;
}

ModalManager.prototype.setResult = function () {
    this.callBacks.forEach(f => {
        f.apply(null, arguments);
    })
    _onResultCallbacks.triggerAll(_publicApi, arguments);
}

ModalManager.prototype.onResult = function (callback) {
    this.callBacks.push(callback);
}

ModalManager.prototype.submit = function (e) {
    e.preventDefault();
    Metro.getPlugin(this.form, 'validator')._submit();
}


ModalManager.prototype.onSubmit = function (data) {
    let me = this,
        url = me.form[0].action,
        params = {};
    data.forEach(d => {
        let p = d.split('=');
        params[decodeURIComponent(p[0])] = decodeURIComponent(p[1]);
    })
    me.showMask();
    $.post(url, params).then(me.onSubmitSuccess.bind(me), me.onSubmitFailure.bind(me));
}

ModalManager.prototype.onSubmitSuccess = function () {
    let me = this;
    me.mask.hide();
    me.setResult(arguments);
    me.modal.close();
}


ModalManager.prototype.onSubmitFailure = function (xhr) {
    let me = this;
    me.mask.hide();
    console.log('onSubmitFailure', arguments);
}

ModalManager.prototype.showMask = function () {
    let me = this,
        modal = me.modal,
        modalEl = modal.element,
        mask = me.mask;
    if(!mask){
        mask = me.mask = $("<div>");
        mask.addClass('mask');
        mask.html(`
            <div class="mask-body">
                <div class="mask-inner">
                    <div class="loading-spinner"><span class="mif-spinner ani-spin"></span></div>
                    <div class="mask-message">${abp.localization.resources.AbpUi.texts.SavingWithThreeDot}</div>
                </div>
            </div>
        `);
        mask.appendTo(modalEl);
    }
    mask.show();
}
