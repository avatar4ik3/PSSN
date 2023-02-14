import { React, StrictMode } from "react"
import DetPage from "./components/DetPage"
import SamplePage from "./components/SamplePage"
import StepByStepUnDetPage from "./components/StepByStepUnDetPage"
import UnDetPage from "./components/UnDetPage"
function App() {
	return (
		<StrictMode>
			<div className="App">
				<StepByStepUnDetPage
					apiHost={"http://localhost:8080"}
				></StepByStepUnDetPage>
			</div>
		</StrictMode>
	)
}

export default App
