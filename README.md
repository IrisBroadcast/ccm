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
+ CCM.Core			: Core Helpers, Entities and models, Kamalio connection
+ CCM.Data			: Database
+ CCM.DiscoveryApi : API Discovery interface
+ CCM.Web			: Administrative interface and frontend
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
