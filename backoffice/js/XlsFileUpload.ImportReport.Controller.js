'use strict';

angular.module("umbraco").controller("XlsFileUploadImportReportController",
    function ($scope, $routeParams, dialogService, notificationsService, navigationService, eventsService, XlsFileUploadResources) {

        $scope.sciaIsLoading = true;
        $scope.sciaReverse = false;
        $scope.sciaPredicate = 'scia.Id';
        $scope.sciaPageSizeList = [10, 20, 50, 100, 200, 500];
        $scope.sciaItemsPerPage = $scope.sciaPageSizeList[1];

        function fetchAuditData() {
            
           XlsFileUploadResources.getFileImportDateList($scope.sciaItemsPerPage, $scope.sciaCurrentPage, $scope.sciaPredicate, $scope.sciaReverse ? "desc" : "asc").then(function (response) {

                $scope.sciaData = response.FileImportAuditItems;
                $scope.sciaTotalPages = response.TotalPages;
                $scope.sciaCurrentPage = response.CurrentPage;
                $scope.sciaItemCount = $scope.sciaData.length;
                $scope.sciaTotalItems = response.TotalItems;
                $scope.sciaRangeTo = ($scope.sciaItemsPerPage * ($scope.sciaCurrentPage - 1)) + $scope.sciaItemCount;
                $scope.sciaRangeFrom = ($scope.sciaRangeTo - $scope.sciaItemCount) + 1;

                $scope.sciaIsLoading = false;

            }, function (response) {
                notificationsService.error("Error", "Could not load log data.");
            });
        }

        $scope.sciaOrder = function (predicate) {
            $scope.sciaReverse = ($scope.sciaPredicate === predicate) ? !$scope.sciaReverse : false;
            $scope.sciaPredicate = predicate;
            fetchAuditData();
        };

        $scope.sciaPrevPage = function () {
            if ($scope.sciaCurrentPage > 1) {
                $scope.sciaCurrentPage--;
                fetchAuditData();
            }
        };

        $scope.sciaNextPage = function () {
            if ($scope.sciaCurrentPage < $scope.sciaTotalPages) {
                $scope.sciaCurrentPage++;
                fetchAuditData();
            }
        };
        $scope.auditItemsChange = function () {
            $scope.sciaCurrentPage = 1;
            fetchAuditData();
        };

        $scope.sciaSetPage = function (pageNumber) {
            $scope.sciaCurrentPage = pageNumber;
            fetchAuditData();
        };

        fetchAuditData();
       
    });
