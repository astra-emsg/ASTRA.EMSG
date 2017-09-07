(function () {
  var Control = OpenLayers.Control;
  var Measure = Control.Measure;
  var $super = Measure.prototype;

  /**
  * Measure Control with a visual component containing the measures
  * @class
  */
  Control.MeasureEx = OpenLayers.Class(Measure, {
    /**
    * @property {DOMElement} to hold the extended div or this control
    */
    element: null

    /**
    * @property {String} before the measurement
    */
   , prefix: ''

    /**
    * @property {String} after the measurement
    */
   , suffix: ''

    /**
    * @property {Number} of digits of the measurements, to show
    */
   , numDigits: 2

    /**
    * @property {String} seperaton sign for thousands groups
    */
   , seperator: "'"
   
    /**
    * @property {String} to set on deactivate if not null
    */
   , emptyString: ''

    /**
    * add neccessary dom elements and register on our own measurepartial event
    */
   , draw: function () {
     $super.draw.apply(this, arguments);
     if (!this.element) {
       this.element = this.div;
       this.events.register("measurepartial", this, this.redraw);
       this.reset();
     };
     return this.div;
   }

    /**
    * update the measurement text in the dom element
    * @param {Object} measurepartial event containing
    *  - {String} units
    *  - {Number} order
    *  - {Number} measure
    */
   , redraw: function (evt) {
     var units = evt.units;
     var order = evt.order;
     var measure = evt.measure;

     var newHtml = this.formatOutput(measure, order, units);
     if (newHtml != this.element.innerHTML) {
       this.element.innerHTML = newHtml;
       $(this.element).show();
     }
   }

   , deactivate: function () {
     this.reset();
     return $super.deactivate.apply(this, arguments);
   }

   , reset: function (evt) {
     if (this.emptyString != null) {
       this.element.innerHTML = this.emptyString;
     }
     if (!this.element.innerHTML) {
       $(this.element).hide();
     };
   }

    /**
    * get formated html string for a measurement
    * @param {Number} measure
    * @param {Number} order
    * @param {String} units
    * @return {String} with possible HTML Markup
    */
   , formatOutput: function (measure, order, units) {
     measure = measure.toFixed(this.numDigits).replace(/\B(?=(\d{3})+(?!\d))/g, this.seperator);
     var newHtml = this.prefix + measure + " " + units;
     if (order != 1) {
       newHtml += "<sup>" + order + "</sup>";
     };
     newHtml += this.suffix;

     return newHtml;
   }
  });

})();
