mergeInto(LibraryManager.library, {

  OpenToBlankWindow: function (_url) {
    console.log('called')
    window.open(Pointer_stringify(_url),'_blank')
  },

  TakeScreenShot : function () {
    console.log('called')
    var png = document.getElementById('canvas').toDataURL('screenShot/png');
}
});