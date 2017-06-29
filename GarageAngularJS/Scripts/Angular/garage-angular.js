(function () {
    var app = angular.module("Garage", []);

    app.controller("MainController", ["$scope", "$http", function ($scope, $http) {

        $scope.getData = getData;

        function getData() {
            $http.get('/api/values/get')
                .then(onRepos) 
        };

        var onRepos = function (response) {
            $scope.vehicles = response.data;
        };

        $scope.orderByMe = function (vehicle) {
            $scope.myOrderBy = vehicle;
        };

    }]);


}());