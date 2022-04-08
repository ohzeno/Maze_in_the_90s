import React from "react";
import Webcam from "react-webcam";
import axios from "axios";
import inkjet from "inkjet";
import FormData from "form-data";
import Unity, { UnityContext } from "react-unity-webgl";
import Resizer from "react-image-file-resizer";
import { useState, useEffect } from "react";
import "./App.css";

const unityContext = new UnityContext({
  loaderUrl: "Build/public.loader.js",
  dataUrl: "Build/public.data",
  frameworkUrl: "Build/public.framework.js",
  codeUrl: "Build/public.wasm",
});

const sleep = async (ms) => {
  return new Promise((resolve, reject) => setTimeout(resolve, ms));
};

const decode = (binary) => {
  return new Promise((resolve, reject) => {
    inkjet.decode(binary, (err, decode) => {
      resolve(decode);
    });
  });
};

const get_time = () => {
  let today = new Date();
  let minutes = today.getMinutes();
  let seconds = today.getSeconds();
  let milseconds = today.getMilliseconds();
  console.log(minutes + " : " + seconds + " : " + milseconds);
};
// let outterCamState = 0;

// const changeState = () => {
//   if (outterCamState === 1) {
//     outterCamState = 0;
//   } else if (outterCamState === 0) {
//     outterCamState = 1;
//   }
// };

const WebcamStreamCapture = () => {
  const webcamRef = React.useRef(null);
  const mediaRecorderRef = React.useRef(null);
  const [imageSrc, setImageSrc] = React.useState(null);
  const zeno = "http://127.0.0.1:8000";
  const ssafy = "https://j6e101.p.ssafy.io";
  const zeno_sub = "/recog/upload/";
  const ssafy_sub = "/recog/upload/";
  const id_class = document.querySelector("#root");
  const id_mode = document.querySelector("#controlmode");
  const api = axios.create({ baseURL: ssafy });
  const resizeImg = (file) =>
    new Promise((resolve) => {
      Resizer.imageFileResizer(
        file,
        280,
        210,
        "PNG",
        100,
        0,
        (uri) => {
          resolve(uri);
        },
        "blob"
      );
    });

  const handleObserveClick = async () => {
    var caputuring = setInterval(async () => {
      // console.log(id_class.style.display);
      if (id_class.className !== "test") {
        id_class.style.display = "block";
        const stream = new MediaStream(webcamRef.current.stream);
        const track = stream.getVideoTracks()[0];
        const image = new ImageCapture(track);

        const blob = webcamRef.current.getScreenshot();
        // console.log("blob", { blob });
        const form = new FormData();
        // console.log(id_class.className);
        form.append("image", blob);
        // if (id_class.display === "none") {
        // } else if (id_class.display === "block") {
        //   console.log("켜져있음");
        // }
        var sub_uid = id_class.className;
        var sub_mode = id_mode.className;
        //console.log("uid " + sub_uid);
        //console.log("mode " + sub_mode);
        // const sub_uid = "ㅇㄹㅇㄹ";
        if (sub_uid !== "test") {
          const url_sub = ssafy_sub + sub_uid + "/" + sub_mode + "/";
          const { data } = api.post(url_sub, form);
        }
      } else if (id_class.className === "test") {
        id_class.style.display = "none";
        // console.log("clear");
        // clearInterval(caputuring);
      }
    }, 250);
  };

  const videoConstraints = {
    width: 240,
    height: 180,
    facingMode: "user",
  };
  handleObserveClick();
  return (
    <>
      <Webcam
        audio={false}
        // videoConstraints={videoConstraints}
        ref={webcamRef}
        style={{
          width: "80%",
          height: "100%",
          position: "absolute",
          top: "-18%",
          left: "10%",
        }}
        onUserMedia={(stream) => {
          console.log(stream);
        }}
      />
      {/* <button onClick={handleObserveClick}>observe</button> */}
    </>
  );
};

// https://www.npmjs.com/package/react-webcam

function App() {
  // const [camState, setCamState] = useState(0);

  // useEffect(function () {
  //   unityContext.on("CallCam", function () {
  //     if (camState === 0) {
  //       setCamState(1);
  //       changeState();
  //     } else if (camState === 1) {
  //       setCamState(0);
  //       changeState();
  //     }
  //   });
  // });
  return (
    <div>
      <WebcamStreamCapture></WebcamStreamCapture>
    </div>
  );
}

export default App;
