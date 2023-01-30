/* global Metro, Cake */
(function(Metro, $) {
    'use strict';
    var Utils = Metro.utils;
    var CrudToolbarDefaultConfig = {
        targets: "button",
        onButtonClick: Metro.noop,
        onCrudToolbarCreate: Metro.noop
    };

    Metro.crudToolbarSetup = function (options) {
        CrudToolbarDefaultConfig = $.extend({}, CrudToolbarDefaultConfig, options);
    };

    if (typeof window["metroCrudToolbarSetup"] !== undefined) {
        Metro.crudToolbarSetup(window["metroCrudToolbarSetup"]);
    }

    Metro.Component('crud-toolbar', {
        init: function( options, elem ) {
            this._super(elem, options, CrudToolbarDefaultConfig,{
                active: null,
                id: Utils.elementId("crud-toolbar")
            });
            return this;
        },

        _create: function(){
            var element = this.element;
            console.log(this.options);

            this._createEvents();

            this._fireEvent("crud-toolbar-create", {
                element: element
            });
        },

        _createEvents: function(){
            var that = this, element = this.element, o = this.options;   
            element.on(Metro.events.click, o.targets, function(){
                var el = $(this);
                var aciton = el.attr('data-action');

                that._fireEvent("button-click", {
                    button: this,
                    action: aciton
                })

            });
        },

        changeAttribute: function(){
        },

        destroy: function(){
            var element = this.element, o = this.options;
            element.off(Metro.events.click, o.targets);
            return element;
        }
    });
}(Metro, m4q));