(function () {
    var Control = OpenLayers.Control;
    var DrawFeature = Control.DrawFeature;
    var $super = DrawFeature.prototype;


    Control.DrawAchsFeature = OpenLayers.Class(DrawFeature, {
       
        setSnappingGeometries: function (geoms) {
            this.handler.setSnappingGeometries(geoms);
        },
        getLength: function () {
            var length = 0;
            for (var i = 0, l = this.layer.features.length; i < l; ++i) {
                var feature = this.layer.features[i];
                length += feature.geometry.getLength();

            } return length;
        }

    });

})();
