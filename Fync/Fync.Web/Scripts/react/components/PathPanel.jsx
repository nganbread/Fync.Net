define('react/components/pathPanel', ['react', 'react/components/pathLink', 'react/stores/folderStore'], function(react, PathLink, folderStore){
    return react.createClass({
        getInitialState: function(){
            return this.props;
        },
        renderPathDirectory: function(directory){
            if(directory == null) return [];

            var pathDirectory = <PathLink directory={directory}/>;
            var items = directory.parent == null
                ? pathDirectory
                : [<span>/</span>, pathDirectory];

            return this.renderPathDirectory(directory.parent).concat(items);
        },
        componentDidMount: function() {
            folderStore.listen(this._onStoreUpdated);
        },

        componentWillUnmount: function() {
            folderStore.stop(this._onStoreUpdated);
        },
        _onStoreUpdated: function(){
            this.setState(folderStore.getFolder());
        },
        render: function() {
            return <h1>{this.renderPathDirectory(this.state)}</h1>
        }
    });
});
