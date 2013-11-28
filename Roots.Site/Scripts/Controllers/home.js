

function HomeController($scope, $http) {
    
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
                this.loaded = false;
                $scope.Fill(this,$http);
            });
        });
    }

    notifier.client.itemAdded = function (item) {
        $scope.$apply(function () {
            $scope.items.push(item);
            item.loaded = false;
            $scope.Fill(item,$http);
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
            //itemToUpdate.Content = item.Content;
            itemToUpdate.MimeType = item.MimeType;
            itemToUpdate.loaded = false;
            $scope.Fill(itemToUpdate,$http);
        });
    }


    $scope.Fill = function (item,$http) {
        if (item.MimeType == MimeTypes.Chemical ) {            
            marvin.onReady(function () {
                $http({ method: "GET", url: "/api/preview/" + item.Id })
                .success(function (data, status, headers, config) {                    
                    var imgData = marvin.ImageExporter.molToDataUrl(data);
                    item.image = imgData;
                    item.loaded = true;
                });                
            });
        }
        else if (item.MimeType == MimeTypes.ImageJPG) {
            item.image = "/api/preview/" + item.Id + "/?w=200&h=200";
        }
    };

    $.connection.hub.start({ transport: [/*'webSockets',*/ 'forverFrame', 'serverSentEvents', 'longPolling'] });

    
}