(function () {
  var MapMode = Emsg.MapMode;
  var $super = MapMode.prototype;

  MapMode.Measure = $.inherit(MapMode,
  {
    constructor: function (map) {
      MapMode.call(this, map);

      var ef = this.app.events.form;
      ef.onKoordinierteMassnahmeCreated = function () { map.onCreated.apply(map, arguments); };
      ef.onKoordinierteMassnahmeSelected = function () { map.onSelected.apply(map, arguments); };
      ef.onKoordinierteMassnahmeDeleted = function () { map.onDeleted.apply(map, arguments); };

      this.urls.PARENTS = this.app.config.get('GIS.CONTROLLER.GET_AXIS_COLLECTION');
      this.urls.PARENT_AT = this.app.config.get('GIS.CONTROLLER.GET_AXIS_AT');
      this.urls.MAIN_AT = this.app.config.get('GIS.CONTROLLER.GET_MASS_AT');
      this.urls.MAIN_ID = this.app.config.get('GIS.CONTROLLER.GET_MASS_BY_ID');
      this.layer = null;
      this.type = "MASS";
    }

   , loadParents: function (feature) {
     var parents = {}, ids = [], feats = this.geoJson.read(feature.attributes.childs);
     for (var i = 0; i < feats.length; ++i) { parents[feats[i].attributes.AchsenSegmentId] = true; };
     for (id in parents) ids.push(id);
     var protocol = new OpenLayers.Protocol.HTTP({ url: this.urls.PARENTS, format: this.geoJson });
     this.loadByIds(ids, protocol, this.map.parentLayer.addFeatures, this.map.parentLayer);
   }

   , onChanged: function () { this.events.onKoordinierteMassnahmeChanged.apply(this.events, arguments); }

   , createFeature: function () {
     if ($super.createFeature.apply(this, arguments)) {
       this.geoData.attributes.type = "Massnahme";
       this.events.onKoordinierteMassnahmeCreated();
     }
   }

   , onSelectFeature: function (fid, readOnly) {
     this.events.onKoordinierteMassnahmeSelected(fid, readOnly);
   }

   , updateChild: function (child) {
     child.attributes.AchsenSegmentId = child.attributes.AchsenSegmentId || child.parent.fid;
     child.attributes.type = child.attributes.type || "";
   }

   , afterOverlays: function () {
     $super.afterOverlays.apply(this, arguments);
     this.map.koordinierte.setVisibility(true);
     this.map.axis.setVisibility(true);
     this.layer = this.map.koordinierte;
   }


   , isChild: function (parent, child) {
     return child.attributes.AchsenSegmentId == parent.fid;
   }

  });
})();