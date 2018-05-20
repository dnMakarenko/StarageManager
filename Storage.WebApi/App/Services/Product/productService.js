(function () {
    'use strict';

    angular
        .module('StorageManager')
        .factory('ProductService', ProductService);

    ProductService.$inject = ['$resource', '$q', '$http'];

    function ProductService($resource, $q, $http) {
        var resource = $resource('/api/Products/:action/:param', { action: '@action', param: '@param' }, {
            'update': { method: 'PUT' }
        });

        var _getProducts = function () {
            showLoadingAnim();
            var deferred = $q.defer();
            resource.query({ action: "get", param: "" },
                function (result) {
                    if (result == null) {
                        result = [];
                    };
                    deferred.resolve(result);
                },
                function (response) {
                    deferred.reject(response);
                });
            hideLoadingAnim();
            return deferred.promise;
        };

        var _getProductById = function (productId) {
            showLoadingAnim();
            var deferred = $q.defer();
            resource.query({ action: 'ById', param: productId },
                function (result) {
                    if (result == null) {
                        result = [];
                    };

                    deferred.resolve(result);
                },
                function (response) {
                    deferred.reject(response);
                });
            hideLoadingAnim();
            return deferred.promise;
        };

        var _createProduct = function (productDto) {
            var deferred = $q.defer();

            $http.post('/api/Products', productDto)
                .then(function (result) {
                    deferred.resolve(result);
                },
                function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        };

        var _updateProduct = function (contactDto) {
            var deferred = $q.defer();

            $http.put('/api/Products/' + contactDto.id, contactDto)
                .then(function (result) {
                    deferred.resolve(result);
                },
                function (response) {
                    deferred.reject(response);
                });

            return deferred.promise;
        };

        var _deleteProduct = function (contactId) {
            var deferred = $q.defer();

            resource.delete({ action: "", param: contactId },
                function (result) {
                    if (result == null) {
                        result = [];

                    };
                    deferred.resolve(result);
                },
                function (response) {
                    deferred.reject(response);
                });
            return deferred.promise;
        };

        return {
            getProducts: _getProducts,
            getProductById: _getProductById,
            createProduct: _createProduct,
            updateProduct: _updateProduct,
            deleteProduct: _deleteProduct,
        };

    }
    function hideLoadingAnim()
    {
        $("#preloader").hide();
        $("#filemanager").show();
    }
    function showLoadingAnim()
    {
        $("#filemanager").hide();
        $("#preloader").show();
    }

})();