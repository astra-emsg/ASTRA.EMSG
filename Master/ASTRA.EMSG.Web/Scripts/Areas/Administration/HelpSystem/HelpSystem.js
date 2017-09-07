var index = function () {
    function getData(dataName) {
        return $("#parameters").attr("data-" + dataName);
    }

    function getResultDiv() {
        return $("#result");
    }

    function onError(e) {
        var errorMessage;

        if (e.XMLHttpRequest.responseText.indexOf("Access is denied.") != -1) {
            errorMessage = getData("errorMessage");
        }
        else {
            errorMessage = e.XMLHttpRequest.responseText;
        }
        getResultDiv().empty().append(errorMessage);

        e.preventDefault();
    }

    function onSuccess(e) {
        $.post(getData("uploadResultUrl"), null, function (data) {
            getResultDiv().empty().append(data);
        });
    }

    function onSelect(e) {
        var files = e.files;
        $.each(files, function () {
            if (this.extension.toLowerCase() != ".zip") {
                alert(getData("fileExtensionErrorMessage"));
                e.preventDefault();
                return false;
            }
        });
        
        getResultDiv().empty();
    }

    function onUpload(e) {
        getResultDiv().empty().append(getData("progressText"));
    }

    function init() {
    }

    return {
        onUpload: onUpload,
        onSelect: onSelect,
        onSuccess: onSuccess,
        onError: onError,
        init: init
    };

} ();

$(index.init);
