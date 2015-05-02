define('react/components/fileActions', ['react', 'react/stores/activeFileStore'], function(react, activeFileStore){
    return react.createClass({
        render: function(){
            return <ul className="actions">
                <li><div className="icon action mdi-action-delete"></div></li>
                <li><div className="icon action mdi-content-create"></div></li>
                <li><div className="icon action mdi-content-content-cut"></div></li>
                <li><div className="icon action mdi-content-content-paste"></div></li>
                <li><div className="icon action mdi-social-share"></div></li>
            </ul>
        }
    });
});