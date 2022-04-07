mergeInto(LibraryManager.library, {
  CallCam: function (data) {
    var dataname = Pointer_stringify(data);
    var bb = document.querySelector("#root"); 
    bb.className = dataname; 
    console.log("Jslib uid " + dataname);    
  },
  SelectControl: function (mode) {
    var modename = Pointer_stringify(mode);
    var qq = document.querySelector("#controlmode"); 
    qq.className = modename;
    console.log("Jslib mode " + modename);
  }
});