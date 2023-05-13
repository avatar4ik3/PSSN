import { React, useEffect } from "react"
import { JSCharting } from "jscharting-react"

const divStyle = {
	maxWidth: "700px",
	height: "500px",
	margin: "0px auto",
}

const Graph = ({ series,xLabel,yLabel,title }) => {
	return (
		<div style={divStyle}>
			<JSCharting
				options={{
					series: series,
					title_label_text: title,
					legend_visible: true,
					legend_template: "%average %sum %icon %name",
					xAxis_label_text: xLabel,
					yAxis_label_text: yLabel,
				}}
			/>
		</div>
	)
}

export default Graph
