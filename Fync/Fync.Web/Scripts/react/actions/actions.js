define('react/actions/actions', ['react/dispatcher/dispatcher', 'react/actions/actionTypes'], function(dispatcher, actionType) {
    return {
        navigateToFolder: function (id, meta) {
            dispatcher.dispatch({
                actionType: actionType.navigateToFolder,
                id: id,
                meta: meta
            });
        },
        goUp : function(meta){
            dispatcher.dispatch({
                actionType: actionType.goUp,
                meta: meta
            });
        },
        addNewFiles : function(files, meta){
            dispatcher.dispatch({
                actionType: actionType.addNewFiles,
                files: files,
                meta: meta
            })
        }
    }
});
