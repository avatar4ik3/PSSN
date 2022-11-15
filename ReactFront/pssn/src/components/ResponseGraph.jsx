import { React, useEffect } from "react";
import * as JSC from "jscharting";

const ResponseGraph = ({ lines }) => {
  useEffect(() => {
    DrawGraph(GetSeries(lines));
  }, []);
  const style1 = {
    height: "500px",
  };
  return <div id="grp" style={style1}></div>;
};

function GetSeries(lines) {
  console.log(lines);
  let names = [];
  let series = [];
  for (let [key, value] of Object.entries(lines[0].values)) {
    names.push(key);
  }
  for (let name of names) {
    series[name] = [];
  }
  for (let entry of lines) {
    for (let name of names) {
      series[name].push({ x: entry.ki, y: entry.values[name] });
    }
  }
  let result = [];
  for (let name of names) {
    result.push({ name: name, points: series[name] });
    console.log({ name: name, points: series[name] });
  }
  return result;
}

function DrawGraph(series) {
  JSC.defaults({
    debug: true,
  });
  JSC.Chart("grp", {
    series: series,
  });
}
export default ResponseGraph;
