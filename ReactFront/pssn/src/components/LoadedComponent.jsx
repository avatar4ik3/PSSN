import React, { Children } from "react";

const LoadedComponent = ({ children, loaded }) => {
  return <div>{loaded && children}</div>;
};

export default LoadedComponent;
