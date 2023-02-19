import React, { useState } from "react"
import axios from "axios"
import * as qs from "qs"
import Graph from "./Graph"
import Array2DInput from "./Array2DInput"

const StepByStepUnDetPage = ({ apiHost, ...rest }) => {
	const [chartData, setChartData] = useState(null)

	const [commonRequestData, setcommonRequestData] = useState({
		GenerationsCount: 100,
		PopulationSize: 10,
		GenotypeSize: 5,
		GeneMutationChance: 0.3,
		MaxGenLengthForCrossingover: 5,
		K_TournamentSelection: 5,
		A: [
			[4, 0],
			[6, 1],
		],
	})

	//переписать так, что не использовались рефы
	async function requestInitialStrategies() {
		return axios
			.get(
				apiHost +
					"/api/v1/research/generate?" +
					qs.stringify({
						count: commonRequestData.PopulationSize,
						genCount: commonRequestData.GenotypeSize,
					})
			)
			.then((r) => r.data)
	}

	async function getOneGeneration(payload) {
		console.log("request split started")
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

	return (
		<div>
			<div>
				<label>
					Эволюционные алгоритмы в дилемме заключенного. <br />
					Где гены — это конкретное решение (C/D) на каждом этапе игры.
				</label>
				{Object.entries(commonRequestData).map(([k, v]) => {
					return k != "A" ? (
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
					) : (
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
				})}
			</div>
			<button
				onClick={async (e) => {
					e.preventDefault()
					setChartData(null)
					let resultData = []
					let payload = await requestInitialStrategies()

					console.log(payload)
					for (let i = 0; i < commonRequestData.GenerationsCount; ++i) {
						const { gameResult, newStrats } = await getOneGeneration(payload)
						console.log(gameResult, newStrats)
						payload = newStrats
						resultData.push(gameResult)
					}

					drawGraph(resultData)

					console.log(chartData)
				}}
			>
				Process
			</button>

			{chartData ? <Graph series={chartData} /> : null}
		</div>
	)
}

export default StepByStepUnDetPage
