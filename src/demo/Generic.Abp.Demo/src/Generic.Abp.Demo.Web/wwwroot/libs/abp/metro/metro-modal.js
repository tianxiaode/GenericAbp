/* global Metro, METRO_LOCALE */
(function(Metro, $) {
    'use strict';
    var Utils = Metro.utils;
    var MoadlDefaultConfig = {
        modalDeferred: 0,
        closeButton: false,
        leaveOverlayOnClose: false,
        toTop: false,
        toBottom: false,
        locale: METRO_LOCALE,
        defaultAction: true,
        overlay: true,
        overlayColor: '#000000',
        overlayAlpha: .5,
        overlayClickClose: false,
        height: 'auto',
        shadow: true,
        closeAction: true,
        clsDefaultAction: "",
        clsOverlay: "",
        autoHide: 0,
        removeOnClose: false,
        show: false,
        mask: true,

        _runtime: false,

        onShow: Metro.noop,
        onHide: Metro.noop,
        onOpen: Metro.noop,
        onClose: Metro.noop,
        onModalCreate: Metro.noop
    };

    Metro.modalSetup = function (options) {
        MoadlDefaultConfig = $.extend({}, MoadlDefaultConfig, options);
    };

    if (typeof window["metroModalSetup"] !== undefined) {
        Metro.modalSetup(window["metroModalSetup"]);
    }

    Metro.Component('modal', {
        _counter: 0,

        init: function( options, elem ) {
            this._super(elem, options, MoadlDefaultConfig, {
                interval: null,
                overlay: null,
                id: Utils.elementId("modal")
            });

            return this;
        },

        _create: function(){
            var o = this.options;
            this.locale = Metro.locales[o.locale] !== undefined ? Metro.locales[o.locale] : Metro.locales["en-US"];
            this._build();
        },

        _build: function(){
            var that = this, element = this.element, o = this.options;
            var overlay;

            element.addClass("dialog");

            if (o.shadow === true) {
                element.addClass("shadow-on");
            }

            if (o.overlay === true) {
                overlay  = this._overlay();
                this.overlay = overlay;
            }

            if (o.closeAction === true) {
                element.on(Metro.events.click, ".js-dialog-close", function(){
                    that.close();
                });
            }

            var closer = element.find("closer");
            if (closer.length === 0) {
                closer = $("<span>").addClass("button square closer js-dialog-close");
                closer.appendTo(element);
            }
            if (o.closeButton !== true) {
                closer.hide();
            }

            element.css({
                width: o.width,
                height: o.height,
                visibility: "hidden",
                top: '100%',
                left: ( $(window).width() - element.outerWidth() ) / 2
            });

            if (o.show) {
                this.open();
            }

            $(window).on(Metro.events.resize, function(){
                that.setPosition();
            }, {ns: this.id});

            this._fireEvent("modal-create", {
                element: element
            });
        },

        _overlay: function(){
            var o = this.options;

            var overlay = $("<div>");
            overlay.addClass("overlay").addClass(o.clsOverlay);

            if (o.overlayColor === 'transparent') {
                overlay.addClass("transparent");
            } else {
                overlay.css({
                    background: Metro.colors.toRGBA(o.overlayColor, o.overlayAlpha)
                });
            }

            return overlay;
        },

        hide: function(callback){
            var element = this.element, o = this.options;
            var timeout = 0;
            if (o.onHide !== Metro.noop) {
                timeout = 500;

                this._fireEvent("hide");
            }
            setTimeout(function(){
                Utils.exec(callback, null, element[0]);
                element.css({
                    visibility: "hidden",
                    top: "100%"
                });
            }, timeout);
        },

        show: function(callback){
            var element = this.element;
            this.setPosition();
            element.css({
                visibility: "visible"
            });

            this._fireEvent("show");

            Utils.exec(callback, null, element[0]);
        },

        setPosition: function(){
            var element = this.element, o = this.options;
            var top, bottom;
            if (o.toTop !== true && o.toBottom !== true) {
                top = ( $(window).height() - element.outerHeight() ) / 2;
                if (top < 0) {
                    top = 0;
                }
                bottom = "auto";
            } else {
                if (o.toTop === true) {
                    top = 0;
                    bottom = "auto";
                }
                if (o.toTop !== true && o.toBottom === true) {
                    bottom = 0;
                    top = "auto";
                }
            }
            element.css({
                top: top,
                bottom: bottom,
                left: ( $(window).width() - element.outerWidth() ) / 2
            });
        },

        close: function(){
            var that = this, element = this.element, o = this.options;

            if (!Utils.bool(o.leaveOverlayOnClose)) {
                $('body').find('.overlay').remove();
            }

            this.hide(function(){
                element.data("open", false);

                that._fireEvent("close")

                if (o.removeOnClose === true) {
                    element.remove();
                }
            });
        },

        open: function(){
            var that = this, element = this.element, o = this.options;

            if (o.overlay === true && $(".overlay").length === 0) {
                this.overlay.appendTo($("body"));
                if (o.overlayClickClose === true) {
                    this.overlay.on(Metro.events.click, function(){
                        that.close();
                    });
                }
            }

            this.show(function(){

                that._fireEvent("open");

                element.data("open", true);
                if (parseInt(o.autoHide) > 0) {
                    setTimeout(function(){
                        that.close();
                    }, parseInt(o.autoHide));
                }
            });
        },

        toggle: function(){
            var element = this.element;
            if (element.data('open')) {
                this.close();
            } else {
                this.open();
            }
        },

        isOpen: function(){
            return this.element.data('open') === true;
        },

        /* eslint-disable-next-line */
        changeAttribute: function(attributeName){
        },

        destroy: function(){
            var element = this.element;

            element.off(Metro.events.click, ".js-dialog-close");
            element.find(".button").off(Metro.events.click);
            $(window).off(Metro.events.resize,{ns: this.id});

            return element;
        }
    });

    Metro.modal = {
        isModal: function(el){
            return Utils.isMetroObject(el, "modal");
        },

        open: function(el, content, title){
            if (!this.isModal(el)) {
                return false;
            }
            var dialog = Metro.getPlugin(el, "modal");
            if (title !== undefined) {
                dialog.setTitle(title);
            }
            if (content !== undefined) {
                dialog.setContent(content);
            }
            dialog.open();
        },

        close: function(el){
            if (!this.isModal(el)) {
                return false;
            }
            Metro.getPlugin($(el)[0], "modal").close();
        },

        toggle: function(el){
            if (!this.isModal(el)) {
                return false;
            }
            Metro.getPlugin($(el)[0], "modal").toggle();
        },

        isOpen: function(el){
            if (!this.isModal(el)) {
                return false;
            }
            Metro.getPlugin($(el)[0], "modal").isOpen();
        },

        remove: function(el){
            if (!this.isModal(el)) {
                return false;
            }
            var dialog = Metro.getPlugin($(el)[0], "modal");
            dialog.options.removeOnClose = true;
            dialog.close();
        },

        create: function(options){
            var dlg;

            dlg = $("<div>").appendTo($("body"));

            var dlg_options = $.extend({}, {
                show: true,
                closeAction: true,
                removeOnClose: true
            }, (options !== undefined ? options : {}));

            dlg_options._runtime = true;

            return Metro.makePlugin(dlg, "modal", dlg_options);
        }
    };
}(Metro, m4q));