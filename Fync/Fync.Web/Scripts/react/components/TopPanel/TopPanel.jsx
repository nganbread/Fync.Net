define('react/components/topPanel', ['react', 'react/stores/activeFileStore', 'react/components/fileActions'], function(react, activeFileStore, FileActions){
    return react.createClass({
        render: function(){
            if(this.state.file != null){
                return <FileActions file={this.state.file} />
            }else{
                return <div></div>
            }
        },
        getInitialState: function () {
            return {
                file: activeFileStore.getActiveFile()
            };
        },
        componentDidMount: function () {
            activeFileStore.listen(this._onStoreUpdated);
        },
        componentWillUnmount: function () {
            activeFileStore.stop(this._onStoreUpdated);
        },
        _onStoreUpdated: function () {
            this.setState({
                file: activeFileStore.getActiveFile()
            });
        }
    });
});