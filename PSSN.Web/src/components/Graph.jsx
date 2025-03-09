import { React, useEffect } from "react"
import { JSCharting } from "jscharting-react"

const divStyle = {
	maxWidth: "700px",
	height: "500px",
	margin: "0px auto",
}

const Graph = ({ series, xLabel, yLabel, title, twoAxis, y1_label, y2_label, y1_max, y1_min, ...rest }) => {
	console.error(series[0].points)

	let opts = {
		series: series,
		title_label_text: title,
		legend_visible: true,
		legend_template: "%average %icon %name",
		xAxis_label_text: xLabel,
		yAxis_label_text: yLabel,
		type: 'line',
	}

	if (twoAxis) {
		opts.yAxis = [{
			id: 'y_left',
			label_text: y1_label,
			ticks: {
				max: y1_max,
				min: y1_min
			}
		},
		{
			id: 'y_right',
			label_text: y2_label,
			orientation: 'right'
		}
		]
	}
	return (
		<div style={divStyle}>
			<JSCharting
				options={opts}
			/>
		</div>
	)
}

export default Graph
