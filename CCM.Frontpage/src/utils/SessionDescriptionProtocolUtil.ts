


export function hasProp(obj, value, ...keys)
{
    if (obj === undefined)
    {
        return false
    }

    if ( keys.length == 0 && Object.prototype.hasOwnProperty.call(obj, value) )
    {
        return true
    }

    return this.hasProp(obj[value], ...keys)
}

export function ParseSDPOld(rawsdp)
{
    let row = "",
        tmpinner = "",
        store = [],
        objStore = {};

    function addY(key, arr, value) {
        if (!module.exports.hasProp(arr, key)) {
            arr[key] = [];
            arr[key].push(value);
        } else {
            arr[key].push(value);
        }
    }

    for (let i = 0; i < rawsdp.length; i++) {
        if (rawsdp[i] == "\n" || rawsdp == "\n\r" || i == rawsdp.length-1) {
            tmpinner = "";
            let multiPayloadType = false;
            let hkey = "";
            let ikey = "";
            let rowPayloadType = "";

            for (let x = 0; x < row.length; x++) {
                if (multiPayloadType && row[x] == " ") {
                    multiPayloadType = false;
                    store.push({ key: ikey, data: tmpinner })
                    addY(ikey, objStore, { data: tmpinner });
                    hkey = tmpinner
                    rowPayloadType = tmpinner;
                    tmpinner = "";
                }
                else if (row[x] == "=") {
                    hkey = tmpinner;
                    tmpinner = "";
                }
                else if (row[x] == ";") {
                    store.push({ key: hkey.trim(), data: tmpinner, payloadType: rowPayloadType })
                    addY(hkey.trim(), objStore, { data: tmpinner, payloadType: rowPayloadType });
                    tmpinner = "";
                }
                else if (row[x] == ":") {
                    multiPayloadType = true;
                    ikey = tmpinner;
                    tmpinner = "";
                }
                else {
                    tmpinner += row[x];
                }

                if (x == row.length-1) {
                    let weightedKey = hkey;
                    //check if key is as first character.. use other key
                    if (row[0] == hkey && ikey != "") {
                        // Should be ikey unless that one is empty
                        weightedKey = ikey;
                    }

                    store.push({ key: weightedKey, data: tmpinner })
                    addY(weightedKey, objStore, { data: tmpinner });

                    tmpinner = "";
                }
            }
            row = "";
        }
        else if (rawsdp[i] == "\r") {
            // Dont do anything
        }
        else {
            // code block
            row += rawsdp[i];
        }
    }
    return objStore;
}

export class SDPAttribute {
    public mediaFormat: string;
    public parameter: string;
    public value: any;
    public get isRelatedToMediaFormat(): boolean {
        return this.mediaFormat !== null || this.mediaFormat !== "";
    }
    public isAcip: boolean = false;

    constructor(parameter: string, value: any, formatId: string = null, isAcip = false) {
        this.mediaFormat = formatId;
        this.parameter = parameter;
        this.value = value;
        this.isAcip = isAcip;
    }

    AsString() {
        return `${this.mediaFormat} => ${this.parameter} ${this.isAcip ? "[acip attr]" : ""}`;
    }
}

export class SessionDescription {
    public info: any;
    public media: any;
    public attr: SDPAttribute[] = [];

    public parsed: any;

    constructor() {

    }

    public static parse(raw: any): SessionDescription {
        const sdp = new SessionDescription();
        sdp.parseSDP(raw);
        return sdp;
    }

    parseAttribute(data, key = "", fmt = null) {
        switch(key) {
        case "fmtp":
            console.log("fmtp-------");
            data.split(";").map(attr => {
                console.log(attr.trim())
                const pair = attr.trim().split("=");
                if (pair.length == 2) {
                    this.attr.push(new SDPAttribute(pair[0], pair[1], fmt))
                }
                return;
            });
            return;
            break;
        case "ebuacip":
            console.log("ebuacip -----");

            // a=ebuacip:plength <format> <milliseconds>
            // a=ebuacip:plength 98 4
            const regex1 = /(plength)\s{1}(\d{1,})\s{1}(\d{1,})/gm;
            let m = regex1.exec(data);
            if (m !== null && m.length == 4) {
                // m.forEach((match, groupIndex) => {
                //     console.log(`Found match, group ${groupIndex}: ${match}`);
                // });
                this.attr.push(new SDPAttribute("plength", m[3], m[2], true));
                return {
                    format: m[2],
                    plength: m[3]
                };
            }

            // a=ebuacip:jb <option list>
            let attri = data.split(" ");
            console.log(attri)
            if (attri !== null && attri[0] === "jb" && attri.length > 1) {
                attri.splice(1).forEach((option) => {
                    this.attr.push(new SDPAttribute(`jb${option}`, true, null, true))
                });

                return {
                    options: attri.splice(1)
                };
            }

            // a=ebuacip:jbdef <option> <jb-option>
            if (attri !== null && attri[0] === "jbdef" && attri.length > 2) {
                this.attr.push(new SDPAttribute(`jbdef${attri[1]}`, attri.splice(2).join(" "), null, true))
                return {
                    option: attri[1],
                    jbdef: attri.splice(2).join(" ")
                };
            }

            break;
        default:
            console.log(" ==", data);
            if (key == null || key == "") {
                this.attr.push(new SDPAttribute(data, true, null));
            } else {
                this.attr.push(new SDPAttribute(key, data, fmt));
            }
            return data;
            break;
        }

        return data.split(";").map(attr => {
            console.log(attr)
            return attr.trim().split("=");
        });
    }

