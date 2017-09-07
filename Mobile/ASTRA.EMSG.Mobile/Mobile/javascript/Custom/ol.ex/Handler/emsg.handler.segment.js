OpenLayers.Handler.Segment = OpenLayers.Class(OpenLayers.Handler, {
    layer: null,
    drag: false,
    mouseDown: false,
    stopDown: false,
    stoppedDown: false,
    lastDown: null,
    lastUp: null,
    snapTolerance: 30,
    multiParent: true, // always try to load a parent if not clicked a parental Geometry or only once

    snapGeometry: null,
    dragTarget: null,
    point: null,
    move: null,
    ref: null,
    segment: null,
    parent: null,
    child: null,
    parentLayer: null,
    editLayer: null,
    points: [],
    can_load_parent: null,


    initialize: function (control, callbacks, options) {
        OpenLayers.Handler.prototype.initialize.apply(this, arguments);
    },

    createSegment: function (pixel) {
        var lonlat = this.map.getLonLatFromPixel(pixel);
        var pos = new OpenLayers.Geometry.Point(lonlat.lon, lonlat.lat);
        var segments = Emsg.Util.createSegments(this.parent, pos);
        for (var i = 0; i < segments.length; ++i) {
            this.callback("create", [segments[i]]);
            this.callback("done", [segments[i]]);
            alert(this.parent);
            alert(this.parent.attributes.IsInverted);
            segments[i].attributes.IsInverted = this.parent.attributes.IsInverted;
            this.refreshDragPoints();
        };
        this.drawFeature();
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
    modifyFeature: function (pixel, drawing) {
        var lonlat = this.map.getLonLatFromPixel(pixel);
        var geometry = new OpenLayers.Geometry.Point(lonlat.lon, lonlat.lat);
        this.snap(geometry);
        if (this.dragTarget && !this.drag && this.mouseDown) {
            this.drag = true;
            this.move = this.dragTarget;
            this.segment = this.dragTarget.attributes.ref;
            this.segment.setSketch(true);
            this.parent = this.segment.parent;
            this.segment.layer.removeFeatures([this.segment]);
            this.ref = this.segment.start;
            if (this.dragTarget.attributes.type == "start") this.ref = this.segment.end;
        };

        this.point.geometry.x = geometry.x;
        this.point.geometry.y = geometry.y;
        this.point.geometry.clearBounds();

        if (this.move) {
            this.move.geometry.x = this.point.geometry.x;
            this.move.geometry.y = this.point.geometry.y;
            this.move.geometry.clearBounds();
        };

        if (this.drag) {
            this.segment.updateGeometry();
            this.callback("modify", [this.segment, drawing]);
        };

        this.drawFeature();
    }

  , getLength: function () {
      var len = 0;
      var s = this.segment;
      var geom = s && s.geometry;
      if (geom && s.sketch) {
          len = geom.getLength();
      }
      return len;
  }

    /**
    * Method: drawFeature
    * Render geometries on the temporary layer.
    */
 , drawFeature: function () {
     if (this.segment) {
         if (this.segment.hasGeometry()) this.layer.addFeatures([this.segment], { silent: true });
         else this.layer.removeFeatures([this.segment], { silent: true });
         this.layer.drawFeature(this.segment, this.style);
     }
     if (this.point) this.layer.drawFeature(this.point, this.style);
     for (var i = 0; i < this.points.length; ++i) {
         this.layer.drawFeature(this.points[i], this.style);
     };
 },

    getSnapTolerance: function () {
        return this.getGeoTolerance(this.snapTolerance);
    },

    getGeoTolerance: function (tolerance) {
        return this.control.map.getResolution() * tolerance;
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
        var stopDown = this.stopDown || this.dragTarget;
        //if ( this.parent && this.drag ) stopDown = true;
        this.mouseDown = true;
        this.lastDown = evt.xy;
        evt.preventDefault;
        this.stoppedDown = stopDown;
        return !stopDown;
    },

    mousemove: function (evt) {
        if (!this.mouseDown || this.stoppedDown) {
            this.modifyFeature(evt.xy, !!this.lastUp);
        };
        return true;
    },

    mouseup: function (evt) {
        if (this.mouseDown && (this.drag || !this.lastUp || this.lastDown.equals(evt.xy))) {
            this.lastUp = evt.xy;
            if (this.child) {
                // we have a child so we clicked on one and want to remove it
                this.child.parent.removeChild(this.child);
                this.editLayer.removeFeatures([this.child]);
                this.refreshDragPoints();
            }
            // not on parent, so load one
            else if (!this.parent) {
                if (this.lastDown.equals(evt.xy)) {
                    this.loadParent(evt);
                    this.lastUp = null;
                };
            }
            else if (!this.segment) {
                this.createSegment(evt.xy);
            }
            else if (this.segment.hasGeometry()) {
                this.finishGeometry()
            }
            else if (this.drag) {
                this.finalize(true);
            };
        };
        this.stoppedDown = this.stopDown;
        this.mouseDown = false;
        this.drag = false;
        this.dragTarget = null;
        this.modifyFeature(evt.xy, false);
        return !this.stopUp;
    },

    destroyFeature: function (cancel) {
        this.segment = null;
        this.move = null;
        this.ref = null;
    },

    /**
    * APIMethod: finishGeometry
    * Finish the geometry and send it back to the control.
    */
    finishGeometry: function () {
        this.segment.setSketch(false);

        if (!this.drag) {
            this.points = this.points.concat(Emsg.Util.createDragFeatures(this.segment));
        };

        this.lastPoint = false;
        this.finalize();
    },

    finalize: function (cancel) {
        var key = cancel ? "cancel" : "done";
        this.mouseDown = false;
        this.lastDown = null;
        this.lastUp = null;
        this.lastTouchPx = null;
        this.parent = null;
        this.layer.removeFeatures([this.segment]);
        this.callback(key, [this.segment]);
        this.refreshDragPoints();
        this.destroyFeature(cancel);
        this.drawFeature();
    },

    findParent: function (point) {
        var geoms = [];
        for (var i = 0; i < this.parentLayer.features.length; ++i) {
            geoms.push(this.parentLayer.features[i].getFreeGeometry());
        };
        var max_dist = this.getSnapTolerance();
        var found = Emsg.Util.closest(point, geoms, max_dist, true);
        var parent = null;
        if (found) {
            parent = this.parentLayer.features[found.index];
        };
        return parent;
    },

    findChild: function (point, max_dist) {
        var geoms = [];
        for (var i = 0; i < this.editLayer.features.length; ++i) {
            geoms.push(this.editLayer.features[i].geometry);
        };
        var found = Emsg.Util.closest(point, geoms, max_dist, true);
        var child = null;
        if (found) {
            child = this.editLayer.features[found.index];
        };
        return child;
    },

    getNodes: function (multiline) {
        var nodes = [];
        var part;
        if (multiline && multiline.components) {
            for (var i = 0; i < multiline.components.length; ++i) {
                part = multiline.components[i];
                if (part.components && part.components.length >= 2) {
                    nodes.push(part.components[0]);
                    nodes.push(part.components[part.components.length - 1]);
                };
            };
        };
        return nodes;
    },

    snap: function (point) {
        var parent = this.findParent(point);
        // axis snapping
        var nodes = [];
        var geoms = [];
        var geom = null;
        var feat;
        for (var i = 0; i < this.parentLayer.features.length; ++i) {
            feat = this.parentLayer.features[i];
            geom = feat.getFreeGeometry();
            nodes = nodes.concat(this.getNodes(geom));
            if (!this.move || feat != this.parent) geoms.push(geom);
        };
        var p = point.clone();
        var max_dist = this.getSnapTolerance();
        this.child = null;

        if (this.move) {
            var ref_geom = Emsg.Util.closest(this.ref, this.parent.getFreeGeometry().components);
            found = p.distanceTo(ref_geom, { details: true });
            if (found) {
                p.x = found.x1;
                p.y = found.y1;
                p.clearBounds();
            };
        }
        else {
            this.parent = parent;

            found = Emsg.Util.closest(p, geoms, max_dist, true);
            if (found) {
                p.x = found.x1;
                p.y = found.y1;
                p.clearBounds();
            };

            this.child = this.findChild(point, found.distance || max_dist);
        };

        if (!this.drag) {
            this.dragTarget = null;
            var points = this.points;
            for (var i = points.length; i--; ) {
                nodes.unshift(points[i].geometry);
            }
        };

        found = Emsg.Util.closest(p, nodes, max_dist, true) || found;
        if (found) {
            p.x = found.x1;
            p.y = found.y1;
            p.clearBounds();
            if (found.found && found.found.ref) {
                this.dragTarget = found.found.ref;
            }
        };
        if (this.move) {
            var ref_geom = Emsg.Util.closest(this.ref, this.parent.getFreeGeometry().components);
            found = p.distanceTo(ref_geom, { details: true });
        };
        if (found) {
            point.x = found.x1;
            point.y = found.y1;
            point.clearBounds();
        };

    },

    getLayerCallbacks: function () {
        return {
            featuresadded: this.refreshDragPoints
     , featuresremoved: this.refreshDragPoints
     , scope: this
        };
    }

    /**
    * APIMethod: activate
    * turn on the handler
    */
 , activate: function () {
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
     this.refreshDragPoints();
     this.parentLayer.events.on(this.getLayerCallbacks());
     this.editLayer.events.on(this.getLayerCallbacks());
     this.map.addLayer(this.layer);
 },

    deactivate: function () {
        if (!OpenLayers.Handler.prototype.deactivate.apply(this, arguments)) {
            return false;
        }
        // call the cancel callback if mid-drawing
        if (this.drawing) {
            this.cancel();
        }
        this.destroyFeature();
        // If a layer's map property is set to null, it means that that layer
        // isn't added to the map. Since we ourself added the layer to the map
        // in activate(), we can assume that if this.layer.map is null it means
        // that the layer has been destroyed (as a result of map.destroy() for
        // example.
        if (this.layer.map != null) {
            this.layer.destroy(false);
        }
        this.parentLayer.events.un(this.getLayerCallbacks());
        this.editLayer.events.un(this.getLayerCallbacks());

        this.layer = null;
        this.point = null;
        this.move = null;
        this.points = [];
        return true;
    },

    refreshDragPoints: function () {
        this.layer.removeFeatures(this.points);
        this.points = [];
        for (var i = 0; i < this.editLayer.features.length; ++i) {
            this.points = this.points.concat(Emsg.Util.createDragFeatures(this.editLayer.features[i]));
        };
        this.layer.addFeatures(this.points);
    },

    canLoadParent: function () {
        var can = this.can_load_parent || {};
        var fn = can.fn || function () { return true; };
        var scope = can.scope || window;
        var can = fn.call(scope);
        return (this.multiParent || this.parentLayer.features.length == 0) && can;
    },

    /**
    * try to get a parent from the server at given pixel
    * @param {OpenLayers.Event}
    */
    loadParent: function (event) {
        var pixel = event.xy;
        if (!this.canLoadParent()) {
            this.callback("afterLoadParent", [event, false]);
            return false;
        }

        var pos = this.map.getLonLatFromPixel(pixel);
        pos = new OpenLayers.Geometry.Point(pos.lon, pos.lat);

        var x1 = pixel.x + 5;
        var y1 = pixel.x + 5;
        var tolerancecoord = this.map.getLonLatFromPixel({ x: x1, y: y1 });

        var tolerance = Math.abs(pos.x - tolerancecoord.lon);

        OpenLayers.Element.addClass(this.map.viewPortDiv, "olCursorWait");
        var response = this.protocol.read({
            params: { x: pos.x, y: pos.y, tolerance: tolerance, dummy: Emsg.Util.now() }
       , callback: function (result) {
           this.onLoadParent(result, event);
       }
       , scope: this
        });
    }
 , onLoadParent: function (result, event) {

     var pos = this.map.getLonLatFromPixel(event.xy);
     pos = new OpenLayers.Geometry.Point(pos.lon, pos.lat);

     var feat = null;
     if (result.success() && !!result.features && result.features.length) {
         result = result.features[0];
         if (!this.parentLayer.getFeatureByFid(result.fid)) {
             this.parentLayer.addFeatures(result);
         };
         var geoms = [], parents = this.parentLayer.getFeaturesByAttribute("fid", result.fid);
         for (var i = parents.length; i--; ) {
             geoms.unshift(parents[i].getFreeGeometry());
         }
         var details = Emsg.Util.closest(pos, geoms, undefined, true);
         if (details && details && details.distance < this.getSnapTolerance()) {
             feat = parents[details.index];
             for (var p = parents.length; p--; ) {
                 var parent = parents[p];
                 var segments = Emsg.Util.createSegments(parent, pos);
                 for (var i = 0; i < segments.length; ++i) {
                     segments[i].attributes.IsInverted = parent.attributes.IsInverted;
                     this.callback("create", [segments[i]]);
                     this.callback("done", [segments[i]]);
                     this.refreshDragPoints();
                 }
             }
         }
         else {
             this.parentLayer.removeFeatures(parents);
         }
     };
     this.callback("afterLoadParent", [event, feat != null, feat]);
     // Reset the cursor.
     OpenLayers.Element.removeClass(this.map.viewPortDiv, "olCursorWait");
 }
 , CLASS_NAME: "OpenLayers.Handler.Segment"
});
