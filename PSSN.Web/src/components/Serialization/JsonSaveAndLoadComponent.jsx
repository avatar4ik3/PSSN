import JsonLoadComponent from "./JsonLoadComponent"
import JsonSaveComponent from "./JsonSaveComponent"

export default JsonSaveAndLoadComponent = 
/**
 * @param {Array} states array like [[state,setState]...] of react states
*/
({states}) => {
	
    return (
		<div>
			<JsonSaveComponent data={states.map(x => x[0])}></JsonSaveComponent>
			<JsonLoadComponent></JsonLoadComponent>
		</div>
	)
}
