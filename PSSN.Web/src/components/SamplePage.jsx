import { React, useState, useEffect } from "react"
import axios from "axios"
import Graph from "./Graph"
const SamplePage = () => {
	const [lines, setlines] = useState(null)

	useEffect(() => {
		async function get() {
			const response = await axios.get("http://localhost:5146/api/v1/test/")
			var data = response.data
			setlines(data)
		}
		get()
	}, [])

	return <div>{lines == null ? <Graph series={lines} /> : null}</div>
}

export default SamplePage
