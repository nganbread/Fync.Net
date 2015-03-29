define('react/stores/newFileStore', ['jquery', 'react/dispatcher/dispatcher', 'react/actions/actionTypes', 'configuration'], function($, dispatcher, actionType, configuration) {
    var listeners = [];
    var newFiles = [];
    var showNewFiles = false;
    var triggerChange = function(meta) {
        for (var key in listeners) {
            listeners[key](meta);
        };
    };

    dispatcher.register(function (action) {
        switch(action.actionType) {
            case actionType.addNewFiles:
                newFiles = newFiles.concat(action.files);
                triggerChange(action.meta);
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
        getNewFiles: function(){
            return newFiles;
        }
    };
});

