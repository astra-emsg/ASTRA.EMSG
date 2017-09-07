(function()
{ "use strict";

  var Events = OpenLayers.Events
    , extend = OpenLayers.Util.extend
    ;

  Emsg.CapabilitiesRecord = OpenLayers.Class({
      id: null

    , url: null

    , capabilities: null

    , initialize: function( cfg )
      {
          this.events = new Events( this );
          extend( this, cfg );
      }
  });
})();