define('react/stores/folderStore', ['jquery', 'react/dispatcher/dispatcher', 'react/actions/actionTypes', 'configuration'], function($, dispatcher, actionType, configuration) {
    var listeners = [];
    var folder = null;
    var triggerChange = function() {
        for (var key in listeners) {
            listeners[key]();
        };
    };

    var navigateTo = function(id){
        $.get(configuration.apiUri + 'fync?id=' + id,
            function(data) {
                folder = data;
                triggerChange();
            });
    };

    dispatcher.register(function (action) {
        switch(action.actionType) {
            case actionType.navigateToFolder:
                navigateTo(action.id)
                break;
            case actionType.goUp:
                navigateTo(folder.parent.id)
                break;
        }
    });

    return {
        listen: function(callback) {
            listeners.push(callback);
        },
        stop: function(callback) {
            var index = listeners.indexOf(callback);
            if(index != -1){
                listeners.splice(index, 1);
            }
        },
        getFolder: function() {
            return folder;
        }
    };
});

