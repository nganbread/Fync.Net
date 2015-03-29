define('react/components/newFileItem', ['react', 'react/stores/newFileStore'], function (react, newFileStore) {
    return react.createClass({
        render: function () {
            //file.name
            //file.type
            //file.size
            //file.lastModifiedDate
            return <span>{this.props.file.name}</span>
        },
        _remove: function () {
            newFileStore.removeNewFile(this.props.file);
        }
    });
});