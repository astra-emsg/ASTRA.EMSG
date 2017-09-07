//this function is taken from OpenLayers 2.13.1 OpenLayers.Layer.Grid
//the 2.12 implementation picks the NEXT HIGHER resolution (if there are serverresolutions available eg.[25,50] the method will return 50 for a resolution like 0.2500311149831979)
//this implementation picks the NEAREST (25 in the above example)
//TODO if upgraded to 2.13 or higher remove this file
OpenLayers.Layer.Grid.prototype.getServerResolution = function (resolution) {
        var distance = Number.POSITIVE_INFINITY;
        resolution = resolution || this.map.getResolution();
        if(this.serverResolutions &&
           OpenLayers.Util.indexOf(this.serverResolutions, resolution) === -1) {
            var i, newDistance, newResolution, serverResolution;
            for(i=this.serverResolutions.length-1; i>= 0; i--) {
                newResolution = this.serverResolutions[i];
                newDistance = Math.abs(newResolution - resolution);
                if (newDistance > distance) {
                    break;
                }
                distance = newDistance;
                serverResolution = newResolution;
            }
            resolution = serverResolution;
        }
        return resolution;
    };