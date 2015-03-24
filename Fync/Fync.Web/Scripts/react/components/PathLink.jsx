define('react/components/pathLink', ['react', 'react/actions/actions'], function(react, actions) {
    return react.createClass({
        render: function () {
            return <a className='pathDirectory' onClick={this._click} href={this._generateUrl(this.props.directory)}>
                {this.props.directory.name}
            </a>
        },
        _click: function(e){
            if(e.nativeEvent.which == 2) return; //middle click
            e.preventDefault();

            actions.navigateToFolder(this.props.directory.id);
        },
        _generateUrl: function (directory) {
            var components = this._generateUrlComponents(directory);
            return '/' + components.join('/');
        },
        _generateUrlComponents: function (directory) {
            if (directory == null) return [];

            return this._generateUrlComponents(directory.parent).concat(directory.name)
        }
    });
});