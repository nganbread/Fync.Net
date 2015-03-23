define('react/components/pathPanel', ['react', 'react/components/pathDirectory'], function(react, PathDirectory){
    return react.createClass({
        displayName: 'PathPanel',
        renderPathDirectory: function(directory){
            if(directory == null) return [];

            var pathDirectory = <PathDirectory directory={directory}/>;
            var items = directory.parent == null
                ? pathDirectory
                : [<span>/</span>, pathDirectory];

            return this.renderPathDirectory(directory.parent).concat(items);
        },
        render: function() {
            return <h1>{this.renderPathDirectory(this.props)}</h1>
        }
    });
});
