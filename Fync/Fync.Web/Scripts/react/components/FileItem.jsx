define('react/components/fileItem', ['react', 'time'], function(react, time){
	return react.createClass({
			render: function(){
				return <li>
					<div className="icon mdi-editor-insert-drive-file"></div>
					<div className="name">
						<a href={this._getUrl()}>{this.props.file.name}</a>
					</div>
					<div className="details">
                        <a href={this._getUrl()}>{this._getDate()}</a>
					</div>
				</li>
			},
            _getUrl: function(){
                return "/Data?id="+this.props.parentId+"&fileName="+this.props.file.name;
            },
            _getDate: function(){
                return time(this.props.file.dateCreatedUtc).format('h:mma DD/MM/YY')
            }
		});
});