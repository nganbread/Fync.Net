define('react/components/fileItem', ['react'], function(react){
	return react.createClass({
			displayName: 'FileItem',
			render: function(){
				return <li>
					<div className="icon mdi-editor-insert-drive-file"></div>
					<div className="name">
						<a href={"/Data?id="+this.props.parentId+"&fileName="+this.props.file.name}>{this.props.file.name}</a>
					</div>
					<div className="details">
						{this.props.file.dateCreatedUtc}
					</div>
				</li>
			}
		});
});