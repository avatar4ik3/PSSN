async function get_and_draw_vectors() {
    const response = await fetch(
        'http://localhost:5146/api/v1/test/'
        ,{
            method:'GET',
            mode: 'cors'
        }
    )
    console.log(await response.json());
}