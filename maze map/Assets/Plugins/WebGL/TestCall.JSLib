mergeInto(LibraryManager.library, {
  CallCamera: function (userName, score) {
    window.dispatchReactUnityEvent(
      "CallCamera",
      Pointer_stringify(userName),
      score
    );
  },
});