var gisReport = function () {

    var form = function () {

        function hookOnReportButtons() {
            $(".emsg-excel-report-button").click(generateReport);
            $(".emsg-pdf-report-button").click(generateReport);
            $(".emsg-map-pdf-report-button").click(generateGisMapReport);
            $(".emsg-pdf-preview-button").click(generateGisMapPreview);
            common.hookOnClosePopup();
        }

        function getGridDiv() {
            return $(gisReport.formParamaters().gridDivId);
        }
        
        function getGridDivId() {
            return gisReport.formParamaters().gridDivId;
        }

        function generateReport() {
            var generateReportUrl = $(this).data("generate-report-action");
            var getLastGeneratedReportUrl = $(this).data("get-last-generated-report-action");
            var outputFormat = $(this).data("output-format");

            generateReportFor(generateReportUrl, getLastGeneratedReportUrl, outputFormat);
        }

        function generateGisMapReport() {
            var gisReportParametersInput = $("#gisReportGenerationParameters");
            
            var generateReportUrl = gisReportParametersInput.data("generate-report-action");
            var getLastGeneratedReportUrl = gisReportParametersInput.data("get-last-generated-report-action");
            var outputFormat = gisReportParametersInput.data("output-format");
            
            var reportParameter = getReportParameter();
            reportParameter.OutputFormat = outputFormat;
            $.post(generateReportUrl, reportParameter, function (data) { $.download(getLastGeneratedReportUrl, reportParameter); });
        }

        function generateGisMapPreview() {
            var gisReportParametersInput = $("#gisReportGenerationParameters");

            var generateReportUrl = gisReportParametersInput.data("preview-report-action");
            var outputFormat = gisReportParametersInput.data("output-format");

            var reportParameter = getReportParameter();
            reportParameter.OutputFormat = outputFormat;

            $("#map-report-preview-image").attr("src", generateReportUrl + '?' + $.param(reportParameter) + '&t=' + new Date().getTime());

            var tWindow = $("#map-report-preview").data("kendoWindow");
            tWindow.restore();
            tWindow.setOptions({
                width: 1280,
                height: 768
            });
            var wrapper = tWindow.wrapper;
            wrapper.css("z-index", 101);
            tWindow.center();
            blockScreen();

            $("#map-report-preview-image").one("load", function() {
                tWindow.restore();
                tWindow.center();
                tWindow.open();
                unblockScreen();
            });
        }

        function onPreviewClose() {
            $("#map-report-preview-image").attr("src", "");
        }

        function generateReportFor(generateReportUrl, getLastGeneratedReportUrl, outputFormat) {
            var reportParameter = getReportParameter();

            //Rendering parameters
            reportParameter.OutputFormat = outputFormat;

            var checkReportAvailable = function () {
                setTimeout(function() {
                    $.post(getLastGeneratedReportUrl + 'Ready', {}, function(data) {
                        if (data.ready) {
                            $.download(getLastGeneratedReportUrl, reportParameter);
                            unblockScreen();
                            $(document).ajaxStart(blockScreen).ajaxStop(unblockScreen);
                        } else {
                            checkReportAvailable();
                        }
                    });
                }, 5000);
            }

            $(document).unbind('ajaxStart');
            $(document).unbind('ajaxStop');
            
            blockScreen();
            $.post(generateReportUrl, reportParameter, function(data) {
                checkReportAvailable();
            });
        }

        function getReportParameter() {
            var mapParameters;
            if (emsg.map.getFilterParameters)
                mapParameters = emsg.map.getFilterParameters();

            mapParameters || (mapParameters = { });

            var commonParameters = {
                //ErfassungsPerod
                ErfassungsPeriodId: $("#ErfassungsPeriod").val(),
                
                //Map Parameters
                BoundingBox: mapParameters.mapBBOX,
                BoundingBoxFilter: mapParameters.mapBBOXFilter,
                MapSize: mapParameters.mapSize,
                BackgroundLayers: mapParameters.mapBackgroundLayers,
                LayersAV: mapParameters.mapLayersAV,
                LayersAVBackground: mapParameters.mapBackgroundLayersAV,
                Layers: mapParameters.mapLayers,
                LayerDefs: mapParameters.mapLayerDefs,
                ScaleWidth: mapParameters.scale_width,
                ScaleText: mapParameters.scale_text
            };

            emsg.form.setFormParameters(commonParameters);
            return commonParameters;
        }

        function onDataBound(e) {
            var grid = getGridDiv().data("kendoGrid");
            $("#ReportButtons").toggle(grid.dataSource.total() != undefined && grid.dataSource.total() != 0);
        }

        function onDataBinding(e) {
            e.data = getReportParameter();
        }

        function raiseFormFilterChanged() {
            if (emsg.events.form.onFilterChanged)
                emsg.events.form.onFilterChanged();
        }

        function refreshGrid() {
            emsg.form.refreshGrid();
            raiseFormFilterChanged();
        }

        function onMapFilterChanged() {
            common.refreshGrid(getGridDivId());
        }

        function onDataLoaded() {
            emsg.events.map.onFilterChanged = onMapFilterChanged;
            raiseFormFilterChanged();
        }

        //Parameters converted for the Map

        function getFormFilterParameters() {
            var par = gisReport.formParamaters().getFormFilterParametersForMap();
            return par;
        }

        function setFormParameters(parameters) {
            gisReport.formParamaters().setFormParameters(parameters);
        }

        function ref() {
            common.refreshGrid(getGridDivId());
        }
        
        function init() {
            hookOnReportButtons();

            emsg.form.refreshGrid = ref;
            
            if (!window['isUdocked']) {
                emsg.form.setFormParameters = setFormParameters;
                emsg.form.getFilterParameters = getFormFilterParameters;
            }

            if (emsg.events.map.onDataLoaded) {
                //We are in the popup
                emsg.events.map.onFilterChanged = onMapFilterChanged;
            }
            else {
                emsg.events.map.onDataLoaded = onDataLoaded;
            }
            
            emsg.form.generateGisMapPdfReport = generateGisMapReport;
            emsg.form.previewGisMapPdfReport = generateGisMapPreview;
        }

        return {
            init: init,
            getReportParameter: getReportParameter,
            refreshGrid: refreshGrid,
            onDataBound: onDataBound,
            onDataBinding: onDataBinding,
            raiseFormFilterChanged: raiseFormFilterChanged,
            onPreviewClose: onPreviewClose
        };
    } ();

    var index = function () {

        function init() {
            form.init();
        }

        return {
            init: init
        };
    } ();

    var formParamaters;

    function init(parmaters) {
        formParamaters = parmaters;
        index.init();
    }

    function getformParamaters() {
        return formParamaters;
    }

    return {
        init: init,
        form: form,
        index: index,
        formParamaters: getformParamaters
    };

} ();

