'use strict';
angular.module('umbraco.resources').factory('XlsFileUploadResources', function ($q, $http, umbRequestHelper) {
    
    var basePath = "Backoffice/XlsFileUpload/XlsFileUpload/";

    return {
        getInventoryList: function (itemsPerPage, pageNumber, sortColumn, sortOrder) {
            return umbRequestHelper.resourcePromise(
                $http.get(basePath + "GetInventoryList", { params: { itemsPerPage: itemsPerPage, pageNumber: pageNumber, sortColumn: sortColumn, sortOrder: sortOrder } })
            );
        },
        getFileImportDateList: function (itemsPerPage, pageNumber, sortColumn, sortOrder) {
            return umbRequestHelper.resourcePromise(
                $http.get(basePath + "GetFileImportDateList", { params: { itemsPerPage: itemsPerPage, pageNumber: pageNumber, sortColumn: sortColumn, sortOrder: sortOrder } })
            );
        },
        save: function (xlsFile) {
            return $http.post(basePath + "SaveData", angular.toJson(xlsFile));
        }
    };
});
