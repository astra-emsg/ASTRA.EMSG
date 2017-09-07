(function () {
    var Map = Emsg.Map;
    var $super = Map.prototype;

    Map.InspectionRoute = $.inherit(Map, {
        current: null
   , routeId: null
   , zoomToCurrentControl: true
   , dirty: false
   , constructor: function (app) {
       Map.apply(this, arguments);
       this.geoJson = new OpenLayers.Format.GeoJSON();
       this.url = {};
       this.url.ParentAt = app.config.get('GIS.CONTROLLER.GET_STRABS_AT');
       this.url.ChildAt = app.config.get('GIS.CONTROLLER.GET_ROUTE_AT');
       this.url.exportRoutes = app.config.get('GIS.CONTROLLER.EXPORT');
       this.url.lastExport = app.config.get('GIS.CONTROLLER.LAST_EXPORT');
       this.url.StrabsByBbox = app.config.get('GIS.CONTROLLER.GET_STRABS_BY_BBOX');
       var that = this;
       this.app.events.form.onStrassenabschnittDeselected = function (id) { that.onDeselected(id); };
       this.app.events.form.onInspektionsrouteSelected = function (id) { that.load.apply(that, arguments); };
       this.app.events.form.onInspektionsrouteCreated = function () { that.activateCreate(); };
       this.app.events.form.onInspektionsrouteDeleted = function () { that.onSaveSuccess.apply(that, arguments); };
       this.app.events.form.onSaveSuccess = function () { that.onSaveSuccess.apply(that, arguments); };
       this.app.events.form.onDataLoaded = function () { that.onDataLoaded.apply(that, arguments); };
   }
   , afterOverlays: function () {
       $super.afterOverlays.apply(this, arguments);
       this.strabs.setVisibility(true);
       this.route.setVisibility(true);
   }
    , init: function () {
        $super.init.apply(this, arguments);
        var config = { projection: this.map.projection, maxExtent: this.map.maxExtent, displayInLayerSwitcher: false };
        config.styleMap = Emsg.StyleMaps.AXIS;
        this.parentLayer = new OpenLayers.Layer.Vector("Parent Layer", config);
        config.styleMap = Emsg.StyleMaps.MAIN_SEGMENT;
        this.editLayer = new OpenLayers.Layer.Vector("Edit Layer", config);
        this.exportLayer = new OpenLayers.Layer.Vector("Export Layer", config);
        this.map.addLayers([this.parentLayer, this.editLayer, this.exportLayer]);

        this.select = new OpenLayers.Control.GetFeatureAt
      ({ displayClass: "emsgControlSelect"
        , title: this.app.config.get("TEXT.TOOLTIP.TOOL_ROUTE_SELECT")
        , type: OpenLayers.Control.TYPE_TOOL
        , protocol: new OpenLayers.Protocol.HTTP({ url: this.url.ChildAt, format: this.geoJson })
      });
        this.select.events.register("featureselected", this, this.selectRoute);

        this.edit = new OpenLayers.Control.GetFeatureAt
      ({ displayClass: "emsgControlEdit"
        , title: this.app.config.get("TEXT.TOOLTIP.TOOL_ROUTE_EDIT")
        , type: OpenLayers.Control.TYPE_TOOL
        , protocol: new OpenLayers.Protocol.HTTP({ url: this.url.ParentAt, format: this.geoJson })
        , protocolParent: new OpenLayers.Protocol.HTTP({ url: this.url.ChildAt, format: this.geoJson })
        , can_select: { fn: this.canEditRoute, scope: this }
          //, cursor: true
      });
        this.edit.events.register("featureselected", this, this.toggleFeature);
        this.edit.events.register("parentselected", this, this.selectRoute);
        this.edit.events.register("parentAndChildselected", this, this.onParentAndChildSelected);
        this.edit.events.register("activate", this, this.onEditActivate);
        this.edit.events.register("deactivate", this, this.onEditDeactivate);

        this.create = new OpenLayers.Control.GetFeatureAt
      ({ displayClass: "emsgControlNew"
        , title: this.app.config.get("TEXT.TOOLTIP.TOOL_ROUTE_NEW")
        , type: OpenLayers.Control.TYPE_TOOL
        , protocol: new OpenLayers.Protocol.HTTP({ url: this.url.ParentAt, format: this.geoJson })
          //, cursor: true
      });
        this.create.events.register("featureselected", this, this.onCreateToggleFeature);
        this.create.events.register("activate", this, this.onCreateActivate);
        this.create.events.register("deactivate", this, this.onCreateDeactivate);

        this.boxSelect = new OpenLayers.Control.BoxSelect
        ({ displayClass: "emsgControlBoxSelect"
        , title: this.app.config.get("TEXT.TOOLTIP.TOOL_BOXSELECT")
        , protocol: new OpenLayers.Protocol.HTTP({ url: this.url.StrabsByBbox, format: this.geoJson })
        });

        
        this.selectExport = new OpenLayers.Control.GetFeatureAt
      ({ displayClass: "emsgControlInspSelectExport"
        , title: this.app.config.get("TEXT.TOOLTIP.TOOL_ROUTE_SELECT_EXPORT")
        , type: OpenLayers.Control.TYPE_TOOL
        , protocol: new OpenLayers.Protocol.HTTP({ url: this.url.ChildAt, format: this.geoJson })
      });
        this.selectExport.events.register("activate", this, this.startExport);
        this.selectExport.events.register("deactivate", this, this.endExport);
        this.selectExport.events.register("featureselected", this, this.toggleRoute);
        var that = this;
        this.exportRoutes = new OpenLayers.Control.Button
      ({ displayClass: "emsgControlInspExport"
        , title: this.app.config.get("TEXT.TOOLTIP.TOOL_ROUTE_EXPORT")
        , trigger: function () { that.doExport(); }
      });

        this.panel = new OpenLayers.Control.Panel({ displayClass: "emsgControlEditingToolbar", allowDepress: true });
        var ctrls = [this.create, this.edit, this.boxSelect, this.select, this.exportRoutes, this.selectExport];
        Emsg.Util.createCtrlDelegates(ctrls, this);
        this.panel.addControls(ctrls);
        var directions = new OpenLayers.Control.ShowDirection();
        this.map.addControls([this.panel]);
        this.addControls();
        this.clear();
        this.activateControl(this.select);
        this.boxSelect.disable();
        this.events.initialized();
    }
   , canEditRoute: function () {
       return this.routeId != null;
   }
   , activateControl: function (control) {
       this.panel.activateControl(control);
   }
   , activateCreate: function () {
       this.activateControl(this.create);
   }
   , clear: function () {
       if (this.parentLayer) this.parentLayer.removeAllFeatures();
       if (this.editLayer) this.editLayer.removeAllFeatures();
       this.routeId = null;
       this.dirty = false;
   }

   , onSaveSuccess: function () {
       this.clear();
       //if (this.create.active) this.activateControl(this.select);
       this.route.redraw(true);
   }

   , executeCancel: function () {
       this.onSaveSuccess();
   }

   , canRemoveFeature: function (feature) {
       if (this.editLayer.getFeatureByFid(feature.fid)) {
           return true;
       };
       return false;
   }
   , onEditActivate: function () {
       this.boxSelect.events.register("boxselected", this, this.onBoxSelected);
       this.boxSelect.enable();
   }
   , onEditDeactivate: function () {
       this.boxSelect.events.unregister("boxselected", this, this.onBoxSelected);
       if (!this.create.active) {
           this.boxSelect.disable();
       }
   }
   , onCreateActivate: function () {
       this.boxSelect.events.register("boxselected", this, this.onBoxCreateSelected);
       this.boxSelect.enable();
   }
   , onCreateDeactivate: function () {
       this.boxSelect.events.unregister("boxselected", this, this.onBoxCreateSelected);
       if (!this.edit.active) {
           this.boxSelect.disable();
       }
   }
   , canAddFeature: function (feature) {
       var iid = feature.attributes.InspektionsrouteID;
       return !this.canRemoveFeature(feature) && (!iid || iid == this.routeId);
   }

   , onCreateToggleFeature: function (feature) {
       if (!this.dirty && this.canAddFeature(feature) && !this.onCreated()) {
           return false;
       }
       this.toggleFeature(feature);
   }

   , onCreated: function () {
       if (this.routeId && !this.app.cancel()) return false;
       this.clear();
       this.events.onInspektionsrouteCreated();
       return true;
   }

   , redraw: function () {
       this.route.redraw(true);
   }

   , toggleFeature: function (feature) {
       this.dirty = true;
       var fid = feature.fid;
       if (this.canRemoveFeature(feature)) {
           this.deselect(fid);
           this.events.onStrassenabschnittDeselected(fid);
       }
       else if (this.canAddFeature(feature)) {
           this.editLayer.addFeatures([feature]);
           this.events.onStrassenabschnittSelected(fid);
       }
   }
   , onBoxSelected: function (evt) {
       var length = evt.features.length;
       for (var i = 0; i < length; i++) {
           var feature = evt.features[i];
           if (this.canAddFeature(feature)) {
               this.dirty = true;
               this.editLayer.addFeatures([feature]);
               this.events.onStrassenabschnittSelected(feature.fid);
           }
       }
   }
   , onBoxCreateSelected: function (evt) {
       var canAddFeatures = false;
       var length = evt.features.length;
       for (var i = 0; i < length; i++) {
           var feature = evt.features[i];
           if (this.canAddFeature(feature)) {
               canAddFeatures = true;
               break;
           }
       }
       if (!this.dirty && canAddFeatures && !this.onCreated()) {
           return false;
       }
       this.onBoxSelected(evt);
   }
   , deselect: function (fid) {
       var existing = this.editLayer.getFeatureByFid(fid);
       if (existing) {
           this.editLayer.removeFeatures([existing]);
       };
   }
   , onDeselected: function (fid) {
       this.deselect(fid);
   }
   , onParentAndChildSelected: function (features) {
       if (features.parent.fid == this.routeId) {
           this.toggleFeature(features.child);
       };
   }
   , toggleRoute: function (feature) {
       var existing = this.exportLayer.getFeatureByFid(feature.fid);
       var locked = feature.attributes.IsLocked;
       if (existing) {
           this.exportLayer.removeFeatures([existing]);
       }
       else if (locked == null || locked.toLowerCase() == "false") {
           this.exportLayer.addFeatures([feature]);
       };
   }

   , startExport: function () {
       this.parentLayer.display(false);
       this.editLayer.display(false);
       this.exportLayer.display(true);
       this.exportLayer.removeAllFeatures();
   }

   , endExport: function () {
       this.parentLayer.display(true);
       this.editLayer.display(true);
       this.exportLayer.display(false);
   }

   , doExport: function () {
       var ids = {};
       for (var i = 0, ii = this.exportLayer.features.length; i < ii; ++i) {
           ids[i] = this.exportLayer.features[i].fid;
       };
       var createButton = function (context, value, func, classname) {
           var button = document.createElement("input");
           button.type = "button";
           button.value = value;
           button.onclick = func;
           button.className = classname;
           context.appendChild(button);
       }

       if (jQuery.isEmptyObject(ids)) {
           alert($('#InspektionsroutenGrid').data('empty-list-warning'));
           return;
       }
       $("#exportConfirmContent").empty().append(('<p>' + $('#InspektionsroutenGrid').data('export-time-warning') + '\n\n' + $('#InspektionsroutenGrid').data('export-map-confirmation') + '</p>').replace(/\n/g, '<br />'));
       var tWindow = $("#exportConfirmDiv").data('kendoWindow');
       var that = this;

       createButton($("#exportConfirmContent")[0], $('#InspektionsroutenGrid').data('cancel'),
            function () { tWindow.close(); }, "ExportDialogButton k-button");

       createButton($("#exportConfirmContent")[0], $('#InspektionsroutenGrid').data('no'),
            function () {
                tWindow.close();
                var exportBackground = false;

                var cb = function (data) {
                    $.download(that.url.lastExport, {});
                    that.exportLayer.removeAllFeatures();
                    that.route.redraw(true);
                };
                $.post(that.url.exportRoutes, { ids: ids, exportBackground: exportBackground }, cb);
            }, "ExportDialogButton k-button");

       createButton($("#exportConfirmContent")[0], $('#InspektionsroutenGrid').data('yes'), function () {
           tWindow.close();
           var exportBackground = true;

           var cb = function (data) {
               $.download(that.url.lastExport, {});
               that.exportLayer.removeAllFeatures();
               that.route.redraw(true);
           };
           $.post(that.url.exportRoutes, { ids: ids, exportBackground: exportBackground }, cb);
       }, "ExportDialogButton k-button");


       tWindow.restore();
       tWindow.center();

       tWindow.open();
       //       var that = this;
       //       var cb = function (data) {
       //           $.download(that.url.lastExport, {});
       //           that.exportLayer.removeAllFeatures();
       //           that.route.redraw(true);
       //       };
       //       $.post(this.url.exportRoutes, { ids: ids }, cb);
   }

   , onDataLoaded: function () {
       if (this.dirty) {
           var feats = this.editLayer.features;
           for (var i = feats.length; i--; ) {
               this.events.onStrassenabschnittSelected(feats[i].fid);
           }
       }
   }

   , onLoad: function (routes, inspectionRouteId) {
       this.clear();
       var bounds = null;
       for (var i = 0, ii = routes.length; i < ii; ++i) {
           if (routes[i].attributes.Strassenabschnitte) {
               bounds = routes[i].geometry.getBounds();
               var strabs = this.geoJson.read(routes[i].attributes.Strassenabschnitte);
               var clone = [];
               for (var s = 0, ss = strabs.length; s < ss; ++s) {
                   clone.push(strabs[s].clone());
               };
               this.parentLayer.addFeatures(clone);
               this.editLayer.addFeatures(strabs);
           };
       };
       this.map.zoomToExtent(bounds);
       this.routeId = inspectionRouteId;
       this.events.onInspektionsrouteSelected(inspectionRouteId);
   }

   , selectRoute: function (feature) {
       if (this.routeId != feature.fid) {
           this.load(feature.fid);
       };
   }

   , load: function (inspectionRouteId) {
       if (this.routeId && !this.app.cancel()) return false;
       var protocol = new OpenLayers.Protocol.HTTP({ url: this.app.config.get("GIS.CONTROLLER.GET_STRABS_BY_ROUTEID"), format: this.geoJson });
       var request = protocol.read({
           params: { id: inspectionRouteId, olsalt: "_" + new Date().getTime() }
      , scope: this
      , callback: function (result) {
          if (result.success()) {
              this.onLoad(result.features, inspectionRouteId);
          }
      }
       });
   }
    });
})();
