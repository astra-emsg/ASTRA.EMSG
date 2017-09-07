(function() {
    var originalNumericTextBox = kendo.ui.NumericTextBox;
    // Select All text on focus: http://docs.telerik.com/kendo-ui/controls/editors/numerictextbox/how-to/select-all-on-focus
    var EmsgNumericTextBox = kendo.ui.NumericTextBox.extend({
        init: function (element, options) {
            $(element).bind("focus", function () {
                var input = $(this);
                clearTimeout(input.data("selectTimeId")); //stop started time out if any

                var selectTimeId = setTimeout(function()  {
                    input.select();
                });

                input.data("selectTimeId", selectTimeId);
            }).blur(function(e) {
                clearTimeout($(this).data("selectTimeId")); //stop started timeout
            });
            originalNumericTextBox.fn.init.call(this, element, options);
        } 
    });

    kendo.ui.plugin(EmsgNumericTextBox);
})();