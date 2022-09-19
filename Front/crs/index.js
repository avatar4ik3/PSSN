async function get_and_draw_vectors() {
    const response = await fetch(
        'http://localhost:5146/api/v1/test/'
        , {
            method: 'GET',
            mode: 'cors'
        }
    )
    const parced = await response.json();
    draw_table(parced);
    draw_graph(parced);
}

function draw_table(parced) {
    var prev = document.getElementById('table')
    if (prev != null) {
        prev.remove();
    }
    let ol = document.createElement('ol');
    for (let entry of parced) {
        var text = " ";
        for (let [key, value] of Object.entries(entry.values)) {
            text += value + " ";
        }
        let li = document.createElement('li');
        li.innerText = text;
        ol.appendChild(li);
    }
    var dv = document.getElementById('rsp');
    ol.style.height = `${dv.offsetHeight}px`
    ol.style.width = `${dv.offsetWidth}px`;
    ol.id = 'table'
    dv.append(ol);

}

function draw_graph(parced) {
    JSC.Chart('grp', {
        series : get_series(parced)
    });
}

function get_series(parced){
    let names = [];
    let series = [];
    for(let [name,value] of Object.entries(parced[0].values)){
        names.push(name);
    }
    for(let name of names){
        series[name] = [];
    }
    for(let entry of parced){
        for(let name of names){
            series[name].push({x : entry.ki,y : entry.values[name]});
        }
    }
    let result = [];
    for(let name of names){
        result.push({name : name,points : series[name]});
    }
    console.log(result);
    return result;

}
