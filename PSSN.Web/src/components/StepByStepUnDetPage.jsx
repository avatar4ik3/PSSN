import React, { useState } from "react"
import axios from "axios"
import * as qs from "qs"
import Graph from "./Graph"

const StepByStepUnDetPage = ({ apiHost, ...rest }) => {
	const [chartData, setChartData] = useState(null)

	const [commonRequestData, setcommonRequestData] = useState({
		population: 100,
		count: 10,
		genCount: 5,
		swapChance: 0.3,
		crossingCount: 5,
		selectionGoupSize: 5,
		ro: [
			[6, 1],
			[4, 0],
		],
	})

	//переписать так, что не использовались рефы
	async function requestInitialStrategies() {
		return axios
			.get(
				apiHost +
					"/api/v1/research/generate?" +
					qs.stringify({
						count: commonRequestData.count,
						genCount: commonRequestData.genCount,
					})
			)
			.then((r) => r.data)
	}

	async function getOneGeneration(payload) {
		console.log("request split started")
		return axios
			.post(apiHost + "/api/v1/research/split", {
				genCount: commonRequestData.genCount,
				swapChance: commonRequestData.swapChance,
				crossingCount: commonRequestData.crossingCount,
				selectionGoupSize: commonRequestData.selectionGoupSize,
				ro: commonRequestData.ro,
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
				<label>Initial Strats</label>
				{Object.entries(commonRequestData).map(([k, v]) => {
					return k != "ro" ? (
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
						""
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
					for (let i = 0; i < commonRequestData.population; ++i) {
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
