(function () {
    var Handler = OpenLayers.Handler;
    var Path = Handler.Path;
    var $super = Path.prototype;


    Handler.SnappingPath = OpenLayers.Class(Path, {
        snappingGeometries: [],
        snappingTolerance: 10,
        drag: null,

        setSnappingGeometries: function (geoms) {
            this.snappingGeometries = geoms;
        },

        snap: function (point) {
            if (!this.evt.ctrlKey) {
                var maxDistance = this.getGeoTolerance(this.snappingTolerance);
                var closest = Emsg.Util.closest(point, this.snappingGeometries, maxDistance, true);
                if (closest) {
                    point = new OpenLayers.Geometry.Point(closest.x1, closest.y1);
                }
            }
            return point;
        },

        /**
        * Method: addPoint
        * Add point to geometry.  Send the point index to override
        * the behavior of LinearRing that disregards adding duplicate points.
        *
        * Parameters:
        * pixel - {<OpenLayers.Pixel>} The pixel location for the new point.
        */
        addPoint: function (pixel) {
            var lonlat = this.layer.getLonLatFromViewPortPx(pixel);
            var pointGeom = new OpenLayers.Geometry.Point(lonlat.lon, lonlat.lat);
            pointGeom = this.snap(pointGeom);
            var verts = this.line.geometry.getVertices();
            if (verts.length > 1) {
                var lastvert = verts[verts.length - 2];
            }
            var pointvert = pointGeom.getVertices()[0];
            if (!(verts.length > 1 && pointvert.x == lastvert.x && pointvert.y == lastvert.y)) {
                this.layer.removeFeatures([this.point]);
                this.point = new OpenLayers.Feature.Vector(pointGeom);
                this.line.geometry.addComponent(this.point.geometry, this.line.geometry.components.length);
                this.layer.addFeatures([this.point]);
                this.callback("point", [this.point.geometry, this.getGeometry()]);
                this.callback("modify", [this.point.geometry, this.getSketch()]);
                this.drawFeature();
                delete this.redoStack;
            }
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
            if (!this.line) {
                this.createFeature(pixel);
            }
            var lonlat = this.layer.getLonLatFromViewPortPx(pixel);
            var pointGeom = new OpenLayers.Geometry.Point(lonlat.lon, lonlat.lat);
            pointGeom = this.snap(pointGeom);
            this.point.geometry.x = pointGeom.x;
            this.point.geometry.y = pointGeom.y;
            this.callback("modify", [this.point.geometry, this.getSketch(), drawing]);
            this.point.geometry.clearBounds();
            this.drawFeature();
        },

        getGeoTolerance: function (tolerance) {
            return this.control.map.getResolution() * tolerance;
        }
    });

})();
