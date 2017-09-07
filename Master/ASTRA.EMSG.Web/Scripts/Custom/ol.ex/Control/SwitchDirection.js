OpenLayers.Control.SwitchDirection = OpenLayers.Class(OpenLayers.Control, {
    enabled: true,

    initialize: function (layer, mapMode, options) {
        OpenLayers.Control.prototype.initialize.call(this, options);
        this.events.addEventType("invert");
        this.layer = layer;
        this.mapMode = mapMode;
    },

    enable: function () {
        if (!this.enabled) {
            this.enabled = true;
            if (this.map) {
                OpenLayers.Element.removeClass(
                this.map.viewPortDiv,
                this.displayClass.replace(/ /g, "") + "Disabled"
            );
            }
        }
    },

    disable: function () {
        if (this.enabled) {
            this.enabled = false;
            if (this.map) {
                OpenLayers.Element.addClass(
                this.map.viewPortDiv,
                this.displayClass.replace(/ /g, "") + "Disabled"
            );
            }
        }
    },

    trigger: function () {
        if (this.enabled) {
            var feature = this.mapMode.getFeature();
            if (feature) {
                feature.attributes.IsInverted = !(feature.attributes.IsInverted === true || (feature.attributes.IsInverted.toLowerCase && feature.attributes.IsInverted.toLowerCase() === "true"));
                feature.attributes.IsInverted = feature.attributes.IsInverted.toString();
            }
            this.events.triggerEvent("trigger");
        }
    },

    CLASS_NAME: "OpenLayers.Control.SwitchDirection"
});
