mergeInto(LibraryManager.library, {
  CallCam: function () {
    var bb = document.querySelector("#root");
    console.log("callcam", bb);
    if (bb.style.display === "none") {
      bb.style.display = "block";
    } else if (bb.style.display === "block") {
      bb.style.display = "none";
    }
  },
});