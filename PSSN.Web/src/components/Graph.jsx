import { React, useEffect } from "react"
import { JSCharting } from "jscharting-react"

const divStyle = {
	maxWidth: "700px",
	height: "500px",
	margin: "0px auto",
}

const Graph = ({ series }) => {
	return (
		<div style={divStyle}>
			<JSCharting options={{ series: series }} />
		</div>
	)
}

export default Graph
