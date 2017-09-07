(function () {
  /// create a new legend with data queried via rest API
  /// @param url the url to the Server accepting the Rest Api Call for the legend
  /// @param [div] a jquery selector to the element where the legend should be rendered to
    Emsg.Legend = $.inherit(Object, {
        app: null
     , url: null
     , layerNames: null

     , constructor: function (app, url, layerNames) {
         this.app = app;
         this.url = url;

         this.layers = {};
         this.data = null;
         this.layerNames = layerNames || {};
     }

        /// get the window for given layer
        /// @param layer either the id (number) or the name(string)
     , getLayer: function (layer) {   
         var w = this.layers[layer];
         return w && w.data('kendoWindow');
     }

        /// show a specific layer
        /// @param layer either the id (number) or the name(string)
     , show: function (layer) {
         var w = this.getLayer(layer);
         w.wrapper.css("zIndex", 1000);
         if (w) w.open();
     }

        /// hide a specific layer
        /// @param layer either the id (number) or the name(string)
     , hide: function (layer) {
         var w = this.getLayer(layer);
         if (w) w.close();
     }

     , hideAll: function () {
         for (var k in this.layerNames) {
             this.hide(k);
         };
     }

     , redraw: function () {
         this.hideAll();
         this.drawLayers(this.layerNames);
     }

     , getLayerTitle: function (layer) {
         return this.layerNames[layer.layerName] && this.layerNames[layer.layerName].title || this.layerNames[layer.layerName]
     }

     , getLayerSubtitle: function (layer) {
         return this.layerNames[layer.layerName] && this.layerNames[layer.layerName].subtitle;
     }

     , drawLayers: function (layers) {
         var layer, hash = {};
         for (var key in layers) {
             if (layers.hasOwnProperty(key)) {
                 title = this.getLayerTitle({ layerName: key });                
                 var html = '';
                 var icons = this.htmlIcons(key);
                 html += this.htmlLayer(key, icons);
                 html += '<div class="end">' + title + '</div>';
                 html = '<div class="legend">' + html + '</div>';
                 var w = $(html).kendoWindow({
                     title: title,
                     visible: false,
                 });
                 this.layers[key] = w;
             }
         }
     }

   , drawLayer: function (layer) {
     var icons = this.htmlIcons(layer);
     return this.htmlLayer(layer, icons);
   }

   , htmlLayer: function (layer, content) {
     var  name = 'Name_' + layer;
     var subtitle, html = '<div class="layer show ' + name + '">';
     if (subtitle = this.getLayerSubtitle({ layerName: layer }))
       html += '<span class="subtitle">' + subtitle + '</span>';

     html += content + '</div>';
     return html;
   }

   , htmlIcons: function (layer) {
       var item, list = '<ul class="icons">';      
       item = '<li>';
       item += '<img src="' + this.url + "?layer=" + layer + '"/>';
       item += '</li>';
       list += item;
       list += '</ul>';
       return list;
   }
   , getByName: function (name) {
       var layer = this.layerNames[name]
       if (!layer) {
           return [];
       } else
       {
           return [layer];
       }
   }

   , hasName: function (name) {
     return !!this.getByName(name).length;
   }

   , onLegendIcon: function (name) {
       this.show(name);
   }
  });
})();
