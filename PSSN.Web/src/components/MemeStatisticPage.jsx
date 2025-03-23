import { useEffect, useState } from "react"
import Array2DInput from "./Array2DInput"
import axios from "axios"
import qs from "qs"
import AverageScoresByExperimentGraph from "./Graphs/AverageScoresByExperimentGraph"
import JsonSerializationComponent from "./Serialization/JsonSerializationComponent"
import ThresholdScoresByExperimentGraph from "./Graphs/ThresholdScoresByExperimentGraph"


const LastPopulationBreackdown = ({allstrats, allMaps, titleMaps, titleStrats, ...rest }) => {
	const PrepareStrats = () => {
		if(!allstrats) return <div></div>

		let lastPopulations = allstrats
			.map(
				(x, i) => x[x.length - 1]
				.map(xx => xx.pattern.coeffs.toString())
				.sort()
			)
			console.error(titleStrats, lastPopulations)
		return <div></div>
	}

	const PrepareMaps = () => {
		if(!allMaps) return <div></div>
		let lastPopuldation = (score) => allMaps
		.map(
			x => x[x.length - 1]
			.map(xx => xx.value
				.map(xxx => Object.entries(xxx.value).filter(xxxx => xxxx[1] === score).length)
				.reduce((x,y) => x + y)
			).reduce((x,y) => x + y) / 2
		)
		console.error(titleMaps, "CC", lastPopuldation(4), "DD", lastPopuldation(1))

		return <div></div>
	}

	return <div>
		<PrepareStrats/>
		<PrepareMaps/>
	</div>
}



const MemeStatisticPage = ({ apiHost, ...rest }) => {
	const [commonRequestData, setcommonRequestData] = useState({
		GenerationsCount: 50,
		PopulationSize: 10,
		GenotypeSize: 8,
		GeneMutationChance: 0.05,
		K_TournamentSelection: 4,
		StrategyTypeDistributionChance: 0,
		CountOfExperiments: 50,
		RandomSeed: 15,
		A: [
			[4, 0],
			[6, 1],
		],
		GameProlongationChance: 0,
		MaxGameProlongationLength: 10,
		maxLengthThreshold: 0.95
	})

	const [allmapsCO, setallmapsCO] = useState(null)
	const [allmapsNCO, setallmapsNCO] = useState(null)
	const [allstratsCO, setallstratsCO] = useState(null)
	const [allstratsNCO, setallstratsNCO] = useState(null)

	function GetInitialStrategies(seed) {
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
				// console.log(response.data)
				return response.data
			})
	}
	function GetOneGeneration(payload, co, seed) {
		return axios
			.post(apiHost + "/api/v1/memes/research-single", {
				genCount: commonRequestData.GenotypeSize,
				swapChance: commonRequestData.GeneMutationChance,
				selectionGroupSize: commonRequestData.K_TournamentSelection,
				payofss: commonRequestData.A,
				models: payload,
				UseCrossingOver: co,
				RandomSeed: seed,
				GameProlongationChance: commonRequestData.GameProlongationChance,
				MaxGameProlongationLength: commonRequestData.MaxGameProlongationLength
			})
			.then((r) => ({
				gameResult: r.data.gameResult,
				newStrats: r.data.newStrats,
				gameLength: r.data.gameLength
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
					for (
						let expIndex = 0;
						expIndex < commonRequestData.CountOfExperiments;
						++expIndex
					) {
						let gameResults = []
						let strategies = []
						let payload = await GetInitialStrategies(
							commonRequestData.RandomSeed + expIndex
						)

						for (let i = 0; i < commonRequestData.GenerationsCount; ++i) {
							const { gameResult, newStrats, gameLength } = await GetOneGeneration(
								payload,
								true,
								commonRequestData.RandomSeed + i + expIndex
							)
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
					for (
						let expIndex = 0;
						expIndex < commonRequestData.CountOfExperiments;
						++expIndex
					) {
						let gameResults = []
						let strategies = []
						let payload = await GetInitialStrategies(
							commonRequestData.RandomSeed + expIndex
						)

						for (let i = 0; i < commonRequestData.GenerationsCount; ++i) {
							const { gameResult, newStrats } = await GetOneGeneration(
								payload,
								false,
								commonRequestData.RandomSeed + i + expIndex
							)
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
			<JsonSerializationComponent
				data={[
					{ commonRequestData, setcommonRequestData },
					{ allmapsCO, setallmapsCO },
					{ allmapsNCO, setallmapsNCO },
					{ allstratsCO, setallstratsCO },
					{ allstratsNCO, setallstratsNCO },
				]}
			/>
			<AverageScoresByExperimentGraph
				allMaps={allmapsCO}
				title={"С применением оператора кроссинговера"}
			/>
			<AverageScoresByExperimentGraph
				allMaps={allmapsNCO}
				title={"Без применения оператора кроссинговера"}
			/>
			<ThresholdScoresByExperimentGraph
				allMaps={allmapsCO}
				populationSize={commonRequestData.PopulationSize}
				threshold={commonRequestData.maxLengthThreshold}
				title={"ЧЕТАТАМ ТРЕШХОЛД С применением оператора кроссинговера"}
			/>
			<ThresholdScoresByExperimentGraph
				allMaps={allmapsNCO}
				populationSize={commonRequestData.PopulationSize}
				threshold={commonRequestData.maxLengthThreshold}
				title={"ЧЕТАТАМ ТРЕШХОЛД Без применением оператора кроссинговера"}
			/>
			<LastPopulationBreackdown
				allMaps={allmapsCO}
				allstrats={allstratsCO}
				titleStrats={"Разбор генов последних популяций исследования С КРОСИНГОВЕРОМ"}
				titleMaps={"Количество уникальных пар последних популяций С КРОСИНГОВЕРОМ"}
			/>
			<LastPopulationBreackdown
				allMaps={allmapsNCO}
				allstrats={allstratsNCO}
				titleStrats={"Разбор генов последних популяций исследования БЕЗ КРОСИНГОВЕРА"}
				titleMaps={"Количество уникальных пар последних популяций БЕЗ КРОСИНГОВЕРА"}
				/>
			{/* <PatternRation allStrats={allstratsCO} title={"Доля CTT паттерна в последней популяции"} patternName={"CttPattern"}></PatternRation> */}
			{/* <CToDGraph stratsByRounds={allstrats[0]}></CToDGraph> */}
		</div>
	)
}

export default MemeStatisticPage
