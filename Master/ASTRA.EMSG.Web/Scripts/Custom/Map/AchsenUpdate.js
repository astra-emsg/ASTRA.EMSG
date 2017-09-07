(function () {
    var Map = Emsg.Map;
    var $super = Map.prototype;

    Map.AchsenUpdate = $.inherit(Map,
  {
      constructor: function (app) {
          Map.apply(this, arguments);
      }
   , afterOverlays: function () {
       $super.afterOverlays.apply(this, arguments);
       this.axis.setVisibility(true);
       this.konflikte.setVisibility(true);
   }
   , init: function () {
       $super.init.apply(this, arguments);
       this.addControls();
       this.overrideMapCookie = true;
   }
  });
})();
