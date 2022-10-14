import { React, useState } from "react";
const DeterminatedRequestForm = ({ onFormSubmited }) => {
  const [strats, setStrats] = useState([]);
  const [a, setA] = useState([]);
  const [k, setK] = useState(0);

  return (
    <div>
      <form className="request_form" onSubmit={() => {}}>
        <input
          onChange={(e) => {
            setStrats(
              e.target.value
                .split(",")
                .filter((s) => s && s != "" && s != " ")
                .map((s) => s.replace(/\s/g, ""))
            );
          }}
          inputMode="text"
          defaultValue="CTT, C, D"
          title="strats"
        />
        <input
          onChange={(e) => {
            var value = e.target.value;
            var values = value.split(" ");
            var line1 = [];
            line1.push(values[0]);
            line1.push(values[1]);
            var line2 = [];
            line2.push(values[2]);
            line2.push(values[3]);
            var res = [];
            res.push(line1);
            res.push(line2);
            setA(res);
          }}
          defaultValue="6 1 4 0"
          inputMode="text"
          title="a"
        />
        <input
          onChange={(e) => setK(e.target.value)}
          inputMode="numeric"
          title="k"
          defaultValue={1000}
        />
        <button
          type="submit"
          onClick={(e) => {
            e.preventDefault();
            onFormSubmited({ strats, a, k });
          }}
        >
          Submit
        </button>
      </form>
    </div>
  );
};

export default DeterminatedRequestForm;
