import Graph from "../Graph"

const ThresholdScoresByExperimentGraph = ({ allMaps, title, threshold, populationSize, ...rest }) => {
    console.log("asdasdasd")
    const population = Number(populationSize)
	const thresh = Number(threshold)

    function PrepareOne(oneMap) {
        console.log(oneMap)
		let gameLength = Object.entries(oneMap[0][0].value[0].value).length
		//2*a11*((PopulationSize^2)/2) * GameLength * threshold

		let maxScore = 4 * 2 * ((Math.pow(population,2)) / 2) * gameLength * thresh;
        console.log("results for population are", oneMap, "game length is", gameLength, "max score", maxScore)
		let scoresByStratId = oneMap.map((x) =>
			x.map((xx) => {

				return xx.value
						.map((xxx) =>
							Object.entries(xxx.value)
								.map(([k, v]) => v)
								.reduce((s1, s2) => s1 + s2)
						)
						.reduce((s1, s2) => s1 + s2)}
				
			).reduce((s1, s2) => s1 + s2) >= maxScore
		)
		let totalScores = scoresByStratId.filter(x => x === true)
		return totalScores.length 
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