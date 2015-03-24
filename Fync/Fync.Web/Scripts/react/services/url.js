define('react/services/url', ['react/stores/folderStore'], function (folderStore) {

    var generateUrlComponents = function (directory) {
        if (directory == null) return [];

        return generateUrlComponents(directory.parent).concat(directory.name)
    }

    return {
        generate: function(directory) {
            var components = generateUrlComponents(directory);
            return '/' + components.join('/');
        }
    };
});