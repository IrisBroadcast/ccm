// import AnimationView from "./service/Animation";
// import AudioPlayer from "./service/AudioInterface";
// import HeadtrackerListener from "./service/HeadtrackerListener";
import { ColorPicker } from "./utils/ColorPicker";
import Events from "./utils/Events";
import { FilterList } from "./utils/FilterList";
import PasswordGenerator from "./utils/PasswordGenerator";
import { SessionDescription } from "./utils/SessionDescriptionProtocolUtil";
import { Sortable } from "./utils/Sortable";
import Tool from './utils/Tools';

export class Application {
    // private listener: HeadtrackerListener;
    // private audio: AudioPlayer;
    // private animation: AnimationView;

    private event: Events = new Events();

    constructor() {
        console.log("Initiated application");

        // this.listener = HeadtrackerListener.getInstance();

        try {
            Sortable("sortable-selection", function (items) {
                if (!items) {
                    return;
                }
                for (var i = 0; i < items.length; i++) {
                    const element = items[i].getElementsByClassName("sortIndex")[0];
                    if (element) {
                        element.setAttribute("value", `${i}`);
                    }
                }
            });
        } catch(error) {
            console.warn(error);
        }

        try {
            FilterList("filterable-selection");
        } catch(error) {
            console.warn(error);
        }

        try {
            ColorPicker("colorpicker-selection");
        } catch(error) {
            console.warn(error);
        }

        try {
            this.SessionDescriptionParse("sdp-field");
        } catch(error) {
            console.warn(error);
        }

        const pass = new PasswordGenerator();
        try {
            const userPwButton = Tool.$dom("pwGen");
            if (userPwButton) {
                Tool.$event(userPwButton, "click", () => {
                    const password = pass.generateUserPassword();
                    (Tool.$dom("Password") as any).value = password;
                    (Tool.$dom("PasswordConfirm") as any).value = password;
                    (Tool.$dom("generatedPassword") as any).value = password;
                    Tool.$dom("generatedWrapper").classList.remove("hidden");
                });
            }
        } catch(error) {
            console.warn(error);
        }
    }

    private SessionDescriptionParse(targetId: string) {
        const sdpElement = document.getElementById(targetId);
        if (sdpElement == null) {
            throw new Error(`Could not bind any SDP fields '.${targetId}'`);
        }

        sdpElement.addEventListener('keyup', function(ev: any) {
            const text = ev.target.value;
            console.log({ev, text});

            const ret = SessionDescription.parse(text);
            console.log(ret);
            console.dir(ret.parsed)
        });




    }
}

export default class Main {
    public static load() {
        console.log("Initiated main");
        const app = new Application();
    }
}

Main.load();