    parseSDP(rawsdp) {
        let mediaFormatKey = "",
            objStore = {};

        function addY(arr, key, value, subkey = "") {
            if (!module.exports.hasProp(arr, key)) {
                arr[key] = {};
            }

            if (subkey == "" || subkey == null) {
                arr[key] = value;
            } else {
                if (!module.exports.hasProp(arr[key], subkey)) {
                    arr[key][subkey] = [];
                }

                arr[key][subkey].push(value);
            }
        }

        const rows = rawsdp.split("\n");
        rows.forEach(row => {
            let content: any = "";
            let rowId = "";
            let subKey = "";

            console.log("Row:", row)

            // Parse each letter
            for (let x = 0; x < row.length; x++)
            {
                if (row[x] == "=" && rowId == "")
                {
                    // Determine the first char in the row
                    rowId = content;
                    content = "";
                }
                else if (row[x] == ":")
                {
                    // Found sub-key
                    subKey = content;
                    content = "";
                    console.log(" >", subKey);
                }
                // Probably reached an attribute with redefined <fmt>
                else if (row[x] == " " && subKey != "" && content == mediaFormatKey)
                {
                    content = "";
                }
                // Pack the row at the end
                else if (x == row.length-1)
                {
                    content += row[x];
                    content = content.trim();

                    if (rowId === "m")
                    {
                        // m=<media> <port>/<number of ports> <proto> <fmt>
                        let fmt = content.split(" ").pop() || `no_fmt`;
                        mediaFormatKey = fmt;
                        console.log(" ¤", fmt);
                        addY(objStore, "media_" + mediaFormatKey, { definition: content });
                    }
                    else if (rowId === "a")
                    {
                        // a=<attribute>:<value>
                        content = this.parseAttribute(content, subKey, mediaFormatKey);
                        if (subKey == "") {
                            addY(objStore, "media_" + mediaFormatKey, true, content);
                        } else {
                            
                            addY(objStore, "media_" + mediaFormatKey, content, subKey);
                        }
                    }
                    else
                    {
                        addY(objStore, rowId.trim() + mediaFormatKey, content, subKey);
                    }

                    console.log("-----------------------------");
                    content = "";
                    subKey = "";
                }
                else
                {
                    content += row[x];
                }
            }
        });

        this.parsed = objStore;
    }
}

// 5. SDP Specification ...............................................7
// 5.1. Protocol Version ("v=") ...................................10
// v=0
//    The "v=" field gives the version of the Session Description Protocol.
//    This memo defines version 0.  There is no minor version number.

// 5.2. Origin ("o=") .............................................11
// o=<username> <sess-id> <sess-version> <nettype> <addrtype> <unicast-address>
// The "o=" field gives the originator of the session (her username and
//     the address of the user's host) plus a session identifier and version
//     number:

// 5.3. Session Name ("s=") .......................................12
// s=<session name>
//    The "s=" field is the textual session name.  There MUST be one and
//    only one "s=" field per session description.  The "s=" field MUST NOT
//    be empty and SHOULD contain ISO 10646 characters (but see also the
//    "a=charset" attribute).  If a session has no meaningful name, the
//    value "s= " SHOULD be used (i.e., a single space as the session
//    name).

// 5.4. Session Information ("i=") ................................12
// 5.5. URI ("u=") ................................................13
// 5.6. Email Address and Phone Number ("e=" and "p=") ............13

// 5.7. Connection Data ("c=") ....................................14
// c=<nettype> <addrtype> <connection-address>
// A session description MUST contain either at least one "c=" field in
//    each media description or a single "c=" field at the session level.
//    It MAY contain a single session-level "c=" field and additional "c="
//    field(s) per media description, in which case the per-media values
//    override the session-level settings for the respective media.

// 5.8. Bandwidth ("b=") ..........................................16

// 5.9. Timing ("t=") .............................................17
// t=<start-time> <stop-time>
// The "t=" lines specify the start and stop times for a session.
//    Multiple "t=" lines MAY be used if a session is active at multiple
//    irregularly spaced times; each additional "t=" line specifies an
//    additional period of time for which the session will be active.

// 5.10. Repeat Times ("r=") ......................................18
// 5.11. Time Zones ("z=") ........................................19
// 5.12. Encryption Keys ("k=") ...................................19

// 5.13. Attributes ("a=") ........................................21
// a=<attribute>:<value>
// A media description may have any number of attributes ("a=" fields)
//    that are media specific.  These are referred to as "media-level"
//    attributes and add information about the media stream.  Attribute
//    fields can also be added before the first media field; these
//    "session-level" attributes convey additional information that applies
//    to the conference as a whole rather than to individual media.


// ... fmt = media format description
// 5.14. Media Descriptions ("m=") ................................22
// m=<media> <port>/<number of ports> <proto> <fmt>
