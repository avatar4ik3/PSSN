import Graph from "../Graph"

const MemesScoresGraph = ({ maps, strats, ...rest }) => {
	function Prepare() {
		console.log("started ptg")
		function toOneBase(strat) {
			if (strat.pattern.name === "CttPattern") {
				return [1, strat.pattern.coeffs[1], 0, 1, 0, strat.pattern.coeffs[4]]
			}
			return strat.pattern.coeffs
		}
		let series = []
		let scoresByStratId = maps.map((x) =>
			x.map((xx) => {
				return {
					key: xx.key,
					scores: xx.value
						.map((xxx) =>
							Object.entries(xxx.value)
								.map(([k, v]) => v)
								.reduce((s1, s2) => s1 + s2)
						)
						.reduce((s1, s2) => s1 + s2),
				}
			})
		)
		let betterMap = scoresByStratId.map((e, i) => [e, strats[i]])
		let realMap = betterMap
			.map((x) =>
				x[0].map((xx, i) => {
					return {
						key: x[1][i],
						scores: xx.scores,
					}
				})
			)
			.map((x) =>
				x.map((xx) => {
					return {
						key: toOneBase(xx.key).toString(),
						scores: xx.scores,
					}
				})
			)

		let alldata = realMap.flat(1).map(x => x.key)
		let names = new Set(alldata)
		console.log(names)
		series = [...names].map((seriesName) => {
			return {
				name: seriesName,
				points: realMap.map((s, i) => {
					return {
						x: i,
						y: s.filter(xx => xx.key === seriesName).map(xx => xx.scores).reduce((s1,s2) => s1+s2,0),
					}
				}),
			}
		})
		console.log(series)
		return series
	}

	return <div>{maps && strats ? <Graph series={Prepare()} xLabel={"Номер поколения"} yLabel={"Количество очков"}></Graph> : ""}</div>
}

export default MemesScoresGraph
