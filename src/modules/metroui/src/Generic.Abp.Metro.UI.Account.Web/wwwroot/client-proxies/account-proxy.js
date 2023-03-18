/* This file is automatically generated by ABP framework to use MVC Controllers from javascript. */


// module account

(function(){

  // controller volo.abp.account.account

  (function(){

    abp.utils.createNamespace(window, 'volo.abp.account.account');

    volo.abp.account.account.register = function(input, ajaxParams) {
      return abp.ajax($.extend({
        url: abp.appPath + 'api/account/register',
        type: 'POST',
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.account.account.sendPasswordResetCode = function(input, ajaxParams) {
      return abp.ajax($.extend({
        url: abp.appPath + 'api/account/send-password-reset-code',
        type: 'POST',
        dataType: null,
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.account.account.verifyPasswordResetToken = function(input, ajaxParams) {
      return abp.ajax($.extend({
        url: abp.appPath + 'api/account/verify-password-reset-token',
        type: 'POST',
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.account.account.resetPassword = function(input, ajaxParams) {
      return abp.ajax($.extend({
        url: abp.appPath + 'api/account/reset-password',
        type: 'POST',
        dataType: null,
        data: JSON.stringify(input)
      }, ajaxParams));
    };

  })();

  // controller volo.abp.account.web.areas.account.controllers.account

  (function(){

    abp.utils.createNamespace(window, 'volo.abp.account.web.areas.account.controllers.account');

    volo.abp.account.web.areas.account.controllers.account.login = function(login, ajaxParams) {
      return abp.ajax($.extend({
        url: abp.appPath + 'api/account/login',
        type: 'POST',
        data: JSON.stringify(login)
      }, ajaxParams));
    };

    volo.abp.account.web.areas.account.controllers.account.logout = function(ajaxParams) {
      return abp.ajax($.extend({
        url: abp.appPath + 'api/account/logout',
        type: 'GET',
        dataType: null
      }, ajaxParams));
    };

    volo.abp.account.web.areas.account.controllers.account.checkPassword = function(login, ajaxParams) {
      return abp.ajax($.extend({
        url: abp.appPath + 'api/account/check-password',
        type: 'POST',
        data: JSON.stringify(login)
      }, ajaxParams));
    };

  })();

  // controller volo.abp.account.profile

  (function(){

    abp.utils.createNamespace(window, 'volo.abp.account.profile');

    volo.abp.account.profile.get = function(ajaxParams) {
      return abp.ajax($.extend({
        url: abp.appPath + 'api/account/my-profile',
        type: 'GET'
      }, ajaxParams));
    };

    volo.abp.account.profile.update = function(input, ajaxParams) {
      return abp.ajax($.extend({
        url: abp.appPath + 'api/account/my-profile',
        type: 'PUT',
        data: JSON.stringify(input)
      }, ajaxParams));
    };

    volo.abp.account.profile.changePassword = function(input, ajaxParams) {
      return abp.ajax($.extend({
        url: abp.appPath + 'api/account/my-profile/change-password',
        type: 'POST',
        dataType: null,
        data: JSON.stringify(input)
      }, ajaxParams));
    };

  })();

})();

