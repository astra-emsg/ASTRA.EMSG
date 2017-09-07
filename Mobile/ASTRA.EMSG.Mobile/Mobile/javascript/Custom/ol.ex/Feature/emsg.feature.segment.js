OpenLayers.Feature.Vector.Cutable = OpenLayers.Class(OpenLayers.Feature.Vector, {
    children: null,

    initialize: function (geometry, attributes, style) {
        OpenLayers.Feature.Vector.prototype.initialize.apply(this, arguments);
        if (this.geometry != null) {
            if (this.geometry.CLASS_NAME == OpenLayers.Geometry.LineString.prototype.CLASS_NAME) {
                this.geometry = new OpenLayers.Geometry.MultiLineString([this.geometry]);
            }
            else if (this.geometry.CLASS_NAME == OpenLayers.Geometry.MultiLineString.prototype.CLASS_NAME) {
                if (this.attributes.mergeDuplicateNodes) Emsg.Util.mergeDuplicateNodes(this.geometry);
            }
            else {
                throw "wrong geometry type, LineString or MultiLineString expected";
            };
        };

        this.children = [];
    },

    getFreeGeometry: function (ignore) {
        var split, child, start, end, parts = [];
        if (this.geometry) parts = parts.concat(this.geometry.components);
        for (var c = 0; c < this.children.length; ++c) {
            child = this.children[c];
            if (child.sketch || child === ignore) continue;
            start = child.getStart();
            end = child.getEnd();
            for (var p = 0; p < parts.length; ++p) {
                split = Emsg.Util.cutSegment(parts[p], start, end);
                if (split) {
                    parts.splice(p, 1);
                    if (split.head) parts.push(split.head);
                    if (split.tail) parts.push(split.tail);
                    break;
                };
            };
        };
        if (parts.length == 0) return null;
        return new OpenLayers.Geometry.MultiLineString(parts);
    },

    addChild: function (child) {
        for (var i = 0, ii = this.children.length; i < ii; ++i) {
            if (this.children[i] == child) return;
        }
        this.children.push(child);
        child.setParent(this);
    },
    removeChild: function (child) {
        this.children = OpenLayers.Util.removeItem(this.children, child);
        child.setParent(null);
    },

    CLASS_NAME: "OpenLayers.Feature.Vector.Cutable"
});

OpenLayers.Feature.Vector.Segment = OpenLayers.Class(OpenLayers.Feature.Vector, {
  parent: null,
  start: null,
  end: null,
  sketch: false,

  initialize: function (geometry, attributes, style) {
    OpenLayers.Feature.Vector.prototype.initialize.apply(this, arguments);
    if (geometry) {
      if (geometry.CLASS_NAME == OpenLayers.Geometry.MultiLineString.prototype.CLASS_NAME) {
        if (geometry.components.length > 1) {
          throw "wrong geometry type, LineString or MultiLineString with 1 component expected";
        }
        else {
          this.geometry = geometry = geometry.components[0];
        }
      }
    }
    if (geometry) {
      var verts = geometry.getVertices();
      if (verts.length >= 2) {
        this.start = verts[0].clone();
        this.end = verts[verts.length - 1].clone();
      };
    };
  },

  findReference: function () {
    if (!this.isComplete) return false;
    var parts = this.parent.geometry.components;
    for (var i = 0, l = parts.length; i < l; ++i) {
      if (Emsg.Util.cutSegment(parts[i], this.start, this.end)) {
        return parts[i];
      }
    };
  },

  updateGeometry: function () {
    if (!this.isComplete) return false;
    var parts = this.parent.getFreeGeometry(this).components;
    var split;
    if (this.geometry != null) this.geometry.components = [];
    for (var i = 0, l = parts.length; i < l && !split; ++i) {
      if (split = Emsg.Util.cutSegment(parts[i], this.start, this.end)) {
        if (!this.geometry) this.geometry = new OpenLayers.Geometry.LineString();
        this.geometry.components = split.body.components;
        this.geometry.clearBounds();
      };
    };
  },

  isComplete: function () { return this.start && this.end && this.parent; },

  hasGeometry: function () {
    return !!this.geometry && !!this.geometry.components && this.geometry.components.length >= 2;
  },

  setStart: function (start) { this.start = start; },
  getStart: function () { return this.start; },
  setEnd: function (end) { this.end = end; },
  getEnd: function () { return this.end; },
  setParent: function (parent) { this.parent = parent; },
  setSketch: function (sketching) {
    var parent_needs_update = this.sketch && !sketching;
    this.sketch = sketching;
    //if( parent_needs_update) this.parent.updateGeometry();
  },

  CLASS_NAME: "OpenLayers.Feature.Vector.Segment"
});
