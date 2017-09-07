Emsg.Util = {};
Emsg.Util.now = function () {
  return (new Date()).getTime();
}
///
/// find the closes geometry to another one
/// @param OpenLayers.Geometry geom geometry to calculate distance from
/// @param [OpenLayers.Geometry] geoms geometries to calculate distance to
/// @param max_dist maximum distance to consider as "found", defaults to Infinity
/// @param details if further details of a find will be returned, defaults to false
/// @return false | OpenLayers.Geometry | {index:integer, found:OpenLayers.Geometry, x0:double, y0:double, x1:double, y1:double, distance:double}
///
Emsg.Util.closest = function (geom, geoms, max_dist, details) {
  var i, found = false, dist, min_dist = { distance: max_dist || Infinity };

  for (i = 0; i < geoms.length && min_dist.distance; ++i) {
    if (!geoms[i]) continue;
    dist = geom.distanceTo(geoms[i], { details: true });
    if (dist.distance <= min_dist.distance) {
      min_dist = dist;
      min_dist.index = i;
      found = geoms[i];
      min_dist.found = found;
    };
  };

  if (details && found) return min_dist;

  return found;
};

Emsg.Util.withinDistance = function (geom, features, max_dist, onlyVisible) {
    var result = [];
    for (i = 0; i < features.length; ++i) {
        if (onlyVisible == true && features[i].style && features[i].style.visibility == 'hidden') {
            continue;
        }
        var dist = geom.distanceTo(features[i].geometry);
        if (dist <= max_dist) {
            var feature = features[i].clone();
            feature.fid = features[i].fid;
            result.push(feature);
        }
    }
    return result;
};
Emsg.Util.closestFeature = function (geom, features, max_dist, details) {
  var geoms = [];
  for (var i = features.length; i--; ) {
    geoms.unshift(features[i].geometry);
  }

  var closest = Emsg.Util.closest(geom, geoms, max_dist, true);
  if (closest.found) {
    closest.found = features[closest.index];
    if (details) {
      return closest;
    }
    return closest.found;
  }

  return false;
};

Emsg.Util.split = function (line, point) {
  if (!point) return false;
  var verts = line.getVertices();
  var seg = new OpenLayers.Geometry.LineString();
  var i, index = false, dist, min_dist = 0.0001;
  var copy = true;

  for (i = 0; i < verts.length; ++i) {
    dist = point.distanceTo(verts[i]);
    if (dist < min_dist) {
      min_dist = dist;
      index = i;
    };
  };

  if (index === false) {
    index = false; min_dist = 0.0001;
    for (i = 1; i < verts.length; ++i) {
      seg.components = [verts[i - 1], verts[i]];
      dist = point.distanceTo(seg);
      if (dist < min_dist) {
        min_dist = dist;
        index = i;
      };
    };
  }
  else {
    copy = false;
    point = verts[index];
  };
  seg.destroy();

  if (index === false) return false;
  var head = verts.slice(0, index);
  var tail = verts.slice(index);
  head.push(point.clone());
  var ret = { head: false, tail: false };
  if (copy) tail.unshift(point.clone());
  if (head.length >= 2) ret.head = new OpenLayers.Geometry.LineString(head);
  if (tail.length >= 2) ret.tail = new OpenLayers.Geometry.LineString(tail);
  return ret;
}

Emsg.Util.cutSegment = function (line, from, to) {
  if (!from || !to) return false;
  var before, ret = {}, first = Emsg.Util.split(line, from);
  if (first === false) return false;
  ret.head = first.head;
  ret.tail = first.tail;
  before = first.head && first.tail && first.head.distanceTo(to) < first.tail.distanceTo(to);
  var second;
  if (before || !first.tail) {
    second = Emsg.Util.split(first.head, to);
    if (second === false) return false;
    ret.head = second.head;
    ret.body = second.tail;
  }
  else {
    // hack if to == from because there are 2 vertexes that are
    // equal then we want to reverse the order of lookup so we get the 2nd vertext
    first.tail.components.reverse();
    second = Emsg.Util.split(first.tail, to);
    if (second === false || !second.tail) return false;
    if (second.head) second.head.components.reverse();
    second.tail.components.reverse();
    ret.body = second.tail;
    ret.tail = second.head;
  }
  return ret;
}

Emsg.Util.getFreeGeometry = function (geometry, children, exclude) {
  if (!children) return geometry;
  var i, l, geom, geoms = [];
  var OLG = OpenLayers.Geometry;
  var LS = OLG.LineString.prototype.CLASS_NAME;
  var MLS = OLG.MultiLineString.prototype.CLASS_NAME;
  for (i = 0, l = children.length; i < l; ++i) {
    if (children[i].fid === exclude) { continue; };
    
    geom = children[i].geometry;
    if ( geom.CLASS_NAME == LS ) geoms.push(geom);
    if ( geom.CLASS_NAME == MLS ) geoms = geoms.concat(geom.components);
  };
  var cut = new OpenLayers.Feature.Vector.Cutable(geometry);
  for (i = 0, l = geoms.length; i < l; ++i) {
    cut.addChild(new OpenLayers.Feature.Vector.Segment(geoms[i]));
  };
  return cut.getFreeGeometry();
};

