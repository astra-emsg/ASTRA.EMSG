(function () {
  var HTTP = OpenLayers.Protocol.HTTP;

  var Parent = Emsg.MapMode;
  var $super = Parent.prototype;

  Parent.Suggestion = $.inherit(Parent,
  {
    constructor: function (map) {
      $super.constructor.call(this, map);

      var ef = this.app.events.form;
      ef.onMassnahmenvorschlagTeilsystemeCreated = function () { map.onCreated.apply(map, arguments); };
      ef.onMassnahmenvorschlagTeilsystemeSelected = function () { map.onSelected.apply(map, arguments); };
      ef.onMassnahmenvorschlagTeilsystemeDeleted = function () { map.onDeleted.apply(map, arguments); };

      this.urls.PARENTS = this.app.config.get('GIS.CONTROLLER.GET_AXIS_COLLECTION');
      this.urls.PARENT_AT = this.app.config.get('GIS.CONTROLLER.GET_AXIS_AT');
      this.urls.MAIN_AT = this.app.config.get('GIS.CONTROLLER.GET_SUGG_AT');
      this.urls.MAIN_ID = this.app.config.get('GIS.CONTROLLER.GET_SUGG_BY_ID');
      this.layer = null;
      this.type = "SUGG";
    }

   , loadParents: function (feature) {
     var parents = {}, ids = [], feats = this.geoJson.read(feature.attributes.childs);
     for (var i = 0; i < feats.length; ++i) { parents[feats[i].attributes.AchsenSegmentId] = true; };
     for (id in parents) ids.push(id);
     var protocol = new HTTP({ url: this.urls.PARENTS, format: this.geoJson });
     this.loadByIds(ids, protocol, this.map.parentLayer.addFeatures, this.map.parentLayer);
   }

   , onChanged: function () { this.events.onMassnahmenvorschlagTeilsystemeChanged.apply(this.events, arguments); }

   , createFeature: function () {
     if ($super.createFeature.apply(this, arguments)) {
       this.geoData.attributes.type = "Vorschlag";
       this.events.onMassnahmenvorschlagTeilsystemeCreated();
     }
   }

   , onSelectFeature: function (fid, readOnly) {
     this.events.onMassnahmenvorschlagTeilsystemeSelected(fid, readOnly);
   }

   , updateChild: function (child) {
     child.attributes.AchsenSegmentId = child.attributes.AchsenSegmentId || child.parent.fid;
     child.attributes.type = child.attributes.type || "";
   }

   , afterOverlays: function () {
     $super.afterOverlays.apply(this, arguments);
     this.map.axis.setVisibility(true);
     this.map.teilsysteme.setVisibility(true);
     this.layer = this.map.teilsysteme;
   }

   , isChild: function (parent, child) {
     return child.attributes.AchsenSegmentId == parent.fid;
   }
  });
})();