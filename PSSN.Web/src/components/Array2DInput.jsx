const Array2DInput = ({
	index1,
	index2,
	v,
	setrequest: set,
	data,
	...rest
}) => {
	return (
		<div>
			<label>
				[{index1}][{index2}]
			</label>
			<input
				type="number"
				value={v[index1][index2]}
				onChange={(e) => {
					const inro = data.A
					inro[index1][index2] = e.target.value
					set({ ...data, A: inro })
				}}
			/>
		</div>
	)
}

export default Array2DInput
