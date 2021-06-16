import * as signalR from "@microsoft/signalr";
// const signalRMsgPack = require("@microsoft/signalr-protocol-msgpack");
import { SignalRConnectionLevels } from "../constants/signalrConnectionLevels";
import StringUtil from "../utils/StringUtil";

export default abstract class AspNetCoreHubListener {

    public connection: signalR.HubConnection;
    public abstract readonly connectionOpenEventType: string;

    private hubName: string;
    private serverUrl: string;
    private hubReconnectDelay: number = 3000;

    private subscriptions: any[] = [];

    public onConnected: () => void;
    public onDisconnected: () => void;

    constructor(serverUrlArg: string, hubNameArg: string) {
        if (StringUtil.isNotNullOrEmpty(serverUrlArg) && StringUtil.isNotNullOrEmpty(hubNameArg)) {
            this.initiate(serverUrlArg, hubNameArg);
        }
    }

    public initiate(serverUrlArg: string = "", hubNameArg: string = "") {
        this.hubName = hubNameArg;
        this.serverUrl = serverUrlArg;

        console.info("AspNetCoreHubListener for hub " + this.hubName + " on " + this.serverUrl);

        // Hub options
        // serverTimeoutInMilliseconds: Timeout for server activity. If the server
        // hasn't sent a message in this interval, the client considers the server
        // disconnected and triggers the onclose event. This value must be large
        // enough for a ping message to be sent from the server and received by the
        // client within the timeout interval. The recommended value is a number at
        // least double the server's KeepAliveInterval value to allow time for pings to arrive.

        // keepAliveIntervalInMilliseconds: Determines the interval at which the
        // client sends ping messages. Sending any message from the client resets
        // the timer to the start of the interval. If the client hasn't sent a message
        // in the ClientTimeoutInterval set on the server, the server considers the
        // client disconnected.

        let hubOptions = {
            transport: signalR.HttpTransportType.WebSockets,
        };

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(this.serverUrl + "/" + this.hubName, hubOptions)
            // .configureLogging(signalR.LogLevel.Trace)
            // .withHubProtocol(new MessagePackHubProtocol())
            .build();

        this.connection.serverTimeoutInMilliseconds = 20000;
        this.connection.keepAliveIntervalInMilliseconds = 6000;

        this.connection.onclose(() => {
            console.info("AspNetCoreHub connection closes");

            console.log(this.connectionOpenEventType, {
                isOpen: false,
                state: SignalRConnectionLevels.Disconnected,
                status: "connection closed",
            });

            if (this.onDisconnected) {
                this.onDisconnected();
            }

            // Restart connection after x seconds
            setTimeout(() => {
                this.startConnection().then(() => {
                    console.info(`AspNetCoreHub initiated a reconnect ${this.hubName} on ${this.serverUrl}`);
                }).catch((error: any) => {
                    console.error(error);
                });
            }, this.hubReconnectDelay);
        });
    }

    public addHubEventListener(eventName: string, callback: any) {
        if (this.connection) {
            console.info("AspNetCoreHub added event listener: " + eventName);
            this.connection.on(eventName, (data: any) => {
                console.debug(`AspNetCoreHub event ${eventName} received data`, data);
                if (callback) {
                    callback(data);
                }
            });
        } else {
            console.warn("AspNetCoreHub no connection object to initiate listener " + this.hubName + " on " + this.serverUrl + " for " + eventName);
            this.subscriptions.push({ eventName, callback});
        }
    }

    public startConnection(): Promise<any> {
        if (!this.connection) {
            return Promise.reject(`Can't start connection to ${this.hubName} on ${this.serverUrl}`);
        }

        return Promise.resolve(this.connection.start()
            .then(() => {
                console.log(`AspNetCoreHub connected to ${this.hubName}`
                    + ` on ${this.serverUrl}`
                    + ` with state ${this.connection.state}`
                    + ` serverTimeout ${this.connection.serverTimeoutInMilliseconds}ms`
                    + ` keepAliveInterval ${this.connection.keepAliveIntervalInMilliseconds}ms`);

                console.log(this.connectionOpenEventType, {
                    isOpen: true,
                    state: SignalRConnectionLevels.Connected,
                    status: "connected",
                });

                if (this.onConnected) {
                    this.onConnected();
                }
            })
            .catch((error: any) => {
                console.error(`AspNetCoreHub error ${this.hubName} on ${this.serverUrl}`);
                console.error(error);
                console.log(this.connectionOpenEventType, {
                    isOpen: false,
                    state: SignalRConnectionLevels.Disconnected,
                    status: "connection failed",
                });
                return console.error(error.toString());
            }));
    }
}