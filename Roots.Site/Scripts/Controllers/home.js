

function HomeController($scope) {
    
    $scope.items = [];

    $scope.browser = [{
        id : "a1",
        label : "title",
        children: [
            {
                id: "a2",
                label: "title 2",
                children: []
            },
            {
                id: "a3",
                label: "title 3",
                children: []
            },
        ]
    }];

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


    notifier.client.itemUpdated = function (item) {
        $scope.$apply(function () {
            var itemToUpdate = $linq($scope.items).single(function (x) { return x.Id == item.Id });
            itemToUpdate.Content = item.Content;
            itemToUpdate.MimeType = item.MimeType;
            $scope.Fill(itemToUpdate);
        });
    }


    $scope.Fill = function (item) {
        if (item.MimeType == MimeTypes.Chemical ) {
            var moleculeText = atob(item.Content);
            marvin.onReady(function () {
                var imgData = marvin.ImageExporter.molToDataUrl(moleculeText);
                item.image = imgData;
            });
        }
    };

    $.connection.hub.start();

    
}