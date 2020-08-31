mergeInto(LibraryManager.library, {

  OpenToBlankWindow: function (_url) {
    console.log('called')
    window.open(Pointer_stringify(_url),'_blank')
  },

  TakeScreenShot : function () {
    console.log('called')
    var png = document.getElementById('#canvas').toDataURL();
    var bufferSize = lengthBytesUTF8(png) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(png, buffer, bufferSize);
    return buffer;
  },
});