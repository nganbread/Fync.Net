define('react/components/newFileItem', ['react', 'react/stores/newFileStore'], function (react, newFileStore) {
    return react.createClass({
        render: function () {
            //file.name
            //file.type
            //file.size
            //file.lastModifiedDate
            return <li>
                <div className="icon mdi-file-cloud-upload"></div>
                <span className="file-name">{this.props.file.name}</span>
            </li>
        },
        _remove: function () {
            newFileStore.removeNewFile(this.props.file);
        }
    });
});