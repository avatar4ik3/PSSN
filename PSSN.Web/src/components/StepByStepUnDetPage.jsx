import React, { Component, useState, useEffect } from "react"
import * as JSC from "jscharting"
import axios from "axios"
import * as qs from "qs"
import UnDetGraph from "./UnDetGraph"
import LoadedComponent from "./LoadedComponent"

const StepByStepUnDetPage = ({ apiHost, ...rest }) => {
	const [genStratsRequest, setgenStratsRequest] = useState({
		// Count: 0,
		// GenCount: 0,
	})

	const [singleResearchRequest, setsingleResearchRequest] = useState({
		// Strats: [],
		// GenCount: 0,
		// SwapChance: 0,
		// CrossingCount: 0,
		// SelectionGoupSize: 0,
		// Ro: [[], []],
	})

	const [strats, setstrats] = useState([])

	const [data, setdata] = useState([])

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

	useEffect(() => {
		setsingleResearchRequest((prev) => {
			prev.strats = strats[0]
			return prev
		})
		console.log("strats have been changed", singleResearchRequest)
	}, [strats])

	useEffect(() => {
		setsingleResearchRequest((prev) => {
			prev.genCount = commonRequestData.genCount
			prev.swapChance = commonRequestData.swapChance
			prev.crossingCount = commonRequestData.crossingCount
			prev.selectionGoupSize = commonRequestData.selectionGoupSize
			prev.ro = commonRequestData.ro
			return prev
		})
		setgenStratsRequest((prev) => {
			prev.Count = commonRequestData.count
			prev.GenCount = commonRequestData.genCount
			return prev
		})
		console.log(singleResearchRequest, genStratsRequest, commonRequestData)
	}, [commonRequestData])

	async function requestInitialStrategies() {
		return axios
			.get(
				apiHost + "/api/v1/research/generate?" + qs.stringify(genStratsRequest)
			)
			.then((r) => {
				setstrats((prev) => {
					while (prev.length != 0) {
						prev.pop()
					}
					prev.push(r.data)
					return prev
				})
			})
	}

	async function getOneGeneration() {
		let request = axios
			.get(
				apiHost +
					"/api/v1/research/split?" +
					qs.stringify(singleResearchRequest, { arrayFormat: "indices" })
			)
			.then((r) => {
				setdata(data.push(r.data))
			})
		console.log(request)
		return request
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
								defaultValue={v}
								onChange={(e) => {
									setcommonRequestData((prev) => {
										prev[[k]] = e.target.value
										return prev
									})
									setcommonRequestData({ ...commonRequestData })
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
					// await requestInitialStrategies()
					console.log(strats)
					console.log(JSON.stringify(strats[0]))

					var genRequest = await getOneGeneration().catch(console.log)
					console.log("one request " + genRequest)
				}}
			>
				Process
			</button>
		</div>
	)
}

export default StepByStepUnDetPage
