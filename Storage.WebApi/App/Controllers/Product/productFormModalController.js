(function () {
    'use strict';

    angular
        .module('StorageManager')
        .controller('ProductFormModalController', ProductFormModalController);

    ProductFormModalController.$inject = ['$scope', '$modalInstance', '$location'];

    function ProductFormModalController($scope, $modalInstance, $location) {

        $scope.ok = function () {
            $modalInstance.close($scope.product);
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
        $scope.addProductToCart = function (product) {
            if ($scope.Global.isAuthenticated == true) {
                $scope.Commands.addProductToCart(product);
            }
            else {
                $modalInstance.dismiss('cancel');
                $location.path('/Account/Login');
            }
        };
        $scope.deleteProductsFromCart = function (productId) {
            if ($scope.Global.isAuthenticated == true) {
                $scope.Commands.deleteProductsFromCart(productId);
            } else {
                $modalInstance.dismiss('cancel');
                $location.path('/Account/Login');
            }
        };

        $scope.redirectToLogin = function () {
            $modalInstance.dismiss('cancel');
            $location.path('/Account/Login');
        };

        $scope.isInCart = function (productId) {
            return isPorductExistInCart(productId);
        };

        var isPorductExistInCart = function (productId) {
            var isExist = false;
            for (var i = 0; i < $scope.cart.products.length; i++) {
                if ($scope.cart.products[i].productId == productId) {
                    isExist = true;
                    break;
                }
            }
            return isExist;
        };
    }
})();