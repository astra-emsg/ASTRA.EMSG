var index = new GisMode.MapOverviewSwitcher("#TabStrip", function () { overview.refreshGrid(); }, function () { window.setTimeout("emsg.map.updateSize()", 410); });

var detailsViewModel = new GisMode.Details();

var overview = new GisMode.OverviewModel(index, '#InspektionsroutenGrid',
    {
        deleted: function () { return emsg.events.form.onInspektionsrouteDeleted; },
        created: function () { return emsg.events.form.onInspektionsrouteCreated; },
        selected: function () { return emsg.events.form.onInspektionsrouteSelected; }
    });
    
overview.onRowDataBound = function (e) {
    var rows = e.sender.tbody.children();
    for (var i = 0; i < rows.length; i++) {
        var dataItem = e.sender.dataItem(rows[i]);
        if (!dataItem.CanEdit) {
            var cellChild = $(rows[i]).find(".k-button.k-grid-delete");
            if (cellChild.length > 0) {
                cellChild.addClass("k-state-disabled");
                cellChild.removeClass("k-grid-delete");
                cellChild.attr("style", "background: none repeat scroll 0 0 lightgray;");
                cellChild.attr("title", $(this).data("is-locked-message"));
            }
        }
    }
};

overview.exportInspektionsrouten = function (exportUrl, getLastExportUrl) {
    var postData = { ids: {}, exportBackground: true };

    postData.tempFileName = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });

    $(".emsg-row-selector:checked").each(function (index, item) {
        postData.ids[index] = item.getAttribute("data-id");
    });

    var createButton = function (context, value, func, classname) {
        var button = document.createElement("input");
        button.type = "button";
        button.value = value;
        button.onclick = func;
        button.className = classname;
        context.appendChild(button);
    };
    
    if (jQuery.isEmptyObject(postData.ids)) {
        alert($('#InspektionsroutenGrid').data('empty-list-warning'));
        return;
    }
    $("#exportConfirmContent").empty().append(('<p>' + $('#InspektionsroutenGrid').data('export-time-warning') + '\n\n' + $('#InspektionsroutenGrid').data('export-map-confirmation') + '</p>').replace(/\n/g, '<br />'));
    var kendoWindow = $("#exportConfirmDiv").data('kendoWindow');


    createButton($("#exportConfirmContent")[0], $('#InspektionsroutenGrid').data('cancel'),
            function () { kendoWindow.close(); }, "ExportDialogButton k-button");

    createButton($("#exportConfirmContent")[0], $('#InspektionsroutenGrid').data('no'),
            function () {
                kendoWindow.close();
                postData.exportBackground = false;
                $.post(exportUrl, postData, function (data) {
                    $.download(getLastExportUrl, postData);
                    overview.refreshGrid(true);
                    emsg.map.redraw();
                });
            }, "ExportDialogButton k-button");

    createButton($("#exportConfirmContent")[0], $('#InspektionsroutenGrid').data('yes'), function () {
        kendoWindow.close();
        postData.exportBackground = true;
        $.post(exportUrl, postData, function (data) {

            $.download(getLastExportUrl, postData);
            overview.refreshGrid(true);
            emsg.map.redraw();
        });
    }, "ExportDialogButton k-button");

    $(document).ajaxError(function (event, jqXHR, ajaxSettings, thrownError) {
        var timeout = "The request timed out";
        var longRunningActions = [
            exportUrl
        ];
        if (longRunningActions.indexOf(ajaxSettings.url) > -1 && jqXHR.status == 500 && jqXHR.responseText.indexOf(timeout) > -1) {
            var checkDone = function() {
                setTimeout(function() {
                    $.post(getExportReady, postData, function(data) {
                        if (data.ready) {
                            unblockScreen();

                            $.download(getLastExportUrl, postData);
                            $(document).ajaxStart(blockScreen).ajaxStop(unblockScreen);
                            overview.refreshGrid(true);
                            emsg.map.redraw();
                        } else {
                            checkDone();
                        }
                    });
                }, 5000);
            }
            $(document).unbind('ajaxStart');
            $(document).unbind('ajaxStop');

            blockScreen();
            checkDone();
        }
    });

    kendoWindow.restore();
    kendoWindow.center();

    kendoWindow.open();
    
};


overview.importInspektionsrouten = function(url) {
    openGeneralWindow(url, 'importDiv', 'ImportWindow');
};

overview.onImportWindowClose = function() {
    overview.refreshGrid(true);
};


