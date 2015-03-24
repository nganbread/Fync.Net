define('react/services/history', ['react/stores/folderStore', 'react/services/url', 'react/actions/actions'], function (folderStore, url, actions) {
    return {
        start: function () {
            folderStore.listen(function (meta) {
                var folder = folderStore.getFolder();
                if (!meta || ! meta.isHistoryNavigation) {
                    history.pushState({ id: folder.id }, folder.name, url.generate(folder));
                }
            });

            window.onpopstate = function (e) {
                if (e.state) {
                    actions.navigateToFolder(e.state.id, {isHistoryNavigation: true});
                }
            };
        }
    }
});