import { React, useState } from "react"
import axios from "axios"
import * as qs from "qs"
import Graph from "./Graph"
const DetPage = ({ apiHost, ...rest }) => {
	console.log(apiHost)
	const [data, setdata] = useState(null)

	const [request, setrequest] = useState({
		k: 1000,
		strats: ["CTT", "CD", "D", "CTT3D", "CTT4D", "CTT5D", "CTT6D"],
		ro: [
			[6, 0],
			[4, 1],
		],
		r: 6,
	})

	async function sendRequest() {
		return axios
			.get(
				apiHost +
					"/api/v1/research/simple/?" +
					qs.stringify(
						{
							k: request.k,
							strats: request.strats,
							po: request.ro,
							r: request.r,
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

	const Array2DInput = ({ index1, index2, v, ...rest }) => {
		return (
			<div>
				<label>
					[{index1}][{index2}]
				</label>
				<input
					type="number"
					value={v[index1][index2]}
					onChange={(e) => {
						const inro = request.ro
						inro[index1][index2] = e.target.value
						setrequest({ ...request, ro: inro })
					}}
				/>
			</div>
		)
	}

	return (
		<div>
			<label>Det request</label>
			<div>
				{Object.entries(request).map(([k, v]) => {
					if (k == "ro") {
						return (
							<div key={k}>
								<label>Ro</label>
								<Array2DInput index1={0} index2={0} v={v} />
								<Array2DInput index1={0} index2={1} v={v} />
								<Array2DInput index1={1} index2={0} v={v} />
								<Array2DInput index1={1} index2={1} v={v} />
							</div>
						)
					} else if (k == "strats") {
						return (
							<div key={k}>
								<label>{k}</label>
								<input
									type="text"
									defaultValue={request.strats}
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
				submit
			</button>
			{data ? <Graph series={data} /> : null}
		</div>
	)
}

export default DetPage
