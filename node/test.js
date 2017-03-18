const PF = require("pathfinding");

function gcun(gamestate){
	let robot = gamestate.robot;
	let points = gamestate.points.filter((p) => {
		return !p.collected && p.score > 0;
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

	return path;
}

var x = { robot: { x: 576, r: 25, y: 417 }, points:
[ { x: 32, collected: false, r: 5, y: 545, score: -1 },
    { x: 22, collected: false, r: 5, y: 651, score: -1 },
    { x: 711, collected: false, r: 5, y: 873, score: -1 },
    { x: 750, collected: false, r: 5, y: 664, score: -1 },
    { x: 722, collected: false, r: 5, y: 692, score: -1 },
    { x: 812, collected: false, r: 5, y: 133, score: 1 },
    { x: 136, collected: false, r: 5, y: 736, score: 1 },
    { x: 824, collected: false, r: 5, y: 511, score: 1 },
    { x: 1170, collected: false, r: 5, y: 916, score: 1 },
    { x: 933, collected: false, r: 5, y: 110, score: 1 },
    { x: 368, collected: false, r: 5, y: 596, score: 1 },
    { x: 226, collected: false, r: 5, y: 53, score: 1 },
    { x: 388, collected: false, r: 5, y: 398, score: 1 },
    { x: 269, collected: false, r: 5, y: 414, score: 1 },
    { x: 1240, collected: false, r: 5, y: 33, score: 1 },
    { x: 1048, collected: false, r: 5, y: 13, score: 1 },
    { x: 224, collected: false, r: 5, y: 230, score: 1 },
    { x: 176, collected: false, r: 5, y: 394, score: 1 },
    { x: 877, collected: false, r: 5, y: 573, score: 1 },
    { x: 480, collected: false, r: 5, y: 675, score: 1 },
    { x: 168, collected: false, r: 5, y: 827, score: 1 },
    { x: 881, collected: false, r: 5, y: 825, score: 1 },
    { x: 791, collected: false, r: 5, y: 161, score: 1 },
    { x: 121, collected: false, r: 5, y: 857, score: 1 },
    { x: 193, collected: false, r: 5, y: 641, score: 1 },
    { x: 24, collected: false, r: 5, y: 490, score: 1 },
    { x: 307, collected: false, r: 5, y: 136, score: 1 },
    { x: 427, collected: false, r: 5, y: 337, score: 1 },
    { x: 927, collected: false, r: 5, y: 823, score: 1 },
    { x: 743, collected: false, r: 5, y: 120, score: 1 },
    { x: 870, collected: false, r: 5, y: 935, score: 1 },
    { x: 903, collected: false, r: 5, y: 16, score: 1 },
    { x: 710, collected: false, r: 5, y: 592, score: 1 },
    { x: 375, collected: false, r: 5, y: 255, score: 1 },
    { x: 580, collected: false, r: 5, y: 305, score: 1 },
    { x: 757, collected: false, r: 5, y: 930, score: 1 },
    { x: 1035, collected: false, r: 5, y: 415, score: 1 },
    { x: 609, collected: false, r: 5, y: 881, score: 1 },
    { x: 523, collected: false, r: 5, y: 775, score: 1 },
    { x: 147, collected: false, r: 5, y: 756, score: 1 },
    { x: 495, collected: false, r: 5, y: 644, score: 1 },
    { x: 455, collected: false, r: 5, y: 723, score: 1 },
    { x: 142, collected: false, r: 5, y: 742, score: 1 },
    { x: 179, collected: false, r: 5, y: 117, score: 1 },
    { x: 884, collected: false, r: 5, y: 762, score: 1 },
    { x: 479, collected: false, r: 5, y: 524, score: 1 },
    { x: 519, collected: false, r: 5, y: 359, score: 1 },
    { x: 298, collected: false, r: 5, y: 108, score: 1 },
    { x: 836, collected: false, r: 5, y: 182, score: 1 },
    { x: 440, collected: false, r: 5, y: 476, score: 1 } ],
	world: { x_max: 1280, y_max: 960 } };



	console.log(closestWay(x));