OpenLayers.Control.SplitStrassenabschnitt = OpenLayers.Class(OpenLayers.Control, {
  enabled: true // activateable or not :)

 , initialize: function (layer, handler, options) {
   OpenLayers.Control.prototype.initialize.call(this, options);
   this.events.addEventType("split");
   this.layer = layer;
   this.callbacks = OpenLayers.Util.extend( {done: this.onSplit }, this.callbacks );
   this.handler = new handler(this, this.callbacks, this.handlerOptions);
 },

 onSplit: function ( point ) {
   this.events.triggerEvent("split", point);
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
  }

 ,activate: function () {
    return this.enabled && OpenLayers.Control.prototype.activate.apply(this, arguments);
  } 


 ,CLASS_NAME: "OpenLayers.Control.SplitStrassenabschnitt"
});
