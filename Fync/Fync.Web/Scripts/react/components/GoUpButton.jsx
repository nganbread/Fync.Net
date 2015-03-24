define('react/components/backButton', ['react', 'react/stores/folderStore', 'react/actions/actions'], function(react, folderStore, actions){
    return react.createClass({
        render: function(){
            return <a href={this._generateUrl(this.props.folder)} className="backButton" onClick={this._click}>
                <div className={"icon " + this._class()}/>
            </a>
        },
        _class: function(){
            return this.props.folder
                ? 'mdi-image-navigate-before'
                : 'mdi-hardware-keyboard-control';
        },
        _click: function(e){
            if(e.nativeEvent.which == 2) return; //middle click
            e.preventDefault();
            actions.goUp();
        },
        _generateUrl: function (directory) {
            if(!directory) return null;
            var components = this._generateUrlComponents(directory);
            return '/' + components.join('/');
        },
        _generateUrlComponents: function (directory) {
            if (directory == null) return [];

            return this._generateUrlComponents(directory.parent).concat(directory.name)
        }
    })
})