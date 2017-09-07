(function () {
    var Control = OpenLayers.Control;
    var Popup = OpenLayers.Popup;
    var createUniqueID = OpenLayers.Util.createUniqueID;

    var Parent = OpenLayers.Control.GetFeatureAt;
    var $super = Parent.prototype;
    var C = Control.GetFeaturesAt = OpenLayers.Class(Parent, {
        last_popup: null
   , title_text: null
   , callbacks: {}
   , removePopup: function () {
       var pop = this.last_popup;
       if (pop) {
           pop.data('tWindow').close();
       };
   }
   , popup: function (event, features, pos) {
       this.removePopup();
//       var id = this.id;
//       this.callbacks[id] = { scope: this, features: features, event: event };

//       var content = '<ul class="olxSelectFeature">';
//       for (var i = features.length; i--; ) {
//           var feat = features[i];
//           var func = "javascript:OpenLayers.Control.GetFeaturesAt.chooseFeature( '" + id + "', " + i + ");";
//           var syst = feat.attributes.System || '';
//           var name = feat.attributes.Name || '';
//           if (name && syst) {
//               name = syst + ': ' + name;
//           }
//           name = name || 'Feature: ' + feat.fid;
//           content += '<li><a href="#" onclick="' + func + '">' + name + '</a></li>';
//       }
//       content += '</ul>';

//       var key = 'TEXT.TITLE.SELECT_FEATURES';
//       var config = emsg.config;
//       var gtitle = null;
//       if (config.has(key)) {
//           gtitle = config.get(key);
//       }
//       var title = this.title_text || gtitle;
//       var mousePosition = emsg.map.map.getPixelFromLonLat(pos);

//       var mapdiv = $("#map");
//       var mapoffset = mapdiv.offset();
//       var mapwidth = mapdiv.width();
//       var mapheight = mapdiv.height();

//       var pop = this.last_popup = $.telerik.window.create({
//           title: title
//      , html: content
//      , closeable: true
//      , draggable: true
//      , scrollable: true
//      , width: 300
//       });

//       //Issue 6674 Expected Output:
//       //Die Klickliste wird ca. 1 cm vom angeklickten Punkt angezeigt
//       mousePosition.y -= 65;
//       mousePosition.x += 30;

//       var correctiontop = 0;
//       var correctionleft = 0;
//       var differencetop = mapheight - (pop.height() + mousePosition.y);
//       var differenceleft = mapwidth - (pop.width() + mousePosition.x);
//       if (differencetop < 0) {
//           correctiontop = differencetop;
//       }
//       if (differenceleft < 0) {
//           correctionleft = differenceleft;
//       }

//       mousePosition.y = mousePosition.y + mapoffset.top;
//       mousePosition.x = mousePosition.x + mapoffset.left;

//       pop.css("top", mousePosition.y + correctiontop);
//       pop.css("left", mousePosition.x + correctionleft);
//       pop.data('tWindow');
   }

  , onSuccess: function (childs, parents, pos) {
      var p = new OpenLayers.Geometry.Point(pos.lon, pos.lat);
      var features = childs.concat(parents);
      var found = Emsg.Util.closestFeature(p, features, null, true);
      if (!found) return;

      features = childs;
      var event = 'featureselected';
      if (found.index >= childs.length) {
          event = 'parentselected';
          features = parents;
      }

      if (features) {
          if (features.length > 1) {
              this.popup(event, features, pos);
          }
          else {
              this.removePopup();
              this.events.triggerEvent(event, features[0]);
          };
      };
  }

   , CLASS_NAME: "OpenLayers.Control.GetFeaturesAt"
    });

    C.chooseFeature = function (id, index) {
        var cb = C.prototype.callbacks[id];
        var ctrl = cb.scope;
        var feat = cb.features[index];
        var event = cb.event;
        ctrl.events.triggerEvent(event, feat);
        ctrl.removePopup();
    };

})();