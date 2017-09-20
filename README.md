# EMSG 

The application abbreviation EMSG stands for "Erhaltungsmanagement im Siedlungsgebiet", which means "Asset Management of urban Road Systems". It is a GIS system consisting of "EMSG-Master" - the main web application (C# .Net 4.5 with SQL-Server as DB and GeoServer for the GIS part), and "EMSG-Mobile" - a simple rich client (WPF .Net 4.0 C#) which allows editing the GIS and technical data in an offline mode. EMSG was originally developed by the Swiss government to support the maintenance of roads in small local municipalities. The application development was stopped in fall 2017 after EMSG was declared open source. For further technical and user documentation please check the "[documentation](/documentation)" folder of this repository.
 
## Install:
```
	1.	Get the latest version of the EMSG application
	2.	Create a new database, called "emsg-local"
	3.	Run the following scripts from the Scripts folder
		a.	Initialize the database
		b.	Create new Mandant
		c.	Create new User

	4.	Restore Kendo
	5.	Search for #TODO# in the Web.config (ASTRA.EMSG.Web), and replace them
		a.	For the GeoServer url (WMS_Url_Development) 
				insert the file path like you see in the comment)
		b.	For the MapProxy url (SWMS_AV_Url_EMSG) 
				insert the file path like you see in the comment)
	6.	Deployment > no special things have to be considered
	7.	Start the application
```

## Optional: Installation of "EMSG-Mobile" rich client

There is a project called Mobile.Setup. This project is responsible to create an installer for the EMSG-Mobile product.
To be able to build it, you need to install the corresponding extension for your Visual Studio version:
* VS2010 - The Setup project type natively supported 
* VS2012 - The Setup project type is not supported, the EMSG Mobile installer cannot be built with VS2012
* [VS2013](https://visualstudiogallery.msdn.microsoft.com/9abe329c-9bba-44a1-be59-0fbf6151054d?SRC=VSIDE) - Install Microsoft Visual Studio 2013 Installer Projects
* [VS2015](https://visualstudiogallery.msdn.microsoft.com/f1cc3f3e-c300-40a7-8797-c509fb8933b9) - Install Microsoft Visual Studio 2015 Installer Projects 
* [VS2017](https://marketplace.visualstudio.com/items?itemName=VisualStudioProductTeam.MicrosoftVisualStudio2017InstallerProjects) - Install Microsoft Visual Studio 2017 Installer Projects 

