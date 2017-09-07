// Browser is removed in jQuery 1.9 - quick implementation for it

jQuery.uaMatch = function (ua) {
    ua = ua.toLowerCase();

    var match = /(chrome)[ /]([w.]+)/.exec( ua ) ||
            /(webkit)[ /]([w.]+)/.exec( ua ) ||
            /(opera)(?:.*version|)[ /]([w.]+)/.exec( ua ) ||
            /(msie) ([w.]+)/.exec( ua ) ||
            ua.indexOf("compatible") < 0 && /(mozilla)(?:.*? rv:([w.]+)|)/.exec( ua ) ||
            [];

    return {
        browser: match[ 1 ] || "",
        version: match[ 2 ] || "0"
    };
};

// Don't clobber any existing jQuery.browser in case it's different
if ( !jQuery.browser ) {
    matched = jQuery.uaMatch( navigator.userAgent );
    browser = {};

    if ( matched.browser ) {
        browser[ matched.browser ] = true;
        browser.version = matched.version;
    }

    // Chrome is Webkit, but Webkit is also Safari.
    if ( browser.chrome ) {
        browser.webkit = true;
    } else if ( browser.webkit ) {
        browser.safari = true;
    }

    jQuery.browser = browser;
}

OpenLayers.Layer.Vector.prototype.renderers = (jQuery.browser.webkit ? ['Canvas', 'VML'] : ['SVG', 'Canvas', 'VML']);

Emsg = {};

Emsg.Config = function() {
  this.data = {};
};
Emsg.Config.prototype.get = function (key) {
    if (this.data[key] === undefined) throw ("Unknown ConfigValue: " + key);
//    if (this.data[key] === undefined) return key;
    return this.data[key];
};
  Emsg.Config.prototype.set = function (key, value) {
    this.data[key] = value;
  };
  Emsg.Config.prototype.has = function (key) {
    return this.data[key] !== undefined;
  };

Emsg.App = function () {
  this.config = new Emsg.Config();
  this.form = new Emsg.FormInterface();
  this.map = new Emsg.MapInterface();
  this.debug = false;
  if (document.location.search.match('debug')) this.debug = true;

  this.events = { form: new Emsg.Events.Form(), map: new Emsg.Events.Map() };
};
Emsg.App.prototype.cancel = function () {
    var message = "";
    if (this.map.prepareCancel) {
        var mapCancel = this.map.prepareCancel();
        if (mapCancel.unsavedChanges)
            message = message + mapCancel.message;
    }

    if (this.form.prepareCancel) {
        var formCancel = this.form.prepareCancel();
        if (formCancel.unsavedChanges)
            message = message + formCancel.message;
    }

    if (message == "" || confirm(message)) {
        if (this.form.executeCancel)
            this.form.executeCancel();
        if (this.map.executeCancel)
            this.map.executeCancel();
        return true;
    }
    return false;
};