Emsg.Util.createSegment = function (cutable, point) {
  if ( cutable == null ) return null;
  var geoms = cutable.getFreeGeometry();
  geoms = (geoms && geoms.components) || []
  var geom = Emsg.Util.closest(point, geoms);
  var segment = geom && new OpenLayers.Feature.Vector.Segment(geom.clone());
  if (segment) cutable.addChild(segment);
  return segment;
};

Emsg.Util.createSegments = function (cutable, point) {
  var segments = [];
  if (cutable.length != null) {
    for (var i = 0, ii = cutable.length; i < ii; ++i) {
      segments = segments.concat(Emsg.Util.createSegments(cutable[i], point));
    };
  }
  else if (cutable != null) {
    var empty = !cutable.attributes.hasOtherChildren;
    if (empty) {
      var features = [], child, geoms = cutable.getFreeGeometry();
      geoms = (geoms && geoms.components) || []
      for (var i = 0; geoms && i < geoms.length; ++i) {
        child = new OpenLayers.Feature.Vector.Segment(geoms[i].clone());
        cutable.addChild(child);
        segments.push(child);
      };
    }
    else {
      var segment = Emsg.Util.createSegment(cutable, point);
      if (segment) segments.push(segment);
    };
  };
  return segments;
};

Emsg.Util.createDragFeatures = function (segment) {
  var points = [];
  if (segment.parent) {
    points.push(new OpenLayers.Feature.Vector(segment.start, { type: "start", ref: segment }));
    points.push(new OpenLayers.Feature.Vector(segment.end, { type: "end", ref: segment }));
    for (var i = 0; i < points.length; ++i) {
      points[i].geometry.ref = points[i];
    };
  }
  return points;
};

Emsg.Util.copyPoint = function (dst, src) {
  dst.x = src.x;
  dst.y = src.y;
  dst.clearBounds();
};

///
/// merge componets of a multilinestring if start or end node are equal
Emsg.Util.mergeDuplicateNodes = function (mls) {
  var dst, src, dstv, srcv, reverse, added = [], removed = [];
  for (var i = 0, ii = mls.components.length; i < ii; ++i) {
    reverse = null;
    src = mls.components[i];
    srcv = src.getVertices();
    for (var p = 0, pp = added.length; p < pp && reverse === null; ++p) {
      dst = added[p];
      dstv = dst.components;
           if (dstv[dstv.length - 1].equals(srcv[0])) reverse = [srcv];
      else if (dstv[dstv.length - 1].equals(srcv[srcv.length - 1])) reverse = [];
      else if (dstv[0].equals(srcv[0])) reverse = [dstv, srcv];
      else if (dstv[0].equals(srcv[srcv.length - 1])) reverse = [dstv];
    };
    if (reverse) {
      for (var l = 0, ll = reverse.length; l < ll; ++l) reverse[l].reverse();
      srcv.pop();
      srcv.reverse();
      dst.addComponents(srcv);
      removed.push(src);
    }
    else {
      added.push(src);
    };
  };
  mls.removeComponents(removed);
};

Emsg.Util.mergeSegment = function (segment) {
  var cutable = segment.parent;
  var merge, child, geoms, removed = [], children = [].concat(cutable.children);
  for (var i = 0; i < children.length; ++i) {
    child = children[i];
    if (child.id != segment.id) {
      merge = null;
      if (segment.start.equals(child.start))    merge = { dst: segment.start, src: child.end  , ref: segment.end};
      else if (segment.start.equals(child.end)) merge = { dst: segment.start, src: child.start, ref: segment.end};
      else if (segment.end.equals(child.start)) merge = { dst: segment.end  , src: child.end  , ref: segment.start};
      else if (segment.end.equals(child.end))   merge = { dst: segment.end  , src: child.start, ref: segment.start};
      if (merge && (segment.findReference() === child.findReference())) {
        Emsg.Util.copyPoint(merge.dst, merge.src);
        cutable.removeChild(child);
        removed.push(child);
      };
    };
  };
  segment.updateGeometry();
  return removed;
};

if (!OpenLayers.Layer.Vector.prototype.getFeaturesByAttribute) {
  OpenLayers.Layer.Vector.prototype.getFeaturesByAttribute = function (name, value) {
    var features = [];
    for (var i = 0, ii = this.features.length; i < ii; ++i) {
      if (this.features[i].attributes[name] === value) features.push(this.features[i]);
    }
    return features;
  };
};

Emsg.Util.createCtrlDelegates = function (ctrls, map) {
  for (var i = 0, ii = ctrls.length; i < ii; ++i) {
    (function (ctrl, map) {
      var activate = ctrl.activate;
      ctrl.activate = function () {
        map.stopZoom();
        activate.apply(this, arguments);
      };
    })(ctrls[i], map);
  };
}

Emsg.Util.round = function (number, decimals) {
  var scale = Math.pow(10, decimals || 0);
  return Math.round(number * scale) / scale;
};

OpenLayers.Handler.Box.prototype.deactivate = function () {
  if (OpenLayers.Handler.prototype.deactivate.apply(this, arguments)) {
    if (this.dragHandler && this.dragHandler.deactivate()) {
      if (this.zoomBox) {
        this.removeBox();
      }
    }
    return true;
  } else {
    return false;
  } 
};

jQuery.inherit = function (base, sub) {
  var subclass = sub.constructor || function () { };

  jQuery.extend(subclass.prototype, base.prototype);
  jQuery.extend(subclass.prototype, sub);

  return subclass;
};
