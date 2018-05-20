(function () {
    'use strict';

    angular
        .module('StorageManager')
        .controller('CartController', CartController);

    CartController.$inject = ['$scope', '$q', 'CartService', 'errorHandler', '$modal'];

    function CartController($scope, $q, CartService, errorHandler, $modal) {
        (function startup() {
            var cartProducts = CartService.getCart();

            $q.all([cartProducts]).then(function (data) {
                if (data != null) {
                    $scope.cart = data[0].data;
                }
            }, function (reason) {
                errorHandler.logServiceError('CartController', reason);
            }, function (update) {
                errorHandler.logServiceNotify('CartController', update);
            });
        })();

        //$scope.cart = {};

        function getCart() {
            CartService.getCart().then(
                function (result) {
                    $scope.cart = result.data;
                    $scope.Global.ShoppingCart = result.data;
                },
                function (response) {
                    console.log(response);
                });
        };

        function clearCart() {
            CartService.clearCart().then(
                function (result) {
                    $scope.cart = result.data;
                    $scope.Global.ShoppingCart = result.data;
                },
                function (response) {
                    console.log(response);
                });
        };

        function addProductToCart(productDto) {
            CartService.addProductToCart(productDto).then(
                function (result) {
                    $scope.cart = result.data;
                    $scope.Global.ShoppingCart = result.data;
                },
                function (response) {

                    console.log(response);
                });
        };

        function deleteProductFromCart(productId) {
            CartService.deleteProductFromCart(productId).then(
                function (result) {
                    $scope.cart = result.data;
                    $scope.Global.ShoppingCart = result.data;
                },
                function (response) {
                    console.log(response);
                });
        };

        function deleteProductsFromCart(productId) {
            CartService.deleteProductsFromCart(productId).then(
                function (result) {
                    $scope.cart = result.data;
                    $scope.Global.ShoppingCart = result.data;
                },
                function (response) {
                    console.log(response);
                });
        };

        $scope.Commands = {
            getCart: function () {
                getCart();
            },
            clearCart: function () {
                clearCart();
            },
            addProductToCart: function (productDto) {
                addProductToCart(productDto);
            },
            deleteProductFromCart: function (productId) {
                deleteProductFromCart(productId);
            },
            deleteProductsFromCart: function (productId) {
                deleteProductsFromCart(productId);
            }
        };
    }
})();