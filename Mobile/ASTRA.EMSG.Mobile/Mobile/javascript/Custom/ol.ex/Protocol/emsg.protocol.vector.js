OpenLayers.Protocol.Vector = OpenLayers.Class(OpenLayers.Protocol, {

    layer: null

   , initialize: function (options) {
       OpenLayers.Protocol.prototype.initialize.apply(this, arguments);
       if (options.layer) {
           if (options.layer.CLASS_NAME == OpenLayers.Layer.Vector.prototype.CLASS_NAME) {
               this.layer = options.layer
           } else {
               throw "OpenLayers.Protocol.Vector requires an \"OpenLayers.Layer.Vector\" object in Property \"layer\" of options";
           }
       } else {
           throw "OpenLayers.Protocol.Vector requires an \"OpenLayers.Layer.Vector\" object in Property \"layer\" of options";
       }
   }

, read: function (options) {
    OpenLayers.Protocol.prototype.read.apply(this, arguments);
    options = options || {};
    options.params = OpenLayers.Util.applyDefaults(options.params, this.options.params);
    options = OpenLayers.Util.applyDefaults(options, this.options);
    var resp = new OpenLayers.Protocol.Response({ requestType: "read" });
    if (options.params.x && options.params.y) {
        var p = new OpenLayers.Geometry.Point(options.params.x, options.params.y);
        var features = Emsg.Util.withinDistance(p, this.layer.features, options.params.tolerance, true);
        resp.features = features;
        resp.code = OpenLayers.Protocol.Response.SUCCESS;
        if (options.callback) {
            options.callback.call(options.scope, resp);
        }
        return resp;
    }
    if (options.params.id) {
        var feature = this.layer.getFeatureByFid(options.params.id);
        resp.features = [feature];
        resp.code = OpenLayers.Protocol.Response.SUCCESS;
        if (options.callback) {
            options.callback.call(options.scope, resp);
        }
        return resp;
    }
}

}
)