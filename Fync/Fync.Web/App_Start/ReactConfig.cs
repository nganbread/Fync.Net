using React;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Fync.Web.ReactConfig), "Configure")]

namespace Fync.Web
{
	public static class ReactConfig
	{
		public static void Configure()
		{
			// ES6 features are enabled by default. Uncomment the below line to disable them.
			// See http://reactjs.net/guides/es6.html for more information.
			//ReactSiteConfiguration.Configuration.SetUseHarmony(false);

			// Uncomment the below line if you are using Flow
			// See http://reactjs.net/guides/flow.html for more information.
			//ReactSiteConfiguration.Configuration.SetStripTypes(true);

			// If you want to use server-side rendering of React components, 
			// add all the necessary JavaScript files here. This includes 
			// your components as well as all of their dependencies.
			// See http://reactjs.net/ for more information. Example:
		    ReactSiteConfiguration.Configuration
		        .AddScript("~/Scripts/react/components/App.jsx")
		        .AddScript("~/Scripts/react/components/FileDropRegion.jsx")
                .AddScript("~/Scripts/react/components/FileItem.jsx")
                .AddScript("~/Scripts/react/components/Folder.jsx")
		        .AddScript("~/Scripts/react/components/FolderItem.jsx")
		        .AddScript("~/Scripts/react/components/NewFilesItem.jsx")
		        .AddScript("~/Scripts/react/components/NewFilesPanel.jsx")
		        .AddScript("~/Scripts/react/components/PathLink.jsx")
                .AddScript("~/Scripts/react/components/GoUpButton.jsx")
		        .AddScript("~/Scripts/react/components/PathPanel.jsx");
		}
	}
}