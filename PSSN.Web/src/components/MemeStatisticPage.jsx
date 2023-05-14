import { useEffect, useState } from "react"
import Array2DInput from "./Array2DInput"
import axios from "axios"
import qs from "qs"
import AverageScoresByExperimentGraph from "./Graphs/AverageScoresByExperimentGraph"
import CToDGraph from "./Graphs/CToDGraph"

const MemeStatisticPage = ({ apiHost, ...rest }) => {
	const [commonRequestData, setcommonRequestData] = useState({
		GenerationsCount: 50,
		PopulationSize: 10,
		GenotypeSize: 8,
		GeneMutationChance: 0.05,
		K_TournamentSelection: 4,
		StrategyTypeDistributionChance: 0,
		CountOfExperiments: 50,
		RandomSeed : 15,
		A: [
			[4, 0],
			[6, 1],
		],
	})

	const [allmapsCO, setallmapsCO] = useState(null)
	const [allmapsNCO, setallmapsNCO] = useState(null)
	const [allstratsCO, setallstratsCO] = useState(null)
	const [allstratsNCO, setallstratsNCO] = useState(null)

	function GetInitialStrategies(seed) {
		console.log(apiHost)
		return axios
			.get(
				apiHost +
					"/api/v1/memes/generate?" +
					qs.stringify({
						Count: commonRequestData.PopulationSize,
						GenotypeSize: commonRequestData.GenotypeSize,
						Distr: commonRequestData.StrategyTypeDistributionChance,
						RandomSeed: seed,
					})
			)
			.then((response, err) => {
				console.log(response.data)
				return response.data
			})
	}
	function GetOneGeneration(payload,co,seed) {
		return axios
			.post(apiHost + "/api/v1/memes/research-single", {
				genCount: commonRequestData.GenotypeSize,
				swapChance: commonRequestData.GeneMutationChance,
				selectionGroupSize: commonRequestData.K_TournamentSelection,
				payofss: commonRequestData.A,
				models: payload,
				UseCrossingOver : co,
				RandomSeed : seed
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
					//CO
					let localmapsCO = []
					let localstratsCO = []
					for (let expIndex = 0; expIndex < commonRequestData.CountOfExperiments; ++expIndex) {
						let gameResults = []
						let strategies = []
						let payload = await GetInitialStrategies(commonRequestData.RandomSeed + expIndex)

						for (let i = 0; i < commonRequestData.GenerationsCount; ++i) {
							const { gameResult, newStrats } = await GetOneGeneration(payload,true,commonRequestData.RandomSeed + i + expIndex)
							gameResults.push(gameResult.result.map)
							strategies.push(gameResult.strats)
							payload = newStrats
						}
						localmapsCO.push(gameResults)
						localstratsCO.push(strategies)
					}
					setallmapsCO(localmapsCO)
					setallstratsCO(localstratsCO)
					//NCO
					let localmapsNCO = []
					let localstratsNCO = []
					for (let expIndex = 0; expIndex < commonRequestData.CountOfExperiments; ++expIndex) {
						let gameResults = []
						let strategies = []
						let payload = await GetInitialStrategies(commonRequestData.RandomSeed + expIndex)

						for (let i = 0; i < commonRequestData.GenerationsCount; ++i) {
							const { gameResult, newStrats } = await GetOneGeneration(payload,false,commonRequestData.RandomSeed + i + expIndex)
							gameResults.push(gameResult.result.map)
							strategies.push(gameResult.strats)
							payload = newStrats
						}
						localmapsNCO.push(gameResults)
						localstratsNCO.push(strategies)
					}
					setallmapsNCO(localmapsNCO)
					setallstratsNCO(localstratsNCO)
				}}
			>
				Run
			</button>
            <AverageScoresByExperimentGraph allMaps={allmapsCO} title={"С применением оператора кроссинговера"}/>
            <AverageScoresByExperimentGraph allMaps={allmapsNCO} title={"Без применения оператора кроссинговера"}/>
            {/* <CToDGraph stratsByRounds={allstrats[0]}></CToDGraph> */}
		</div>
	)
}

export default MemeStatisticPage
