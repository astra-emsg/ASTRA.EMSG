Emsg.StyleMaps = {};
Emsg.StyleMaps.AXIS = new OpenLayers.StyleMap({
  "default" : new OpenLayers.Style(null, {
                rules: [
                  new OpenLayers.Rule({
                    symbolizer: {
                       "Point" : { strokeWidth: 1, strokeOpacity: 1, strokeColor: "#333333", pointRadius: 5, graphicName: "square", fillColor: "white", fillOpacity: 0.5}
                     , "Line"  : { strokeWidth: 7, strokeOpacity: 0.2, strokeColor: "#666666" }
                    }
                 })
                ]
              })

, "select" : new OpenLayers.Style({ strokeWidth: 5, strokeColor: "#00ccff" })
, "temporary" : new OpenLayers.Style(null, {
                    rules: [
                        new OpenLayers.Rule({
                            symbolizer: {
                                "Point" : { strokeWidth: 1, strokeOpacity: 1, strokeColor: "#333333", pointRadius: 5, graphicName: "square", fillColor: "white", fillOpacity: 0.5 }
                              , "Line"  : { strokeWidth: 3, strokeOpacity: 1, strokeColor: "#00ccff" }
                            }
                        })
                    ]
                })
});

Emsg.StyleMaps.STRABS = new OpenLayers.StyleMap({
                "default": new OpenLayers.Style({
                    strokeColor: "#00ffcc",
                    strokeWidth: 5
                })
            });

// Highlight of the current Straﬂenabschnitt while in edit-mode of zustandsabschnitt.
Emsg.StyleMaps.PARENT_STRABS = new OpenLayers.StyleMap({
  "default": new OpenLayers.Style({
      strokeWidth: 18
    , strokeOpacity: 0.5
    , strokeColor: "#666666"
  })
});

Emsg.StyleMaps.MAIN_SEGMENT = new OpenLayers.StyleMap({
                "default": new OpenLayers.Style(null, {
                    rules: [
                        new OpenLayers.Rule({
                            symbolizer: {
                                "Point": {
                                    pointRadius: 5,
                                    graphicName: "square",
                                    fillColor: "white",
                                    fillOpacity: 0.5,
                                    strokeWidth: 1,
                                    strokeOpacity: 1,
                                    strokeColor: "#333333"
                                },
                                "Line": {
                                    strokeWidth: 5,
                                    strokeOpacity: 0.5,
                                    strokeColor: "#ffcc00"
                                }
                            }
                        })
                    ]
                }),
                "select": new OpenLayers.Style({
                    strokeColor: "#00ccff",
                    strokeWidth: 5
                }),
                "temporary": new OpenLayers.Style(null, {
                    rules: [
                        new OpenLayers.Rule({
                            symbolizer: {
                                "Point": {
                                    pointRadius: 5,
                                    graphicName: "square",
                                    fillColor: "white",
                                    fillOpacity: 0.5,
                                    strokeWidth: 1,
                                    strokeOpacity: 1,
                                    strokeColor: "#333333"
                                },
                                "Line": {
                                    strokeWidth: 5,
                                    strokeOpacity: 1,
                                    strokeColor: "#00ccff"
                                }
                            }
                        })
                    ]
                })
            });
