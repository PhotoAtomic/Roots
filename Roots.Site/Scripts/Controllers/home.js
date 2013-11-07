

function HomeController($scope) {
    
    $scope.items = [];   

    var notifier = $.connection.notificationsHub;

    notifier.client.allItems = function (fileNames) {
        $scope.$apply(function () {
            $scope.items = fileNames;
        });
    }

    notifier.client.itemAdded = function (fileName) {
        $scope.$apply(function () {
            $scope.items.push(fileName);
        });
    }

    notifier.client.itemRenamed = function (oldName, newName) {
        $scope.$apply(function () {
            var i = $scope.items.indexOf(oldName);
            $scope.items[i] = newName;
        });
    }

    $.connection.hub.start();
}

