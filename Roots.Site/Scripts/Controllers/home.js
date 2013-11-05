

function HomeController($scope) {

    //var i = 0;
    $scope.items = [];
    //$scope.timer = function timer()
    //{
    //    $scope.$apply(function () { $scope.items.push(i++) });
    //    setTimeout(timer, 1000);
    //};

    //$scope.timer();



    var notifier = $.connection.notificationsHub;


    notifier.client.allItems = function (fileNames) {
        $scope.$apply(function () {
            $scope.items = fileNames;
        });
    }

    notifier.client.itemAdded = function (fileName) {
        $scope.$apply(function () {
            $scope.items.push(fileName);
        })
    };

    $.connection.hub.start();
}

