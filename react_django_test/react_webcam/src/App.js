import React from "react";
import Webcam from "react-webcam";
import axios from "axios";
import inkjet from "inkjet";
import FormData from "form-data";
import Unity, { UnityContext } from "react-unity-webgl";
import Resizer from "react-image-file-resizer";
import { useState, useEffect } from "react";

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
  const ssafy = "https://j6e101.p.ssafy.io:8001/";
  const zeno_sub = "/recog/upload/";
  const ssafy_sub = "/recog/upload";
  const id_class = document.querySelector("#root");
  const api = axios.create({ baseURL: zeno });
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
      console.log(id_class.style.display);
      if (id_class.style.display === "block") {
        const stream = new MediaStream(webcamRef.current.stream);
        const track = stream.getVideoTracks()[0];
        const image = new ImageCapture(track);

        const blob = webcamRef.current.getScreenshot();
        console.log("blob", { blob });
        const form = new FormData();
        // console.log(id_class.className);
        form.append("image", blob);
        // if (id_class.display === "none") {
        // } else if (id_class.display === "block") {
        //   console.log("켜져있음");
        // }
        const sub_uid = id_class.className;
        if (sub_uid !== "test") {
          const url_sub = zeno_sub + sub_uid + "/";
          const { data } = api.post(url_sub, form);
        }
      } else if (id_class.style.display === "none") {
        // console.log("clear");
        // clearInterval(caputuring);
      }
    }, 2000);
  };

  const videoConstraints = {
    width: 240,
    height: 180,
    facingMode: "user",
  };
  handleObserveClick();
  return (
    <>
      <div>input webcam</div>
      <Webcam
        audio={false}
        height={180}
        width={240}
        videoConstraints={videoConstraints}
        ref={webcamRef}
        style={{
          width: "240px",
          height: "180px",
        }}
        onUserMedia={(stream) => {
          console.log(stream);
        }}
      />
      <button onClick={handleObserveClick}>observe</button>
      <div>output image</div>
      {/* <img
        src={imageSrc}
        style={{
          width: "320px",
          height: "240px",
        }}
        alt="캡쳐이미지"
      ></img> */}
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
    <div className="App">
      <br />
      {/* {camState === 0 ? null : <WebcamStreamCapture></WebcamStreamCapture>} */}
      <WebcamStreamCapture></WebcamStreamCapture>
      {/* <h1>{`상태는 ${camState} 입니다.`}</h1> */}
      <br />
      {/* <Unity
        id="unity-ins"
        style={{
          width: "1080px",
          height: "720px",
          justifySelf: "center",
          alignSelf: "center,",
        }}
        unityContext={unityContext}
      /> */}
    </div>
  );
}

export default App;
