
# Database initiation

### Install MySQL (if you're running that)

Download installer that has all the necessary applications you might need for dealing with MySQL. 
([https://dev.mysql.com/downloads/installer/](https://dev.mysql.com/downloads/installer/))

Select: 
* MySQL for Visual Studio
* MySQL Server
* MySQL Router (Maybe not needed)
* MySQL Workbench (Good graphical view of your local dB)
* Connector 

Start a local MySQL server where you set the username “root” to use password “root” or according to the settings in "Web.config" file.

Verify the file "Web.config" and that the "CCMDbContext" settings are present as follows or depending on your setup.
* &lt;add name="CCMDbContext" connectionString="server=localhost;port=3306;database=uccm;uid=root;password=root" providerName="MySql.Data.MySqlClient" /&gt;

### Initiate new database

TODO: Remove this part.. not the way for .NET core
If this is your first time running, set the Web.Config variable “Environment” to “Initiate”.

Select what type of creation you prefer in the file “CCM.Data -&gt; CCMDbContext.cs” you can select between:

* CreateDatabaseIfNotExists (Default)
* DropCreateDatabaseIfModelChanges
* DropCreateDatabaseAlways

Then the database will be created on the localhost server when the application is built. (Or whatever server is specified in the config variable "CCMDbContext")

On first run and build it should be created. Then you can change back your Environment variable to “Test”, “Prod” or “Dev” after it’s creation.

### Verify state

* Open Visual Studio
* View -&gt; Server Explorer
* The server should be visible in the Server Explorer under “Data Connections”.
* If the database-server is not visible?
  * First make sure the you actually have a server running "localhost" (or at configured location) with the configuration found in Web-Config file
  * Right click on Data Connections -&gt; Add Connection
  * Press “Change” if the Data source is not “MySQL Database (MySQL Data provider)" or the database you're using of choice
  * Set Server Name as “localhost”, user name as “root” and password as “root” (again as configured)
  * Test the connection to make sure the server responds
  * If successful select the database you wish to use. If it hasen't been initiated cancel the Add connection wizard and try again with previous initiation steps.
