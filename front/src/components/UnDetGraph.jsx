import { React, useEffect } from "react";
import * as JSC from "jscharting";

const UnDetGraph = ({ series }) => {
  useEffect(() => {
    DrawGraph(series);
  }, [series]);
  const style1 = {
    height: "500px",
  };
  return <div id="g" style={style1}></div>;
};

function DrawGraph(series) {
  JSC.defaults({
    debug: true,
  });
  JSC.Chart("g", {
    series: series,
  });
}
export default UnDetGraph;
