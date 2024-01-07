import { useEffect, useState } from "react"
import Array2DInput from "./Array2DInput"
import axios from "axios"
import qs from "qs"
import AverageScoresByExperimentGraph from "./Graphs/AverageScoresByExperimentGraph"
import CToDGraph from "./Graphs/CToDGraph"
import PatternRation from "./Graphs/CTTPatternRation"
import JsonSerializationComponent from "./Serialization/JsonSerializationComponent"

const PatternRationStatisticPage = ({ apiHost, ...rest }) => {
	const [commonRequestData, setcommonRequestData] = useState({
		GenerationsCount: 50,
		PopulationSize: 10,
		GenotypeSize: 8,
		GeneMutationChance: 0.05,
		K_TournamentSelection: 4,
		StrategyTypeDistributionChanceStep: 0.2,
		CountOfExperiments: 50,
		A: [
			[4, 0],
			[6, 1],
		],
	})
	const [allstratsCO, setallstratsCO] = useState(null)

	function GetInitialStrategies(distr) {
		console.log(apiHost)
		return axios
			.get(
				apiHost +
					"/api/v1/memes/generate?" +
					qs.stringify({
						Count: commonRequestData.PopulationSize,
						GenotypeSize: commonRequestData.GenotypeSize,
						Distr: distr,
					})
			)
			.then((response, err) => {
				console.log(response.data)
				return response.data
			})
	}
	function GetOneGeneration(payload, co) {
		return axios
			.post(apiHost + "/api/v1/memes/research-single", {
				genCount: commonRequestData.GenotypeSize,
				swapChance: commonRequestData.GeneMutationChance,
				selectionGroupSize: commonRequestData.K_TournamentSelection,
				payofss: commonRequestData.A,
				models: payload,
				UseCrossingOver: co,
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
					let localstratsCO = []
                    let step = Number(commonRequestData.StrategyTypeDistributionChanceStep)
					for (
						let distr = 0.0;
						distr <= 1;
						distr += step
					) {
                        let currentLastStrat = [];
						for (
							let expIndex = 0;
							expIndex < commonRequestData.CountOfExperiments;
							++expIndex
						) {
							let payload = await GetInitialStrategies(distr)

							for (let i = 0; i < commonRequestData.GenerationsCount; ++i) {
								const { newStrats } = await GetOneGeneration(
									payload,
									true
								)
								payload = newStrats
							}
                            currentLastStrat.push(payload)
						}
						localstratsCO.push({strats:currentLastStrat,ds: distr})
					}
					setallstratsCO(localstratsCO)
				}}
			>
				Run
			</button>
			<JsonSerializationComponent
				data={[
					{ commonRequestData, setcommonRequestData },
					{ allstratsCO, setallstratsCO }
				]}
			/>
            <PatternRation allStrats={allstratsCO} title={""} patternName={"CttPattern"}></PatternRation>
		</div>
	)
}

export default PatternRationStatisticPage