var details = function () {

  var dirtyTracker = new GisMode.DirtyTracker('#inspektionsrouteEditFormDiv');

  var inspektionsrouteeditformdiv = '#inspektionsrouteEditFormDiv';
  var inspektionsrtstrabschnittegrid = '#InspektionsRtStrAbschnitteGrid';
  
  emsg.form.executeCancel = onCloseForm;

  function subscribeOnStrassenabschnittAddedRemoved() {

    emsg.events.map.onStrassenabschnittSelected = function (id) {
      $.ajax({
        url: $(inspektionsrouteeditformdiv).data('getstrassenabschnitt-action') + '/' + id,
        type: 'POST',
        success: function (data) {
          var grid = getGrid();
          var gridData = grid.dataSource.data();
          gridData.push(data);
          grid.total = grid.total + 1;
          fixReihenfolge(gridData);
          grid.dataSource.data(gridData);
          dirtyTracker.setIsDirty();
        }
      });
    };

    emsg.events.map.onStrassenabschnittDeselected = function (id) {
        removeStrassenabschnittFromGrid(id);
        dirtyTracker.setIsDirty();
    };
  }

  function unSubscribeOnStrassenabschnittAddedRemoved() {
    emsg.events.map.onStrassenabschnittSelected = function (id) { };
    emsg.events.map.onStrassenabschnittDeselected = function (id) { };
  }

  function removeStrassenabschnitt(id) {
    removeStrassenabschnittFromGrid(id);
    emsg.events.form.onStrassenabschnittDeselected(id);
    dirtyTracker.setIsDirty();
  }

  function removeStrassenabschnittFromGrid(id) {
    var grid = getGrid();
    var gridData = grid.dataSource.data();
    var foundAt = findIndex(gridData, function (item) { return item.StrassenabschnittId === id; });
    gridData.splice(foundAt, 1);
    grid.total = grid.total - 1;
    fixReihenfolge(gridData);
    grid.dataSource.data(gridData);
  }

  function fixReihenfolge(strassenabschnitten) {
    for (var index = 0; index < strassenabschnitten.length; index++) {
      strassenabschnitten[index].Reihenfolge = index;
    }
  }

  emsg.events.map.onInspektionsrouteCreated = function () {
      loadEditorFormAndList($(inspektionsrouteeditformdiv).data('create-action'), true);
  };

  emsg.events.map.onInspektionsrouteSelected = function (id) {
    loadEditorFormAndList($(inspektionsrouteeditformdiv).data('edit-action') + '/' + id);
  };

  function loadEditorFormAndList(url, setIsDirty) {
    $.ajax({
      url: url,
      type: 'POST',
      success: function (data) {
        replaceFormData(data);
        index.disableTab(1);
        init();
        emsg.events.form.onDataLoaded();
        if (setIsDirty)
              dirtyTracker.setIsDirty();
      }
    });
  }

  function replaceFormData(data) {
    var splittedData = data.split('**SPLIT**');
    $(inspektionsrouteeditformdiv).empty().append(splittedData[0]);
    $('#inspektionsRtStrAbschnitteListDiv').empty().append(splittedData[1]);
    // #5341, ME, 27.02.2013: this table has to resize the map once again, so we'd have to call adjustMapHeight().
    adjustMapHeight();
  }

  function init() {
    detailsViewModel.showForm();
    subscribeOnStrassenabschnittAddedRemoved();
    hookOnShowStatusverlauf();
    hookOnCancel();
    hookOnSubmit();
    common.hookOnButton(".emsg-delete-button", hookOnDelete);
    if ($("#IsValid").val().toLowerCase() === 'true')
        dirtyTracker.resetIsDirty();
  }

  function onCloseForm() {
    $('#inspektionsRtStrAbschnitteListDiv').empty();
    $(inspektionsrouteeditformdiv).empty();
    index.enableTab(1);
    unSubscribeOnStrassenabschnittAddedRemoved();
    detailsViewModel.hideForm();
    dirtyTracker.resetIsDirty();
  }

  function hookOnSubmit() {
    common.hookOnButton(".emsg-submit-button", function (that) {
      var comfirmationMessage = $(that).attr('data-submit-comfirmation-message');
      if (!comfirmationMessage || confirm(comfirmationMessage)) {
        var form = $(that).closest('form');
        var formAction = form.attr('action');
        var postData = form.serializeObject();
        addGridData(postData);
        $.ajax({
          type: 'POST',
          url: formAction,
          data: JSON.stringify(postData),
          success: function (data) {
            if (typeof (data) != "string") data = "";
            replaceFormData(data);
            if (data !== "") {
              init();
            } else {
              onCloseForm();
              emsg.events.form.onSaveSuccess();
            }
          },
          contentType: 'application/json'
        });
      }
    });
  }

  function hookOnShowStatusverlauf() {
    common.hookOnButton(".emsg-statusverlauf-button", function (that) {
      var url = $(that).attr("data-url");
      var id = $(that).attr("data-id");
      openGeneralWindowWithPostedData(url, { id: id }, "statusverlaufDiv", "StatusverlaufWindow", null, showStatusverlauf.init);

    });
  }

  function hookOnDelete(that) {
    var routeId = $('#inspektionsrouteEditFormDiv #Id').val();
    var comfirmationMessage = $(that).data('delete-comfirmation-message');
    if (!comfirmationMessage || confirm(comfirmationMessage)) {
      $.post($(that).data('delete-action') + '/' + routeId, function () {
        $('#inspektionsRtStrAbschnitteListDiv').empty();
        $(inspektionsrouteeditformdiv).empty();
        onCloseForm();
        emsg.events.form.onInspektionsrouteDeleted(routeId);
      });
    }
  }

  function hookOnCancel() {
    common.hookOnButton(".emsg-cancel-button", function () {
      emsg.cancel();
    });
  }

  function addGridData(formData) {
      var gridData = getGrid().dataSource.data();
    for (var i = 0; i < gridData.length; i++) {
      delete gridData[i].Strasseneigentuemer; //To make modelbinder happy
    }
    formData["InspektionsRtStrAbschnitteModelList"] = gridData;
  }

  function getGrid() {
    return $(inspektionsrtstrabschnittegrid).data("kendoGrid");
  }

  function moveStrassenabschnittUp(id) {
    var grid = getGrid();
    var gridData = grid.dataSource.data();
    var foundAt = findIndex(gridData, function (item) { return item.StrassenabschnittId === id; });
    if (foundAt !== 0) {
      var clickedItem = gridData[foundAt];
      clickedItem.Reihenfolge = clickedItem.Reihenfolge - 1;
      var previousItem = gridData[foundAt - 1];
      previousItem.Reihenfolge = previousItem.Reihenfolge + 1;
      grid.dataSource.data(_.sortBy(gridData, function (item) { return item.Reihenfolge; }));
      dirtyTracker.setIsDirty();
    }
  }

  function moveStrassenabschnittDown(id) {
    var grid = getGrid();
    var gridData = grid.dataSource.data();
    var foundAt = findIndex(gridData, function (item) { return item.StrassenabschnittId === id; });
    if (foundAt + 1 !== gridData.length) {
      var clickedItem = gridData[foundAt];
      clickedItem.Reihenfolge = clickedItem.Reihenfolge + 1;
      var nextItem = gridData[foundAt + 1];
      nextItem.Reihenfolge = nextItem.Reihenfolge - 1;
      grid.dataSource.data(_.sortBy(gridData, function (item) { return item.Reihenfolge; }));
      dirtyTracker.setIsDirty();
    }
  }

  return {
    moveStrassenabschnittUp: moveStrassenabschnittUp,
    moveStrassenabschnittDown: moveStrassenabschnittDown,
    removeStrassenabschnitt: removeStrassenabschnitt,
    trackChanges: dirtyTracker.trackChanges,
    dropDownChanged: dirtyTracker.setIsDirty
  };

  function findIndex(collection, filter) {
    var foundAt = -1;
    _.find(collection, function (item, index) {
      var result = filter(item);
      if (result)
        foundAt = index;
      return result;
    });
    return foundAt;
  }

} ();


var showStatusverlauf = function () {

    function init() {
        common.hookOnClosePopup();
    }

    function onDataBinding(e) {
        e.data = { id: $("id").val() };
    }

    return {
        init: init,
        onDataBinding: onDataBinding
    };
} ();






var importInspektionsrouten = function () {

    function closeImportWindow() {
        $('#ImportWindow').data('kendoWindow').close();
    }
    
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
        emsg.events.form.onSaveSuccess();
        getResultDiv().empty().append(getData("successMessage"));
    }

    function onSelect(e) {
        var files = e.files;
        $.each(files, function () {
            if (this.extension.toLowerCase() != ".emsgi") {
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
        closeImportWindow: closeImportWindow,
        onUpload: onUpload,
        onSelect: onSelect,
        onSuccess: onSuccess,
        onError: onError
    };
    
} ();

$(function () {
    //overview.init();
    details.trackChanges();
});