var myWindow;

    function onErfassungsPeriodChanged() {
        gisReport.form.init();
        if (myWindow)
            myWindow.close();
        SetupUndock();
    }

    function SetupUndock() {

        $("#filterDiv").append('<div id="UndockForm" style="display: none"></div>');

        $('#Undock').on("click", function () {
            var tabStrip = $('#TabStrip').data("kendoTabStrip");
            if (tabStrip) {
                tabStrip.disable($(".k-item", tabStrip.element)[1]);
                tabStrip.select($(".k-item", tabStrip.element)[0]);
            }
            myWindow = window.open('', 'Undock', 'width=1280,height=768,resizable=1,scrollbars=1');
            buildUndockForm($("#UndockForm"));
            $("#UndockForm").find('form')[0].submit();
            $("#UndockForm").empty();
            $("#formDiv").empty();
            myWindow.focus();

            $(window).unload(function () {
                if (!myWindow.closed) {
                    clearTimeout(watchClose);
                    myWindow.close();
                }
            });

            var watchClose = setInterval(function () {
                if (myWindow.closed) {
                    clearTimeout(watchClose);
                    onPopopClose();
                }
            }, 200);

            function onPopopClose() {
                jQuery.telerik = false;
                $.ajax({
                    url: $("#formDiv").data('dock-url'),
                    type: 'POST',
                    data: JSON.stringify(gisReport.form.getReportParameter()),
                    contentType: 'application/json',
                    success: function(data) {
                        $('#formDiv').empty().append(data);
                        var tabStrip = $('#TabStrip').data("kendoTabStrip");
                        if (tabStrip) {
                            tabStrip.enable($(".k-item", tabStrip.element)[1]);
                        }
                        gisReport.form.init();
                        gisReport.form.raiseFormFilterChanged();
                        SetupUndock();
                    }
                });
            };
        });
    };

    $(function() {
        SetupUndock();
    });

    function buildUndockForm(form) {
        var formHtml = '<form method="POST" target="Undock" action="' + $("#formDiv").data('undock-url') + '">';
        var parameters = gisReport.form.getReportParameter();
        for (var parameter in parameters) {
            formHtml = formHtml + '<input type="hidden" name="' + parameter + '" value="' + parameters[parameter] + '" />';
        }
        formHtml = formHtml + "</form>";
        form.empty().append(formHtml);
    }