'use strict';
angular.module("umbraco")
    .controller("XlsFileUploadDashboardController",
        function ($scope, $routeParams, dialogService, notificationsService, navigationService, eventsService, XlsFileUploadResources, userService, $http) {

            $scope.isLoading = true;
            $scope.validMessage = false;
            $scope.reverse = false;
            $scope.predicate = 'sci.CompanyName';
            $scope.pageSizeList = [10, 20, 50, 100, 200, 500];
            $scope.itemsPerPage = $scope.pageSizeList[1];
           
            function fetchData() {

                XlsFileUploadResources.getInventoryList($scope.itemsPerPage, $scope.currentPage, $scope.predicate, $scope.reverse ? "desc" : "asc").then(function (response) {

                    $scope.sciData = response.CompanyInventoryItems;
                    $scope.totalPages = response.TotalPages;
                    $scope.currentPage = response.CurrentPage;
                    $scope.itemCount = $scope.sciData.length;
                    $scope.totalItems = response.TotalItems;
                    $scope.rangeTo = ($scope.itemsPerPage * ($scope.currentPage - 1)) + $scope.itemCount;
                    $scope.rangeFrom = ($scope.rangeTo - $scope.itemCount) + 1;

                    $scope.isLoading = false;

                }, function (response) {
                    notificationsService.error("Error", "Could not load log data.");
                });
            }


            $scope.order = function (predicate) {
                $scope.reverse = $scope.predicate === predicate ? !$scope.reverse : false;
                $scope.predicate = predicate;
                fetchData();
            };

            $scope.prevPage = function () {
                if ($scope.currentPage > 1) {
                    $scope.currentPage--;
                    fetchData();
                }
            };

            $scope.nextPage = function () {
                if ($scope.currentPage < $scope.totalPages) {
                    $scope.currentPage++;
                    fetchData();
                }
            };

            $scope.setPage = function (pageNumber) {
                $scope.currentPage = pageNumber;
                fetchData();
            };
            $scope.itemsPerPageChange = function () {
                $scope.currentPage = 1;
                fetchData();
            };

            $scope.currUserId = 0;
            $scope.currUserName = "admin";
            function getUserData() {
                userService.getCurrentUser().then(function (user) {
                    $scope.currUserId = user.id;
                    $scope.currUserName = user.email;
                });
            }

            $scope.xlsFileUpload = function () {
                getUserData();
           
                $scope.showImportMessage = true;

                $scope.isLoading = true;
                $http({
                    method: 'POST',
                    url: '/umbraco/api/XlsFileUploadApi/SaveData',
                    headers: { 'Content-Type': false },
                    transformRequest: function (data) {
                        var formData = new FormData();
                        formData.append("UploadedFile", data.file[0]);
                        formData.append("CurrentUserId", $scope.currUserId);
                        formData.append("CurrentUserName", $scope.currUserName);
                        return formData;
                    },
                    data: { file: document.getElementsByName("xlsFile")[0].files }
                })
                    .success(function (data, status, headers, config) {
                        $scope.importMessage = data;
                        document.querySelector("div[id=importMessage]").innerText = data;
                        document.querySelector("input[name=xlsFile]").value ="";
                        fetchData();
                    })
                    .error(function (data, status, headers, config) {
                        $scope.importMessage = data;
                        document.querySelector("div[id=importMessage]").innerText = data;
                        document.querySelector("input[name=xlsFile]").value = "";
                        fetchData();
                    });

                fetchData();
            };

            fetchData();
        });
