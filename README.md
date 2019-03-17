The IRIS Call Control Manager and discovery service - CCM
=========================================================

* Web Site: https://www.irisbroadcast.org
* Github: https://github.com/irisbroadcast

This is the code for the IRIS Call Control Manager and Discovery
service developed by Sveriges Radio AB in Sweden. This code has
been in production for many years and we're proud to share it
with a larger community!

Release plan after 1.0
======================

This first release is a first, but important, step for us as
maintainers of this Open Source project. The code is licensed
using the BSD 3-clause license and we have gone through the process
internally of releasing in-house software as Open Source.

We will now work to move our in-house development to github
and move our production servers to the Open Source code. This
means that we will release a new set of code at some point
during the first half of 2018 and from that point continue
development in the open on github.

The changes between the 1.0 release and the coming version will
not be committed to the github repository until we have a new
process for our in-house development.

If you like to contribute documentation, local translations
or code you are more than welcome with a pull request on github.com.

Documentation
=============

### Requirements

- Minimum support and tested MySQL 5.7
- Visual Studio 2017
- Minimum Microsoft Windows Server 20XX

### How to get started

(If you want codec control, set up that project separetely.)

1. Open the Visual Studio solution and get the nuget packages required.
2. The application is built as follows:
+ CCM.Core:	The core of the IRIS CCM platform. 
+ CCM.Data:	Module for data storage. 
+ CCM.Web: 	The web user interface (ASP.NET MVC 5 project)
+ CCM.DiscoveryApi:	The IRIS discovery service (ASP.NET MVC 5 project)
+ CCM.Tests:	Unit and integration tests
		Note: The unit tests are in working shape, but the rest of the tests may or
		may not work at this stage.

4. Set up the database of choice, instructions here [database setup](CCM.Data/README.md)
4. Run the application
5. Set up messages from your SIP-registrar (e.g Kamalio) to start recieving data at your enpoint

License
=======

IRIS CCM and Discovery Service is (C) Sveriges Radio AB, Stockholm, Sweden 2017
The code is licensed under the BSD 3-clause license.

The license for CCM is in the LICENSE.txt file

3rd party libraries
===================
The repository contains 3rd party javascript libraries that are included
for convience. In a future release, these will not be included
but downloaded by scripts. These libraries have their own
copyright and licenses.





The IRIS CCM - Codec Call Monitor
=================================

CCM is a platform for administration and management of an ACIP-compliant SIP
infrastructure for live radio broadcast.

Architecture
------------
CCM consists of a web application, a web service and a windows service built
with Microsoft .NET framework in C# storing data in a MySQL or MariaDB database.
Authentication for SIP is done using a Radius service. The CCM interacts
and manages accounts in the Radius service.

In production use, the CCM is by default running on two separate servers in 
different data centers. Data is replicated using MySQL replication. Kamailio
updates both CCM servers with current registration states as well as call states.

Load balancing between the two CCM servers can be done with external HTTP load
balancers, using DNS or virtual IP failover.

Platforms and tools
-------------------
CCM is build on a Microsoft Windows platform interacting with a Kamailio SIP server
running on a Linux server. 
CCM is developed in C# using Microsoft .NET Framework 4.5. ASP.NET MVC 5 is used for
the web services. Development is done in Microsoft Visual Studio 2017.

For databases, the Entity Framework v6 is used.

The CCM web
-----------
The web interface provides an overview of available (and registered) devices as well
as current broadcast sessions. A user can view registration IP addresses, device types
and for some devies, manage the device in the CCM. When searching, filters based on
region, type of device or other criteria may be applied.

When logging in as an administrator, account management is available - both CCM web log ins
as well as SIP accounts. 

The IRIS Discovery service
--------------------------
The IRIS discovery services, also known by the name "Active Phonebook", is used by the
connected devices to find other available devices to set up a live session. The service
has three main functions:
* Profiles:	List available call profiles (Profiles is SDP's)
* Filters:	List available pre-defined search filters (Based on location filters on IP-ranges)
* Devices:	List available devices based on applied filters and profiles

All functions require a valid authentication.

IRIS Connect - Kamailio configuration
-------------------------------------
Please see the separate IRIS project for Kamailio
[Github.com/irisbroadcast/Connect](https://github.com/IrisBroadcast/Connect)

IRIS Codec Control
------------------
The codec control has become a powerful addition to CCM. Enabling the same user interface regardless of what codec the user controls. This was formerly integrated into CCM. But now it's a separate project. It uses CCM for discovery and management of devices. 
Codec control is written in dotnet core. So this should enable you to run it on a linux platform as well.
[Github.com/irisbroadcast/CodecControl](https://github.com/IrisBroadcast/CodecControl)

Code modules
------------
* CCM.Core:	The core of the IRIS CCM platform. 
* CCM.Data:	Module for data storage. 
* CCM.Web: 	The web user interface (ASP.NET MVC 5 project)
* CCM.DiscoveryApi:	The IRIS discovery service (ASP.NET MVC 5 project)
* CCM.Tests:	Unit and integration tests
		Note: The unit tests are in working shape, but the rest of the tests may or
		may not work at this stage.

Installation of a test platform
===============================

CCM
---
1. Create MySQL database
2. Install windows tools needed
3. Create default root user
4. ??




Feedback, bugs, comments?
* Please open an issue on Github
