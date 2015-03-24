define('react/services/pageTitle', ['react/stores/folderStore', 'jquery'], function (folderStore, $) {
    return {
        start: function () {
            folderStore.listen(function () {
                var folder = folderStore.getFolder();
                document.title = folder.name || "Fync";
            });
        }
    }
});