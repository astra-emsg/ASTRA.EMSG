(function () {
  var HTTP = OpenLayers.Protocol.HTTP;

  var Parent = Emsg.MapMode;
  var $super = Parent.prototype;

  Parent.RealisierteMassnahmen = $.inherit( Parent,
  {
    constructor: function (map) {
      $super.constructor.call(this, map);

      var ef = this.app.events.form;
      ef.onRealisierteMassnahmeCreated = function () { map.onCreated.apply(map, arguments); };
      ef.onRealisierteMassnahmeSelected = function () { map.onSelected.apply(map, arguments); };
      ef.onRealisierteMassnahmeDeleted = function () { map.onDeleted.apply(map, arguments); };

      this.urls.PARENTS = this.app.config.get('GIS.CONTROLLER.GET_AXIS_COLLECTION');
      this.urls.PARENT_AT = this.app.config.get('GIS.CONTROLLER.GET_AXIS_AT');
      this.urls.MAIN_AT = this.app.config.get('GIS.CONTROLLER.GET_REAL_AT');
      this.urls.MAIN_ID = this.app.config.get('GIS.CONTROLLER.GET_REAL_BY_ID');
      this.layer = null;
      this.type = "REAL";
    }

   ,loadParents: function (feature) {
      var parents = {}, ids = [], feats = this.geoJson.read(feature.attributes.childs);
      for (var i = 0; i < feats.length; ++i) { parents[feats[i].attributes.AchsenSegmentId] = true; };
      for (id in parents) ids.push(id);
      var protocol = new HTTP({ url: this.urls.PARENTS, format: this.geoJson });
      this.loadByIds(ids, protocol, this.map.parentLayer.addFeatures, this.map.parentLayer);
    }
   
   ,onChanged: function () { this.events.onRealisierteMassnahmeChanged.apply(this.events, arguments); }

   ,createFeature: function () {
     if ($super.createFeature.apply(this, arguments)) {
       this.geoData.attributes.type = "Realisiert";
       this.events.onRealisierteMassnahmeCreated();
     }
    }
   
   ,onSelectFeature: function (fid, readOnly) {
      this.events.onRealisierteMassnahmeSelected(fid, readOnly);
    }

   ,updateChild: function (child) {
      child.attributes.AchsenSegmentId = child.attributes.AchsenSegmentId || child.parent.fid;
      child.attributes.type = child.attributes.type || "";
    }

   , afterOverlays: function () {
     $super.afterOverlays.apply(this, arguments);
     this.map.axis.setVisibility(true);
     this.map.realisiert.setVisibility(true);
     this.layer = this.map.realisiert;
   }

   ,isChild: function (parent, child) {
      return child.attributes.AchsenSegmentId == parent.fid;
    }
  });
})();