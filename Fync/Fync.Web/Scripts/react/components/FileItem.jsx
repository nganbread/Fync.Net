define('react/components/fileItem', ['react', 'time', 'react/actions/actions','react/stores/activeFileStore'], function(react, time, actions, activeFileStore){
	return react.createClass({
			render: function(){
				return <li onClick={this._onClick} className={"file " + (this.state.isActive ? 'active' : '')}>
					<div className="icon mdi-editor-insert-drive-file"></div>
					<div className="name">
						<a href={this._getUrl()}>{this.props.file.name}</a>
					</div>
					<div className="details">
                        {this._getDate()}
					</div>
				</li>
			},
            _onClick: function(e){
                actions.toggleActiveFile(this.props.file);
            },
            _getUrl: function(){
                return "/Data?id="+this.props.parentId+"&fileName="+this.props.file.name;
            },
            _getDate: function(){
                return time(this.props.file.dateCreatedUtc).format('h:mma DD/MM/YY')
            },
            getInitialState: function () {
                return {
                    isActive: activeFileStore.getActiveFile() == this.props.file
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
                    isActive: activeFileStore.getActiveFile() == this.props.file
                });
            }
		});
});