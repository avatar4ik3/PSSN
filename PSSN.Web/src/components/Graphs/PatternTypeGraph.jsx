import Graph from "../Graph"

const PatternTypeGraph = ({ strats, ...rest }) => {
	function Prepare() {
		console.log("started ptg")
		let series = []
		function stratPatternName(strat) {
			if (strat.pattern === null) {
				return "None"
			}
			return strat.pattern.name
		}

		function f(name) {
			if (name === "CttPattern") return "CTT Players"
			if (name === "MemePattern") return "Evolving Players"
			return ""
		}
		let alldata = strats.flat(1)
		let names = new Set(alldata.map(stratPatternName))
		console.log(names)
		series = [...names].map((seriesName) => {
			return {
				name: f(seriesName),
				type: "marker",
				points: strats.map((s, i) => {
					return {
						x: i,
						y: s.filter((xx) => stratPatternName(xx) === seriesName).length,
					}
				}),
			}
		})
		console.log(series)
		return series
	}

	return (
		<div>
			{strats ? (
				<Graph
					series={Prepare()}
					xLabel={"Номер поколения"}
					yLabel={"Количество игроков в популяции"}
				></Graph>
			) : (
				""
			)}
		</div>
	)
}

export default PatternTypeGraph
