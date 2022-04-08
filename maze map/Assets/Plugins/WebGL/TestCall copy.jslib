mergeInto(LibraryManager.library, {
  CallCamtest: function (data) {
    const datatostr = Pointer_stringify(data);
    var bb = document.querySelector("#root"); 
    if (bb.style.display === "none") {
      bb.style.display = "block";
      bb.className = datatostr;
    } else if (bb.style.display === "block") {
      bb.style.display = "none";
    }
  },
});