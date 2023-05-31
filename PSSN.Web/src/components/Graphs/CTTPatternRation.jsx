import Graph from "../Graph"
const PatternRation = ({ allStrats, title, patternName, ...rest }) => {

	function PrepareOne(oneStrat) {
        return oneStrat.filter(x => x.every(xx => xx.pattern.name === patternName)).length
	}

	function PrepareAll() {
		return [
			{
				name: `количество ${MapToReadableName(patternName)}`,
				points: allStrats.map((x, i) => {
					return {
						x: x.ds,
						y: PrepareOne(x.strats),
					}
				}),
			},
		]
	}
    function MapToReadableName(name){
        if(name === "CttPattern") return "CTT"
        if(name === "MemePattern" ) return "Стратегия с мемами"
        return ""
    }
	return (
		<div>
			{allStrats ? (
				<Graph
					series={PrepareAll()}
					xLabel={`Доля начальной популяции со стратегией ${MapToReadableName(patternName)}`}
					yLabel={`Кол-во запусков в которых выжили только игроки со стратегией ${MapToReadableName(patternName)}`}
					title={title}
				></Graph>
			) : (
				""
			)}
		</div>
	)
}

export default PatternRation
