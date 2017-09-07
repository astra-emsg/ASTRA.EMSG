(function () {
    "use strict";
    var MapMode = Emsg.MapMode;
    var $super = MapMode.prototype;

    /**
    * Class: MapMode.StreetSection
    * Karte im StrassenAbschnitsmodus bearbeiten
    */
    MapMode.StreetSection = $.inherit(MapMode,
  {
      constructor: function (map) {
          MapMode.call(this, map);

          var ef = this.app.events.form;
          ef.onStrassenabschnittCreated = function () { map.onCreated.apply(map, arguments); };
          ef.onStrassenabschnittSelected = function () { map.onSelected.apply(map, arguments); };
          ef.onStrassenabschnittDeleted = function () { map.onDeleted.apply(map, arguments); };

          this.urls.PARENTS = this.app.config.get('GIS.CONTROLLER.GET_AXIS_COLLECTION');
          this.urls.PARENT_AT = this.app.config.get('GIS.CONTROLLER.GET_AXIS_AT');
          this.urls.MAIN_AT = this.app.config.get('GIS.CONTROLLER.GET_STRABS_AT');
          this.urls.MAIN_ID = this.app.config.get('GIS.CONTROLLER.GET_STRABS_BY_ID');
          this.urls.SPLIT = this.app.config.get('GIS.CONTROLLER.SPLIT_STRABS_AT');
          this.type = "STRABS"
          this.layer = null;
      }

   , loadParents: function (feature) {
       var parents = {}, ids = [], feats = this.geoJson.read(feature.attributes.childs);
       for (var i = 0; i < feats.length; ++i) { parents[feats[i].attributes.AchsenSegmentId] = true; };
       for (var id in parents) ids.push(id);
       var protocol = new OpenLayers.Protocol.HTTP({ url: this.urls.PARENTS, format: this.geoJson });
       this.loadByIds(ids, protocol, this.map.parentLayer.addFeatures, this.map.parentLayer);
   }

   , onChanged: function (geoData, geoLength) {
       this.events.onStrassenabschnittChanged.apply(undefined, [geoData, geoLength, this.readGeoNames(geoData, geoLength)]);
   }

   , createFeature: function () {
       if ($super.createFeature.apply(this, arguments)) {
           this.geoData.attributes.type = "Strassenabschnitt";
           this.events.onStrassenabschnittCreated();
       }
   }

   , onSelectFeature: function (fid, readOnly) {
       this.checkCanSplit();
       this.events.onStrassenabschnittSelected(fid, readOnly);
   }

   , removeFeature: function (feature) {
       this.load(feature, this.events.onStrassenabschnittDeleted);
   }

   , updateChild: function (child) {
       child.attributes.AchsenSegmentId = child.attributes.AchsenSegmentId || child.parent.fid;
       child.attributes.type = child.attributes.type || "";
   }

   , afterOverlays: function () {
       $super.afterOverlays.apply(this, arguments);
       this.map.axis.setVisibility(true);
       this.map.strabs.setVisibility(true);
       this.layer = this.map.strabs;
   }

   , init: function () {
       $super.init.call(this);
       this.split = new OpenLayers.Control.SplitStrassenabschnitt
      (this.map.editLayer
       , OpenLayers.Handler.SegmentPoint
       , { displayClass: "emsgControlSplit"
          , title: this.app.config.get("TEXT.TOOLTIP.TOOL_" + this.type + "_SPLIT")
          , type: OpenLayers.Control.TYPE_TOOL
          , handlerOptions: { featureOwner: this, layerOptions: { styleMap: Emsg.StyleMaps.MAIN_SEGMENT} }
       }
      );

       this.split.events.register("split", this, this.onSplit);

       this.panel.addControls([this.split]);
   }

   , isChild: function (parent, child) {
       return child.attributes.AchsenSegmentId == parent.fid;
   }

   , getChildren: function (feature) {
       return feature.attributes.Strassenabschnitte;
   }

      /**
      * Method: onSplit
      * eventCallback der aufgerufen wird nachdem mit dem Split-Tool
      * ein Punkt auf einem Strassenabschnit geclickt wurde
      */
   , onSplit: function (point) {
       if (this.geoData.data.Zustandsabschnitte.features.length > 0) {
           if (!confirm(this.app.config.get("TEXT.WARNING.SPLIT_CONTAINS_ZABS"))) {
               return;
           }

       }
       blockScreen();
       var protocol = new OpenLayers.Protocol.HTTP({ url: this.urls.SPLIT, format: this.json })
       var me = this;
       var response = protocol.read({
           params: { strassenabschnitId: this.geoData.fid, x: point.x, y: point.y },
           callback: function (result) {
               if (result.success()) {
                   me.onSplitComplete();
               }
               unblockScreen();
           }
       });
   }

      /**
      * Method: onSplitComplete
      * event callback der aufgerufen wird nachdem ein Split an den Server
      * gesendet wurde und erfolgreich war
      */
   , onSplitComplete: function () {
       this.app.cancel();
       this.select.activate();
   }

   , checkCanSplit: function () {
       var allow = false;
       if (this.geoData) {
           var locked = this.geoData != null && this.geoData.attributes.IsLocked;
           allow = locked == null || locked.toLowerCase() == "false";
       };
       if (allow) this.split.enable();
       else this.split.disable();
   }

   , clear: function () {
       $super.clear.apply(this, arguments);
       var active = this.split.active;
       this.checkCanSplit();
       if (active && this.split.active == false) {
           this.select.activate();
       }
   }
  });
})();