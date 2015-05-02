define('react/components/fileDropRegion', ['react', 'react/actions/actions', 'react/stores/newFileStore'], function (react, actions, newFileStore) {
    return react.createClass({
        render: function () {
            return <div
                ref='dropRegion'
                className={
                    "drop-region " +
                    (this.state.isActive ? "active " : "") +
                    (this.state.hasNewFiles ? "has-files " : "")}>
                {this.props.children}
            </div>
        },
        _onStoreUpdated: function () {
            this.setState({
                hasNewFiles: newFileStore.getNewFiles().length > 0
            });
        },
        componentDidMount: function () {
            newFileStore.listen(this._onStoreUpdated);

            var dropRegion = react.findDOMNode(this.refs.dropRegion);
            dropRegion.addEventListener('dragover', this._dragOver, false);
            dropRegion.addEventListener('dragenter', this._dragEnter, false);
            dropRegion.addEventListener('dragleave', this._dragLeave, false);
            dropRegion.addEventListener('drop', this._drop, false);
        },
        componentWillUnmount: function () {
            newFileStore.stop(this._onStoreUpdated);

            var dropRegion = react.findDOMNode(this.refs.dropRegion);
            dropRegion.removeEventListener('dragenter', this._dragEnter);
            dropRegion.removeEventListener('dragleave', this._dragLeave);
            dropRegion.removeEventListener('drop', this._drop);
        },
        _count: 0,
        _dragOver: function(e){
            e.stopPropagation();
            e.preventDefault();
            //need this for drop to work
        },
        _dragEnter: function (e) {
            e.stopPropagation();
            e.preventDefault();
            this._count++;
            this.setState({isActive: true});
        },
        _drop: function (e) {
            e.stopPropagation();
            e.preventDefault();
            this.setState({isActive: false});

            var files = [];
            for (var i = 0, f; f = e.dataTransfer.files[i]; i++) {
                files.push(f)
            }
            actions.addNewFiles(files);
        },
        _dragLeave: function (e) {
            e.stopPropagation();
            e.preventDefault();
            this._count--;
            if(this._count == 0){
                this.setState({isActive: false});
            }
        },
		getInitialState: function(){
			return {
				isActive: false,
                files: newFileStore.getNewFiles().length > 0
            };
		}
    });
});