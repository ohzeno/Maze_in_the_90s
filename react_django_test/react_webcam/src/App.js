import React from "react";
import Webcam from "react-webcam";
// import jpeg from "jpeg-js";
// import pako from "pako";
import axios from "axios";
// import decode from "image-decode";
import inkjet from "inkjet";
import FormData from "form-data";

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

const WebcamStreamCapture = () => {
  const webcamRef = React.useRef(null);
  const mediaRecorderRef = React.useRef(null);
  const [imageSrc, setImageSrc] = React.useState(null);

  const handleObserveClick = async () => {
    await sleep(300);
    setInterval(async () => {
      const stream = new MediaStream(webcamRef.current.stream);
      const track = stream.getVideoTracks()[0];
      const image = new ImageCapture(track);
      const blob = await image.takePhoto(); // Blob(size=~, type=image/jpeg)
      // console.log("blob", blob);
      const buffer = await blob.arrayBuffer();
      // console.log("buffer", buffer);
      const compressed = new Uint8Array(buffer);
      const decompressed = await decode(compressed);
      // console.log("decompressed", decompressed);
      // console.log("size", decompressed.width * decompressed.height * 4);

      // const { data } = await axios.put(
      //   `http://115.22.130.177:3000/recog/blob`,
      //   blob,
      //   {
      //     headers: {
      //       "Content-Type": "arraybuffer",
      //     },
      //   }
      // );

      // Uint8Array to arrayBuffer: uarray.slice(0, uarray.byteOffset): https://stackoverflow.com/questions/37228285/uint8array-to-arraybuffer
      // arrayBuffer to Uint8Array: new Uint8Array(arrayBuffer)

      // const form = new FormData();
      // form.append("blob", blob, "blob");
      const { data } = axios
        .post(
          // `http://115.22.130.177:3000/recog/upload`,
          `http://127.0.0.1:8000/recog/upload`,
          decompressed,
          // compressed,
          // blob,
          {
            headers: {
              // "Content-Type": "image/png",
              // "Content-Type": "application/x-www-form-urlencoded",
              "Content-Type": "multipart/form-data",
              // "Content-Type": " application/octet-stream",
              // filename: "what",
            },
          }
        )
        .then((res) => {
          console.log("RESPONSE RECEIVED: ", res);
        })
        .catch((err) => {
          console.log("AXIOS ERROR: ", err);
        });

      // const { data } = axios.post(`http://115.22.130.177:3000/recog/blob`, {
      //   width: decompressed.width,
      //   height: decompressed.height,
      //   data: decompressed.data,
      // });

      console.log("data", data);

      setImageSrc(URL.createObjectURL(blob)); // blob to output image // response data --> blob is also ok
    }, 5000);
  };

  return (
    <>
      <div>input webcam</div>
      <Webcam
        audio={false}
        ref={webcamRef}
        onUserMedia={(stream) => {
          console.log(stream);
        }}
      />
      <button onClick={handleObserveClick}>observe</button>
      <div>output image</div>
      <img src={imageSrc}></img>
    </>
  );
};

// https://www.npmjs.com/package/react-webcam

function App() {
  return <WebcamStreamCapture></WebcamStreamCapture>;
}

export default App;
