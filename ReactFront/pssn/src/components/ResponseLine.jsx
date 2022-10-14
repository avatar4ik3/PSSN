import React, { Children } from "react";

const ResponseLine = ({ ki, strats, ...props }) => {
  return (
    <div className="line">
      <div className="line__content">
        <h1>{ki}</h1>
        {Object.entries(strats).map(([key, value]) => {
          return (
            <div className="line__content__element" key={key}>
              <div className="key">{key}</div>
              <div className="value">{value}</div>
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default ResponseLine;
