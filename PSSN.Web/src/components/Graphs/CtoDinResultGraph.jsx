import Graph from "../Graph"

const CtoDinResultGraph = ({ maps, ...rest }) => {
	function Prepare(){
        console.log(maps)

        function valueToStrat(value){
            if(value === 4 || value == 0){
                return "C"
            }
            return "D"
        }

        function flatOneGeneration(genRes){
            return genRes.map(x => x.value.map(xx => xx.value)).flat(2).map(x => Object.entries(x).map(([k,v]) => valueToStrat(v))).flat(1)
        }
        let flattened  = maps.map(x => flatOneGeneration(x)) 
        let series = []
        series.push({
            name:"C",
            points: flattened.map(x => x.filter(xx => xx === "C").length)
        })
        series.push({
            name:"D",
            points: flattened.map(x => x.filter(xx => xx === "D").length)
        })
        return series
    }
    return <div>{maps ? <Graph series={Prepare()} xLabel={"Номер поколения"} yLabel={"Количество коопераций и предательств в популяции"}/> : ""}</div>
}

export default CtoDinResultGraph
