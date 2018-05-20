(function () {
    'use strict';

    angular
        .module('StorageManager')
        .factory('AuthService', AuthService);

    AuthService.$inject = ['$q', '$window', 'errorHandler', '$http', 'tokenHandler'];

    function AuthService($q, $window, errorHandler, $http, tokenHandler) {

        var serviceBase = 'http://localhost:11111/';
        var authServiceFactory = {};

        var _saveRegistration = function (registration) {

            return $http.post('/api/account/register', registration)
                .then(function (response) {
                    return response;
                });

        };

        var _login = function (loginData) {
            var deferred = $q.defer();
            var data = "userName=" + loginData.userName + "&password=" + loginData.password + "&grant_type=password";

            $http.post(serviceBase + 'token', data, {
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            })
                .success(function (result) {
                    tokenHandler.setLoginToken(result.access_token);
                    tokenHandler.setLoginName(result.userName);
                    tokenHandler.setRole(result.role);

                    deferred.resolve(result);
                })
                .error(function (err, status) {
                    _logOut();

                    deferred.reject(err);
                });

            return deferred.promise;

        };

        var _logOut = function () {
            var deferred = $q.defer();

            $http.post('/api/Account/Logout')
                .success(function (response) {

                    tokenHandler.removeLoginToken();
                    tokenHandler.removeRole();

                    deferred.resolve(response);

                })
                .error(function (err, status) {
                    _logOut();
                    deferred.reject(err);
                });

            return deferred.promise;
        };

        var _isAuthenticated = function () {
            return tokenHandler.hasLoginToken();
        }
        var _isUserInRole = function () {
            return tokenHandler.hasRole();
        }
        var _isAdmin = function () {
            if (_isUserInRole()) {
                var role = tokenHandler.getRole();
                if (role.indexOf("Admin")!==-1) {
                    return true;
                }
            }
            return false;
        }

        authServiceFactory.saveRegistration = _saveRegistration;
        authServiceFactory.login = _login;
        authServiceFactory.logOut = _logOut;
        authServiceFactory.isAuthenticated = _isAuthenticated;
        authServiceFactory.isUserInRole = _isUserInRole;
        authServiceFactory.isAdmin = _isAdmin;

        return authServiceFactory;
    };
})();