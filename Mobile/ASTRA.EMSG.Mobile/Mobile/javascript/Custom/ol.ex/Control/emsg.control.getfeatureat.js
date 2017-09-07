OpenLayers.Control.GetFeatureAt = OpenLayers.Class(OpenLayers.Control.GetFeature, {
    enabled: true
 , can_select: null
 , format: new OpenLayers.Format.GeoJSON()

 , initialize: function () {
     this.can_select = {};
     OpenLayers.Control.GetFeature.prototype.initialize.apply(this, arguments);
     this.events.addEventType("parentselected");
     this.events.addEventType("parentAndChildselected");
     if (this.cursor) {
         var options = OpenLayers.Util.extend({ featureOwner: this, layerOptions: { styleMap: Emsg.StyleMaps.MAIN_SEGMENT} }, this.handlerOptions);
         this.handlers.cursor = new OpenLayers.Handler.SegmentPoint(this, {}, options);
     };
 }
 , getFeature: function () { return null; }
 , selectClick: function (evt) {
     var pos = this.map.getLonLatFromPixel(evt.xy); ;
     var x1 = evt.xy.x + 5;
     var y1 = evt.xy.x + 5;
     var tolerancecoord = this.map.getLonLatFromPixel({ x: x1, y: y1 });

     var tolerance = Math.abs(pos.lon - tolerancecoord.lon);


     this.setModifiers(evt);
     this.request(pos, tolerance);
 }
 , canSelectFeature: function () {
     var can = this.can_select;
     var fn = can.fn || function () { return true; };
     var scope = can.scope || window;
     var can = fn.call(scope);
     return can;
 }

 , createCallback: function (property_name, data) {
     return (function (result) {
         data[property_name] = [];
         if (result.success()) {
             data[property_name] = result.features;
         }
         this.onResult(data);
     })
 }

 , request: function (pos, tolerance) {
     var data = { childs: [], parents: [], pos: pos };
     OpenLayers.Element.addClass(this.map.viewPortDiv, "olCursorWait");
     var params = { x: pos.lon, y: pos.lat, tolerance: tolerance, dummy: Emsg.Util.now() };
     if (this.protocolParent) {
         data.parents = false;
     }

     if (this.canSelectFeature()) {
         data.childs = false;
         this.protocol.read({
             params: params
      , callback: this.createCallback('childs', data)
      , scope: this
         });
     }

     if (this.protocolParent) {
         data.parents = false;
         this.protocolParent.read({
             params: params
      , callback: this.createCallback('parents', data)
      , scope: this
         });
     }
 }

 , onResult: function (data) {
     var childs = data.childs, parents = data.parents;
     if (!childs || !parents) return;

     // Reset the cursor.
     OpenLayers.Element.removeClass(this.map.viewPortDiv, "olCursorWait");

     this.onSuccess(childs, parents, data.pos);
 }

  , onSuccess: function (childs, parents, pos) {
      var p = new OpenLayers.Geometry.Point(pos.lon, pos.lat);
      var features = childs.concat(parents);
      var found = Emsg.Util.closestFeature(p, features, null, true);
      if (!found) {
          this.events.triggerEvent('notfound');
          return;
      }

      var event = 'featureselected';
      if (found.index >= childs.length) {
          event = 'parentselected';
      }

      this.events.triggerEvent(event, found.found);

      var foundParent = Emsg.Util.closestFeature(p, parents, null, true);
      var foundChild = Emsg.Util.closestFeature(p, childs, null, true);
      if (foundChild && foundParent) {
          this.events.triggerEvent('parentAndChildselected', { parent: foundParent.found, child: foundChild.found });
      }

  }

 , enable: function () {
     this.enabled = true;
     if (this.map) {
         OpenLayers.Element.removeClass(
                this.map.viewPortDiv,
                this.displayClass.replace(/ /g, "") + "Disabled"
            );
     }
 }

 , disable: function () {
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

 , activate: function () {
     return this.enabled && OpenLayers.Control.GetFeature.prototype.activate.apply(this, arguments);
 }

 , CLASS_NAME: "OpenLayers.Control.GetFeatureAt"
});
