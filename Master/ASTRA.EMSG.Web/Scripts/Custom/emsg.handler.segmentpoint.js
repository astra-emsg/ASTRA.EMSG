OpenLayers.Handler.SegmentPoint = OpenLayers.Class(OpenLayers.Handler, {
  layer: null,
  mouseDown: false,
  stopDown: false,
  stoppedDown: false,
  lastDown: null,
  lastUp: null,
  style: 'temporary',
  featureOwner: null,

  point: null,

  initialize: function (control, callbacks, options) {
    OpenLayers.Handler.prototype.initialize.apply(this, arguments);
  },

  /**
  * Method: modifyFeature
  * Modify the existing geometry given the new point
  *
  * Parameters:
  * pixel - {<OpenLayers.Pixel>} The updated pixel location for the latest
  *     point.
  * drawing - {Boolean} Indicate if we're currently drawing.
  */
  modifyFeature: function (pixel) {
    var lonlat = this.map.getLonLatFromPixel(pixel);
    var geometry = new OpenLayers.Geometry.Point(lonlat.lon, lonlat.lat);
    this.snap(geometry);

    this.point.geometry.x = geometry.x;
    this.point.geometry.y = geometry.y;
    this.point.geometry.clearBounds();

    this.drawFeature();
  },

  /**
  * Method: drawFeature
  * Render geometries on the temporary layer.
  */
  drawFeature: function () {
    if (this.point) this.layer.drawFeature(this.point, this.style);
  },

  /**
  * Method: down
  * Handle mousedown and touchstart.  Add a new point to the geometry and
  * render it. Return determines whether to propagate the event on the map.
  * 
  * Parameters:
  * evt - {Event} The browser event
  *
  * Returns: 
  * {Boolean} Allow event propagation
  */
  mousedown: function (evt) {
    var stopDown = this.stopDown;
    this.mouseDown = true;
    this.lastDown = evt.xy;
    this.stoppedDown = stopDown;
    return !stopDown;
  },

  mousemove: function (evt) {
    if (!this.mouseDown || this.stoppedDown) {
      this.modifyFeature(evt.xy);
    };
    return true;
  },

  mouseup: function (evt) {
    if (this.mouseDown && ( this.lastDown.equals(evt.xy)) ) {
      if ( this.getFeature() ) {
        this.finalize();
      };
    };
    this.stoppedDown = this.stopDown;
    this.mouseDown = false;
    this.modifyFeature(evt.xy, false);
    return !this.stopUp;
  },

  finalize: function (cancel) {
    var key = cancel ? "cancel" : "done";
    this.mouseDown = false;
    this.lastDown = null;
    this.lastUp = null;
    this.lastTouchPx = null;
    this.callback(key, [this.point.geometry]);
    this.drawFeature();
  },

  getFeature: function () {
    return this.featureOwner.getFeature();
  },

  snap: function (point) {
    var parent = this.getFeature(point);
    if ( parent && parent.geometry ) {
      found = point.distanceTo(parent.geometry, { details: true });
      if (found) {
        point.x = found.x1;
        point.y = found.y1;
        point.clearBounds();
      };
    };

  },


  /**
  * APIMethod: activate
  * turn on the handler
  */
  activate: function () {
    if (!OpenLayers.Handler.prototype.activate.apply(this, arguments)) {
      return false;
    }

    // create temporary vector layer for rendering geometry sketch
    // TBD: this could be moved to initialize/destroy - setting visibility here
    var options = OpenLayers.Util.extend({
      displayInLayerSwitcher: false,
      // indicate that the temp vector layer will never be out of range
      // without this, resolution properties must be specified at the
      // map-level for this temporary layer to init its resolutions
      // correctly
      calculateInRange: OpenLayers.Function.True
    }, this.layerOptions);
    this.point = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.Point());
    this.layer = new OpenLayers.Layer.Vector(this.CLASS_NAME, options);
    this.layer.addFeatures([this.point]);
    this.map.addLayer(this.layer);
  },

  deactivate: function () {
    if (!OpenLayers.Handler.prototype.deactivate.apply(this, arguments)) {
      return false;
    }
    // If a layer's map property is set to null, it means that that layer
    // isn't added to the map. Since we ourself added the layer to the map
    // in activate(), we can assume that if this.layer.map is null it means
    // that the layer has been destroyed (as a result of map.destroy() for
    // example.
    if (this.layer.map != null) {
      this.layer.destroy(false);
    }
    this.layer = null;
    this.point = null;
    return true;
  },

  CLASS_NAME: "OpenLayers.Handler.SegmentPoint"
});
