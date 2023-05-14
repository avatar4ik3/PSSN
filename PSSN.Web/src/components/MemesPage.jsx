import { useState } from "react"
import Array2DInput from "./Array2DInput"
import axios from "axios"
import qs from "qs"
import CToDGraph from "./Graphs/CToDGraph"
import PatternTypeGraph from "./Graphs/PatternTypeGraph"
import CtoDinResultGraph from "./Graphs/CtoDinResultGraph"
import StratsByNameGraph from "./Graphs/StratsByNameGraph"
import StratsByPatternGraph from "./Graphs/StratsByPattern"
import MemesScoresGraph from "./Graphs/MemesScoresGraph"
import PatternRationGraph from "./Graphs/PatternRationGraph"
const MemesPage = ({ apiHost, ...rest }) => {
	const [commonRequestData, setcommonRequestData] = useState({
		GenerationsCount: 100,
		PopulationSize: 10,
		GenotypeSize: 5,
		GeneMutationChance: 0.05,
		MaxGenLengthForCrossingover: 5,
		K_TournamentSelection: 5,
		StrategyTypeDistributionChance: 0.5,
		DistributionChance: 0.5,
		UseCrossingOver: 1,
		A: [
			[4, 0],
			[6, 1],
		],
	})

	const [ctd, setctd] = useState(null)
	const [rs, setrs] = useState(null)
	function GetInitialStrategies() {
		console.log(apiHost, "a")
		return axios
			.get(
				apiHost +
					"/api/v1/memes/generate?" +
					qs.stringify({
						Count: commonRequestData.PopulationSize,
						GenotypeSize: commonRequestData.GenotypeSize,
						Distr: commonRequestData.StrategyTypeDistributionChance,
						RandomSeed: null,
					})
			)
			.then((response, err) => {
				return response.data
			})
	}
	function GetOneGeneration(payload) {
		return axios
			.post(apiHost + "/api/v1/memes/research-single", {
				genCount: commonRequestData.GenotypeSize,
				swapChance: commonRequestData.GeneMutationChance,
				selectionGroupSize: commonRequestData.K_TournamentSelection,
				payofss: commonRequestData.A,
				models: payload,
				useCrossingOver : commonRequestData.UseCrossingOver === 1 ? true : false,
				RandomSeed : null
			})
			.then((r) => ({
				gameResult: r.data.gameResult,
				newStrats: r.data.newStrats,
			}))
	}
	return (
		<div>
			<div>
				{Object.entries(commonRequestData).map(([k, v]) => {
					if (k == "A") {
						return (
							<div key={k}>
								<label>{k}</label>
								<Array2DInput
									index1={0}
									index2={0}
									v={v}
									set={setcommonRequestData}
									data={commonRequestData}
								/>
								<Array2DInput
									index1={0}
									index2={1}
									v={v}
									set={setcommonRequestData}
									data={commonRequestData}
								/>
								<Array2DInput
									index1={1}
									index2={0}
									v={v}
									set={setcommonRequestData}
									data={commonRequestData}
								/>
								<Array2DInput
									index1={1}
									index2={1}
									v={v}
									set={setcommonRequestData}
									data={commonRequestData}
								/>
							</div>
						)
					} else {
						return (
							<div key={k}>
								<label>{k}</label>
								<input
									type="number"
									value={v}
									onChange={(e) => {
										setcommonRequestData({
											...commonRequestData,
											[k]: e.target.value,
										})
									}}
								/>
							</div>
						)
					}
				})}
			</div>

			<button
				onClick={async (e) => {
					e.preventDefault()
					let gameResults = []
					let strategies = []
					let payload = await GetInitialStrategies()

					for (let i = 0; i < commonRequestData.GenerationsCount; ++i) {
						const { gameResult, newStrats } = await GetOneGeneration(payload)
						gameResults.push(gameResult.result.map)
						strategies.push(gameResult.strats)
						payload = newStrats
					}
					setctd(strategies)
					setrs(gameResults)
					let a = 2
				}}
			>
				Calculate
			</button>
			<CtoDinResultGraph maps={rs}></CtoDinResultGraph>
			<PatternTypeGraph strats={ctd}></PatternTypeGraph>
			{/* <StratsByNameGraph strats={ctd}></StratsByNameGraph> */}
			{/* <StratsByPatternGraph strats={ctd}></StratsByPatternGraph> */}
			<MemesScoresGraph maps={rs} strats={ctd}></MemesScoresGraph>
			<PatternRationGraph
				strats={ctd}
				count={commonRequestData.PopulationSize}
			></PatternRationGraph>
		</div>
	)
}

export default MemesPage
