mergeInto(LibraryManager.library, {

  OpenToBlankWindow: function (_url) {
    window.open(Pointer_stringify(_url),'_blank');
  },

  TakeScreenShot : function () {
    var png = document.getElementById('#canvas').toDataURL();
    var bufferSize = lengthBytesUTF8(png) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(png, buffer, bufferSize);
    console.log(buffer);
    return buffer;
  },
});