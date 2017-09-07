var index = function () {

    function init() {
        common.hookOnFilter(refreshGrid);
        $("#HilfeSucheGrid .k-grid-header").css('display', 'none');
    }

    function getFilteredData(e) {
        return { HilfeFilter: $("#HilfeFilter").val() };
    }
    
    function onDataBound(e) {
        if ($("#HilfeFilter").val()) {
            $("#CountDiv").show();
            $("#ResultCount").text(getGrid().data('kendoGrid').total);
        } else {
            $("#CountDiv").hide();
        }
        
    }

    function getGrid() {
        return $('#HilfeSucheGrid');
    }

    function refreshGrid() {
        common.refreshGrid('#HilfeSucheGrid');
    }

    return {
        init: init,
        getFilteredData: getFilteredData,
        onDataBound: onDataBound,
        refreshGrid: refreshGrid,
        getGrid: getGrid
    };

} ();


$(function () {
    index.init();
});