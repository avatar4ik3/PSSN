const JsonLoadComponent =
	/**
	 * @param {Array} setStates array of setState functions
	 */
	({ setStates }) => {
		let fileReader = new FileReader()

		fileReader.onload = (fe) => {
			console.log("Uploaded file", fe.target.result)
			let uploadedObject = JSON.parse(fe.target.result)
			let entries = Object.entries(uploadedObject)
			for (let i = 0; i < entries.length; i++) {
				//тут аккуратно. Object.entries отдает массив, в котором массивы по 2 элемента. что-то типо такого
				//[[имя, значение],[имя, значение]]. нам нужно только значение
				setStates[i](entries[i][1])
			}
		}

		return (
			<div>
				<label>Выбрать и загрузить файл исследования</label>
				<input
					type="file"
					accept="application/json"
					onChange={(e) => {
						fileReader.readAsText(e.target.files[0], "UTF-8")
					}}
				/>
			</div>
		)
	}

export default JsonLoadComponent
