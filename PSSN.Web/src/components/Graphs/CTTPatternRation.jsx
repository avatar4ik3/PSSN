import Graph from "../Graph"
const PatternRation = ({ allStrats, title, patternName, winThreshold, ...rest }) => {

	const threashold_rate = winThreshold
	console.log("WIN THREASHOLD IS ", threashold_rate)
	function IsPatternCTT(pattern) {
		if (pattern.coeffs.length === 6) {
			return pattern.coeffs[1] === 0 && pattern.coeffs[3] === 1 && pattern.coeffs[4] === pattern.coeffs[5]
		}

		return true;
	}
	function PrepareOne(oneStrat) {
		return oneStrat.filter((x) => {
			let countOfPattern = x.filter((xx) => IsPatternCTT(xx.pattern)).length
			let distrib = countOfPattern / x.length
			let result = distrib >= threashold_rate
			console.debug("strats are", x.map(xx => [xx.behaviors, xx.pattern.coeffs]), "count is", countOfPattern, "total length", x.length, "distrib is", distrib, "threashols is", threashold_rate, "result is", result)
			return result
		}
		).length
	}

	function AvgWinPoints(experimentsWins) {
		let len = experimentsWins.length
		let sum = experimentsWins.map(x => {
			return x.result.map.map(xx => {
				return xx.value.map(xxx => Object.values(xxx.value)).flat().reduce((partialSum, ax) => partialSum + ax, 0)
			}).reduce((partialSum, ax) => partialSum + ax, 0)
		}).reduce((partialSum, ax) => partialSum + ax, 0)
		return (sum / len)  /len ;
	}

	function PrepareAll() {
		return [
			{
				name: `количество CTT`,
				type: "marker",
				// yAxis: "y_left",
				points: allStrats.map((x, i) => {
					return {
						x: x.ds,
						y: PrepareOne(x.strats),
					}
				}),
			},
			// {
			// 	name: "Средний выйгриш в эксперименте'",
			// 	yAxis: "y_right",
			// 	type: "marker",
			// 	points: allStrats.map((x, i) => {
			// 		return {
			// 			x: x.ds,
			// 			y: AvgWinPoints(x.win)
			// 		}
			// 	})
			// }
		]
	}
	function MapToReadableName(name) {
		if (name === "CttPattern") return "CTT"
		if (name === "MemePattern") return "Стратегия с мемами"
		return ""
	}
	return (
		<div>
			{allStrats ? (
				<Graph
					series={PrepareAll()}
					xLabel={`Доля начальной популяции со стратегией ${MapToReadableName(
						patternName
					)}`}
					yLabel={`Кол-во запусков в которых выжили только игроки со стратегией ${MapToReadableName(
						patternName
					)}`}
					title={title}
					// twoAxis={{any:true}}
					// y1_label={""}
					// y2_label={""}
					// y1_max={allStrats[0].length}
					// y1_min={10}
				></Graph>
			) : (
				""
			)}
		</div>
	)
}

export default PatternRation
