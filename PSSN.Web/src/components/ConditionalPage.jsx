import axios from "axios"
import { useState } from "react"
import * as qs from "qs"
const ConditionalPage = () => {
	const [request, setRequest] = useState({
		PopulationSize: 10,
		GenotypeSize: 5,
		GeneMutationChance: 0.3,
		MaxGenLengthForCrossingover: 5,
		K_TournamentSelection: 5,
		A: [
			[4, 0],
			[6, 1],
		],
	})
	const [data, setdata] = useState(null)
	return (
		<div>
			{Object.entries(request).map(([k, v]) => (
				<div key={k}>
					<label>{k}</label>
					<input
						type="number"
						value={v}
						onChange={(e) => setRequest({ ...request, [k]: e.target.value })}
					/>
				</div>
			))}
			<button
				onClick={async (e) => {
					setdata(
						await axios
							.get(
								"http://localhost:8080/api/v1/research/conditional?" +
									qs.stringify({
										genCount: request.GenotypeSize,
										swapChance: request.GeneMutationChance,
										crossingCount: request.MaxGenLengthForCrossingover,
										selectionGroupSize: request.K_TournamentSelection,
										population: request.PopulationSize,
										ro: request.A,
									})
							)
							.then((r) => r.data)
					)
				}}
			></button>
			{data ? JSON.stringify(data) : null}
		</div>
	)
}

export default ConditionalPage
