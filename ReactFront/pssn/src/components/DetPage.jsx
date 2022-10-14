import { React, useState } from "react";
import DetermitanedRequestForm from "./DeterminatedRequestForm";
import LoadedComponent from "./LoadedComponent";
import ResponseGraph from "./ResponseGraph";
import axios from "axios";
import * as qs from "qs";
const DetPage = () => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [data, setdata] = useState([]);
  async function OnFormSubmited({ strats, a, k }) {
    setIsLoaded(false);
    const response = await axios.get(
      "http://localhost:5146/api/v1/research/simple/?" +
        qs.stringify(
          {
            k: k,
            strats: strats,
            po: a,
            r: "1",
          },
          { arrayFormat: "indices" }
        )
    );
    setdata(response.data);
    setIsLoaded(true);
  }
  return (
    <div>
      <DetermitanedRequestForm onFormSubmited={OnFormSubmited} />
      <LoadedComponent loaded={isLoaded}>
        <ResponseGraph lines={data}></ResponseGraph>
      </LoadedComponent>
    </div>
  );
};

export default DetPage;
