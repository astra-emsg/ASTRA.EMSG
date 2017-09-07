/**
 * inspired by http://geometricnet.sourceforge.net/examples/Direction.js
 * @class
 */
OpenLayers.Control.ShowDirection = OpenLayers.Class(OpenLayers.Control, {
    layer: null,
    lineLayer: null,
    drawBoth: false, // don't draw arrows on both ends if direction is double sided
    arrowScale: 1,
    initialize: function (lineLayer, options) {
        OpenLayers.Control.prototype.initialize.call(this, options);
        this.lineLayer = lineLayer;
        OpenLayers.Renderer.symbol.arrow = [0, 2, 1, 0, 2, 2, 1, 0, 0, 2];
        //ir red=#7D1919
        this.styleMap = new OpenLayers.StyleMap({ 'strokeWidth': 5, 'strokeColor': '#00ccff', graphicName: "arrow", rotation: "${angle}", pointRadius: 10 });
    },

    setMap: function () {
        OpenLayers.Control.prototype.setMap.apply(this, arguments);
        if (this.layer) {
            this.layer.destroy();
        }
        this.layer = new OpenLayers.Layer.Vector(this.CLASS_NAME, { styleMap: this.styleMap, displayInLayerSwitcher: false });
        this.map.addLayer(this.layer);
    },

    activate: function () {
        OpenLayers.Control.prototype.activate.apply(this, arguments);
        this.lineLayer.events.register("featureadded", this, this.update);
        this.lineLayer.events.register("featureremoved", this, this.update);
        this.lineLayer.events.register("featuresadded", this, this.update);
        this.lineLayer.events.register("featuresremoved", this, this.update);
        this.lineLayer.events.register("sketchmodified", this, this.update);
        this.lineLayer.events.register("sketchmodified", this, this.update);
        this.lineLayer.events.register("featuremodified", this, this.update);
        this.map.events.register("changelayer", this, this.layerChanged);
        var index = this.map.getLayerIndex(this.lineLayer);
        this.map.setLayerIndex(this.layer, index - 1);
        this.update();
    },

    deactivate: function () {
        OpenLayers.Control.prototype.deactivate.apply(this, arguments);

        this.lineLayer.events.unregister("featureadded", this, this.update);
        this.lineLayer.events.unregister("featureremoved", this, this.update);
        this.lineLayer.events.unregister("featuresadded", this, this.update);
        this.lineLayer.events.unregister("featuresremoved", this, this.update);
        this.lineLayer.events.unregister("sketchmodified", this, this.update);
        this.lineLayer.events.unregister("sketchmodified", this, this.update);
        this.lineLayer.events.unregister("featuremodified", this, this.update);

        this.layer.removeAllFeatures();
    },

    firstSegment: function (line) {
        var verts = line.getVertices();
        var segment = null;
        if (verts.length >= 2) {
            segment = [verts[0], verts[1]];
        }
        return segment;
    },

    lastSegment: function (line) {
        var verts = line.getVertices();
        var segment = null;
        var len = verts.length;
        if (len >= 2) {
            segment = [verts[len - 1], verts[len - 2]];
        }
        return segment;
    },

    bearing: function (seg, offset) {
        var b_x = 0;
        var b_y = 1;
        var a_x = seg[0].x - seg[1].x;
        var a_y = seg[0].y - seg[1].y;
        var angle_rad = Math.acos((a_x * b_x + a_y * b_y) / Math.sqrt(a_x * a_x + a_y * a_y));
        var angle = offset + 360 / (2 * Math.PI) * angle_rad;
        if (a_x < 0) {
            return 360 - angle;
        } else {
            return angle;
        }
    },

    layerChanged: function (event) {
        if (event.layer == this.lineLayer && event.property == "visibility") {
            this.update();
        }
    },

    update: function () {
        this.layer.removeAllFeatures();
        if (this.map == null) return;
        var index = this.map.getLayerIndex(this.lineLayer);
        this.map.setLayerIndex(this.layer, index - 1);
        var showDirection = this.active && this.lineLayer.getVisibility();
        for (var f = 0, ff = this.lineLayer.features.length; showDirection && f < ff; ++f) {
            this.drawDirection(this.lineLayer.features[f]);
        };
    },

    drawDirection: function (feature) {
        var dir = feature.attributes.direction || this.DIRECTION.BOTH;
        var segments = [];
        switch (dir) {
            case this.DIRECTION.IN:
                segments.push(this.lastSegment(feature.geometry));
                break;

            case this.DIRECTION.OPPOSITE:
                segments.push(this.firstSegment(feature.geometry));
                break;

            case this.DIRECTION.BOTH:
                if (this.drawBoth) {
                    var isInverted = false;
                    if (feature.attributes.IsInverted !== undefined && feature.attributes.IsInverted !== null) {
                        isInverted = feature.attributes.IsInverted === true || (feature.attributes.IsInverted.toLowerCase && feature.attributes.IsInverted.toLowerCase() === "true");
                    } else {
                        feature.attributes.IsInverted = isInverted.toString();
                    }
                    segments.push({ seg: this.firstSegment(feature.geometry), offset: isInverted ? 0 : 180 });
                    segments.push({ seg: this.lastSegment(feature.geometry), offset: isInverted ? 180 : 0 });
                };
                break;
        };

        for (var s = 0, ss = segments.length; s < ss; ++s) {
            var seg = segments[s];
            if (seg) {
                var offset = seg.offset || 0;
                seg = seg.seg || seg;
                if (seg[0] && seg[1]) {
                    var feature, angle = this.bearing(seg, offset);
                    feature = new OpenLayers.Feature.Vector(new OpenLayers.Geometry.MultiPoint(seg[0]), { angle: angle });
                    this.layer.addFeatures(feature);
                }
            };
        };
    },

    DIRECTION: { "IN": "POSITIVE", "OPPOSITE": "NEGATIVE", "BOTH": "BOTH" },
    CLASS_NAME: "OpenLayers.Control.ShowDirection"
});
