import { React, useState } from "react"
import axios from "axios"
import * as qs from "qs"
import Graph from "./Graph"
import Array2DInput from "./Array2DInput"
const DetPage = ({ apiHost, ...rest }) => {
	console.log(apiHost)
	const [data, setdata] = useState(null)

	const [request, setrequest] = useState({
		CycleCount: 1000,
		Strategies: ["CTT", "CD", "D", "CTT3D", "CTT4D", "CTT5D", "CTT6D"],
		A: [
			[4, 0],
			[6, 1],
		],
		NTimesRepeatedGame: 6,
	})

	async function sendRequest() {
		return axios
			.get(
				apiHost +
					"/api/v1/research/simple/?" +
					qs.stringify(
						{
							k: request.CycleCount,
							strats: request.Strategies,
							po: request.A,
							r: request.NTimesRepeatedGame,
						},
						{ arrayFormat: "indices" }
					)
			)
			.then((r) => r.data)
	}

	function GetSeries(lines) {
		console.log(lines)
		let names = []
		let series = []
		for (let [key, value] of Object.entries(lines[0].values)) {
			names.push(key)
		}
		for (let name of names) {
			series[name] = []
		}
		for (let entry of lines) {
			for (let name of names) {
				series[name].push({ x: entry.ki, y: entry.values[name] })
			}
		}
		let result = []
		for (let name of names) {
			result.push({ name: name, points: series[name] })
			console.log({ name: name, points: series[name] })
		}
		return result
	}

	return (
		<div>
			<label>Det request</label>
			<div>
				{Object.entries(request).map(([k, v]) => {
					if (k == "A") {
						return (
							<div key={k}>
								<label>{k}</label>
								<Array2DInput
									index1={0}
									index2={0}
									v={v}
									set={setrequest}
									data={request}
								/>
								<Array2DInput
									index1={0}
									index2={1}
									v={v}
									set={setrequest}
									data={request}
								/>
								<Array2DInput
									index1={1}
									index2={0}
									v={v}
									set={setrequest}
									data={request}
								/>
								<Array2DInput
									index1={1}
									index2={1}
									v={v}
									set={setrequest}
									data={request}
								/>
							</div>
						)
					} else if (k == "Strategies") {
						return (
							<div key={k}>
								<label>{k}</label>
								<input
									type="text"
									defaultValue={request.Strategies}
									onChange={(e) => {
										if (e.target.value.endsWith(",") === false) {
											console.log("editing!")
											const strats = e.target.value
												.split(",")
												.filter((s) => s && s != "" && s != " ")
												.map((s) => s.replace(/\s/g, ""))

											console.log(strats)
											setrequest({ ...request, [k]: strats })
										}
									}}
								/>
							</div>
						)
					} else {
						return (
							<div key={k}>
								<label>{k}</label>
								<input
									type="number"
									defaultValue={v}
									onChange={(e) =>
										setrequest({ ...request, [k]: e.target.value })
									}
								/>
							</div>
						)
					}
				})}
			</div>

			<button
				onClick={async (e) => {
					setdata(null)
					const response = await sendRequest()
					setdata(GetSeries(response))
				}}
			>
				Process
			</button>
			{data ? <Graph series={data} /> : null}
		</div>
	)
}

export default DetPage
