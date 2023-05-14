import { React, StrictMode } from "react"
import DetPage from "./components/DetPage"
import StepByStepUnDetPage from "./components/StepByStepUnDetPage"

import SamplePage from "./components/SamplePage"
import { createBrowserRouter, RouterProvider } from "react-router-dom"
import Root from "./components/Root"

import ConditionalPage from "./components/ConditionalPage"
import MemesPage from "./components/MemesPage"
import MemeStatisticPage from "./components/MemeStatisticPage"
const router = createBrowserRouter([
	{
		path: "/",
		element: <Root />,
		errorElement: <div>Page not Found</div>,
	},
	{
		path: "/sample",
		element: <SamplePage />,
	},
	{
		path: "/memes",
		element: (
			<MemesPage
				apiHost={
					"http://" +
					process.env.REACT_APP_SERVER_NAME +
					":" +
					process.env.REACT_APP_SERVER_PORT
				}
			/>
		),
	},
	{
		path: "/memes-statistic",
		element: (
			<MemeStatisticPage
				apiHost={
					"http://" +
					process.env.REACT_APP_SERVER_NAME +
					":" +
					process.env.REACT_APP_SERVER_PORT
				}
			/>
		),
	},
	{
		path: "/det",
		element: (
			<DetPage
				apiHost={
					"http://" +
					process.env.REACT_APP_SERVER_NAME +
					":" +
					process.env.REACT_APP_SERVER_PORT
				}
			/>
		),
	},
	{
		path: "/undet",
		element: (
			<StepByStepUnDetPage
				apiHost={
					"http://" +
					process.env.REACT_APP_SERVER_NAME +
					":" +
					process.env.REACT_APP_SERVER_PORT
				}
			/>
		),
	},
])
function App() {
	return <RouterProvider router={router}></RouterProvider>
}

export default App
