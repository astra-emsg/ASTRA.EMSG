var common = function () {

    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    function hookOnForm(init, cancelCallback, submitCallback, needsConfirmation, getSubmitComfirmationMessage, getApplyComfirmationMessage) {
        hookOnSubmit(init, submitCallback, null, needsConfirmation, getSubmitComfirmationMessage);
        hookOnApply(init, null, needsConfirmation, getApplyComfirmationMessage);
        hookOnCancel(cancelCallback);
    }

    function hookOnNotAjaxForm(cancelMethod, submitConfirmationMethod) {
        common.hookOnButton(".emsg-submit-button", function (that) {
            if (submitConfirmationMethod)
                if (!submitConfirmationMethod())
                    return;

            common.destroyErrorDialogs();
            $(that).closest('form').submit();
        });

        if (cancelMethod) {
            common.hookOnButton(".emsg-cancel-button", function(that) {
                cancelMethod();
            });
        }
    }

    function hookOnSubmit(init, successCallback, getAdditionalData, needsConfirmation, getSubmitComfirmationMessage) {
        if (!needsConfirmation)
            needsConfirmation = function () { return true; };
        subscribeOnButtonEvents($(".emsg-submit-button"), function (that) {
            submitForm(that, init, getAdditionalData, successCallback, needsConfirmation, getSubmitComfirmationMessage);
        });
    }

    function hookOnApply(init, getAdditionalData, needsConfirmation, getApplyComfirmationMessage) {
        if (!needsConfirmation)
            needsConfirmation = function () { return true; };
        subscribeOnButtonEvents($(".emsg-apply-button"), function (that) {
            applyForm(that, init, getAdditionalData, needsConfirmation, getApplyComfirmationMessage);
        });
    }

    function hookOnCancel(callback) {
        subscribeOnButtonEvents($(".emsg-cancel-button"), function (that) {
            cancelForm(that);
            if (callback)
                callback();
        });
    }

    function hookOnFilter(callback) {
        subscribeOnButtonEvents($(".emsg-filter-button"), function (that) {
            callback();
        });
    }

    function hookOnButton(buttonClass, callback) {
        subscribeOnButtonEvents($(buttonClass), function (that) {
            callback(that);
        });
    }

    function subscribeOnButtonEvents(button, callback) {
        button.bind({
            click: function () { callback(this); },
            keypress: function (event) {
                if (event.which == 13 || event.which == 32) {
                    callback(this);
                    event.preventDefault();
                    return false;
                }
                return true;
            }
        });
    }

    function hookOnClosePopup() {
        $(".emsg-popup-close-button").click(closePopup);
    }

    function submitForm(that, init, getAdditionalData, successCallback, needsConfirmation, getSubmitComfirmationMessage) {
        var comfirmationMessage = $(that).data('submit-comfirmation-message');
        if (getSubmitComfirmationMessage)
            comfirmationMessage = getSubmitComfirmationMessage();

        if (!(comfirmationMessage && needsConfirmation()) || confirm(comfirmationMessage)) {
            common.destroyErrorDialogs();
            var form = $(that).closest('form');
            var formDiv = form.data('ajax-update');
            var formAction = form.attr('action');
            var postData = form.serializeObject();
            if (getAdditionalData)
                getAdditionalData(postData);
            $.ajax({
                type: 'POST',
                url: formAction,
                dataType: 'html',
                data: JSON.stringify(postData),
                success: function (data) {
                    $(formDiv).empty().append(data);
                    if (data !== "") {
                        init();
                    }
                    else {
                        if (successCallback)
                            successCallback();
                    }
                },
                contentType: 'application/json'
            });
        }
    }

    function applyForm(that, init, getAdditionalData, needsConfirmation, getApplyComfirmationMessage) {
        var comfirmationMessage = $(that).data('apply-comfirmation-message');
        if (getApplyComfirmationMessage)
            comfirmationMessage = getApplyComfirmationMessage();
        
        if (!(comfirmationMessage && needsConfirmation()) || confirm(comfirmationMessage)) {
            common.destroyErrorDialogs();
            var applyActionUrl = $(that).data('apply-action');
            var form = $(that).closest('form');
            var formDiv = form.data('ajax-update');
            var postData = form.serializeObject();
            if (getAdditionalData)
                getAdditionalData(postData);
            $.ajax({
                type: 'POST',
                url: applyActionUrl,
                dataType: 'html',
                data: JSON.stringify(postData),
                success: function (data) {
                    $(formDiv).empty().append(data);
                    init();
                },
                contentType: 'application/json'
            });
        }
    }

    function cancelForm(that) {
        var windowDiv = $(that).closest('.k-window-content');
        windowDiv.data('kendoWindow').close();
    }

    function closePopup() {
        var windowDiv = $(this).closest('.k-window-content');
        windowDiv.data('kendoWindow').close();
    }

    function createErrorDialogs() {
        $(".error-message-div").dialog({ autoOpen: false, zIndex: 20000, resizable: false, minHeight: 0, minWidth: 0 });
        
        $(".error-image").mouseenter(function (event) {
            var element = $(this);
            var propery = element.attr('data-property');
            var div = "div[data-property='" + propery + "']";
            $(div).dialog("option", "position", {
                my: "left center",
                at: "right center",
                collision: "flip flip",
                of: element,
                offset: "10 0"
            });
            $(div).dialog('open');
        });

        $(".error-image").mouseout(function () {
            var propery = $(this).attr('data-property');
            var div = "div[data-property='" + propery + "']";
            $(div).dialog('close');
        });
    }

    function destroyErrorDialogs() {
        $(".error-message-div").dialog("destroy");
        $(".error-message-div").remove();
    }

    function isPageable(grid) {
        return  grid.dataSource.pageSize();
    }
    
    function refreshGrid(gridId, keepCurentPage) {
        var grid = $(gridId).data('kendoGrid');

        if (grid) {
            if (isPageable(grid)) {
                if (!keepCurentPage)
                    grid.dataSource.page(1);
                else
                    grid.dataSource.page(grid.dataSource.page());
            } else {
                grid.dataSource.read();
            }
        }
    }

    return {
        hookOnForm: hookOnForm,
        hookOnSubmit: hookOnSubmit,
        hookOnApply: hookOnApply,
        hookOnClosePopup: hookOnClosePopup,
        hookOnCancel: hookOnCancel,
        hookOnButton: hookOnButton,
        createErrorDialogs: createErrorDialogs,
        destroyErrorDialogs: destroyErrorDialogs,
        hookOnFilter: hookOnFilter,
        refreshGrid: refreshGrid,
        hookOnNotAjaxForm: hookOnNotAjaxForm,
        submitForm: submitForm,
        subscribeOnButtonEvents: subscribeOnButtonEvents,
        applyForm: applyForm,
    };
} ();