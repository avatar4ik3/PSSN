import React, { Component, useState, useEffect } from "react"
import * as JSC from "jscharting"
import axios from "axios"
import * as qs from "qs"
import UnDetGraph from "./UnDetGraph"
import LoadedComponent from "./LoadedComponent"

const UnDetPage = () => {
	const [data, setdata] = useState([])

	const [isLoaded, setIsLoaded] = useState(false)

	const [chartData, setChartData] = useState([])

	const [request, setRequest] = useState({
		GenCount: 10,
		Population: 100,
		SwapChance: 0.3,
		CrossingCount: 5,
		SelectionGoupSize: 15,
		Ro: [
			[4, 0],
			[6, 1],
		],
	})

	async function download() {
		setIsLoaded(false)
		console.log(
			"http://localhost:8080/api/v1/research/hard/?" +
				qs.stringify(request, { arrayFormat: "indices" })
		)
		return axios
			.get(
				"http://localhost:8080/api/v1/research/hard/?" +
					qs.stringify(request, { arrayFormat: "indices" })
			)

			.then((r) => {
				console.log(r.request)
				setdata(r.data)
			})
			.catch((r) => console.error(r))
	}

	async function drawGraph() {
		console.log("drawing graph")
		let items = data.items.map((x) => x.strats)
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
		setIsLoaded(true)
	}

	return (
		<div>
			{Object.entries(request).map(([k, v]) => {
				return k != "Ro" ? (
					<div>
						<label>{k}</label>
						<input
							type="number"
							defaultValue={v}
							onChange={(e) => setRequest({ ...request, [k]: e.target.value })}
						/>
					</div>
				) : (
					""
				)
			})}
			<button
				onClick={async (e) => {
					await download().then((_) => drawGraph())
				}}
			>
				populations
			</button>
			<button
				onClick={(e) => {
					drawGraph()
				}}
			>
				draw
			</button>
			<LoadedComponent loaded={isLoaded}>
				<UnDetGraph series={chartData} />
			</LoadedComponent>
		</div>
	)
}

export default UnDetPage
