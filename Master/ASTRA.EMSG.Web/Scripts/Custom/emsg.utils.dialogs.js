function openDialog(dialogDiv, titleText) {
    $("#" + dialogDiv).dialog({ modal: true, resizable: true, title: titleText, autoOpen: false, closeOnEscape: true, closeText: '' });
}

function closeDialog(dialogDiv) {
    $("#" + dialogDiv).dialog("close");
}

function openEditDialog(dialogDiv, action) {
    $.ajax({
        url: action,
        type: "POST",
        success: function (data) {
            $("#" + dialogDiv).empty().append(data);
            $("#" + dialogDiv).dialog({ modal: true, resizable: true, title: 'Details', closeOnEscape: true, closeText: '', width: 'auto', height: 'auto' });
        },
        error: function (xhr, textStatus, errorThrown) {
            $("#" + dialogDiv).empty().append(xhr.responseText);
            $("#" + dialogDiv).dialog({ modal: true, resizable: true, title: "Error calling action: " + action, closeOnEscape: true, closeText: '', width: 1024, height: 768 });
        }
    });
}

function isOpenDialog(dialogDiv) {
    return $("#" + dialogDiv).dialog("isOpen");
}

function selectRadioButton(name, value) {
    $("[name$='" + name + "'][value$='" + value + "']").prop('checked', true);
}

function buildUrl(url, parameters) {
    var qs = "";
    for (var key in parameters) {
        var value = parameters[key];
        qs += encodeURIComponent(key) + "=" + encodeURIComponent(value) + "&";
    }

    if (qs.length > 0) {
        qs = qs.substring(0, qs.length - 1); //trim the last "&"
        url = url + "?" + qs;
    }

    return url;
}

function replaceTextContent(url, target) {
    $.ajax({
        url: url,
        type: 'POST',
        datatype: 'text',
        success: function (data) {
            $('#' + target).empty().append(data);
        }
    });
}

function openEditWindow(id, url, divName, windowName, windowTitle, onOpenedFunction) {
    $.ajax({
        url: url,
        type: 'POST',
        data: { id: id },
        success: function (data) {
            openWindow(data, windowName, divName, windowTitle, onOpenedFunction);
        }
    });
}

function openFullScreenEditWindow(id, url, divName, windowName, windowTitle, onOpenedFunction) {
    $.ajax({
        url: url,
        type: 'POST',
        data: { id: id },
        success: function (data) {
            openFullScreenWindow(data, windowName, divName, windowTitle, onOpenedFunction);
        }
    });
}

function openCreateWindow(url, divName, windowName, windowTitle, onOpenedFunction) {
    $.post(url, function (data) { openWindow(data, windowName, divName, windowTitle, onOpenedFunction); });
}

function openGeneralWindow(url, divName, windowName, windowTitle, onOpenedFunction) {
    $.post(url, function (data) { openWindow(data, windowName, divName, windowTitle, onOpenedFunction); });
}

function openGeneralWindowWithPostedData(url, dataToPost, divName, windowName, windowTitle, onOpenedFunction) {
    $.ajax({
        url: url,
        type: 'POST',
        data: dataToPost,
        success: function (data) {
            openWindow(data, windowName, divName, windowTitle, onOpenedFunction);
        }
    });
}

function openGeneralFullScreenWindowWithPostedData(url, dataToPost, divName, windowName, windowTitle, onOpenedFunction) {
    $.ajax({
        url: url,
        type: 'POST',
        data: dataToPost,
        success: function (data) {
            openFullScreenWindow(data, windowName, divName, windowTitle, onOpenedFunction);
        }
    });
}

function openWindow(data, windowName, divName, windowTitle, onOpenedFunction) {
    $('#' + divName).empty().append(data);
    var window = $('#' + windowName).data('kendoWindow');

    if (windowTitle)
        window.title(windowTitle);

    window.options.animation = { open: { effects: false }, close: { effects: false, reverse: true } };

    window.restore();
    window.center();
    window.open();
    
    setTimeout(
        function () {
            focusFirstInput(divName);
        });

    if (onOpenedFunction)
        onOpenedFunction();
}

function openFullScreenWindow(data, windowName, divName, windowTitle, onOpenedFunction) {
    $('#' + divName).empty().append(data);
    openFullScreenWindowWithoutData(windowName, divName, windowTitle, onOpenedFunction);
}

function openFullScreenWindowWithoutData(windowName, divName, windowTitle, onOpenedFunction) {
    var window = $('#' + windowName).data('kendoWindow');

    if (windowTitle)
        window.title(windowTitle);

    window.options.animation = { open: { effects: false }, close: { effects: false, reverse: true } };
    
    window.restore();
    window.center();
    window.maximize();
    
    window.open();

    //remove overflow: hidden added by the Telerik full screen popup
    //the overflow: hidden causes the validation tooltips to cut off 
    $('body').css('overflow', 'visible');

    setTimeout(
        function() {
            focusFirstInput(divName);
        });

    if (onOpenedFunction)
        onOpenedFunction();
}

function focusFirstInput(divName) {
    var firstInput = $('#' + divName + " input[type!='hidden']").not('[readonly]').first();
    var dropDown = $(firstInput).closest('.k-dropdown');
    if (dropDown) {
        dropDown.focus();
        return;
    }   

    var numericTextBox = $(firstInput).closest('.k-numerictextbox').data("kendoNumericTextBox");
    if (numericTextBox) {
        numericTextBox.focus();
        return;
    }

    firstInput.focus();
}

function triggerFilterOnEnter(e) {
    var eventArg = e;
    if (eventArg == null)
        eventArg = event;
    
    if ((eventArg.keyCode || eventArg.which) == 13) {
        $('.filterButton').click(); 
    }
}

function triggerFilterOn3Char(e) {
    var eventArg = e;
    if (eventArg == null)
        eventArg = event;

    if ((eventArg.keyCode || eventArg.which) == 13) {
        $('.filterButton').click();
    }
    var inputField = eventArg.target || eventArg.srcElement;
    if (inputField.value.length >= 3 || inputField.value.length == 0) {
        // disable "please wait" while the user is typing
        var tmp = $.blockUI;
        $.blockUI = function () { };
        $('.filterButton').click();
        // enable it again.
        $.blockUI = tmp;
    }
}

function handleDecimalSeparator(e) {
    var dot = 46;
    var comma = 44;
    var numpadDecimalSeparator = 110;
    if (e.keyCode == dot || e.keyCode == comma || e.keyCode == numpadDecimalSeparator) {
        if (e.preventDefault) {
            e.preventDefault();
        }
        
        return false;
    }
}

function convertGuidToHexa(guid) {
    if (guid == null || guid.length < 36)
        return null;
    return guid;
    //g = guid.replace(/-/gi, "");
    //var hexaGuid = g[6] + g[7] + g[4] + g[5] + g[2] + g[3] + g[0] + g[1] + "" + g[10] + g[11] + g[8] + g[9] + "" + g[14] + g[15] + g[12] + g[13] + "" + g.substring(16, 32);
    //return hexaGuid.toUpperCase();
}

String.format = String.prototype.format = function() {
    var i=0;
    var string = (typeof(this) == "function" && !(i++)) ? arguments[0] : this;
 
    for (; i < arguments.length; i++)
        string = string.replace(/\{\d+?\}/, arguments[i]);
 
    return string;
}