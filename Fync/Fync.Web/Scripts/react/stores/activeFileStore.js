define('react/stores/activeFileStore', ['jquery', 'react/dispatcher/dispatcher', 'react/actions/actionTypes', 'configuration'], function($, dispatcher, actionType, configuration) {
    var listeners = [];
    var activeFile = null;

    var triggerChange = function(meta) {
        for (var key in listeners) {
            listeners[key](meta);
        };
    };

    dispatcher.register(function (action) {
        switch(action.actionType) {
            case actionType.toggleActiveFile:
                if(activeFile == action.file){
                    activeFile = null;
                }else{
                    activeFile = action.file;
                }
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
        getActiveFile: function(){
            return activeFile;
        }
    };
});

