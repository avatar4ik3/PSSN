import { useState } from "react"

const JsonSaveComponent =
	/**
	 * @param {*} data data to be serialized
	 * @returns {import("react").ReactNode}
	 */
	({ data }) => {
		let [name, setName] = useState(`expirement ${Date.now()}`)

		return (
			<div>
				<input
					type="text"
					defaultValue={name}
					onChange={(e) => setName(e.target.value)}
				></input>
				<button
					onClick={(e) => {
						let stringified = JSON.stringify(data)
						let blob = new Blob([stringified], { type: "application/json" })
						let href = URL.createObjectURL(blob)

						// create "a" HTLM element with href to file
						const link = document.createElement("a")
						link.href = href
						link.download = name + ".json"
						document.body.appendChild(link)
						link.click()

						// clean up "a" element & remove ObjectURL
						document.body.removeChild(link)
						URL.revokeObjectURL(href)
					}}
				>
					скачать файл исследования
				</button>
			</div>
		)
	}

export default JsonSaveComponent
