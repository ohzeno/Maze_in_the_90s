mergeInto(LibraryManager.library, {
  CallCam: function (data) {
    const datatostr = Pointer_stringify(data);
    var bb = document.querySelector("#root"); 
    bb.className = datatostr;    
  },
});