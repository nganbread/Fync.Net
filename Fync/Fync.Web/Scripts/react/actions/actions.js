define('react/actions/actions', ['react/dispatcher/dispatcher', 'react/actions/actionTypes'], function(dispatcher, actionType) {
    return {
        navigateToFolder: function (id) {
            dispatcher.dispatch({
                actionType: actionType.navigateToFolder,
                id: id
            });
        },
        goUp : function(){
            dispatcher.dispatch({
               actionType: actionType.goUp
            });
        }
    }
});
