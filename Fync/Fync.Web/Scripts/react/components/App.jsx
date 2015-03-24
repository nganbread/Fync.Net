define('react/components/app', ['react', 'react/components/fileDropRegion', 'react/components/pathPanel', 'react/components/Folder'], function (react, FileDropRegion, PathPanel, Folder) {
    return react.createClass({
        render: function () {
            return <FileDropRegion>

                <PathPanel/>
                <Folder/>

            </FileDropRegion>
        }
    });
});

define('react/components/fileDropRegion', ['react', 'react/actions/actions'], function (react, actions) {
    return react.createClass({
        render: function () {
            return <div ref='dropRegion' className={this.state.isActive ? "active" : ""}>
                {this.props.children}
            </div>
        },
        componentDidMount: function () {
            var dropRegion = react.findDOMNode(this.refs.dropRegion);
            dropRegion.addEventListener('dragover', this._dragOver, false);
            dropRegion.addEventListener('dragstart', this._dragStart, false);
            dropRegion.addEventListener('dragleave', this._dragLeave, false);
            dropRegion.addEventListener('drop', this._drop, false);
        },
        componentWillUnmount: function () {
            var dropRegion = react.findDOMNode(this.refs.dropRegion);
            dropRegion.removeEventListener('dragover', this._dragOver);
            dropRegion.removeEventListener('dragstart', this._dragStart);
            dropRegion.removeEventListener('dragleave', this._dragLeave);
            dropRegion.removeEventListener('drop', this._drop);
        },
        _dragOver: function (e) {

        },
        _drop: function (e) {
            var files = e.dataTransfer.files;
            actions.addNewFiles(files);
        },
        _dragStart: function (e) {
            this.setState({isActive: true});
        },
        _dragLeave: function (e) {
            this.setState({isActive: false});
        }
    });
});

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
            return <ol>
                    {this.state.files.map(file => {
                        return <li>
                                <NewFileItem file={file}/>
                            </li>
                    })}
            </ol>
        }
    });
});

define('react/components/newFileItem', ['react', 'react/stores/newFileStore'], function (react, newFileStore) {
    return react.createClass({
        render: function () {
            //file.name
            //file.type
            //file.size
            //file.lastModifiedDate
        },
        _remove: function () {
            newFileStore.removeNewFile(this.props.file);
        }
    });
})