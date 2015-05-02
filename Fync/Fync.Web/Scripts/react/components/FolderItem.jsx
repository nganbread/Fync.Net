define('react/components/folderItem', ['react', 'react/actions/actions'], function(react, actions){
    return react.createClass({
        render: function(){
            return <li className="folder">
                <div className="icon mdi-file-folder-open">
                </div>
                <div className="name">
                    <a onClick={this._click} href={this.props.parentName + "/" + this.props.folder.name}>{this.props.folder.name}</a>
                </div>
                <div className="details">

                </div>
            </li>
        },
        _click: function(e){
            if(e.nativeEvent.which == 2) return; //middle click
            e.preventDefault();
            actions.navigateToFolder(this.props.folder.id);
        }
    });
});