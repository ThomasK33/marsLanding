"use strict";

const mqtt = require("mqtt");

const SERVER = "10.10.10.30";
const PORT = 1883;

const PLAYER_NAME = "oculUS";

var c;

class Wrapper {

	constructor(timeLimit=30000) {
		this.useTimeLimit = !!timeLimit; 		
		this.timeLimit = timeLimit;

		this.client = mqtt.connect({host: SERVER, port: PORT});
		c = this.client;

		this.client.on("connect", () => {
			console.log("Connected " + this.client.connected);
			this.client.subscribe("players/" + PLAYER_NAME + "/#");
			this.client.subscribe("robot/state");
			this.client.subscribe("robot/error");

			this.connected = true;
		});

		this.client.on("message", (topic, message) => {
			console.log(topic);
			var obj = JSON.parse(message.toString());
			console.log(obj);

			if (topic == "players/" + PLAYER_NAME + "/incoming" && obj.command == "start") {
				this.started = true;
			}
			else if (topic == "players/" + PLAYER_NAME + "/incoming" && obj.command == "finished") {
				this.finished = true;
			}
			else if (topic == "players/" + PLAYER_NAME + "/game") {
				if (this.gamestateUpdateFn)
					this.gamestateUpdateFn(obj);
			}
			else if (topic == "robot/state") {
				if (this.hardwareUpdateFn)
					this.hardwareUpdateFn(obj);
			}
			else if (topic == "robot/error") {
				if (this.errorFn)
					this.errorFn(obj);
			}
		});
	}

	waitUntilConnected(timeLimit) {
		let useTimeLimit = true; 		
		if(typeof timeLimit === "undefined") { 			
			useTimeLimit = this.useTimeLimit; 			
			timeLimit = this.timeLimit; 		
		}

		return new Promise((resolve, reject) => {
			if (useTimeLimit && timeLimit <= 0) {
				console.log("Failed to establish a working connection");
				reject();
			}

			if (!this.connected) {
				let next = useTimeLimit ? timeLimit - 100 : undefined;
				setTimeout(() => this.waitUntilConnected(next).then(resolve, reject), 100);
				return;
			}

			resolve();
		});
	}

	waitUntilStarted(timeLimit) {
		let useTimeLimit = true; 		
		if(typeof timeLimit === "undefined") { 			
			useTimeLimit = this.useTimeLimit; 			
			timeLimit = this.timeLimit; 		
		}

		return new Promise((resolve, reject) => {
			if (useTimeLimit && timeLimit <= 0) {
				console.log("Failed to receive a start command");
				reject();
			}

			if (!this.started) {
				let next = useTimeLimit ? timeLimit - 100 : undefined;
				setTimeout(() => this.waitUntilStarted(next).then(resolve, reject), 100);
				return;
			}

			resolve();
		});
	}

	sendRegister() {
		return this.waitUntilConnected().then(() => {
			return this.client.publish("players/" + PLAYER_NAME, "{\"command\": \"register\"}");
		});
	}

	sendStart() {
		return this.waitUntilConnected().then(() => this.waitUntilStarted()).then(() => {
			return this.client.publish("players/" + PLAYER_NAME, "{\"command\": \"start\"}");
		});
	}

	sendControl(command, args) {
		return new Promise((resolve) => {
			let obj = {"command": command};

			if (command != "stop" && command != "reset")
				obj["args"] = parseInt(args);

			this.client.publish("robot/process", JSON.stringify(obj), resolve);
		});
	}

	isFinished() {
		return !!this.finished;
	}

	onGamestateUpdate() {
		return new Promise((resolve) => {
			this.gamestateUpdateFn = resolve;
		});
	}

	onHardwareUpdate() {
		return new Promise((resolve) => {
			this.hardwareUpdateFn = resolve;
		});
	}

	onError() {
		return new Promise((resolve) => {
			this.errorFn = resolve;
		});
	}
}


function tearDown() {
	console.log("\nTearing down...");
	if (c)
		c.end();
}

// doing a cleanup action just before node.js exits,
// see http://stackoverflow.com/questions/14031763/doing-a-cleanup-action-just-before-node-js-exits

// handling exits
process.stdin.resume();//so the program will not close instantly

function exitHandler(options, err) {
	if (options.cleanup) tearDown();
	if (err) console.log(err.stack);
	if (options.exit) process.exit();
}

// do something when app is closing
process.on("exit", exitHandler.bind(null, {cleanup: true}));

// catches ctrl+c event
process.on("SIGINT", exitHandler.bind(null, {exit: true}));

// catches uncaught exceptions
process.on("uncaughtException", exitHandler.bind(null, {exit: true}));

module.exports = Wrapper;