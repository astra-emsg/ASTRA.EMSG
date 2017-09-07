OpenLayers.Control.BoxSelect = OpenLayers.Class(OpenLayers.Control, {

    boxhandler: null
    , type: OpenLayers.Control.TYPE_TOGGLE
    , enabled: true
    , protocol: null

    , initialize: function (options) {
        OpenLayers.Control.prototype.initialize.apply(this, [options]);
        this.boxhandler = new OpenLayers.Handler.Box(
            this, { done: this.selectBox },
            { boxDivClassName: "olHandlerBoxSelectFeature" });
    }


    , selectBox: function (position) {
        if (position instanceof OpenLayers.Bounds) {
            var minXY = this.map.getLonLatFromPixel({
                x: position.left,
                y: position.bottom
            });
            var maxXY = this.map.getLonLatFromPixel({
                x: position.right,
                y: position.top
            });
            var params = {
                minX: minXY.lon, minY: minXY.lat, maxX: maxXY.lon, maxY: maxXY.lat, dummy: Emsg.Util.now()
            };

            this.protocol.read({
                params: params
                , callback: this.onResult
                , scope: this
            });
            OpenLayers.Element.addClass(this.map.viewPortDiv, "olCursorWait");
        }
    }

    , onResult: function (result) {
        OpenLayers.Element.removeClass(this.map.viewPortDiv, "olCursorWait");
        if (result.success()) {
            this.events.triggerEvent("boxselected", { features: result.features });
        }
    }

    /**
    * Method: setMap
    * Set the map property for the control.
    *
    * Parameters:
    * map - {<OpenLayers.Map>}
    */
    , setMap: function (map) {
        this.boxhandler.setMap(map);
        OpenLayers.Control.prototype.setMap.apply(this, arguments);
    }

    /**
    * Method: activate
    * Activates the control.
    *
    * Returns:
    * {Boolean} The control was effectively activated.
    */
    , activate: function () {
        if (!this.enabled) {
            return false;
        }
        if (!this.active) {
            this.boxhandler.activate();
        }

        return OpenLayers.Control.prototype.activate.apply(this, arguments);
    }
    /**
    * Method: deactivate
    * Deactivates the control.
    *
    * Returns:
    * {Boolean} The control was effectively deactivated.
    */
    , deactivate: function () {
        if (this.active) {
            this.boxhandler.deactivate();
        }
        return OpenLayers.Control.prototype.deactivate.apply(this, arguments);
    }

    , enable: function () {
        if (!this.enabled) {
            this.enabled = true;
            if (this.map) {
                OpenLayers.Element.removeClass(
                this.map.viewPortDiv,
                this.displayClass.replace(/ /g, "") + "Disabled"
            );
            }
        }
    }

  , disable: function () {
      if (this.enabled) {
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
  }
    , CLASS_NAME: "OpenLayers.Control.BoxSelect"
});