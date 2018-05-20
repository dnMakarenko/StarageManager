(function () {
    'use strict';

    angular
        .module('StorageManager')
        .factory('tokenHandler', tokenHandler);

    tokenHandler.inject = ['storageHandler'];

    function tokenHandler(storageHandler) {
        var loginTokenId = 'storageManager-loginToken-Token';
        var nameTokenId = 'storageManager-loginName-Token';
        var roleTokenId = 'storageManager-role-Token';
        var redirectUrl = null;

        return {
            setLoginToken: function (token) {
                storageHandler.setItem(loginTokenId, token);
            },
            getLoginToken: function () {
                return storageHandler.getItem(loginTokenId);
            },
            removeLoginToken: function () {
                storageHandler.removeItem(loginTokenId);
            },
            hasLoginToken: function () {
                return this.getLoginToken() != null;
            },
            setRedirectUrl: function (url) {
                redirectUrl = url;
            },
            getRedirectUrl: function () {
                return redirectUrl;
            },
            setLoginName: function (name) {
                storageHandler.setItem(nameTokenId, name);
            },
            getLoginName: function () {
                return storageHandler.getItem(nameTokenId);
            },
            setRole: function (name) {
                storageHandler.setItem(roleTokenId, name);
            },
            getRole: function () {
                return storageHandler.getItem(roleTokenId);
            },
            hasRole: function () {
                return this.getRole() != null;
            },
            removeRole: function () {
                storageHandler.removeItem(roleTokenId);
            }
        }

    }

}());