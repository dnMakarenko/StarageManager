(function () {
    'use strict';

    angular
        .module('StorageManager')
        .controller('LoginController', LoginController);

    LoginController.$inject = ['$scope', '$location', 'AuthService'];

    function LoginController($scope, $location, AuthService) {
        $scope.loginData = {
            userName: "",
            password: ""
        };

        $scope.message = "";

        $scope.login = function () {

            AuthService.login($scope.loginData).then(
                function (response) {
                    $scope.Global.isAuthenticated = true;
                    $scope.Global.isAdmin = AuthService.isAdmin();
                    $location.path('/');
                },
             function (data) {
                 $scope.message = data.error_description;
             });
        };
    }
})();