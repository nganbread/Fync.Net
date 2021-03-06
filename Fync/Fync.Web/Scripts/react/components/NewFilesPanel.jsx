define('react/components/newFilesPanel', ['react', 'react/stores/newFileStore', 'react/components/newFileItem'], function (react, newFileStore, NewFileItem) {
    return react.createClass({
        getInitialState: function () {
            return {
                files: newFileStore.getNewFiles()
            };
        },
        componentDidMount: function () {
            newFileStore.listen(this._onStoreUpdated);
        },
        componentWillUnmount: function () {
            newFileStore.stop(this._onStoreUpdated);
        },
        _onStoreUpdated: function () {
            this.setState({
                files: newFileStore.getNewFiles()
            });
        },
        render: function () {
            return <div className='new-files-panel z-depth-2 right-panel'>
				<ul className="new-file-list">
					{this.state.files.map(file => {
						return <NewFileItem file={file}/>
					})}
				</ul>
			</div>
        }
    });
});