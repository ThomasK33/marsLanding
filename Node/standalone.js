const PF = require("pathfinding");
const wrapper = new (require("./wrapper"));

function gcun(gamestate){
	let robot = gamestate.robot;

	let unpassable = gamestate.points.filter((p) => {
		return p.score < 0;
	});

	let upobj = {};

	unpassable.forEach((u) => {
		let ua = radiusAroundPixel(u.x, u.y, u.r);

		ua.forEach((uap) => {
			if (!(upobj[uap.x]))
				upobj[uap.x] = [uap.y];
			else
				upobj[uap.x].push(uap.y);
		});
	});

	let points = gamestate.points.filter((p) => {
		return !p.collected && p.score > 0 && (upobj[p.x] ? (upobj[p.x].indexOf(p.y) == -1) : true);
	});

	let rp = {
		x: range(robot.x, robot.r),
		y: range(robot.y, robot.r)
	};

	for (let k = 0; k < points.length; k++) {
		let p = points[k];

		let d = Infinity;

		let pp = {
			x: range(p.x, p.r),
			y: range(p.y, p.r)
		};

		for (let i = 0; i < 2; i++) {
			for (let j = 0; j < 2; j++) {
				let dis = distance(rp.x[i], pp.x[j], rp.y[i], pp.y[j]);

				if (dis < d)
					d = dis;
			}
		}

		points[k]["distance"] = d;
	}

	let globalMin = {distance: Infinity};

	points.forEach((p) => {
		if (p.distance < globalMin.distance)
			globalMin = p;
	});

	return globalMin;
}

function range(val, r) {
	return [val-r, val+r];
}

function distance (x1, x2, y1, y2) {
	let deltaX = Math.pow((x2 - x1), 2);
	let deltaY = Math.pow((y2 - y1), 2);

	return Math.sqrt(deltaX + deltaY);
}

function radiusAroundPixel(x, y, r) {
	let x_min = x-r;
	let x_max = x+r;
	let y_min = y-r;
	let y_max = y+r;

	let arr = [];

	for (let dx = x_min; dx < x_max + 1; dx++) {
		for (let dy = y_min; dy < y_max + 1; dy++) {
			arr.push({x: dx, y: dy});
		}
	}

	return arr;
}

function closestWay(gamestate) {
	let closestNode = gcun(gamestate);
	let world = gamestate.world;
	let robot = gamestate.robot;

	let grid = new PF.Grid(world.x_max, world.y_max);

	let unpassable = gamestate.points.filter((p) => {
		return p.score < 0;
	});

	unpassable.forEach((u) => {
		let ua = radiusAroundPixel(u.x, u.y, u.r);

		ua.forEach((uap) => {
			grid.setWalkableAt(uap.x, uap.y, false);
		});
	});

	let finder = new PF.BiBestFirstFinder({diagonalMovement: false});

	let path = finder.findPath(robot.x, robot.y, closestNode.x, closestNode.y, grid);

	return {path: path, cn: closestNode};
}

function navigateToCW (cw){
	let path = cw.path;
	let closestNode = cw.cn;

	console.log(path);
}

wrapper.onGamestateUpdate().then((gamestate) => {
	let cw = closestWay(gamestate);
	navigateToCW(cw);
});

// Register at the counter party and immediately start a session
wrapper.sendRegister().then(() => wrapper.sendStart());