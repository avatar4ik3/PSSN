import { React, useState, useEffect } from "react";
import axios from "axios";
import LoadedComponent from "./LoadedComponent";
import ResponseGraph from "./ResponseGraph";
const SamplePage = () => {
  const [lines, setlines] = useState([]);

  const [isLoaded, setisLoaded] = useState(false);
  useEffect(() => {
    async function get() {
      const response = await axios.get("http://localhost:5146/api/v1/test/");
      var data = response.data;
      setlines(data);
      setisLoaded(true);
    }
    get();
  }, []);

  return (
    <div>
      <LoadedComponent loaded={isLoaded}>
        <ResponseGraph lines={lines}></ResponseGraph>
      </LoadedComponent>
    </div>
  );
};

export default SamplePage;
