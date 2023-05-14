import { React, useState, useEffect } from "react"

const StrategiesRationGraph = ({ games }) => {
	const [data, setData] = useState(null)

	function uniqBy(a, key) {
		let seen = new Set()
		return a.filter((item) => {
			let k = key(item)
			return seen.has(k) ? false : seen.add(k)
		})
	}

	useEffect(() => {
		let series = []
		let all_possible_coeffs = games
			.map((x) => x.strats)
			.map((x) => x.patterns[0].coefs)
		let unique_coeffs = uniqBy(all_possible_coeffs, JSON.stringify)
		let pre_series = []
		for (let i = 0; i < length(games); ++i) {
            for (let strats of game.strats) {
				
			}
		}
	}, [games])

	return <div></div>
}

export default StrategiesRationGraph
