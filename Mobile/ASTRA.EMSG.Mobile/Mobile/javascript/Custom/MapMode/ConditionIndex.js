(function () {
    "use strict";
    var Cutable = OpenLayers.Feature.Vector.Cutable;

    var Parent = Emsg.MapMode;
    var $super = Parent.prototype;

    Parent.ConditionIndex = $.inherit(Parent,
  {
      parentStyleMap: Emsg.StyleMaps.PARENT_STRABS
   , constructor: function (map) {
       $super.constructor.call(this, map);

       var ef = this.app.events.form;
       ef.onZustandsabschnittCreated = function () { map.onCreated.apply(map, arguments); };
       ef.onZustandsabschnittSelected = function () { map.onSelected.apply(map, arguments); };
       ef.onZustandsabschnittDeleted = function () { map.onDeleted.apply(map, arguments); };

       this.protocols.MAIN = new OpenLayers.Protocol.Vector({ layer: map.zabs });
       this.protocols.PARENT = new OpenLayers.Protocol.Vector({ layer: map.strabs });
       this.type = "ZABS";
       this.references = [];
   }

   , loadParents: function (feature) {
       var protocol = this.protocols.PARENT;
       this.loadById(feature.attributes.StrassenabschnittsID, protocol, this.map.parentLayer.addFeatures, this.map.parentLayer);
   }

   , onChanged: function () {
       this.events.onZustandsabschnittChanged.apply(undefined, arguments);
   }

   , createFeature: function (event) {
       var parent = event.parent;
       if (parent && $super.createFeature.apply(this, arguments)) {
           var fid = parent.fid;
           this.geoData.attributes.type = "Zustandsabschnitt";
           this.geoData.attributes.StrassenabschnittsID = fid;
           this.events.onZustandsabschnittCreated(fid);
       }
   }

   , onSelectFeature: function (fid, readOnly) {
       this.events.onZustandsabschnittSelected(fid, readOnly);
   }

   , updateChild: function (child) {
       child.attributes.AchsenSegmentId = child.attributes.AchsenSegmentId || child.parent.attributes.AchsenSegmentId;
       child.attributes.type = child.attributes.type || "";
   }

   , afterOverlays: function () {
       $super.afterOverlays.apply(this, arguments);
       this.map.strabs.setVisibility(true);
       this.map.zabs.setVisibility(true);
       this.layer = this.map.zabs;
   }

   , isChild: function (parent, child) {
       return child.attributes.AchsenSegmentId == parent.attributes.AchsenSegmentId;
   }

   , getChildren: function (feature) {
       return feature.attributes.Zustandsabschnitte;
   }

   , showDifferentParentMessage: function () {
       this.events.onZustandsabschnittDifferentParent();
   }
   , isMultiParent: function () {
       return false;
   }

   , convertParent: function (event) {
       if (event.feature.CLASS_NAME == Cutable.prototype.CLASS_NAME) {
           return true;
       };
       var feature, exclude = null;
       var children = this.mode.getChildren(event.feature);
       children = this.mode.geoJson.read(children);
       if (this.mode.geoData) exclude = this.mode.geoData.fid;
       var geom, features = [], parents = this.mode.geoJson.read(event.feature.attributes.childs);
       for (var p = 0, pp = parents.length; p < pp; ++p) {
           geom = Emsg.Util.getFreeGeometry(parents[p].geometry, children, exclude);
           feature = new Cutable(geom, parents[p].attributes);
           feature.fid = event.feature.fid;
           feature.attributes.fid = event.feature.fid;
           feature.attributes.hasOtherChildren = !!children && !!children.length;
           if (geom != null) features.push(feature);
           for (var i = 0; i < children.length; ++i) {
               if (this.mode.geoData && this.mode.geoData.fid) {
                   feature.attributes.hasOtherChildren = false;
                   for (var j = 0; !feature.attributes.hasOtherChildren && j < children.length; ++j) {
                       feature.attributes.hasOtherChildren = children[j].fid != this.mode.geoData.fid;
                   };
               };
           };
       };

       for (p = 0, pp = features.length; p < pp; ++p) {
           feature = features[p];
           children = [].concat(this.editLayer.features);
           var id, child;
           for (var i = 0; i < children.length; ++i) {
               if (this.mode.isChild(feature, children[i])) {
                   feature.addChild(children[i]);
               };
           };
       };
       event.object.addFeatures(features, { silent: true });
       return false;
   }
  });
})();