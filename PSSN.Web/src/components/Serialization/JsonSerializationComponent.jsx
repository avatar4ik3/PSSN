import JsonLoadComponent from "./JsonLoadComponent"
import JsonSaveComponent from "./JsonSaveComponent"

const JsonSerializationComponent =
	/**
	 * @param {Array<Array<any,func>>} data array with react states that must be serialized. like [{state1,setState1},{state2,setState2}]
	 * @returns {import("react").ReactNode} node with save (with optional name) and load button
	 */
	({ data }) => {
		let object_to_serialize = {}
		let set_funcs = []

		for (let obj of data) {
			//боже как же я люблю оператор [] в жсе. параша нереальная
			let entries = Object.entries(obj)
			object_to_serialize[entries[0][0]] = entries[0][1]
			set_funcs.push(entries[1][1])
		}

		return (
			<>
				<JsonSaveComponent data={object_to_serialize} />
				<JsonLoadComponent setStates={set_funcs} />
			</>
		)
	}

export default JsonSerializationComponent
