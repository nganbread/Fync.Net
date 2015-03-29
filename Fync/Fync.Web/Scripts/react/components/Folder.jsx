define('react/components/folder', ['react', 'react/components/fileItem', 'react/components/folderItem', 'react/stores/folderStore', 'react/components/backButton'], function(react, FileItem, FolderItem, folderStore, GoUpButton){
    return react.createClass({
		getInitialState: function(){
			return folderStore.getFolder();
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
        _renderFiles: function() {
            return this.state.files.map(file => <FileItem file={file} parentId={this.state.id}/> );
        },
        _renderFolders: function() {
            return this.state.folders.map(folder => <FolderItem folder={folder} parentName={this.state.name}/> );
        },
        render: function() {
            return <ol className="file-list">
                <li className="column-headers">
                    <div>
                        <GoUpButton folder={this.state.parent}/>
                    </div>
                    <div className="name">
                        Name
                    </div>
                    <div className="details">
                        Date
                    </div>
                </li>
                {this._renderFolders()}
                {this._renderFiles()}
            </ol>
        }
    });
});
