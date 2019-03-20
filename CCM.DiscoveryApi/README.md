# SR Discovery

## Introduction

This document presents a simple protocol that enables devices and systems to access filtered lists of SIP devices that are available in the SR network. SR hopes for formal standardisation of a protocol like this within EBU or AES, either by making a new specification based on something like this document or endorsing adaptation to existing IETF documents.

## Background

Most IP codecs provide the users with "phone books"; these are static lists of SIP identities. While these lists are easy to use when the number of codecs is low and the users know what codecs are online and exactly where they are connected, a static phone book quickly becomes unintuitive as the number of codes increase. In instant messaging applications we have already become accustomed to seeing who we can call on screen and this is something that IP codecs could also provide.

SIP is what makes this possible. Every codec registers to a SIP registrar server, this means that we know which codecs are powered up and connected. If all calls are sent through a central SIP proxy server we also know which codecs are busy or available. If each codec could access this information, the phone book could show only codecs that are available.

## IETF Presence

There already are specifications on how to let user agents (IP codecs in our case) see the presence information. [RFC 2778](http://tools.ietf.org/html/rfc2778) defines a model for presence in instant messaging applications, [RFC 3859](http://tools.ietf.org/html/rfc3859) defines a common profile for presence applications and [RFC 3863](http://tools.ietf.org/html/rfc3863) defines a data format for the information. The model described supports both polling and subscription-based presence. However, none of these appear to provide the required functionality.

## SR Discovery

Since we at SR have not, in detail, studied the RFCs mentioned above, we have defined a protocol of our own. This protocol might be simpler than the generic presence model described in [RFC 2778](http://tools.ietf.org/html/rfc2778). However, this proposal also include a standardized mechanism for filtering the information and presenting it in multiple languages where required.

### HTTP Query/Response

HTTP is a simple protocol and servers with rich functionality to support it are wide spread. We selected this protocol for a number of reasons. One of the important reasons was that we already had a web server with access to the databases of the SIP registrar and proxy servers and that with server scripting we were able to extract the information we need for SR discovery.

### HTTPS Security

The server will present a certificate that the client has to verify before setting up an encrypted message channel. When this is done, each request sent to the server will contain a username and password hash that the server will verify.

### XML data format

XML is a widely accepted encoding style for information and parsers are available for just about any platform. The size of the data transmitted is not large enough to make the byte-overhead of XML a significant issue. The body of each successful HTTP request is formatted as XML.

#### Meta data

Profiles, filters, filter options and user agents may hold a container for meta data. Meta data is a sequence of key-value pairs that hold additional information about the item. This data is configured in the SR Discovery provider and consumers may have the ability to modify presentation based on matching against specific meta data.

#### Localisation

Some names may be localised by specifying additional strings paired with a language code. User interfaces that has localisation may use the names that match the current language.

## Technical description of SR Discovery

- The codec periodically calls profiles() to get an updated list of all profiles that a user might want to use when making manual connections from the codec, the parameters in each profile either replaces the parameters of existing profiles with the same name or fill new profiles in the codec
- The codec periodically calls filters() to get an updated list of the available options for filtering, the options are made available through the codec user interface
- Whenever the list of available codes needs to be updated, i.e. when the dial form is visible, the codec periodically calls useragents(), passing the currently selected filter options and displays only the units listed in the response. The codec also makes a new call to useragents() every time a filter value is changed in the user interface.

### profiles(username, pwdhash)

Returns a list of profile definitions that can be used for making manual calls. The returned list may be empty; in this case the server doesn&#39;t provide any profiles and the codec has to rely on manually configured profiles. The number of profiles is unlimited.

#### Parameters
| Argument | Type | Description
| --- | --- | --- |
| username | String | For authorisation with the server |
| pwdhash | String | For authorisation with the server |

#### Response

Body of successful response contains a document complying with the srprecence.xsd schema, if the response contains a \<filters> or a \<user-agents> section, those shall be ignored.

#### Example syntax
    https://example.com
    POST /profiles HTTP/1.1
    Content-Type: application/x-www-form-urlencoded
    username=user&pwdhash=DFG4FGDd23443gHGs12D5

#### Example response

    <?xml version="1.0" encoding="utf-8"?>
    <sr-discovery>
    <profiles>
    <profile name="Internal">
    <localised-name lang="sv">Intern\</localised-name>
    <meta-data>
        <data key="IconSmall"value="http://icons.example.net/InternalSmall.png" />
    </meta-data>
    <sdp>v=0
    o=mtu-02 3599989344 3599989344 IN IP4 example.com
    s=mtu-02
    c=IN IP4 203.0.113.12
    t=0 0
    m=audio 5004 RTP/AVP 96 97
    a=rtpmap:96 L24/48000/2
    a=rtpmap:97 parityfec/48000
    a=fmtp:97 5006 IN IP4 203.0.113.12
    a=sendrecv
    a=ptime:4
    a=ebuacip:jb 0
    a=ebuacip:jbdef 0 fixed 6
    a=ebuacip:plength 96 4
    a=ebuacip:qosrec 34\</sdp>
    </profile>
    <profile name="EWAN">
    <sdp>v=0
    o=mtu-02 3599992693 3599992693 IN IP4 example.com
    s=mtu-02
    c=IN IP4 203.0.113.12
    t=0 0
    m=audio 5004 RTP/AVP 96
    a=rtpmap:96 aptx/48000/2
    a=fmtp:96 variant=enhanced; bitresolution=24
    a=sendrecv
    a=ptime:10
    a=ebuacip:jb 0
    a=ebuacip:jbdef 0 fixed 30
    a=ebuacip:plength 96 10
    a=ebuacip:qosrec 34\</sdp>
    </profile>
    <profile name="Mobile A">
    <localised-name lang="sv">Mobil A\</localised-name>
    <meta-data>\<data key="IconSmall" value="http://icons.example.net/InternalMobileASmall.png" />\</meta-data>
    <sdp>v=0
    o=mtu-02 3599993974 3599993974 IN IP4 example.com
    s=mtu-02
    c=IN IP4 203.0.113.12
    t=0 0
    m=audio 5004 RTP/AVP 96 97
    a=rtpmap:96 aptx/32000/1
    a=fmtp:96 variant=enhanced; bitresolution=24
    a=rtpmap:97 parityfec/32000
    a=fmtp:97 5006 IN IP4 203.0.113.12
    a=sendonly
    a=ptime:20
    a=ebuacip:jb 0
    a=ebuacip:jbdef 0 fixed 50
    a=ebuacip:plength 96 20
    a=ebuacip:qosrec 34
    m=audio 5004 RTP/AVP 9
    a=rtpmap:9 G722/8000
    a=recvonly
    a=ptime:20
    a=ebuacip:jb 0
    a=ebuacip:jbdef 0 fixed 150
    a=ebuacip:plength 9 20
    a=ebuacip:qosrec 34\</sdp>
    </profile>
    <profile name="Internet HQ">
    <meta-data>
        <data key="IconSmall" value="http://icons.example.net/InternalInternetHQSmall.png" />
    </meta-data>
    <sdp>v=0
    o=mtu-02 3599993476 3599993476 IN IP4 example.com
    s=mtu-02
    c=IN IP4 203.0.113.12
    t=0 0
    m=audio 5004 RTP/AVP 96
    a=rtpmap:96 opus/48000/2
    a=fmtp:96 stereo=1; sprop-stereo=1;maxaveragebitrate=256000;cbr=1;useinbandfec=1
    a=sendrecv
    a=ptime:20
    a=ebuacip:jb 0
    a=ebuacip:jbdef 0 fixed 500
    a=ebuacip:plength 96 20
    a=ebuacip:qosrec 34\</sdp>
    </profile>
    <profile name="Telephone">
    <localised-name lang="sv">Telefon\</localised-name>
    <sdp>v=0
    o=mtu-02 3599993974 3599993974 IN IP4 example.com
    s=mtu-02
    c=IN IP4 203.0.113.12
    t=0 0
    m=audio 5004 RTP/AVP 9 8
    a=rtpmap:9 G722/8000
    a=rtpmap:8 PCMA/8000
    a=sendrecv
    a=ptime:20
    a=ebuacip:jb 0
    a=ebuacip:jbdef 0 fixed 150
    a=ebuacip:plength 9 20
    a=ebuacip:plength 8 20
    a=ebuacip:qosrec 34\</sdp>
    </profile>
    </profiles>
    </sr-discovery>

### filters(username, pwdhash)

Returns a list of filters that can be used for calling useragents(). The returned list may be empty; in this case no server side filtering is available. The number of filters is unlimited.

#### Parameters

| Argument | Type | Description
| --- | --- | --- |
| username | String | For authorisation with the server |
| pwdhash | String | For authorisation with the server |

#### Response

Body of successful response contains a document complying with the sr-discovery.xsd schema, if the response contains a \<profiles> or a \<user-agents> section, those shall be ignored.

#### Example syntax

    https://example.com
    POST /filters HTTP/1.1
    Content-Type: application/x-www-form-urlencoded
    username=user&pwdhash=DFG4FGDd23443gHGs12D5

#### Example response

    <?xml version="1.0" encoding="UTF-8"?>
    <sr-discovery>
    <filters>
    <filter name="Location">
    <localised-name lang="sv">Placering\</localised-name>
    <meta-data>
        <data key="IconSmall" value="http://icons.example.net/LocationSmall.png" />
    </meta-data>
    <option name="Filt"/>
    <option name="Gamla Ullevi"/>
    <option name="Globen">
        <localised-name lang="en-UK">Stockholm Globe Arena\</localised-name>
    </option>
    <option name="Hovet"/>
    <option name="Löfbergs Lila"/>
    <option name="Ospecificerad"/>
    <option name="RH"/>
    <option name="RH ADSL"/>
    <option name="RH WLAN"/>
    <option name="Strömvallen Gävle"/>
    <option name="Swedbank Arena"/>
    <option name="Wantech 3G"/>
    </filter>
    <filter name="Owner">
    <localised-name lang="sv">Kanal/redaktion\</localised-name>
    <meta-data>
        <data key="IconSmall" value="http://icons.example.net/OwnerSmall.png" />
    </meta-data>
    <option name="DC"/>
    <option name="DC-TON">
        <meta-data>
        <data key="IconSmall" value="http://icons.example.net/DCTONSmall.png" />
        </meta-data>
    </option>
    <option name="Ekot"/>
    <option name="MTU"/>
    <option name="Radiosporten"/>
    <option name="SR Dalarna"/>
    <option name="SR Örebro"/>
    </filter>
    </filters>
    </sr-discovery>

### useragents(username, pwdhash, caller, callee, ...)

Returns a list of available user agents that conform to the provided filter options. The returned list may be empty. The number of user-agents is unlimited. The filter list defines the filters that are available from the server side. It provides a name for each filter and lists all available options. In addition, each filter and option can be given several localized names for different languages. The language is specified by the "lang" attribute holding language name abbreviations according to IETF BCP 47. If a codec provides localised user interfaces it should use the corresponding names. If no matching language is provided by the sr-discovery server, the default name should be used.

#### Parameters

| Argument | Type | Description
| --- | --- | --- |
| username | String | For authorisation with the server |
| pwdhash | String | For authorisation with the server |
| caller | String | Optional: The SIP id that will be used for originating calls to the returned user agents. This information is used by the service to optimise the recommended profiles for each callee in the result. |
| callee | String | Optional: If the SIP id of a callee is given, the result will only contain information about that callee. |
| _filtername_ | String[] | Optional: A list of string parameters whose names match the names of filters in the most recent response from a call to filters() |

#### Response

Body of successful response contains a document complying with the sr-discovery.xsd schema, if the response contains a \<filters> section, this shall be ignored. If the response contains a \<profiles> section, these shall update existing profiles in the same way as if a call to the profiles function was made.

#### Example syntax

    https://example.com
    POST /useragents HTTP/1.1
    Content-Type: application/x-www-form-urlencoded
    username=user&pwdhash=DFG4FGDd23443gHGs12D5&caller=dc%2d17%40contrib%2esr%2ese&Location=Stockholm&Owner=MTU

#### Example response

    <?xml version="1.0" encoding="utf-8"?>
    <sr-discovery>
    <profiles>
    <profile name="Internal">
    <localised-name lang="sv">Intern</localised-name>
    <sdp>v=0
    o=mtu-02 3599989344 3599989344 IN IP4 example.com
    s=mtu-02
    c=IN IP4 203.0.113.12
    t=0 0
    m=audio 5004 RTP/AVP 96 97
    a=rtpmap:96 L24/48000/2
    a=rtpmap:97 parityfec/48000
    a=fmtp:97 5006 IN IP4 203.0.113.12
    a=sendrecv
    a=ptime:4
    a=ebuacip:jb 0
    a=ebuacip:jbdef 0 fixed 6
    a=ebuacip:plength 96 4
    a=ebuacip:qosrec 34\</sdp>
    </profile>
    <profile name="EWAN">
    <sdp>v=0
    o=mtu-02 3599992693 3599992693 IN IP4 example.com
    s=mtu-02
    c=IN IP4 203.0.113.12
    t=0 0
    m=audio 5004 RTP/AVP 96
    a=rtpmap:96 aptx/48000/2
    a=fmtp:96 variant=enhanced; bitresolution=24
    a=sendrecv
    a=ptime:10
    a=ebuacip:jb 0
    a=ebuacip:jbdef 0 fixed 30
    a=ebuacip:plength 96 10
    a=ebuacip:qosrec 34\</sdp>
    </profile>
    <profile name="Internet HQ">
    <sdp>v=0
    o=mtu-02 3599993476 3599993476 IN IP4 example.com
    s=mtu-02
    c=IN IP4 203.0.113.12
    t=0 0
    m=audio 5004 RTP/AVP 96
    a=rtpmap:96 opus/48000/2
    a=fmtp:96 stereo=1; sprop-stereo=1;maxaveragebitrate=256000;cbr=1;useinbandfec=1
    a=sendrecv
    a=ptime:20
    a=ebuacip:jb 0
    a=ebuacip:jbdef 0 fixed 500
    a=ebuacip:plength 96 20
    a=ebuacip:qosrec 34\</sdp>
    </profile>
    <profile name="Telephone">
    <localised-name lang="sv">Telefon\</localised-name>
    <sdp>v=0
    o=mtu-02 3599993974 3599993974 IN IP4 example.com
    s=mtu-02
    c=IN IP4 203.0.113.12
    t=0 0
    m=audio 5004 RTP/AVP 9 8
    a=rtpmap:9 G722/8000
    a=rtpmap:8 PCMA/8000
    a=sendrecv
    a=ptime:20
    a=ebuacip:jb 0
    a=ebuacip:jbdef 0 fixed 150
    a=ebuacip:plength 9 20
    a=ebuacip:plength 8 20
    a=ebuacip:qosrec 34\</sdp>
    </profile>
    </profiles>
    <user-agents>
    <user-agent sip-id="Stockholm Driftcentralen 17&lt;dc-17@sip.sr.se&gt;">
    <profile-rec>
        <profile-ref name="Internal"/>
        <profile-ref name="EWAN"/>
    </profile-rec>
    <meta-data>
        <data key="Location" value="Stockholm"/>
    </meta-data>
    </user-agent>
    <user-agent sip-id="Stockholm Driftcentralen 18&lt;dc-18@sip.sr.se&gt;">
    <profile-rec>
        <profile-ref name="Internal"/>
    </profile-rec>
    <meta-data>
        <data key="Location" value="Stockholm"/>
    </meta-data>
    </user-agent>
    <user-agent sip-id="Stockholm Hovet 01&lt;hovet-01@sip.sr.se&gt;">
    <profile-rec>
        <profile-ref name="EWAN"/>
    </profile-rec>
    <meta-data>
        <data key="Location" value="Stockholm"/>
        <data key="InputCount" value="1"/>
    </meta-data>
    </user-agent>
    <user-agent sip-id="p1-nyc-01@sip.sr.se">
    <profile-rec>
        <profile-ref name="Internet HQ"/>
    </profile-rec>
    <meta-data>
        <data key="Location" value="Stockholm"/>
        <data key="Portable" value="yes">
        <localised-value lang="sv">Ja\</localised-value>
        </data>
    </meta-data>
    </user-agent>
    <user-agent sip-id="voip-01@sip.sr.se">
    <profile-rec>
        <profile-ref name="Telephone"/>
    </profile-rec>
    <meta-data>
        <data key="Location" value="Stockholm"/>
    </meta-data>
    </user-agent>
    </user-agents>
    </sr-discovery>

### sr-discovery.xsd

This schema specifies the format for responses to each of the functions of the SR DiscoveryAPI.

    <?xml version="1.0" encoding="utf-8"?>
    <xs:schema elementFormDefault="qualified" xmlns:xs="http://www.w3.org/2001/XMLSchema">
    <xs:element name="sr-discovery">
        <xs:complexType>
        <xs:sequence>
            <xs:element minOccurs="0" ref="profiles" />
            <xs:element minOccurs="0" ref="filters" />
            <xs:element minOccurs="0" ref="user-agents" />
        </xs:sequence>
        </xs:complexType>
    </xs:element>
    <xs:element name="profiles">
        <xs:annotation>
        <xs:documentation>Definitions of profiles\</xs:documentation>
        </xs:annotation>
        <xs:complexType>
        <xs:sequence>
            <xs:element minOccurs="0" maxOccurs="unbounded" name="profile">
            <xs:annotation>
                <xs:documentation>Definition of a profile.\</xs:documentation>
            </xs:annotation>
            <xs:complexType>
                <xs:sequence>
                <xs:element minOccurs="0" maxOccurs="unbounded" ref="localised-name" />
                <xs:element minOccurs="0" maxOccurs="1" ref="meta-data" />
                <xs:element name="sdp" type="xs:string" />
                </xs:sequence>
                <xs:attribute name="name" type="xs:string" use="required">
                <xs:annotation>
                    <xs:documentation>The name of the profile. If no localised name for the current locale is found, this is the default name to fall back to. This is also the name used in all profile-ref elements.\</xs:documentation>
                </xs:annotation>
                </xs:attribute>
            </xs:complexType>
            </xs:element>
        </xs:sequence>
        </xs:complexType>
    </xs:element>
    <xs:element name="filters">
        <xs:annotation>
        <xs:documentation>Definitions of filters and options\</xs:documentation>
        </xs:annotation>
        <xs:complexType>
        <xs:sequence>
            <xs:element minOccurs="0" maxOccurs="unbounded" name="filter">
            <xs:complexType>
                <xs:sequence>
                <xs:element minOccurs="0" maxOccurs="unbounded" ref="localised-name" />
                <xs:element minOccurs="0" maxOccurs="1" ref="meta-data" />
                <xs:element minOccurs="0" maxOccurs="unbounded" name="option">
                    <xs:complexType>
                    <xs:sequence>
                        <xs:element minOccurs="0" maxOccurs="unbounded" ref="localised-name" />
                        <xs:element minOccurs="0" maxOccurs="1" ref="meta-data" />
                    </xs:sequence>
                    <xs:attribute name="name" type="xs:string" />
                    </xs:complexType>
                </xs:element>
                </xs:sequence>
                <xs:attribute name="name" type="xs:string" use="required" />
            </xs:complexType>
            </xs:element>
        </xs:sequence>
        </xs:complexType>
    </xs:element>
    <xs:element name="user-agents">
        <xs:annotation>
        <xs:documentation>List of available user agents.\</xs:documentation>
        </xs:annotation>
        <xs:complexType>
        <xs:sequence>
            <xs:element minOccurs="0" maxOccurs="unbounded" name="user-agent">
            <xs:annotation>
                <xs:documentation>Identification of a user agent.\</xs:documentation>
            </xs:annotation>
            <xs:complexType>
                <xs:sequence>
                <xs:element minOccurs="0" name="profile-rec">
                    <xs:complexType>
                    <xs:sequence>
                        <xs:element minOccurs="0" maxOccurs="unbounded" name="profile-ref">
                        <xs:annotation>
                            <xs:documentation>Reference to a recommended profile. The sequence is ordered by preference.\</xs:documentation>
                        </xs:annotation>
                        <xs:complexType>
                            <xs:attribute name="name" type="xs:string" use="required" />
                        </xs:complexType>
                        </xs:element>
                    </xs:sequence>
                    </xs:complexType>
                </xs:element>
                <xs:element minOccurs="0" maxOccurs="1" ref="meta-data" />
                </xs:sequence>
                <xs:attribute name="sip-id" type="xs:string" use="required">
                <xs:annotation>
                    <xs:documentation>SIP URI that can be used to contact this user agent. May contain a display name as per RFC 2822.\</xs:documentation>
                </xs:annotation>
                </xs:attribute>
                <xs:attribute name="connected-to" type="xs:string" use="optional">
                <xs:annotation>
                    <xs:documentation>Indicates the SIP URI of the user agent that this user agent is currently connected to. May contain a display name as per RFC 2822.\</xs:documentation>
                </xs:annotation>
                </xs:attribute>
            </xs:complexType>
            </xs:element>
        </xs:sequence>
        </xs:complexType>
    </xs:element>
    <xs:element name="localised-name">
        <xs:annotation>
        <xs:documentation>Name to use in localised user interfaces.\</xs:documentation>
        </xs:annotation>
        <xs:complexType>
        <xs:simpleContent>
            <xs:extension base="xs:string">
            <xs:attribute name="lang" type="xs:string" use="required">
                <xs:annotation>
                <xs:documentation>Language identifier as defined by IETF BCP 47.\</xs:documentation>
                </xs:annotation>
            </xs:attribute>
            </xs:extension>
        </xs:simpleContent>
        </xs:complexType>
    </xs:element>
    <xs:element minOccurs="0" name="meta-data">
        <xs:complexType>
        <xs:sequence>
            <xs:element minOccurs="0" maxOccurs="unbounded" name="data">
            <xs:annotation>
                <xs:documentation>Generic container for meta-data. TODO: There is no way to signal localised named for keys, do we need this?\</xs:documentation>
            </xs:annotation>
            <xs:complexType>
                <xs:sequence>
                <xs:element minOccurs="0" maxOccurs="unbounded" name="localised-value">
                    <xs:annotation>
                    <xs:documentation>Value to use in localised user interfaces.\</xs:documentation>
                    </xs:annotation>
                    <xs:complexType>
                    <xs:simpleContent>
                        <xs:extension base="xs:string">
                        <xs:attribute name="lang" type="xs:string" use="required">
                            <xs:annotation>
                            <xs:documentation>Language identifier as defined by IETF BCP 47.\</xs:documentation>
                            </xs:annotation>
                        </xs:attribute>
                        </xs:extension>
                    </xs:simpleContent>
                    </xs:complexType>
                </xs:element>
                </xs:sequence>
                <xs:attribute name="key" type="xs:string" use="required" />
                <xs:attribute name="value" type="xs:string" use="optional" />
            </xs:complexType>
            </xs:element>
        </xs:sequence>
        </xs:complexType>
    </xs:element>
    </xs:schema>