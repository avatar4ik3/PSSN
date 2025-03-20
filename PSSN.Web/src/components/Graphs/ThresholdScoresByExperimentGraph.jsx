import Graph from "../Graph"

const ThresholdScoresByExperimentGraph = ({ allMaps, title, threshold, populationSize, ...rest }) => {
	const population = Number(populationSize)
	const thresh = Number(threshold)

	function PrepareOne(oneMap) {
		// console.log(oneMap)
		let lastGeneration = oneMap[oneMap.length - 1]
		let lastGenerationGameLength = Object.entries(lastGeneration[0].value[0].value).length
		let lastGenerationScores = lastGeneration.map((x) => 
			x.value.map((xx) => Object.entries(xx.value).map(([k, v]) => v).reduce((s1, s2) => s1 + s2)
		)
		.reduce((s1, s2) => s1 + s2)).reduce((s1, s2) => s1 + s2)
		// console.log("last generation", lastGeneration, "game length is", lastGenerationGameLength, "scores are", lastGenerationScores)
		return lastGenerationScores / lastGenerationGameLength
	}

	function PrepareAll() {
		return [
			{
				name: "Количество популяций",
				type: 'marker',
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
					yLabel={"Количество стратегий которые планочку то самое это самое."}
					title={title}
				></Graph>
			) : (
				""
			)}
		</div>
	)
}

export default ThresholdScoresByExperimentGraph;