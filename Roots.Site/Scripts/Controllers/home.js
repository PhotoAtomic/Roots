

function HomeController($scope) {
    
    $scope.items = [];   

    var notifier = $.connection.notificationsHub;

    notifier.client.allItems = function (fileItems) {
        $scope.$apply(function () {
            $scope.items = fileItems;
            $.each($scope.items, function () {
                $scope.Fill(this);
            });
        });
    }

    notifier.client.itemAdded = function (item) {
        $scope.$apply(function () {
            $scope.items.push(item);
            $scope.Fill(item);
        });
    }

    notifier.client.itemRenamed = function (id, newName) {
        $scope.$apply(function () {
            var item = $linq($scope.items).single(function (x) { return x.Id == id });
            item.Name = newName;
        });
    }

    notifier.client.itemRemoved = function (id) {
        $scope.$apply(function () {            
            $scope.items = $linq($scope.items).where(function (x) { return x.Id != id}).toArray();
        });
    }
    $scope.Fill = function (item) {
        if (item.MimeType == "chemical/x-mdl-sdf") {
            var moleculeText = atob(item.Content);//ToString(item.Content.split(""));
            marvin.onReady(function () {
                var imgData = marvin.ImageExporter.molToDataUrl(moleculeText);
                item.image = imgData;
            });
        }
    };

    $.connection.hub.start();

    
}


function ToString(array) {
    return String.fromCharCode.apply(String, array);
}