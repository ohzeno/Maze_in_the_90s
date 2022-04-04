mergeInto(LibraryManager.library, {
  CallCam: function (userName, score) {
    window.dispatchReactUnityEvent(
      "CallCam",
      Pointer_stringify(userName),
      score
    );
  },
});