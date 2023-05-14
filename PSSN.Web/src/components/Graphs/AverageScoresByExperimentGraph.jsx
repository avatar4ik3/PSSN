import Graph from "../Graph"
const AverageScoresByExperimentGraph = ({ allMaps, title, ...rest }) => {
	function PrepareOne(oneMap) {
		let scoresByStratId = oneMap.map((x) =>
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
		let totalScores = scoresByStratId
			.flat(1)
			.map((x) => x.scores)
			.reduce((s1, s2) => s1 + s2)
		return totalScores / oneMap.length
	}

	function PrepareAll() {
		return [
			{
				name: "AverageScore",
				points: allMaps.map((x, i) => {
					return {
						x: i,
						y: PrepareOne(x),
					}
				}),
			},
		]
	}

	return (
		<div>
			{allMaps ? (
				<Graph
					series={PrepareAll()}
					xLabel={"Номер Запуска"}
					yLabel={"Среднее количество очков в поколении"}
					title={title}
				></Graph>
			) : (
				""
			)}
		</div>
	)
}

export default AverageScoresByExperimentGraph
