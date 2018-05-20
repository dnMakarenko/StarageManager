(function () {
    'use strict';

    angular
        .module('StorageManager')
        .factory('CartService', CartService);

    CartService.$inject = ['$resource', '$q', '$http'];

    function CartService($resource, $q, $http) {
        var resource = $resource('/api/Basket/:action/:param', { action: '@action', param: '@param' }, {
            'update': { method: 'PUT' }
        });

        var _getCart = function () {
            var deferred = $q.defer();
            $http({ method: 'GET', url: '/api/Basket' })
                .then(function (result) {
                    deferred.resolve(result);
                },
                function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        };

        var _clearCart = function () {
            var deferred = $q.defer();
            $http({ method: 'GET', url: '/api/Basket/Clear' })
                .then(function (result) {
                    deferred.resolve(result);
                },
                function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        };

        var _addProductToCart = function (productDto) {
            var deferred = $q.defer();

            $http.post('/api/Basket', productDto)
                .then(function (result) {
                    deferred.resolve(result);
                },
                function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        };

        var _deleteProductFromCart = function (productId) {
            var deferred = $q.defer();

            $http({ method: 'DELETE', url: '/api/Basket/Delete/' + productId })
                .then(function (result) {
                    deferred.resolve(result);
                },
                function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        };

        var _deleteProductsFromCart = function (productId) {
            var deferred = $q.defer();

            $http({ method: 'DELETE', url: '/api/Basket/DeleteAll/' + productId })
                .then(function (result) {
                    deferred.resolve(result);
                },
                function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        };

        return {
            getCart: _getCart,
            addProductToCart: _addProductToCart,
            deleteProductFromCart: _deleteProductFromCart,
            deleteProductsFromCart: _deleteProductsFromCart,
            clearCart: _clearCart
        };
    }
})();