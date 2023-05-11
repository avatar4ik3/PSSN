import Graph from "../Graph";

const StratsByNameGraph = ({strats,...rest}) => {
    function Prepare(){
        var names = strats[0].map(x => x.name)

        let series = names.map(stratName => {
            return {
                name : stratName,
                points: strats.map((x,i) => {
                    return {
                        x : i,
                        y : x.filter(xx => xx.name === stratName).length 
                    }
                })
            }
        })
        return series
    }
    
    return (<div>
        {strats ? <Graph series={Prepare()}></Graph> : ""}
    </div>)
}

export default StratsByNameGraph