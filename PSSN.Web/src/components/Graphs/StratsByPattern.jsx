import Graph from "../Graph"
const StratsByPatternGraph = ({ strats, ...rest }) => {
	function Prepare() {
		function toOneBase(strat) {
			if (strat.pattern.name === "CttPattern") {
				return [1, strat.pattern.coeffs[1], 0, 1, 0, strat.pattern.coeffs[4]]
			}
			return strat.pattern.coeffs
		}

		var names = strats.map((x) => x.map(xx => toOneBase(xx).toString())).flat(1)
        var unique = new Set(names)
		let series = [...unique].map((stratPattern) => {
			return {
				name: stratPattern,
				points: strats.map((x, i) => {
					return {
						x: i,
						y: x.filter((xx) => toOneBase(xx).toString() === stratPattern).length,
					}
				}),
			}
		})
		return series
	}

	return <div>{strats ? <Graph series={Prepare()}></Graph> : ""}</div>
}

export default StratsByPatternGraph
