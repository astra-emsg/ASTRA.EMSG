OpenLayers.Layer.WMTS.prototype.getURL = function (bounds) {
    bounds = this.adjustBounds(bounds);
    var url = "";
    if (!this.tileFullExtent || this.tileFullExtent.intersectsBounds(bounds)) {
        var center = bounds.getCenterLonLat();
        var info = this.getTileInfo(center);
        var matrixId = this.matrix.identifier;
        var dimensions = this.dimensions, params;
        if (OpenLayers.Util.isArray(this.url)) {
            url = this.selectUrl([
this.version, this.style, this.matrixSet,
this.matrix.identifier, info.row, info.col
].join(","), this.url);
        } else {
            url = this.url;
        }
        if (this.requestEncoding.toUpperCase() === "REST") {
            params = this.params;
            if (url.indexOf("{") !== -1) {
                var template = url.replace(/\{/g, "${");
                var context = {
                    // spec does not make clear if capital S or not
                    style: this.style, Style: this.style,
                    TileMatrixSet: this.matrixSet,
                    TileMatrix: this.matrix.identifier,
                    TileRow: info.row,
                    TileCol: info.col
                };
                if (dimensions) {
                    var dimension, i;
                    for (i = dimensions.length - 1; i >= 0; --i) {
                        dimension = dimensions[i];
                        context[dimension] = params[dimension.toUpperCase()];
                    }
                }
                url = OpenLayers.String.format(template, context);
            } else {
                // include 'version', 'layer' and 'style' in tile resource url
                var path = this.version + "/" + this.layer + "/" + this.style + "/";
                // append optional dimension path elements
                if (dimensions) {
                    for (var i = 0; i < dimensions.length; i++) {
                        if (params[dimensions[i]]) {
                            path = path + params[dimensions[i]] + "/";
                        }
                    }
                }
                // append other required path elements
                path = path + this.matrixSet + "/" + this.matrix.identifier +
"/" + info.row + "/" + info.col + "." + this.formatSuffix;
                if (!url.match(/\/$/)) {
                    url = url + "/";
                }
                url = url + path;
            }
        } else if (this.requestEncoding.toUpperCase() === "KVP") {
            // assemble all required parameters
            params = {
                SERVICE: "WMTS",
                REQUEST: "GetTile",
                VERSION: this.version,
                LAYER: this.layer,
                STYLE: this.style,
                TILEMATRIXSET: this.matrixSet,
                TILEMATRIX: this.matrix.identifier,
                TILEROW: info.row,
                TILECOL: info.col,
                FORMAT: this.format
            };
            url = OpenLayers.Layer.Grid.prototype.getFullRequestString.apply(this, [params]);
        }
    }
    return url;
}