(function () {
    var addClass = OpenLayers.Element.addClass;
    var observe = OpenLayers.Event.observe;
    var bindAsEventListener = OpenLayers.Function.bindAsEventListener;

    var Parent = OpenLayers.Control.LayerSwitcher;
    var $super = Parent.prototype;

    Parent.Grouping = OpenLayers.Class(Parent, {
        openGroups: null
        /** 
        * Method: redraw
        * Goes through and takes the current state of the Map and rebuilds the
        *     control to display that state. Groups base layers into a 
        *     radio-button group and lists each data layer with a checkbox.
        *
        * Returns: 
        * {DOMElement} A reference to the DIV DOMElement containing the control
        */
   , redraw: function () {
       //if the state hasn't changed since last redraw, no need 
       // to do anything. Just return the existing div.
       if (!this.checkRedraw()) {
           return this.div;
       }

       addClass(this.div, "olButton");

       //clear out previous layers 
       this.clearLayersArray("base");
       this.clearLayersArray("data");

       var containsOverlays = false;
       var containsBaseLayers = false;

       // Save state -- for checking layer if the map state changed.
       // We save this before redrawing, because in the process of redrawing
       // we will trigger more visibility changes, and we want to not redraw
       // and enter an infinite loop.
       var len = this.map.layers.length;
       this.layerStates = new Array(len);
       for (var i = 0; i < len; i++) {
           var layer = this.map.layers[i];
           this.layerStates[i] = {
               'name': layer.name,
               'visibility': layer.visibility,
               'inRange': layer.inRange,
               'id': layer.id
           };
       }

       var layers = this.map.layers.slice();
       var allGroupDivs = { base: {}, data: {} };
       if (!this.ascending) { layers.reverse(); }
       for (var i = 0, len = layers.length; i < len; i++) {
           var layer = layers[i];
           var baseLayer = layer.isBaseLayer;
           var groupDivs, group = layer.group;

           if (layer.displayInLayerSwitcher) {

               if (baseLayer) {
                   containsBaseLayers = true;
                   groupDivs = allGroupDivs.base;
               } else {
                   containsOverlays = true;
                   groupDivs = allGroupDivs.data;
               }

               // only check a baselayer if it is *the* baselayer, check data
               //  layers if they are visible
               var checked = (baseLayer) ? (layer == this.map.baseLayer)
                                          : layer.getVisibility();

               // create input element
               var inputElem = document.createElement("input");
               inputElem.id = this.id + "_input_" + layer.name;
               inputElem.name = (baseLayer) ? this.id + "_baseLayers" : layer.name;
               inputElem.type = (baseLayer) ? "radio" : "checkbox";
               inputElem.value = layer.name;
               inputElem.checked = checked;
               inputElem.defaultChecked = checked;
               inputElem.className = "olButton";
               inputElem._layer = layer.id;
               inputElem._layerSwitcher = this.id;

               if (!baseLayer && !layer.inRange) {
                   inputElem.disabled = true;
               }

               // create span
               var labelSpan = document.createElement("label");
               labelSpan["for"] = inputElem.id;
               OpenLayers.Element.addClass(labelSpan, "labelSpan olButton");
               labelSpan._layer = layer.id;
               labelSpan._layerSwitcher = this.id;
               if (!baseLayer && !layer.inRange) {
                   labelSpan.style.color = "gray";
               }
               labelSpan.innerHTML = layer.name;
               labelSpan.style.verticalAlign = (baseLayer) ? "bottom"
                                                            : "baseline";
               // create line break
               var br = document.createElement("br");

               // create legend icon
               var legendIcon = null;
               if (layer.legend) {
                   var name = layer.BaseName;
                   legendIcon = document.createElement("span");
                   addClass(legendIcon, "olButton legendIconSpan");
                   var cb = function (e) {
                       window.external.CallShowLegend(this.name);
                       OpenLayers.Event.stop(e);
                   };
                   observe(legendIcon, "click", bindAsEventListener(cb, { name: name }));
               };


               var groupArray = (baseLayer) ? this.baseLayers
                                             : this.dataLayers;
               groupArray.push({
                   'layer': layer
                 , 'inputElem': inputElem
                 , 'labelSpan': labelSpan
                 , 'legendIconSpan': legendIcon
               });


               var groupDiv = (baseLayer) ? this.baseLayersDiv
                                           : this.dataLayersDiv;

               if (group != null) {
                   groupDiv = groupDivs[group] = groupDivs[group] || this.createGroupDiv(group, groupDiv, baseLayer)
               };

               if (baseLayer || group != undefined) { // this means either base layer group OR something grouped (where the order is in the same way as adding - Zusatzinformationen
                   // #7988: original workflow: just add.
                   groupDiv.appendChild(inputElem);
                   legendIcon && groupDiv.appendChild(legendIcon);
                   groupDiv.appendChild(labelSpan);
                   groupDiv.appendChild(br);
               }
               else {
                   // #7988: reversed adding (on top of the layer group)
                   // this is the reverse - display ordering of EMSG Layers - Axis is on bottom of the list and - now - also on bottom of the layer switcher.
                   if (groupDiv.childNodes.length > 0)
                       groupDiv.insertBefore(br, groupDiv.childNodes[0]);
                   else
                       groupDiv.appendChild(br);
                   groupDiv.insertBefore(labelSpan, groupDiv.childNodes[0]);
                   legendIcon && groupDiv.insertBefore(legendIcon, groupDiv.childNodes[0]);
                   groupDiv.insertBefore(inputElem, groupDiv.childNodes[0]);
               }
           }
       }

       // if no overlays, dont display the overlay label
       this.dataLbl.style.display = (containsOverlays) ? "" : "none";

       // if no baselayers, dont display the baselayer label
       this.baseLbl.style.display = (containsBaseLayers) ? "" : "none";

       return this.div;
   }

   , isGroupOpen: function (group_name, is_base) {
       var g = this.openGroups = this.openGroups || {};
       var prefix = is_base ? "base" : "data";
       return !!g[prefix + group_name];
   }

   , setGroupOpen: function (group_name, is_base, is_open) {
       var g = this.openGroups = this.openGroups || {};
       var prefix = is_base ? "base" : "data";
       g[prefix + group_name] = is_open;
   }

   , createGroupDiv: function (group_name, parent_div, is_base) {
       var label = $('<div class="dataLbl groupLbl">' + group_name + '</div>');
       var groupDiv = $('<div class="group closed"/>');
       var that = this;
       label.click(groupDiv, function (event) {
           var g = event.data;
           g.toggleClass('closed');
           that.checkScrollSize();
           that.setGroupOpen(group_name, is_base, !g.hasClass("closed"));
           OpenLayers.Event.stop(event);
       });
       groupDiv.append(label);
       if (this.isGroupOpen(group_name, is_base)) {
           groupDiv.removeClass("closed");
       };

       var div = $('<div class="dataLayersDiv groupLayersDiv"/>');
       groupDiv.append(div);

       $(parent_div).append(groupDiv);

       return div[0];
   }
 , checkScrollSize: function () {
     if (this.div) {
         var d = $(this.div);
         var l = d.children(".layersDiv");
         l.css("max-height", "none");
         var h = d.height();
         h = (h) ? (h - 10) + "px" : "none";
         l.css("max-height", h);
     }
 }

 , maximizeControl: function () {
     $super.maximizeControl.apply(this, arguments);
     this.checkScrollSize();
 }

 , draw: function () {
     var div = $super.draw.apply(this, arguments);
     var that = this;
     $(window).resize(function () { that.checkScrollSize() });
     return div;
 }

   , CLASS_NAME: "OpenLayers.Control.LayerSwitcher"
    });
})();