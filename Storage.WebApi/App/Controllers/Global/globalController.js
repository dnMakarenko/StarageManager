(function () {
    'use strict';

    angular
        .module('StorageManager')
        .controller('GlobalController', GlobalController);

    GlobalController.$inject = ['$scope', '$location', 'AuthService', 'CartService'];

    function GlobalController($scope, $location, AuthService, CartService) {
        $scope.$on('$viewContentLoaded', onLoaded);
        $scope.$on('viewContentLoadComplete', onLoadComplete);

        function onLoaded() {
            $scope.$broadcast('viewContentLoadComplete');
        }

        function onLoadComplete() {

        }

        function GetShoppingCart() {
            if (AuthService.isAuthenticated()) {
                CartService.getCart().
                    then(function (result) {
                        $scope.Global.ShoppingCart = result.data;
                        return result.data;
                    },
                    function (response) {
                        console.log(response);
                    })
            }
        };

        $scope.Global = {
            logOut : function () {
                AuthService.logOut();
                $scope.Global.isAuthenticated = false;
                $scope.Global.isAdmin = false;
                $scope.Global.ShoppingCart = {};
                $location.path('/Account/Login');
            },
            isAuthenticated: AuthService.isAuthenticated(),
            isAdmin: AuthService.isAdmin(),
            ShoppingCart: GetShoppingCart()
        }
    };
})();
