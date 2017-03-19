const express = require("express");
const app = express();

const BL = require("./wrapper");
const wrapper = new BL;

let gameState = null;
let hardware = null;

app.get("/", (req, res) => {
	console.log(req.connection.remoteAddress);
	res.status(200).send("Running");
});

app.get("/register", (req, res) => {
	wrapper.sendRegister().then(() => {
		res.status(200).send("Success");
	}).catch((err) => {
		res.status(400).send("Failure: " + JSON.stringify(err));
	});
});

app.get("/start", (req, res) => {
	wrapper.sendStart().then(() => {
		res.status(200).send("Success");
	}).catch((err) => {
		res.status(400).send("Failure: " + JSON.stringify(err));
	});
});

app.get("/command/:command/:value?", (req, res) => {
	wrapper.sendControl(req.params.command, parseInt(req.params.value));
	res.status(200).send("Success");
});

app.get("/finished", (req, res) => {
	res.status(200).send(wrapper.isFinished());
});

app.get("/gamestate", (req, res) => {
	res.status(200).send(JSON.stringify(gameState));
});

app.get("/hardware", (req, res) => {
	res.status(200).send(JSON.stringify(hardware));
});

wrapper.onGamestateUpdate().then((state) => {
	gameState = state;
});

wrapper.onHardwareUpdate().then((hw) => {
	hardware = hw;
});

app.listen(8080, () => {
	console.log("Server running at: http://localhost:8080");
});