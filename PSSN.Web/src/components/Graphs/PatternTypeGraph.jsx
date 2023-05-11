import Graph from "../Graph"

const PatternTypeGraph = ({ strats, ...rest }) => {
	function Prepare() {
        console.log("started ptg")
        let series = []
        function stratPatternName(strat){
            if(strat.pattern === null){
                return "None"
            }
            return strat.pattern.name
        }
        let alldata =  strats.flat(1)
        let names = new Set(alldata.map(stratPatternName))
        console.log(names)
        series =  [...names].map(seriesName => {
            return {
                name: seriesName ,
                points : strats.map((s,i) => {
                    return {
                        x: i,
                        y: s.filter(xx => stratPatternName(xx) === seriesName).length
                    }
                })
            }
        })
        console.log(series)
        return series
    }

	return <div>{strats ? <Graph series={Prepare()} key={"1"}></Graph> : ""}</div>
}

export default PatternTypeGraph
