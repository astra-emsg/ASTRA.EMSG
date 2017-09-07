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

            updateMandantImage();
        });
    }

    function updateMandantImage() {
        $("#currentMandantImage").attr("src", getData("currentMandantImageUrl") + "?now=" + new Date().getTime());
    }

    function onSelect(e) {
        var files = e.files;
        $.each(files, function () {
            var ext = this.extension.toLowerCase();
            if (ext != ".jpeg" && ext != ".jpg" && ext != ".bmp" && ext != ".png") {
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

    return {
        updateMandantImage: updateMandantImage,
        onUpload: onUpload,
        onSelect: onSelect,
        onSuccess: onSuccess,
        onError: onError
    };

} ();

$(index.updateMandantImage);
