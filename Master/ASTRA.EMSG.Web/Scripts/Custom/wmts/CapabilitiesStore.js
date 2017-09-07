(function()
{ "use strict";

  var Console             = OpenLayers.Console
    , Events              = OpenLayers.Events
    , WMTSCapabilities    = OpenLayers.Format.WMTSCapabilities
    , Request             = OpenLayers.Request
    , CapabilitiesRecord  = Emsg.CapabilitiesRecord
    ;

  Emsg.CapabilitiesStore = OpenLayers.Class({
      records: null

    , ids: null

    , loading: null

    , format: null
      
      //------------------------------------------------------------------------
    , initialize: function()
      {
          this.events = new Events( this );
          this.records = {};
          this.ids = {};
          this.loading = { length: 0 };
          this.format = new WMTSCapabilities();
      }

      //------------------------------------------------------------------------
    , isLoading: function()
      {
          return !!this.loading.length
      }

      //------------------------------------------------------------------------
    , add: function( data )
      {
          var records = this.records
            , ids = this.ids
            , url = data.url
            , id = data.id
            ;
          if( ids[id] = records[url] ) return;

          this.load( ids[id] = records[url] = new CapabilitiesRecord( data ) );
      }

      //------------------------------------------------------------------------
    , createLayer: function( id, options )
      {
          var record = this.ids[id]
            , format  = this.format
            , capabilities = null
            , layer = null
            ;
          
          if ( record && ( capabilities = record.capabilities ) )
          {
              layer = format.createLayer( capabilities, options );
          }

          return layer;
      }

      //------------------------------------------------------------------------
    , load: function( record )
      {
        var loading = this.loading
          , url     = record.url
          ;

        loading[url] = record;
        ++loading.length;
        Request.GET({
            url: url
          , params: {
                SERVICE: "WMTS"
              , VERSION: "1.0.0"
              , REQUEST: "GetCapabilities"
            }

          , success : function( request ){ this.onRequestSuccess ( record, request ); }
          , failure : function( request ){ this.onRequestFailure ( record, request ); }
          , scope: this
        });
      }

      //------------------------------------------------------------------------
    , loaded: function( record )
      {
        var loading = this.loading
          , url     = record.url
          ;

        delete loading[url];
        --loading.length;
        this.events.triggerEvent( 'loaded', record );
      }

      //------------------------------------------------------------------------
    , onRequestFailure: function( record )
      {
          Console.error.apply( Console, arguments );
          this.loaded( record );
      }

      //------------------------------------------------------------------------
    , onRequestSuccess: function( record, request )
      {
          var doc = request.responseXML;
          if (!doc || !doc.documentElement) {
              doc = request.responseText;
          }
          var format = this.format
          record.capabilities = format.read( doc );
          this.loaded( record );
      }
  });
})();