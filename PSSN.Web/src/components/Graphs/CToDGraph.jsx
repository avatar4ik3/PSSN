import Graph from "../Graph"

const CToDGraph = ({ stratsByRounds, ...rest }) => {
	function Prepare() {
		let items = stratsByRounds
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
				type: "marker",
				points: Object.entries(countOf(items, char)).map(([k, v]) => {
					return { x: Number.parseInt(k), y: Number.parseInt(v) }
				}),
			}
		}
		series.push(toSeries(items, "C"))
		series.push(toSeries(items, "D"))

		return series
	}

	return (
		<div>
			{stratsByRounds ? <Graph series={Prepare()} key={"2"}></Graph> : ""}
		</div>
	)
}

export default CToDGraph
