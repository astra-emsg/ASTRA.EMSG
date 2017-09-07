(function () {
  var Map = Emsg.Map;
  var $super = Map.prototype;

  Map.Edit = $.inherit(Map, {
    zoomToCurrentControl: true
    , constructor: function (app, mode) {
      Map.apply(this, arguments);
      this.geoData = null;
      this.mode = new mode(this);

      var that = this;
      this.app.events.form.onSaveSuccess = function () { that.onSaveSuccess.apply(that, arguments); };
      this.app.events.form.onDataLoaded = function () { that.onDataLoaded.apply(that, arguments); };
    }

    , beforeBaseLayers: function () {
      $super.beforeBaseLayers.call(this);
      this.mode.beforeBaseLayers();
    }

    , afterBaseLayers: function () {
      $super.afterBaseLayers.call(this);
      this.mode.afterBaseLayers();
    }

    , beforeOverlays: function () {
      $super.beforeOverlays.call(this);
      this.mode.beforeOverlays();
    }
    , afterOverlays: function () {
      $super.afterOverlays.call(this);
      this.mode.afterOverlays();
    }

    , init: function () {
      $super.init.apply(this, arguments);

      var config = { projection: this.map.projection, maxExtent: this.map.maxExtent, displayInLayerSwitcher: false };
      config.styleMap = this.mode.parentStyleMap;
      this.parentLayer = new OpenLayers.Layer.Vector("Parent Layer", config);

      //config = { projection: config.projection, maxExtent: config.maxExtent, displayInLayerSwitcher: false };
      config.styleMap = Emsg.StyleMaps.MAIN_SEGMENT;
      this.editLayer = new OpenLayers.Layer.Vector("Edit Layer", config);
      this.dirs = new OpenLayers.Control.ShowDirection(this.editLayer, { autoActivate: true, drawBoth: true });

      this.parentLayer.events.register("beforefeatureadded", this, this.mode.convertParent);
      this.editLayer.events.register("beforefeatureadded", this, this.mode.convertChild);
      this.editLayer.events.register("featureadded", this, this.updateGeoData);
      this.editLayer.events.register("featuremodified", this, this.updateGeoData);
      this.editLayer.events.register("featureremoved", this, this.updateGeoData);
      this.editLayer.events.register("sketchmodified", this, this.updateGeoData);

      this.mode.init();
      this.map.addControls([this.mode.getPanel(), this.dirs]);
      this.map.addLayers([this.parentLayer, this.editLayer]);

      this.addControls();

      this.events.initialized();
      this.mode.clear();
      this.mode.activateSelect();
    }

    , onSaveSuccess: function () {
      this.editLayer.removeAllFeatures();
      this.parentLayer.removeAllFeatures();
      this.mode.clear();
    }

    , onCreated: function () { this.mode.create(); }
    , onSelected: function (id) {
      this.mode.open(id);
      this.mode.activateEdit();
    }
    , onDeleted: function () {
        //this.mode.activateSelect();
        this.onSaveSuccess(); 
        }
    , onCanceled: function () { this.onSaveSuccess(); }
    , onDataLoaded: function () {
      if (this.mode.isDirty) {
        this.updateGeoData(true);
      };
      this.stopZoom();
      this.zoomToCurrent();
    }
    , updateGeoData: function (emitEvent) { this.mode.updateGeoData(emitEvent); }
    , getGeoData: function () { return this.mode.getGeoData(); }
    , getGeoLength: function () { return this.mode.getGeoLength(); }
    , getGeoNames: function () { return this.mode.getGeoNames(); }
    , prepareCancel: function () {
      // maybe highlight unsaved changes
      return { unsavedChanges: false, message: null };
    }
    , executeCancel: function () { this.onCanceled(); }
  });
})();
