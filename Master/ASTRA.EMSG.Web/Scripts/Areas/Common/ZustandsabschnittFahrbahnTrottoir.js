var zustandsabschnittFahrbahnTrottoir = function () {
    function ZustandsabschnittFahrbahnTrottoirEditors(editorWindow) {
        this.init = init;
        this.closeEditWindow = closeEditWindow;

        function init() {
            common.hookOnForm(init, closeEditWindow, function () {
                closeEditWindow();
            });
        }

        function closeEditWindow() {
            $(editorWindow).data('kendoWindow').close();
        }

        return {
            init: init
        };
    }

    return {
        ZustandsabschnittFahrbahnTrottoirEditors: ZustandsabschnittFahrbahnTrottoirEditors
    };
} ();