import { useState } from 'react';
import './App.css';
import { href } from './constants';

function formLink(str) {
  return window.location.protocol + "//" + href + "/" + str;
}

function App() {

  let [resultText, setResultText] = useState(href);


  let createButton = (apiPath) => {
    let path = formLink(apiPath);
  

    return (<button onClick={async () => {
      let message = "no message";

      await fetch(path).then(async (res) => {
        if(res.status >= 400 && res.status < 600) {
          message = "Bad response from server"
        }
        return await res.text()
      })
      .then((body) => {
        message = body
        console.log(body);
      })
      .catch((err) => message = err)

      message = "Answer from server at location " + path + "\n" + message;

      setResultText(message);
      
    }}> {apiPath} </button>)
  }

  return (
    <div className="App wrapper">
      <div className="button-container" >
        {createButton("api")}
        {createButton("identity")}
        {createButton("catalog")}
      </div>

      <pre id="text-output" >{resultText}</pre>
      <a href={formLink("ref")}>{formLink("ref")}</a>
      <br />
      <a href={formLink("admin")}>{formLink("admin")}</a>
    </div>
  );
}

export default App;