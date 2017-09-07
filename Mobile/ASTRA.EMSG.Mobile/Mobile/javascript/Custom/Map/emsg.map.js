(function () {
    var OL = OpenLayers;
    var Bounds = OL.Bounds;
    var Control = OL.Control;
    var Path = OL.Handler.Path;
    var Polygon = OL.Handler.Polygon;
    var Layer = OL.Layer;
    var Map = OL.Map;
    var Projection = OL.Projection;

    Emsg.Map = $.inherit(Object, {
        /**
        * @property {Boolean} true to add legend control
        */
        legend: false
        /**
        * @property {Boolean} true to add pdf download control for reports only
        */
        ,
        pdfDownload: false
        /**
        * @property {Boolean} true to add zoom to current feature control
        */
        ,
        zoomToCurrentControl: false
        /**
        * @property {OpenLAyers.Control} control to zoom to current feature
        */
        ,
        zoom_current: null

        /**
        * @property {Emsg.App} the application this map runs in
        */
        ,
        app: null

        /**
        * @property {Emsg.Events.Map} map event callback functions
        */
        ,
        events: null

        /**
        * @property {OpenLayers.Bounds} the default extent for this mandant
        */
        ,
        extent: null

        /**
        * @property {OpenLayers.Control.Panel} top left panel
        */
        ,
        panel_: null
        /**
        *@property {OpenLayers.Control.ScaleLine} scale line in bottom left corner
        */
        ,
        scale_line: null

        /**
        * some arcgis layers
        */
        ,
        axis: null,
        strabs: null,
        zabs: null,
        measure: null,
        realisiert: null,
        suggestion: null,
        route: null,
        konflikte: null,
        vectorlayers: [],

        constructor: function (app) {
            this.app = app;
            this.events = app.events.map;
            this.axis = new OpenLayers.Layer.Vector(this.app.localization.DisplayLayerName_MV_ACHSENSEGMENT);
            this.axis.styleMap.styles["default"] = new OpenLayers.Style({
                'strokeWidth': 10,
                'strokeColor': '#000000'
            });
            this.axis.BaseName = "MV_ACHSENSEGMENT";
            this.vectorlayers["MV_ACHSENSEGMENT"] = this.axis;
            this.strabs = new OpenLayers.Layer.Vector(this.app.localization.DisplayLayerName_STRASSENABSCHNITTGIS);
            this.strabs.styleMap.styles["default"] = new OpenLayers.Style({
                'strokeWidth': 5,
                'strokeColor': '#666666'
            });
            this.strabs.BaseName = "STRASSENABSCHNITTGIS";
            this.vectorlayers["STRASSENABSCHNITTGIS"] = this.strabs;
            this.zabs = new OpenLayers.Layer.Vector(this.app.localization.DisplayLayerName_ZUSTANDSABSCHNITTGIS);
            this.zabs.styleMap.styles["default"] = new OpenLayers.Style({
                'strokeWidth': 2,
                'strokeColor': '#aa00aa'
            });
            this.zabs.BaseName = "ZUSTANDSABSCHNITTGIS";
            this.vectorlayers["ZUSTANDSABSCHNITTGIS"] = this.zabs;

            //There are two layers for Trottoir in the legend but we need only one so: left
            this.zabtrott = new OpenLayers.Layer.Vector(this.app.localization.DisplayLayerName_ZUSTANDTROTTOIRLEFT);
            this.zabs.styleMap.styles["default"] = new OpenLayers.Style({
                'strokeWidth': 2,
                'strokeColor': '#aa00aa'
            });
            this.zabtrott.BaseName = "ZUSTANDTROTTOIRLEFT";
            this.vectorlayers["ZUSTANDTROTTOIRLEFT"] = this.zabtrott;

        }
        ,
        createWmtsLayer: function (layer, isbaseLayer) {
            var matrices = [];
            var serverres = [];
            for (var j = 0; j < layer.MatrixSet.length; ++j) {
                var m = layer.MatrixSet[j];
                var matrix = {};
                matrix.identifier = m.Identifier;
                matrix.scaleDenominator = m.ScaleDenominator;
                matrix.topLeftCorner = new OpenLayers.LonLat(m.Left, m.Top);
                matrix.tileWidth = m.TileWidth;
                matrix.tileHeight = m.TileHeight;
                matrices.push(matrix);
                serverres.push(this.roundToSignificantFigures((m.ScaleDenominator * 0.00028), 3));
            }
            var wmts = new OpenLayers.Layer.WMTS({
                name: layer.LocalizedName,
                url: "file://" + layer.BasePath + "\\" + layer.SourceUrl,
                layer: layer.Name,
                style: "",
                matrixSet: "",
                requestEncoding: "REST",
                matrixIds: matrices,
                serverResolutions: serverres,
                wrapDateLine: false,
                isBaseLayer: isbaseLayer,
                visibility: layer.IsDefaultVisible
            });
            return wmts;
        }
       ,
        beforeBaseLayers: function () { },
        addBaseLayers: function (argsobject) {
            var li = argsobject.LayerInfo;
            li.sort(function (a, b) { return a.Order - b.Order; });

            for (index = 0; index < li.length; ++index) {
                var layer = li[index];
                if (layer.Container === 0) {
                    this.map.addLayer(this.createWmtsLayer(layer));
                }
            }
        },
        afterBaseLayers: function () { }

        ,
        beforeOverlays: function () { },
        addOverlays: function (argsobject) {
            var gformat = new OpenLayers.Format.GeoJSON();
            var jformat = new OpenLayers.Format.JSON();

            var axisfeats = gformat.read(argsobject.AchsenGeoJson);
            var strabsfeats = gformat.read(argsobject.StrabsGeoJson);
            var zabsfeats = gformat.read(argsobject.ZabsGeoJson);
            var zabTrottFeats = [];
            var length = zabsfeats.length;

            for (var i = 0; i < length; i++) {
                var zab = zabsfeats[i];
                if (zab.attributes.Trottoir) {
                    var features = gformat.read(jformat.write(zab.attributes.Trottoir));
                    if (features) {
                        zabTrottFeats = zabTrottFeats.concat(features);
                    }
                }
            }
            this.axis.removeAllFeatures();
            this.strabs.removeAllFeatures();
            this.zabs.removeAllFeatures();
            this.zabtrott.removeAllFeatures();

            this.axis.addFeatures(axisfeats);
            this.strabs.addFeatures(strabsfeats);
            this.zabs.addFeatures(zabsfeats);
            this.zabtrott.addFeatures(zabTrottFeats);

            this.map.addLayer(this.axis);
            this.map.addLayer(this.strabs);
            this.map.addLayer(this.zabtrott);
            this.map.addLayer(this.zabs);

            this.axis.legend = true;
            this.strabs.legend = true;
            this.zabs.legend = true;
            this.zabtrott.legend = true;

            var av_group = this.app.localization.LabelAdditionalInformation;
            var li = argsobject.LayerInfo;
            li.sort(function (a, b) { return a.Order - b.Order; });

            for (index = 0; index < li.length; ++index) {
                var layer = li[index];
                if (layer.Container === 2) {
                    var ollayer = this.createWmtsLayer(layer)
                    ollayer.group = av_group;
                    ollayer.isBaseLayer = false;

                    this.map.addLayer(ollayer);
                }
                if (layer.Container === 1) {
                    this.map.addLayer(this.createWmtsLayer(layer));
                }
            }
        }

        ,
        afterOverlays: function () { }

        ,
        init: function (args) {
            OL.DOTS_PER_INCH = 96;
            var config = {};

            //TODO if necessary move to app.config
            config.projection = new Projection("EPSG:27181");
            config.units = 'm';
            config.maxExtent = new Bounds(485869.5728, 76443.1884, 837076.5648, 299941.7864);

            //TODO if necessary move to app.config
            config.scales = [378, 945, 1890, 3780, 6000, 9449, 15000, 18898, 28000, 37795, 56000, 75591, 132000, 188976, 283000, 377953, 600000, 944882];
            config.controls = [];
            if (this.map) {
                this.map.destroy();
            }
            this.map = new Map('map', config);

            this.beforeBaseLayers();
            this.addBaseLayers(args);
            this.afterBaseLayers();
            this.beforeOverlays();
            this.addOverlays(args);
            this.afterOverlays();
            this.readStyles(args.Slds);
        }
        , readStyles: function (slds) {
            var format = new OpenLayers.Format.SLD();
            for (var i = 0; i < slds.length; i++) {
                var sld = format.read(slds[i]);
                for (var l in sld.namedLayers) {
                    var styles = sld.namedLayers[l].userStyles, style;
                    for (var j = 0, ii = styles.length; j < ii; ++j) {
                        style = styles[j];
                        if (style.isDefault) {
                            if (this.vectorlayers[l]) {
                                this.vectorlayers[l].styleMap.styles["default"] = style;
                            }
                            break;
                        }
                    }
                }
            }
        }
        ,
        addControls: function () {
            var map = this;
            var nav, ctrl = [],
                panel = this.panel_ = new Control.Panel({
                    displayClass: "panelTopLeft",
                    allowDepress: true
                });
            this.tool_nav_ = new Control({
                displayClass: "DummyPan",
                title: this.app.localization.Pan
            });
            this.tool_zoom_ = new Control.ZoomBox({
                title: this.app.localization.ZoomToolText
            });
            ctrl.push(this.tool_nav_);
            ctrl.push(this.tool_zoom_);
            var trigger = function () {
                map.zoomToDefault();
            };
            ctrl.push(new Control.Button({
                displayClass: "FullExtent",
                trigger: trigger,
                title: this.app.localization.FullExtent
            }));
            if (this.zoomToCurrentControl) {
                trigger = function () {
                    map.zoomToCurrent();
                };
                ctrl.push(this.zoom_current = new Control.Button({
                    displayClass: "Current",
                    trigger: trigger,
                    title: this.app.localization.ZoomToActiveElement
                }));
                var l = this.editLayer;
                if (l) {
                    l.events.register("featureadded", this, this.hasFeature);
                    l.events.register("featuresadded", this, this.hasFeature);
                    l.events.register("featureremoved", this, this.hasFeature);
                    l.events.register("featuresremoved", this, this.hasFeature);
                };
            };
            this.history = new Control.NavigationHistory({
                previousOptions: {
                    displayClass: "Prev"
                },
                nextOptions: {
                    displayClass: "Next"
                }
            });
            ctrl.push(this.history.previous);
            ctrl.push(this.history.next);

            ctrl.push(new Control.MeasureEx(Path, {
                displayClass: "MeasureLine",
                title: this.app.localization.MeasureLineToolText
            }));
            ctrl.push(new Control.MeasureEx(Polygon, {
                displayClass: "MeasureArea",
                title: this.app.localization.MeasureAreaToolText,
                numDigits: 0
            }));

            if (this.legend) {
                trigger = function () {
                    var legend = emsg.legend;
                    if (legend) {
                        legend.open();
                    };
                };
                ctrl.push(new Control.Button({
                    displayClass: "Legend",
                    trigger: trigger,
                    title: this.app.localization.Legend
                }));
            };
            panel.addControls(ctrl);
            ctrl = [];
            var navigationcontrol = new Control.Navigation();
            navigationcontrol.dragPanOptions = { interval: 0 };
            ctrl.push(navigationcontrol);
            ctrl.push(this.history);
            ctrl.push(panel);
            ctrl.push(new Control.PanZoomBar());
            ctrl.push(this.scale_line = new Control.ScaleLine());
            ctrl.push(new Control.LayerSwitcher.Grouping());
            var str_format = this.app.localization.MousePos_Format;
            var format = function (pos) {
                var r = Emsg.Util.round;
                pos = {
                    lon: r(pos.lon, 1),
                    lat: r(pos.lat, 1)
                };
                return OL.String.format(str_format, pos);
            };
            ctrl.push(new Control.MousePosition({
                formatOutput: format
            }));

            this.map.addControls(ctrl);
            this.activateNav();
        }

            , activateNav: function () {
                this.panel_.activateControl(this.tool_nav_);
            }

            ,
        updateSize: function () {
            if (this.map) this.map.updateSize.apply(this.map, arguments);
        }

            ,
        zoomToDefault: function () {
            if (this.extent) this.map.zoomToExtent(this.extent);
        }

            ,
        zoomToCurrent: function () {
            var l = this.editLayer;
            l = l && l.getDataExtent();
            l && this.map.zoomToExtent(l);
        }


            ,
        readExtent: function () {
            this.extent = this.map.getMaxExtent();
        }

            ,
        stopZoom: function () {
            this.activateNav();
        }

            ,
        hasFeature: function (has) {
            var c = this.zoom_current;
            if (c == null) return;
            var l = this.editLayer;
            if (l && l.getDataExtent()) {
                c.activate();
            } //check if the control is already destroyed (events == null) 
            else if (c.events) {
                c.deactivate();
            };
        },
        deleteZustandsabschnitt: function (id) {
            var feature = this.zabs.getFeatureByFid(id);
            if (feature) {
                this.zabs.removeFeatures([feature]);
                var trotts = this.zabtrott.getFeaturesByAttribute("Zustandsabschnitt", feature.fid);

                this.zabtrott.removeFeatures(trotts);
            }
            var i,
           feature,
            len = this.strabs.features.length;
            for (i = 0; i < len; i++) {
                feature = this.strabs.features[i];
                if (feature && feature.attributes) {
                    var featureCollection = feature.attributes["Zustandsabschnitte"];
                    if ($.isArray(featureCollection.features)) {

                        var j,
                        feature,
                        attlen = featureCollection.features.length;
                        for (j = 0; j < attlen; j++) {
                            feature = featureCollection.features[j];
                            if (feature.fid == id || feature.id == id) {
                                featureCollection.features.splice(j, 1);
                                attlen--;
                            }
                        }
                    }
                }
            }
        },
        updateZustandsabschnitt: function (id, zindex, geoJson) {
            this.deleteZustandsabschnitt(id);
            //Zab is a Feature in the zabs-layer and a normal json object in strabs attributes
            var jformat = new OpenLayers.Format.JSON();
            var gformat = new OpenLayers.Format.GeoJSON();
            var newFeatures = gformat.read(geoJson);
            var attribute = jformat.read(geoJson);
            var strab = this.strabs.getFeatureByFid(newFeatures[0].attributes["StrassenabschnittsID"]);
            strab.attributes["Zustandsabschnitte"].features.push(attribute);

            var zab = newFeatures[0];
            var trotts = this.zabtrott.getFeaturesByAttribute("Zustandsabschnitt", zab.fid);

            this.zabtrott.removeFeatures(trotts);
            if (zab.attributes.Trottoir) {
                var features = gformat.read(jformat.write(zab.attributes.Trottoir));
                if (features) {
                    this.zabtrott.addFeatures(features);
                }
            }

            this.zabs.addFeatures(newFeatures);
            this.zabs.refresh();
        },
        roundToSignificantFigures: function (num, n) {
            if (num == 0) {
                return 0;
            }

            var d = Math.ceil((Math.log(num < 0 ? -num : num) / Math.LN10));
            var power = n - Math.floor(d);

            var magnitude = Math.pow(10, power);
            var shifted = Math.round(num * magnitude);
            return shifted / magnitude;
        },
        destroy: function () {
            this.map.destroy();
        }
    });
})();