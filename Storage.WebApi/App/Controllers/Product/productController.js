(function () {
    'use strict';

    angular
        .module('StorageManager')
        .controller('ProductController', ProductController);

    ProductController.$inject = ['$scope', '$q', 'ProductService', 'CartService', 'errorHandler', '$modal', '$location'];

    function ProductController($scope, $q, ProductService, CartService, errorHandler, $modal, $location) {
        (function startup() {

            var products = ProductService.getProducts();
            
            $q.all([
                products
            ]).then(function (data) {
                if (data != null) {
                    $scope.products = data[0];
                }
            }, function (reason) {
                errorHandler.logServiceError('ProductController', reason);
            }, function (update) {
                errorHandler.logServiceNotify('ProductController', update);
            });

            if ($scope.Global.isAuthenticated == true) {
                getCart();
            }

        })();

        function getCart() {
            CartService.getCart().then(
                function (result) {
                    $scope.cart = result.data;
                },
                function (response) {
                    console.log(response);
                });
        };

        function removeProduct(productId) {
            for (var i = 0; i < $scope.products.length; i++) {
                if ($scope.products[i].id == productId) {
                    $scope.products.splice(i, 1);
                    break;
                }
            }
        };

        $scope.products = [];
        $scope.cart = {};

        $scope.Commands = {
            saveProduct: function (product) {
                ProductService.createProduct(product).then(
                    function (result) {
                        $scope.products.push(result.data);
                    },
                    function (response) {
                        console.log(response);
                    });
            },
            updateProduct: function (product) {
                ProductService.updateProduct(product).then(
                    function (result) {
                        
                    },
                    function (response) {
                        console.log(response);
                    });
            },
            getCart: function () {
                CartService.getCart().then(
                    function (result) {
                        $scope.cart = result.data;
                        $scope.Global.ShoppingCart = result.data;
                    },
                    function (response) {
                        console.log(response);
                    });
            },
            clearCart: function () {
                CartService.clearCart().then(
                    function (result) {
                        $scope.cart = result.data;
                        $scope.Global.ShoppingCart = result.data;
                    },
                    function (response) {
                        console.log(response);
                    });
            },
            addProductToCart: function (product) {
                    CartService.addProductToCart(product).then(
                        function (result) {
                            $scope.cart = result.data;
                            $scope.Global.ShoppingCart = result.data;
                            alert("Product added to Shopping Cart.");
                        },
                        function (response) {

                            console.log(response);
                        });
            },
            deleteProductFromCart: function (productId) {
                CartService.deleteProductFromCart(productId).then(
                    function (result) {
                        $scope.cart = result.data;
                        $scope.Global.ShoppingCart = result.data;
                        alert("Product removed from Shopping Cart.");
                    },
                    function (response) {
                        console.log(response);
                    });
            },
            deleteProductsFromCart: function (productId) {
                CartService.deleteProductsFromCart(productId).then(
                    function (result) {
                        $scope.cart = result.data;
                        $scope.Global.ShoppingCart = result.data;
                        alert("Product removed from Shopping Cart.");
                    },
                    function (response) {
                        console.log(response);
                    });
            }
        };

        $scope.Queries = {
            getProducts: function () {
                ProductService.getProducts();
            },
            getProductById: function (productId) {
                ProductService.getProductById(productId);
            }
        };

        $scope.Modals = {
            open: function (product) {
                $scope.product = product;

                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/App/Templates/Product/ProductFormModal.html',
                    controller: 'ProductFormModalController',
                    size: 'lg',
                    scope: $scope,
                    backdrop: 'static'
                });

                modalInstance.result.then(
                    function (product) {
                        if (product.id != null) {
                            $scope.Commands.updateProduct(product);
                        }
                        else {
                            $scope.Commands.saveProduct(product);
                        }
                    },
                    function (event) {

                    });
            },
            deleteProduct: function (productId) {
                if (confirm('Are you sure you want to delete this product?')) {
                    ProductService.deleteProduct(productId).then(
                        function (data) {
                            removeProduct(productId);
                        },
                        function (response) {
                            console.log(response);
                        });
                }
                else {
                    console.log('delete cancelled');
                }
            }
        };
    };
})
    ();