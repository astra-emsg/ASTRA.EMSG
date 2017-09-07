(function () {
    "use strict";
    var MapMode = Emsg.MapMode;
    var $super = MapMode.prototype;

    /**
    * Class: MapMode.Achsen
    * Karte im Achsenmodus bearbeiten
    */
    MapMode.Achsen = $.inherit(MapMode,
  {
      protocol: null,
      constructor: function (map) {
          MapMode.call(this, map);

          var ef = this.app.events.form;
          ef.onAchseCreated = function () { map.onCreated.apply(map, arguments); };
          ef.onAchseSelected = function () { map.onSelected.apply(map, arguments); };
          ef.onAchseDeleted = function () { map.onDeleted.apply(map, arguments); };


          this.urls.MAIN_AT = this.app.config.get('GIS.CONTROLLER.GET_ACHSEN_AT');
          this.urls.MAIN_ID = this.app.config.get('GIS.CONTROLLER.GET_ACHSEN_BY_ID');
          this.urls.MAIN_BBOX = this.app.config.get('GIS.CONTROLLER.GET_ACHSEN_BY_BBOX');
          this.type = "ACHSEN"
          this.layer = null;
      }
      , init: function () {

          this.protocol = new OpenLayers.Protocol.HTTP({ url: this.urls.MAIN_BBOX, format: this.geoJson });

          this.map.map.events.on({
              zoomend: this.onMapExtentChanged
              , moveend: this.onMapExtentChanged
              , scope: this
          });

          var ctrl_create = this.create_ = new OpenLayers.Control.DrawAchsFeature
      (this.map.editLayer
       , OpenLayers.Handler.SnappingPath
       , { displayClass: "emsgControlNew"
          , title: this.app.config.get("TEXT.TOOLTIP.TOOL_" + this.type + "_NEW")
          , handlerOptions: { freehandToggle: "altKey" }
          , type: OpenLayers.Control.TYPE_TOOL
          , protocol: new OpenLayers.Protocol.HTTP({ url: this.urls.MAIN_AT, format: this.geoJson })
       }
      );
          var ctrl_edit = this.edit = new OpenLayers.Control.ModifyAchsFeature
      (this.map.editLayer
       , { displayClass: "emsgControlEdit"
          , title: this.app.config.get("TEXT.TOOLTIP.TOOL_" + this.type + "_EDIT")
          , type: OpenLayers.Control.TYPE_TOOL
          , protocol: new OpenLayers.Protocol.HTTP({ url: this.urls.MAIN_AT, format: this.geoJson })
          , clickout: false
          , toggle: false
          , standalone: true
          , virtualStyle: { strokeWidth: 1, strokeOpacity: 1, strokeColor: "#FFFF00", pointRadius: 5, graphicName: "square", fillColor: "white", fillOpacity: 0.25 }
       }
      );

          ctrl_edit.setMap(this.map.map);

          this.select = new OpenLayers.Control.GetFeaturesAt
      ({ displayClass: "emsgControlSelect"
       , title: this.app.config.get("TEXT.TOOLTIP.TOOL_" + this.type + "_SELECT")
       , type: OpenLayers.Control.TYPE_TOOL
       , protocol: new OpenLayers.Protocol.HTTP({ url: this.urls.MAIN_AT, format: this.geoJson })
      });

          this.switchdir = new OpenLayers.Control.SwitchDirection(this.map.editLayer, this, {
              displayClass: "emsgControlSwitchDirection",
              //TODO Correct title
              title: this.app.config.get("TEXT.TOOLTIP.TOOL_" + this.type + "_REVERSE"),
              type: OpenLayers.Control.TYPE_BUTTON
          });
          this.switchdir.events.on({
              trigger: this.onSwitchTrigger,
              scope: this
          }
          );
          ctrl_create.events.on({
              activate: this.onCreateActivate
              , featureadded: this.onCreateFeatureAdded
              , scope: this
          });

          ctrl_edit.events.on({
              activate: this.onAchsenEditActivate
              , deactivate: this.onAchsenEditDeactivate
              , scope: this
          });

          this.select.events.on({
              activate: this.onSelectActivate,
              scope: this
          });

          this.edit.events.register("featureselected", this, this.editFeature);
          this.select.events.register("featureselected", this, this.selectFeature);

          if (!this.isMultiParent()) {
              var listener = { parentNotloaded: this.onParentNotloaded, scope: this };
              ctrl_edit.events.on(listener);
              ctrl_create.events.on(listener);
          }

          this.panel = new OpenLayers.Control.Panel({ displayClass: "emsgControlEditingToolbar", allowDepress: false });
          var ctrls = [ctrl_create, this.edit, this.select, this.switchdir];
          Emsg.Util.createCtrlDelegates(ctrls, this.map);

          this.panel.addControls(ctrls);
      },
      editFeature: function (feature) {
          return this.selectFeature(feature, false);
      },
      afterOverlays: function () {
          $super.afterOverlays.apply(this, arguments);
          this.map.axis.setVisibility(true);
          this.layer = this.map.axis;
      },
      onCreateActivate: function () {
          $super.onCreateActivate.apply(this, arguments);
          this.onMapExtentChanged();
      },

      onAchsenEditActivate: function () {
          if (this.map.dirs) {
              this.map.dirs.deactivate();
          }
          if (this.getFeature()) {
              this.edit.selectFeature(this.getFeature(), true);
          }
          this.onMapExtentChanged();
      },
      onAchsenEditDeactivate: function () {
          this.map.dirs.activate();
      },
      onSwitchTrigger: function () {
          if (this.map.dirs.active) {
              this.map.dirs.update();
          }
          if (this.edit.active) {
              this.edit.resetVertices();
          }
          this.onChanged(this.getGeoData(), this.getGeoLength());
      },
      onSelectActivate: function () {
          if (this.getFeature()) {
              this.map.editLayer.drawFeature(this.getFeature(), "default");
              if (this.map.dirs.active) {
                  this.map.dirs.update();
              }
          }
      },
      onCreateFeatureAdded: function (evt) {
          this.isDirty = true;
          this.create_.deactivate();
          this.edit.activate();
          this.events.onAchseCreated();
          this.setFeature(evt.feature);
          this.map.editLayer.removeAllFeatures();
          this.map.editLayer.addFeatures([this.getFeature()], { silent: true });
          this.getFeature().renderIntent = "select";
          this.getFeature().attributes.IsInverted = "false";
          this.edit.selectFeature(this.getFeature(), true);
          this.isDirty = true;
      },
      onSelectFeature: function (fid, readOnly) {
          this.checkCanInvert();
          this.events.onAchseSelected(fid, readOnly);
      },
      checkCanInvert: function () {
          if (this.getFeature() && !this.switchdir.enabled) {
              this.switchdir.enable();
          } else if (!this.getFeature() && this.switchdir.enabled) {
              this.switchdir.disable();
          }
      },
      setFeature: function (feature) {
          this.geoData = feature;
          if (feature) {
              this.switchdir.enable();
              if (this.map.dirs.active) {
                  this.map.dirs.update;
              }
          } else {
              this.switchdir.disable();
          }
      },
      load: function (feature, loadParents, readOnly) {
          if (!feature.fid) return;
          this.map.editLayer.removeAllFeatures();
          this.setFeature(feature);
          this.isDirty = false;

          // don't call events when loading since we mark he geodata as dirty on changes in that layer
          this.map.editLayer.addFeatures(feature, { silent: true });
          this.map.hasFeature();
          if (this.map.dirs.active) {
              this.map.dirs.update();
          }
          if (this.edit.active) {
              this.edit.selectFeature(this.getFeature(), true);
          }
          //       if (this.edit.active) {
          //           this.edit.handler.refreshDragPoints();
          //       };

          this.map.zoomToCurrent();
          this.onSelectFeature(feature.fid, readOnly);
      },
      updateGeoData: function (emitEvent) {
          if (!this.geoData) return;

          var feat, child, childs = [];
          for (var i = 0, l = this.map.editLayer.features.length; i < l; ++i) {
              child = this.map.editLayer.features[i];
              child.fid = child.fid || "";
          };
          this.isDirty = true;
          if (!!emitEvent) {
              this.onChanged(this.getGeoData(), this.getGeoLength());
          };
      },
      onChanged: function (geoData, geoLength) {
          this.events.onAchseChanged.apply(undefined, [geoData, geoLength]);
      },

      request: function (bounds, callback, scope) {
          if (this.currentRequest) {
              this.protocol.abort(this.currentRequest);
          }

          var params = { minX: bounds.left, minY: bounds.bottom, maxX: bounds.right, maxY: bounds.top, dummy: Emsg.Util.now() };
          this.currentRequest = this.protocol.read({ params: params, callback: callback, scope: scope });
      },
      onMapExtentChanged: function () {
          var callback = function (result) {
              this.currentRequest = null;
              if (result.features) {
                  var length = result.features.length;
                  var geoms = [];
                  for (var i = 0; i < length; i++) {
                      if (!this.geoData || result.features[i].fid !== this.geoData.fid) {
                          geoms.push(result.features[i].geometry);
                      }
                      this.edit.setSnappingGeometries(geoms);
                      this.create_.setSnappingGeometries(geoms);
                  }
              }
          };
          this.request(this.map.map.getExtent(), callback, this);

      },
      getGeoData: function () {
          if (!this.map.editLayer.features.length) return null;
          var layer;
          if (this.geoData.layer) {
              layer = this.geoData.layer;
              delete this.geoData.layer;
          }
          var jsonstring = this.geoJson.write(this.geoData);
          if (layer) {
              this.geoData.layer = layer;
          }
          return jsonstring;
      },
      clear: function () {
          $super.clear.apply(this, arguments);
          if (this.edit.active) {
              this.edit.unselectFeature();
          }
          this.checkCanInvert();
      }
  });
})();