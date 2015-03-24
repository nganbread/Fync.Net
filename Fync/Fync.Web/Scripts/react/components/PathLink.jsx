define('react/components/pathLink', ['react', 'react/actions/actions', 'react/services/url'], function(react, actions, url) {
    return react.createClass({
        render: function () {
            return <a className='pathDirectory' onClick={this._click} href={url.generate(this.props.directory)}>
                {this.props.directory.name}
            </a>
        },
        _click: function(e){
            if(e.nativeEvent.which == 2) return; //middle click
            e.preventDefault();

            actions.navigateToFolder(this.props.directory.id);
        },
    });
});