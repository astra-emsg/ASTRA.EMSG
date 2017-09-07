var index = function () {

    function onRowDataBound(e) {
        var rows = e.sender.tbody.children();
        for (var i = 0; i < rows.length; i++) {
            var dataItem = e.sender.dataItem(rows[i]);
            if (!dataItem.CanCancell) {
                var cellChild = $(rows[i].children[0]);
                if(cellChild[0].innerHTML.indexOf("k-button") != -1)
                    cellChild[0].innerHTML = "<span></span>";
            }
        }
    }

    function cancellExport(id) {
        var url = $('#InspektionsroutenGrid').data("cancellexport-url");
        var confirmationMesage = $('#InspektionsroutenGrid').data("confirmation-mesage");
        if (confirm(confirmationMesage)) {
            $.ajax({
                url: url,
                type: 'POST',
                dataType: "text",
                data: { id: id },
                success: function (data) {
                    if (data) {
                        alert(data);
                    }
                    refreshGrid();
                }
            });
        }
    }

    function refreshGrid() {
        common.refreshGrid('#InspektionsroutenGrid', true);
    }

    return {
        onRowDataBound: onRowDataBound,
        cancellExport: cancellExport
    };

} ();