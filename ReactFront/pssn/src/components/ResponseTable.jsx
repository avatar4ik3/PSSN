import React from "react";
import ResponseLine from "./ResponseLine";

const ResponseTable = ({ lines, ...props }) => {
  return (
    <div className="ResponseTable">
      {lines.map(({ ki, values }) => {
        return <ResponseLine ki={ki} strats={values} key={ki}></ResponseLine>;
      })}
    </div>
  );
};

export default ResponseTable;
