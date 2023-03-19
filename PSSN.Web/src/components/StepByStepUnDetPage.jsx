import React, { useState } from "react"
import axios, { all } from "axios"
import * as qs from "qs"
import Graph from "./Graph"
import Array2DInput from "./Array2DInput"
import { encodeStrategy } from "./DetPage"

const StepByStepUnDetPage = ({ apiHost, ...rest }) => {
	const [chartData, setChartData] = useState(null)
	const [cttChartData, setcttChartData] = useState(null)

	const [commonRequestData, setcommonRequestData] = useState({
		GenerationsCount: 100,
		PopulationSize: 10,
		GenotypeSize: 5,
		GeneMutationChance: 0.3,
		MaxGenLengthForCrossingover: 5,
		K_TournamentSelection: 5,
		DistributionChance: 0.5,
		DeterminatedStrategies: [["CTT"], ["D"]],
		A: [
			[4, 0],
			[6, 1],
		],
	})

	async function playAgainstDeterminatedStrategies(strats, additionalStrats) {
		let stratsWithCtt = Array.from(strats)
		for (const x of additionalStrats) {
			console.log(x)
			stratsWithCtt.push(encodeStrategy(x))
		}
		return axios
			.post(apiHost + "/api/v1/research/against", {
				Strats: stratsWithCtt,
				K_repeated: commonRequestData.GenotypeSize,
				A: commonRequestData.A,
			})
			.then((r) => r.data.result)
	}

	//переписать так, что не использовались рефы
	async function requestInitialStrategies() {
		return axios
			.get(
				apiHost +
					"/api/v1/research/generate?" +
					qs.stringify({
						count: commonRequestData.PopulationSize,
						genCount: commonRequestData.GenotypeSize,
						DistributionChance: commonRequestData.DistributionChance,
					})
			)
			.then((r) => r.data)
	}

	async function getOneGeneration(payload) {
		// console.log("request split started")
		return axios
			.post(apiHost + "/api/v1/research/split", {
				genCount: commonRequestData.GenotypeSize,
				swapChance: commonRequestData.GeneMutationChance,
				crossingCount: commonRequestData.MaxGenLengthForCrossingover,
				selectionGroupSize: commonRequestData.K_TournamentSelection,
				ro: commonRequestData.A,
				strats: payload,
			})
			.then((r) => ({
				gameResult: r.data.gameResult,
				newStrats: r.data.newStrats,
			}))
	}

	async function drawGraph(data) {
		console.log("drawing graph")
		let items = data.map((x) => x.strats)
		let series = []

		function countOf(items, char) {
			return items.map((arr1) =>
				arr1
					.map(
						(item) =>
							Object.entries(item.behaviors).filter(([k, v]) => v == char)
								.length
					)
					.reduce((ca, a) => a + ca)
			)
		}

		function toSeries(items, char) {
			return {
				name: char,
				points: Object.entries(countOf(items, char)).map(([k, v]) => {
					return { x: Number.parseInt(k), y: Number.parseInt(v) }
				}),
			}
		}
		series.push(toSeries(items, "C"))
		series.push(toSeries(items, "D"))

		setChartData(series)
	}
	function* enumerate(iterable) {
		let i = 0
		for (const item of iterable) {
			yield [i, item]
			i++
		}
	}

	async function drawResultSeries(data, additionalStratsNames) {
		let series = []
		additionalStratsNames.forEach((stratName) => {
			series.push({
				name: stratName,
				points: data.map((tree, index) => {
					console.log(tree)
					return {
						x: index,
						y: Object.entries(tree.map[stratName])
							.map(([otherStrat, scoresByRound]) => {
								if (otherStrat !== stratName) {
									return Object.entries(scoresByRound)
										.map(([round, score]) => score)
										.reduce((sum, a) => sum + a, 0.0)
								} else return 0.0
							})
							.reduce((sum, a) => sum + a, 0.0),
					}
				}),
			})
		})

		console.log(series)
		return series
	}
	return (
		<div>
			<div>
				<label>
					Эволюционные алгоритмы в дилемме заключенного. <br />
					Где гены — это конкретное решение (C/D) на каждом этапе игры.
				</label>
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
					} else if (k == "DeterminatedStrategies") {
						return (
							<div key={k}>
								<label>{k}</label>
								{commonRequestData.DeterminatedStrategies.map((arr, idx) => {
									return (
										<input
											key={idx}
											type="text"
											defaultValue={arr}
											onChange={(e) => {
												console.log("started event")
												console.log(e.target.value)
												if (e.target.value.endsWith(",") == false) {
													console.log(arr, idx)
													console.log("editing!")
													const strats = e.target.value
														.split(",")
														.filter((s) => s && s != "" && s != " ")
														.map((s) => s.replace(/\s/g, ""))

													console.log(strats)
													let allArs = commonRequestData.DeterminatedStrategies
													allArs[idx] = strats
													console.log(allArs)
													setcommonRequestData({
														...commonRequestData,
														[k]: allArs,
													})
												}
											}}
										/>
									)
								})}
								<button
									onClick={(e) => {
										let allArs = commonRequestData.DeterminatedStrategies
										allArs.push(["CTT"])
										setcommonRequestData({
											...commonRequestData,
											DeterminatedStrategies: allArs,
										})
									}}
								>
									+
								</button>
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
					setChartData(null)
					setcttChartData(null)
					let resultData = []
					let againstCttData = []
					for (const names in commonRequestData.DeterminatedStrategies) {
						againstCttData.push([])
					}
					let payload = await requestInitialStrategies()
					console.log(payload)
					for (let i = 0; i < commonRequestData.GenerationsCount; ++i) {
						const { gameResult, newStrats } = await getOneGeneration(payload)
						for (const [idx, names] of enumerate(
							commonRequestData.DeterminatedStrategies
						)) {
							const againstCttResult = await playAgainstDeterminatedStrategies(
								payload,
								names
							)
							againstCttData[idx].push(againstCttResult)
						}

						payload = newStrats
						resultData.push(gameResult)
					}

					let series = []
					for (const [idx, names] of enumerate(
						commonRequestData.DeterminatedStrategies
					)) {
						console.log(againstCttData[idx])
						series.push(
							await drawResultSeries(
								againstCttData[idx],
								commonRequestData.DeterminatedStrategies[idx].concat(
									payload.map((x) => x.name)
								)
							)
						)
					}
					console.log(series)
					setcttChartData(series)
					drawGraph(resultData)

					// console.log(chartData)
				}}
			>
				Process
			</button>
			{chartData ? (
				<>
					{" "}
					<label>
						Количества генов типа C и генам типа D в популяции по поколениям
					</label>
					<Graph series={chartData} />
				</>
			) : null}

			{cttChartData ? (
				<div>
					{cttChartData.map((s, idx) => (
						<>
							<label>
								Количество очков, набранное детерминированными стратегиями при
								подселение в популяцию по поколениям
							</label>
							<Graph series={s} key={idx} />
						</>
					))}
				</div>
			) : null}
		</div>
	)
}

export default StepByStepUnDetPage
