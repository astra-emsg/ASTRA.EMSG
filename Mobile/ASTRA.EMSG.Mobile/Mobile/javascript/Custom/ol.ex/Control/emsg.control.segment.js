OpenLayers.Control.DrawSegment = OpenLayers.Class(OpenLayers.Control.DrawFeature, {
  // activateable or not :)
  enabled: true
 ,initialize: function (layer, handler, options) {
   this.callbacks = { 'afterLoadParent': this.afterLoadParent }
   OpenLayers.Control.DrawFeature.prototype.initialize.apply(this, arguments);
   this.events.addEventType("featureselected");
   this.selectFeature = new OpenLayers.Control.GetFeaturesAt({ protocol: this.protocol });
   this.selectFeature.events.register("featureselected", this, this.onSelectFeature);
 },

  onSelectFeature: function () {
    var args = Array.prototype.slice.call(arguments)
    args = ["featureselected"].concat(args);
    this.events.triggerEvent.apply(this.events, args);
  },

  onNotFound: function () {
    this.selectFeature.events.unregister("notfound", this, this.onNotFound);
    this.events.triggerEvent('parentNotloaded');
  },

  setMap: function (map) {
    if (this.map != null) this.map.removeControl(this.selectFeature);
    OpenLayers.Control.DrawFeature.prototype.setMap.apply(this, arguments);
    if (this.map != null) this.map.addControl(this.selectFeature);
  },

  enable: function () {
    this.enabled = true;
    if (this.map) {
      OpenLayers.Element.removeClass(
                this.map.viewPortDiv,
                this.displayClass.replace(/ /g, "") + "Disabled"
            );
    }
  },

  disable: function () {
    this.deactivate();
    this.enabled = false;
    if (this.map) {
      OpenLayers.Element.addClass(
                this.map.viewPortDiv,
                this.displayClass.replace(/ /g, "") + "Disabled"
            );
    }
    this.events.triggerEvent("deactivate");
  },

  activate: function () {
    if (this.enabled) return OpenLayers.Control.DrawFeature.prototype.activate.apply(this, arguments);
    return false;
  },

  /**
  * Method: afterLoadParent
  */
  afterLoadParent: function (event, parentLoaded, parent) {
    if (!parentLoaded) {
      this.selectFeature.events.register("notfound", this, this.onNotFound);
      this.selectFeature.selectClick(event);
    }
    else {
      this.events.triggerEvent("parentloaded", { parent: parent });
    }
  },

  /**
  * Method: drawFeature
  */
  drawFeature: function (feature) {
    var proceed = this.layer.events.triggerEvent(
        "sketchcomplete", { feature: feature }
      );
    if (proceed !== false) {
      feature.state = OpenLayers.State.INSERT;
      //feature.style = OpenLayers.Feature.Vector.style.default;
      if (feature.parent != null) {
        var removed = Emsg.Util.mergeSegment(feature);
        this.layer.removeFeatures(removed);
        this.layer.addFeatures([feature]);
        this.featureAdded(feature);
        this.events.triggerEvent("featureadded", { feature: feature });
      }
      else {
        this.layer.removeFeatures([feature]);
      };
      this.layer.redraw();
    };
  }
, getLength: function () {
  var handler = this.handler;
  if (handler) {
    return handler.getLength();
  }
  return 0;
}

 , CLASS_NAME: "OpenLayers.Control.DrawSegment"
});
