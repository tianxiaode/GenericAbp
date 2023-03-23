/* global Metro, METRO_TIMEOUT, METRO_ANIMATION_DURATION */
(function(Metro, $) {
    'use strict';
    var Utils = Metro.utils;
    var AlertDefaultConfig = {
        dismissible: null,
        type: 'default',
        onAlertCreate: Metro.noop
    };

    Metro.alertSetup = function(options){
        AlertDefaultConfig = $.extend({}, AlertDefaultConfig, options);
    };

    if (typeof window["metroAlertSetup"] !== undefined) {
        Metro.alertSetup(window["metroAlertSetup"]);
    }

    Metro.Component('alert', {
        init: function( options, elem ) {
            this._super(elem, options, AlertDefaultConfig);
            return this;
        },

        _create: function(){
            var element = this.element,
                o = this.options;
            element.addClass('alert');
            element.addClass(o.type);

            this._createButtons();

            this._fireEvent("alert-create", {
                element: element
            });
        },

        _createButtons(){
            var me = this, element = this.element, o = this.options;
            console.log(element)
            if(o.dismissible == 'True'){
                var close = $('<span>');
                close.addClass('closer');
                close.appendTo(element);
                close.on(Metro.events.click, function(){
                    me.remove();
                });
            }
        },

        remove: function(){
            var element = this.element
                close = element.find('.closer');
            if(close) close.off(Metro.events.click);
            this.remove();
        }
    });
}(Metro, m4q));