define('react/components/app', ['react', 'react/components/fileDropRegion', 'react/components/pathPanel', 'react/components/folder', 'react/stores/folderStore', 'react/components/newFilesPanel', 'react/components/topPanel'], function (react, FileDropRegion, PathPanel, Folder, folderStore, NewFilesPanel, TopPanel) {

    return react.createClass({
        componentWillMount: function () {
            folderStore.setFolder(this.props.folder);
        },
        render: function () {
            return <FileDropRegion>
                <div className='full-flex-row'>
                    <div className='full-flex-row'>
                        <div className='content-panel'>
                            <div style={{width:'100%'}}>
                                <div className='header full-flex-row'>
                                    <div className='content-panel'>
                                        <div className='left-panel'>
                                            <span className="icon mdi-file-cloud-done"></span>
                                        </div>
                                        <div className='content-panel z-depth-1'>
                                            <TopPanel/>
                                        </div>
                                    </div>
                                </div>
                                <div className='body full-flex-row'>
                                    <div className='content-panel'>
                                        <div className='left-panel z-depth-2'>

                                        </div>
                                        <div className='content-panel'>
                                            <div style={{width:'100%'}}>
                                                <PathPanel/>
                                                <Folder/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <NewFilesPanel />
                        </div>
                    </div>
                    <div className='full-flex-row footer'>
                    </div>
                </div>
            </FileDropRegion>
        }
    });
});