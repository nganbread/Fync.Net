define('react/components/backButton', ['react', 'react/stores/folderStore', 'react/actions/actions', 'react/services/url'], function(react, folderStore, actions, url){
    return react.createClass({
        render: function(){
            return <a href={url.generate(this.props.folder)} className="backButton" onClick={this._click}>
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
        }
    })
})