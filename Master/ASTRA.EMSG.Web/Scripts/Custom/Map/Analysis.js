(function () {
    "use strict";
    var Bounds = OpenLayers.Bounds;

    var Parent = Emsg.Map
    var $super = Parent.prototype;

    var Type = {
        TILES: 'mapBackgroundLayers'
   , BACKGROUND: 'mapBackgroundLayers'
   , BACKGROUND2: 'mapBackgroundLayers'
   , BACKGROUND_AV: 'mapBackgroundLayersAV'
   , OVERLAY: 'mapLayersAV'
   , OVERLAY_AV: 'mapLayersAV'
   , OVERLAY_AV_GWVUL: 'mapLayersAV'
   , OVERLAY_AV_GWVK: 'mapLayersAV'
   , OVERLAY_AV_KBS: 'mapLayersAV'
   , ARCGIS: 'mapLayers'
   , OTHER: 'OTHER'
    };
    for (var k in Type) {
        Type[k] = { name: k, value: Type[k] };
    };

    Parent.Analysis = $.inherit(Parent,
  {
      pdfDownload: true
   , layer_property: null
   , currentMapExtent: null
   , sublayers: []
   , sublayer_properties: []
   , hasMultipleReportLayers: false
   , constructor: function (app) {
       Emsg.Map.prototype.constructor.apply(this, arguments);
       this.settings = {};

       var that = this;
       this.app.events.form.onFilterChanged = function () { that.updateSetting.apply(that, arguments); };
   }

   , afterOverlays: function () {
       $super.afterOverlays.apply(this, arguments);
       this.layerId = this.app.config.get('WMS.ARCGIS.LAYERID');
       this.layer = this[this.layer_property];
       if (this.layer) {
           this.layer.setVisibility(true);
       };
       if (this.sublayer_properties && this.sublayer_properties.length > 0) {
           this.hasMultipleReportLayers = true;
           for (var i = 0; i < this.sublayer_properties.length; i++) {
               var sublayer = this[this.sublayer_properties[i].name];
               if (sublayer) {
                   this.sublayers.push({ layer: sublayer, possibleFilter: this.sublayer_properties[i].possibleFilter, isRelevant: this.sublayer_properties[i].isRelevant });
                   sublayer.setVisibility(true);
               }
           }

       }
       this.update();
   }

   , init: function () {
       $super.init.apply(this, arguments);
       this.addControls();
       this.events.initialized();
       this.events.onDataLoaded();
       this.map.events.register("moveend", this, this.onMoveEnd);
       this.updateSetting();
       this.onMoveEnd();
   }

   , updateSetting: function () {
       $.extend(this.settings, emsg.form.getFilterParameters());
       this.update();
   }

   , onMoveEnd: function () {
       this.events.onFilterChanged();
   }

   , getFilterParameters: function () {
       var params = {
           mapBackgroundLayers: ""
      , mapBackgroundLayersAV: ""
      , mapLayers: ""
      , mapLayersAV: ""
       };
       var size = this.map.getSize();
       $.extend(params, this.layers());
       //if the div is not visible (if the List Tab is active)  the div width and height are 0, Openlayers needs width and height to calculate the extent
       //OL defaults to the style info if the div width and height are 0, this works for the height but the width is "100%" wich gets interpreted as 100 px
       //to avoid this problem the last visible Extent will be saved and used if the div is not visible
       if ($(emsg.map.map.div).is(":visible") || !this.currentMapExtent) {
           params.mapBBOX = this.map.getExtent() != null ? this.map.getExtent().toBBOX() : "0,0,0,0";
           this.currentMapExtent = params.mapBBOX
       } else {
           params.mapBBOX = this.currentMapExtent;
       }
       params.mapBBOXFilter = params.mapBBOX;
       params.mapSize = "" + size.w + "," + size.h;
       params.mapLayerDefs = "";
       if (!this.hasMultipleReportLayers || this.layer.visibility) {
           params.mapLayerDefs = this.layerDefs();
       }
       if (this.sublayers && this.sublayers.length > 0) {
           for (var i = 0; i < this.sublayers.length; i++) {
               if (this.sublayers[i].layer.visibility) {
                   if (params.mapLayerDefs) {
                       params.mapLayerDefs += ';'
                   }
                   params.mapLayerDefs += this.layerDefs(this.sublayers[i].layer.params.LAYERS, this.sublayers[i].possibleFilter);
               }
           }
       }

       var scale = this.scale_line;
       var div = $(scale.div);
       var width = div.width();
       var txt = div.children('.olControlScaleLineTop').text();
       params.scale_width = width;
       params.scale_text = txt;

       return params;
   }

   , layerDefs: function (lyrId, possibleFilter) {
       if (!lyrId) {
           lyrId = this.layerId;
       }
       var layerDefs = "", props = [], value;
       for (var prop in this.settings) {
           value = this.settings[prop];
           if (value != null && value != "" && (!possibleFilter || $.inArray(prop, possibleFilter) > -1)) {
               switch (prop) {
                   case "ZustandsindexVon":
                       prop = prop.replace("ZustandsindexVon", "ZST_INDEX>");
                       props.push("(" + prop + "='" + value.replace(",", ".") + "')");  // replace , with . (for different languages)
                       break;
                   case "ZustandsindexBis":
                       prop = prop.replace("ZustandsindexBis", "ZST_INDEX<");
                       props.push("(" + prop + "='" + +value.replace(",", ".") + "')"); // replace , with . (for different languages)
                       break;
                   //STRING operations                                                     
                   case "Strassenname":
                   case "Projektname":
                   case "Inspektionsroutename":
                   case "Ortsbezeichnung":
                       props.push("(" + prop + " like '%25" + value.toLowerCase() + "%25')");
                       break;
                   case "InspektionsrouteInInspektionBei":
                       props.push("(" + "ININSPEKTIONBEI" + " like '%25" + value.toLowerCase() + "%25')");
                       break;
                   //DATE operations                                                    
                   case "InspektionsrouteInInspektionBisVon":
                       var date = this.parseDate(value);
                       props.push("(" + "ININSPEKTIONBIS>" + "='" + date.toISOString() + "')"); // replace / with . (for different languages)
                       break;
                   case "InspektionsrouteInInspektionBisBis":
                       var date = this.parseDate(value);
                       props.push("(" + "ININSPEKTIONBIS<" + "='" + date.toISOString() + "')");  // replace / with . (for different languages)
                       break;
                   case "AusfuehrungsanfangVon":
                       var date = this.parseDate(value);
                       props.push("(" + "AUSFUEHRUNGSANF>" + "='" + date.toISOString() + "')");  // replace / with . (for different languages)
                       break;
                   case "AusfuehrungsanfangBis":
                       var date = this.parseDate(value);
                       props.push("(" + "AUSFUEHRUNGSANF<" + "='" + date.toISOString() + "')");  // replace / with . (for different languages)
                       break;
                   default:
                       props.push("(" + prop + "='" + value + "')");
                       break;
               }
           };
       };
       if (props.length) {
           layerDefs = lyrId + ":(" + props.join(" and ") + ")";
       };
       return layerDefs;
   },

      parseDate:function(input) {
        var parts = input.match(/(\d+)/g);
        return new Date(parts[2], parts[1]-1, parts[0]);
    }

   , update: function () {
       if (this.layer) this.layer.mergeNewParams({ layerDefs: this.layerDefs() });
       if (this.sublayers) {
           var length = this.sublayers.length;
           for (var i = 0; i < length; i++) {
               this.sublayers[i].layer.mergeNewParams({ layerDefs: this.layerDefs(this.sublayers[i].layer.params.LAYERS, this.sublayers[i].possibleFilter) });
           }
       }
   }
   , hasVisibleReportLayer: function () {
       var ret = !this.hasMultipleReportLayers;
       ret = ret || this.layer.visibility;
       for (var i = 0; i < this.sublayers.length; i++) {
           ret = ret || (this.sublayers[i].layer.visibility && this.sublayers[i].isRelevant);
       }
       return ret;
   }

   , isReportLayer: function (layer) {
       var l = layer;
       var layers = l.params && l.params.LAYERS || l.layer;
       //return !!(l.visibility && l.displayInLayerSwitcher && layers && l.url);
       // we're sending the same layer to the report engine and not the currently visible one because the reporting engine is FIXED to this layer.
       // if we don't send it it won't be displayed.
       var t = this.getLayerType(l);
       var isVisibleReportLayer = false;
       if (this.hasMultipleReportLayers) {
           if (l.visibility) {
               for (var i = 0; i < this.sublayers.length; i++) {
                   if (layers == this.sublayers[i].layer.params.LAYERS) {
                       isVisibleReportLayer = true;
                       break;
                   }
               }
               if (layers == this.layerId) {
                   isVisibleReportLayer = true;
               }
           }
       } else {
           isVisibleReportLayer = layers == this.layerId;
       }
       return !!(
            ((isVisibleReportLayer) || (t.name != "ARCGIS" && l.visibility)) // layer is default layer OR something different than ArcGIS AND visible.
            && l.displayInLayerSwitcher
            && layers
            && l.url);
   }

   , getLayerType: function (layer) {
       var c = this.app.config;
       for (var t in Type) {
           var type = Type[t];
           var key = 'WMS.' + t + '.URL';
           var keylayer = 'WMS.' + t + '.LAYERS';
           if (type != Type.OTHER && c.has(key) && c.get(key) == layer.url && (!c.has(keylayer) || $.inArray(layer.params.LAYERS, c.get(keylayer)) !== -1 || $.inArray(layer.layer, c.get(keylayer)) !== -1)) {
               return type;
           }
       }
       return Type.OTHER;
   }

   , layers: function () {
       var layers = this.map.layers, mapping = {};
       var c = this.app.config;
       for (var i = 0, ii = layers.length; i < ii; ++i) {
           var l = layers[i];
           if (this.isReportLayer(l)) {
               var type = this.getLayerType(l);
               var param = mapping[type] || [];
               var param_layers = l.params && l.params.LAYERS || l.layer
               var key = 'WMS.' + type.name + '.PRINT_MAPPING';
               if (c.has(key)) {
                   var print = c.get(key);
                   var view = c.get('WMS.' + type.name + '.LAYERS');
                   for (var p = view.length; p--; ) {
                       if (view[p] == param_layers) {
                           param_layers = print[p];
                           break;
                       }
                   }
               }
               param_layers = param_layers.split(",");
               if (mapping[type.value]) {
                   mapping[type.value] = param.concat(param_layers).concat(mapping[type.value]);
               } else {
                   mapping[type.value] = param.concat(param_layers);
               }
           };
       };
       delete mapping[Type.OTHER.value];
       for (var k in mapping) {
           mapping[k] = mapping[k].join(",");
       };
       return mapping;
   }
  });
})()