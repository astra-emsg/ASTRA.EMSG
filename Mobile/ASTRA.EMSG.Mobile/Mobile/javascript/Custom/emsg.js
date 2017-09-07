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

Emsg.App.prototype.datachanged = function (data) {
    window.emsg = window.emsg || new Emsg.App();
    window.emsg.localization = data.MobileLocalization;
};

Emsg.App.prototype.destroy = function () {
    if (this.map.destroy) {
        this.map.destroy();
    }
};
