export default class Events {
    private topics = {};

    private static instance: Events;

    constructor() {
        if (Events.instance) {
            return Events.instance;
        }

        Events.instance = this;
    }

    private parseTopic(topic: string): string[] {
        let split = topic.split(' ');
        if (split.length >= 3) {
            console.warn(`The published topic contains too many identifiers: '${topic}', will publish on '${split[1]}'`);
            return [split[0], split[1]];
        } else if(split.length >= 2) {
            return split;
        } else {
            return [topic, topic];
        }
    }

    /**
     * Publish messages or objects with a topic. Any subscribers to that topic will receive the data in callback
     * @param topic publish topic identifier
     * @param args the data to send
     * @returns returns true if anyone is subscribing to the topic
     */
    public pub = function (topic: string, ...args: any): boolean {
        if (!this.topics[topic]) return false;

        this.topics[topic].forEach((x: any) => {
            x(...args);
        });
        return true;
    };

    /**
     * Subscribe to any topic with a callback function that is triggered on published messages.
     * @param topic topic to subscribe to
     * @param fn callback function with corresponding arguments
     */
    public on = function (topic: string, fn: any) {
        if (!this.topics[topic]) {
            this.topics[topic] = [fn];
        } else {
            this.topics[topic].push(fn);
        }
    };
}
