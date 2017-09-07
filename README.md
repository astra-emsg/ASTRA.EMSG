EMSG - Erhaltungsmanagement im Siedlungsgebiet
 
Install:
	1.	Get the latest version of the EMSG application
	2.	Create a new database, called "emsg-local"
	3.	Run the following scripts from the Scripts folder
		a.	Initialize the database
		b.	Create new Mandant
		c.	Create new User

	4.	Restore Kendo
	5.	Search for #TODO# in the Web.config (ASTRA.EMSG.Web), and replace them
		a.	For the GeoServer url (WMS_Url_Development) insert the file path like you see in the comment)
		b.	For the MapProxy url (SWMS_AV_Url_EMSG) insert the file path like you see in the comment)
	6.	Deployment > no special things have to be considered
	7.	Start the application


Optional: Installation of "EMSG-Mobile" rich client
	8.	There is a project called Mobile.Setup. This project is responsible to create an installer for the EMSG-Mobile product.
		To be able to build it, you need to install the corresponding extension for your Visual Studio version:
		a.	VS2010 - The Setup project type natively supported 
		b.	VS2012 - The Setup project type is not supported, the EMSG Mobile installer cannot be built with VS2012
		c.	VS2013 - Install Microsoft Visual Studio 2013 Installer Projects https://visualstudiogallery.msdn.microsoft.com/9abe329c-9bba-44a1-be59-0fbf6151054d?SRC=VSIDE
		d.	VS2015 - Install Microsoft Visual Studio 2015 Installer Projects  https://visualstudiogallery.msdn.microsoft.com/f1cc3f3e-c300-40a7-8797-c509fb8933b9
		e.	VS2017 - Install Microsoft Visual Studio 2017 Installer Projects https://marketplace.visualstudio.com/items?itemName=VisualStudioProductTeam.MicrosoftVisualStudio2017InstallerProjects
