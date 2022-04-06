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
let outterCamState = 0;

const changeState = () => {
  if (outterCamState === 1) {
    outterCamState = 0;
  } else if (outterCamState === 0) {
    outterCamState = 1;
  }
};
const WebcamStreamCapture = () => {
  const webcamRef = React.useRef(null);
  const mediaRecorderRef = React.useRef(null);
  const [imageSrc, setImageSrc] = React.useState(null);
  const zeno = "http://127.0.0.1:8000";
  const ssafy = "https://j6e101.p.ssafy.io:8001/";
  const zeno_sub = "/recog/upload/zeno/";
  const ssafy_sub = "/recog/upload";
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
    await sleep(300);
    var caputuring = setInterval(async () => {
      if (outterCamState === 1) {
        /* ========================================================
         * convert: stream -> image -> blob
         * ======================================================== */
        const stream = new MediaStream(webcamRef.current.stream);
        const track = stream.getVideoTracks()[0];
        const image = new ImageCapture(track);
        // const blob = await image.takePhoto(); // Blob(size=~,
        // const grab = await image.grabFrame();
        // console.log("blob", { blob });
        // console.log("grab", { grab });
        const blob = webcamRef.current.getScreenshot();
        // console.log("screenshot", { screenshot1 });

        // const imgSrc = URL.createObjectURL(blob); // current image's imgSrc used in <img src={imgSrc}>
        // setImageSrc(imgSrc); // blob to output image // response data --> blob is also ok
        // console.log({ imgSrc });

        // const buffer = await blob.arrayBuffer();
        // const compressed = new Uint8Array(buffer);
        // const decompressed = await decode(compressed); // we dont need to decompress here

        /* ========================================================
         * send: blob -> server -> data -> binary
         * ======================================================== */
        // console.log("리사이즈 전");
        // get_time();
        // const resizedImg = await resizeImg(blob);
        // console.log("리사이즈 후");
        // get_time();
        // console.log("resized", { resizedImg });
        const form = new FormData();
        // form.append("image", resizedImg);
        form.append("image", blob);
        // const { data } = await api.post(zeno_sub, form, {
        //   responseType: "blob",
        // });
        // console.log("포스트 전");
        // get_time();
        // const { data } = await api.post(zeno_sub, form, {
        //   responseType: "text",
        // });
        const { data } = api.post(zeno_sub, form);
        // console.log("포스트 후");
        // get_time();
        // console.log(data["control"]);
        // unityContext.send("Witch3", "SetDir", data["control"]);
        // console.log("유니티후");
        // get_time();
        /* ========================================================
         * convert: binary -> image -> imageSrc
         * ref: https://stackoverflow.com/questions/50620821/uint8array-to-image-in-javascript
         *      https://seunggabi.tistory.com/entry/JS-convert-bytes-array-to-image
         *      https://stackoverflow.com/questions/4564119/how-to-convert-a-byte-array-into-an-image
         *      https://mineeeee.tistory.com/12#:~:text=PNG%ED%8C%8C%EC%9D%BC%EA%B5%AC%EC%A1%B0%EB%8A%94%20%ED%8C%8C%EC%9D%BC,%EC%A7%91%ED%95%A9%EC%9C%BC%EB%A1%9C%20%EA%B5%AC%EC%84%B1%EB%90%98%EC%96%B4%20%EC%9E%88%EC%8A%B5%EB%8B%88%EB%8B%A4.&text=PNG%20%ED%8C%8C%EC%9D%BC%20%EC%8B%9C%EA%B7%B8%EB%8B%88%EC%B2%98%EB%8A%94%2089,ASCII%20CODE%EB%A1%9C%20PNG%EC%9E%85%EB%8B%88%EB%8B%A4!
         * 서버에서 받은 이미지를 띄우고 싶다면, w*h*4 개의 uint8 데이터를 imgSrc로 변환해서 <img src={}> 에 넣으면됨.
         * 동작하는 케이스를 찾아서 사용하면 될듯
         * ======================================================== */
        // const imgSrc = URL.createObjectURL(
        //   new Blob([binary], { type: "image/png" })
        // );
        // const imgSrc = "data:image/png;base64," + btoa(binary);
        // console.log({ imgSrc });
        // setImageSrc(imgSrc);
      } else if (outterCamState === 0) {
        clearInterval(caputuring);
      }
    }, 200);
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
  const [camState, setCamState] = useState(0);

  useEffect(function () {
    unityContext.on("CallCam", function () {
      if (camState === 0) {
        setCamState(1);
        changeState();
      } else if (camState === 1) {
        setCamState(0);
        changeState();
      }
    });
  });
  return (
    <div className="App">
      <br />
      {camState === 0 ? null : <WebcamStreamCapture></WebcamStreamCapture>}
      <h1>{`상태는 ${camState} 입니다.`}</h1>
      <br />
      <Unity
        style={{
          width: "1080px",
          height: "720px",
          justifySelf: "center",
          alignSelf: "center,",
        }}
        unityContext={unityContext}
      />
    </div>
  );
}

export default App;
