define('react/stores/folderStore', ['jquery', 'react/dispatcher/dispatcher', 'react/actions/actionTypes', 'configuration'], function($, dispatcher, actionType, configuration) {
    var listeners = {};
    var folder = null;
    var triggerChange = function() {
        for (var key in listeners) {
            listeners[key]();
        };
    };

    dispatcher.register(function (action) {
        switch(action.actionType) {
            case actionType.retrieveFolder:
                $.get(configuration.apiUri + 'folder?id=' + action.id,
                    function(data) {
                        folder = data;
                        triggerChange();
                    });
                break;
        }
    });

    return {
        listen: function(callback) {
            listeners[callback] = callback;
        },
        stop: function(callback) {
            delete listeners[callback];
        },
        getFolder: function() {
            return folder;
        }
    };
});

