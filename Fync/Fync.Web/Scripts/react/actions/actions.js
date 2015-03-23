define('react/actions/actions', ['react/dispatcher/dispatcher', 'react/actions/actionTypes'], function(dispatcher, actionType) {
    return {
        retrieveFolder: function (id) {
            dispatcher.dispatch({
                actionType: actionType.retrieveFolder,
                id: id
            });
        }
    }
});
