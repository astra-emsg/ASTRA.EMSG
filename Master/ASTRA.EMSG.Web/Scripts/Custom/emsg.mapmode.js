(function () {

    /**
    * Class: MapMode
    * Class for generic stuff concernign different modes in the map
    */
    Emsg.MapMode = $.inherit(Object, {

        geoData: null
   , isDirty: false
   , geoJson: new OpenLayers.Format.GeoJSON({ ignoreExtraDims: true })
   , json: new OpenLayers.Format.JSON()
   , urls: null
   , parentStyleMap: Emsg.StyleMaps.AXIS

   , constructor: function (map) {
       this.map = map;
       this.app = map.app;
       this.events = map.events;
       this.urls = {};
   }

   , create: function () {
       this.panel.activateControl(this.create_);
   }
   , activateSelect: function () {
       this.panel.activateControl(this.select);
   }
   , activateEdit: function () {
       this.panel.activateControl(this.edit);
   }
    , open: function (id) {
        var callback = function (features) {
            var feature = features && features.length && features[0];
            if (feature) {
                if (feature.geometry) this.map.map.zoomToExtent(feature.geometry.getBounds());
                this.selectFeature(features[0]);
                this.events.onDataLoaded();
            };
        };
        var protocol = new OpenLayers.Protocol.HTTP({ url: this.urls.MAIN_ID, format: this.geoJson });
        this.loadById(id, protocol, callback, this);
    }

   , getGeoData: function () {
       if (!this.map.editLayer.features.length) return null;
       return this.geoJson.write(this.geoData);
   }

   , getFeature: function () {
       return this.geoData;
   }

   , setFeature: function (feature) {
       this.geoData = feature;
   }

   , loadById: function (id, protocol, callback, scope) {
       if (!id) return;
       var response = protocol.read({
           params: { id: id, dummy: Emsg.Util.now() },
           callback: function (result) {
               if (result.success()) {
                   //layer.addFeatures(result.features);
                   callback.call(scope, result.features)
               }
           }
       });
   }

   , loadByIds: function (ids, protocol, callback, scope) {
       if (!ids || !ids.length) return;
       var response = protocol.read({
           params: { ids: ids, dummy: Emsg.Util.now() },
           callback: function (result) {
               if (result.success()) {
                   //layer.addFeatures(result.features);
                   callback.call(scope, result.features)
               }
           }
       });
   }

   , load: function (feature, loadParents, readOnly) {
       if (!feature.fid) return;
       this.map.editLayer.removeAllFeatures();
       this.setFeature(feature);
       this.isDirty = false;
       var child, children = this.geoJson.read(feature.attributes.childs);
       for (var i = 0; i < children.length; ++i) {
           child = children[i];
           child = new OpenLayers.Feature.Vector.Segment(children[i].geometry, children[i].attributes);
           child.fid = children[i].fid;
           children[i] = child;
       };
       // don't call events when loading since we mark he geodata as dirty on changes in that layer
       this.map.editLayer.addFeatures(children, { silent: true });
       this.map.hasFeature();
       this.map.dirs.update();
       if (this.edit.active) {
           this.edit.handler.refreshDragPoints();
       };

       if (loadParents) {
           this.map.parentLayer.removeAllFeatures();
           this.loadParents(feature);
       };

       this.map.zoomToCurrent();
       this.onSelectFeature(feature.fid, readOnly);
   }

   , getPanel: function () {
       return this.panel;
   }

   , isMultiParent: function () {
       return true;
   }

   , beforeBaseLayers: function () { }
   , afterBaseLayers: function () { }
   , beforeOverlays: function () { }
   , afterOverlays: function () { }
   , onParentNotloaded: function (event) {
       if (event.object == this.edit && !this.canLoadParent()) return;

       this.showDifferentParentMessage();
   }

   , showDifferentParentMessage: function () { }

   , init: function () {
       var handlerOptions = {};
       handlerOptions.parentLayer = this.map.parentLayer;
       handlerOptions.editLayer = this.map.editLayer;
       handlerOptions.multiParent = this.isMultiParent();
       handlerOptions.protocol = new OpenLayers.Protocol.HTTP({ url: this.urls.PARENT_AT, format: this.geoJson });

       var ctrl_create = this.create_ = new OpenLayers.Control.DrawSegment
      (this.map.editLayer
       , OpenLayers.Handler.Segment
       , { displayClass: "emsgControlNew"
          , title: this.app.config.get("TEXT.TOOLTIP.TOOL_" + this.type + "_NEW")
          , type: OpenLayers.Control.TYPE_TOOL
          , handlerOptions: handlerOptions
          , protocol: new OpenLayers.Protocol.HTTP({ url: this.urls.MAIN_AT, format: this.geoJson })
       }
      );
       var ctrl_edit = this.edit = new OpenLayers.Control.DrawSegment
      (this.map.editLayer
       , OpenLayers.Handler.Segment
       , { displayClass: "emsgControlEdit"
          , title: this.app.config.get("TEXT.TOOLTIP.TOOL_" + this.type + "_EDIT")
          , type: OpenLayers.Control.TYPE_TOOL
          , handlerOptions: $.extend({}, handlerOptions, { can_load_parent: { fn: this.canLoadParent, scope: this} })
          , protocol: new OpenLayers.Protocol.HTTP({ url: this.urls.MAIN_AT, format: this.geoJson })
       }
      );
       this.select = new OpenLayers.Control.GetFeaturesAt
      ({ displayClass: "emsgControlSelect"
       , title: this.app.config.get("TEXT.TOOLTIP.TOOL_" + this.type + "_SELECT")
       , type: OpenLayers.Control.TYPE_TOOL
       , protocol: new OpenLayers.Protocol.HTTP({ url: this.urls.MAIN_AT, format: this.geoJson })
      });

       ctrl_create.events.on({
           activate: this.onCreateActivate
      , parentloaded: this.createFeature
      , scope: this
       });

       this.edit.events.register("featureselected", this, this.editFeature);
       this.select.events.register("featureselected", this, this.selectFeature);

       if (!this.isMultiParent()) {
           var listener = { parentNotloaded: this.onParentNotloaded, scope: this };
           ctrl_edit.events.on(listener);
           ctrl_create.events.on(listener);
       }

       this.panel = new OpenLayers.Control.Panel({ displayClass: "emsgControlEditingToolbar", allowDepress: true });
       var ctrls = [ctrl_create, this.edit, this.select];
       Emsg.Util.createCtrlDelegates(ctrls, this.map);

       this.panel.addControls(ctrls);
   }

   , onCreateActivate: function () {
       this.switch_tool = false;
       if (this.geoData && !this.app.cancel()) {
           this.switch_tool = true;
           return false;
       };
       this.switch_tool = true;
       this.map.editLayer.removeAllFeatures();
       this.map.parentLayer.removeAllFeatures();
   }

   , createFeature: function () {
       if (!this.geoData) {
           this.geoData = new OpenLayers.Feature.Vector();
           this.geoData.fid = "";
           this.isDirty = true;
           return true;
       }
   }


   , canLoadParent: function () {
       return !!this.geoData;
   }

   , onSelectFeature: function (fid, readOnly) { }

   , selectFeature: function (feature, readOnly) {
       if (this.geoData && !this.app.cancel()) { return false; };
       this.load(feature, true, readOnly === true);
   }

   , editFeature: function (feature) {
       this.selectFeature(feature, false);
   }

   , redraw: function () {
       this.layer.redraw(true);
   }

   , clear: function () {
       this.map.parentLayer.removeAllFeatures();
       this.map.editLayer.removeAllFeatures();
       this.setFeature(null);
       this.isDirty = false;
       this.redraw();
   }

   , updateGeoData: function (emitEvent) {
       if (!this.geoData) return;
       var parts = [];
       var feat, child, childs = [];
       for (var i = 0, l = this.map.editLayer.features.length; i < l; ++i) {
           child = this.map.editLayer.features[i];
           if (child.parent) {
               if (child.parent.attributes) {
                   if (child.parent.attributes.AchsenId) {
                       child.attributes.AchsenId = child.parent.attributes.AchsenId;
                   }
                   if (child.parent.attributes.AchsenName) {
                       child.attributes.AchsenName = child.parent.attributes.AchsenName;
                   }
               }
           }
           child.fid = child.fid || "";
           this.updateChild(child);
           childs.push(child);
           parts.push(child.geometry);
       };
       this.geoData.geometry = new OpenLayers.Geometry.MultiLineString(parts);
       this.geoData.attributes.childs = this.json.read(this.geoJson.write(childs));
       this.isDirty = true;
       if (!!emitEvent) {
           this.onChanged(this.getGeoData(), this.getGeoLength());
       };
   }

   , getGeoLength: function () {
       var child, len = 0;
       for (var c = 0, cc = this.map.editLayer.features.length; c < cc; ++c) {
           child = this.map.editLayer.features[c];
           len += child.geometry && child.geometry.getLength() || 0;
       };
       len += this.create_.getLength() || 0;
       len += this.edit.getLength() || 0;

       return len;
   }
   , getGeoNames: function () {
       this.updateGeoData(false);
       return this.readGeoNames(this.getGeoData(), this.getGeoLength());
   }
   , readGeoNames: function (geoData, geoLength) {
       //if geodata is null leave achsenNames null to indicate no change
       //geodata is null if only the length changes
       var achsenNames = null;
       //if the last segment is removed from the Strab there is also no geodata, 
       //in order to correctly indicate that there is no axis left achsenNames is set to an empty array
       if (geoLength == 0) {
           achsenNames = [];
       }
       if (geoData) {
           var features = this.geoJson.read(geoData);
           var featurelength = features.length;
           for (var i = 0; i < featurelength; i++) {
               var feature = features[i];
               var collection = feature.attributes.childs;
               var length = collection.features.length;
               var map = {};
               achsenNames = [];
               for (var j = 0; j < length; j++) {
                   var child = collection.features[j];
                   if (child && child.properties) {
                       var achsenId = child.properties.AchsenId;
                       var achsenName = child.properties.AchsenName;
                       if (!map[achsenId]) {
                           achsenNames.push({ AchsenId: achsenId, AchsenName: achsenName });
                       } else {
                           map[achsenId] = achsenName;
                       }
                   }
               }
           }
       }
       return achsenNames;
   }
   , convertChild: function (event) {
       if (event.feature.CLASS_NAME == OpenLayers.Feature.Vector.Segment.prototype.CLASS_NAME) {
           return true;
       };
       var feat = new OpenLayers.Feature.Vector.Segment(event.feature.geometry, event.feature.attributes);
       feat.fid = event.feature.fid;
       if (event.feature.geometry) event.object.addFeatures([feat]);
       return false;
   }

   , convertParent: function (event) {
       if (event.feature.CLASS_NAME == OpenLayers.Feature.Vector.Cutable.prototype.CLASS_NAME) {
           return true;
       };

       var children = this.mode.getChildren(event.feature);
       if (children) children = this.mode.geoJson.read(children);
       children = children || [];

       var exclude = null;
       if (this.mode.geoData) exclude = this.mode.geoData.fid;
       var geom = Emsg.Util.getFreeGeometry(event.feature.geometry, children, exclude);
       var feature = new OpenLayers.Feature.Vector.Cutable(geom, event.feature.attributes);
       feature.fid = event.feature.fid;
       feature.attributes.fid = event.feature.fid;
       feature.attributes.hasOtherChildren = !!children && !!children.length;

       if (this.mode.geoData && this.mode.geoData.fid) {
           feature.attributes.hasOtherChildren = false;
           for (var i = 0; !feature.attributes.hasOtherChildren && i < children.length; ++i) {
               feature.attributes.hasOtherChildren = children[i].fid != this.mode.geoData.fid;
           };
       };

       children = [].concat(this.editLayer.features);
       var id, child;
       for (var i = 0; i < children.length; ++i) {
           child = children[i];
           if (this.mode.isChild(feature, child)) {
               feature.addChild(child);
           };
       };
       if (event.feature.geometry) event.object.addFeatures([feature]);
       return false;
   }

   , isChild: function (feature) {
       return false;
   }

   , getChildren: function (feature) {
       return null;
   }
    });
})();