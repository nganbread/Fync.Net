define('react/components/pathDirectory', ['react'], function(react) {
    return react.createClass({
        displayName: 'PathDirectory',
        render: function () {
            return <a href={this.generateUrl(this.props.directory)}>{this.props.directory.name}</a>
        },
        generateUrl: function (directory) {
            var components = this.generateUrlComponents(directory);
            return '/' + components.join('/');
        },
        generateUrlComponents: function (directory) {
            if (directory == null) return [];

            return this.generateUrlComponents(directory.parent).concat(directory.name)
        }
    });
});