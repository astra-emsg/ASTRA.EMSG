var GisMode = function () {

    function MapOverviewSwitcher(tabStripId, onNavigateToOverview, onNavigatedToMap) {

        this.onTabSelected = onTabSelected;
        this.disableTab = disableTab;
        this.enableTab = enableTab;
        this.selectDetailsTab = selectDetailsTab;


        function onTabSelected(e) {
            var tabStrip = $(tabStripId).data("kendoTabStrip");
            if (tabStrip.select().index() == 0) {
                onNavigateToOverview();
            } else if (tabStrip.select().index() == 1) {
                onNavigatedToMap();
            }
        }

        function selectDetailsTab() {
            var tabStrip = $(tabStripId).data("kendoTabStrip");
            onTabSelected();
            tabStrip.select($(".k-item", tabStrip.element)[0]);
        }

        function disableTab(index) {
            var tabStrip = $(tabStripId).data("kendoTabStrip");
            tabStrip.disable($(".k-item", tabStrip.element)[index]);
        }

        function enableTab(index) {
            var tabStrip = $(tabStripId).data("kendoTabStrip");
            tabStrip.enable($(".k-item", tabStrip.element)[index]);
        }
    }


    function OverviewModel(tabSwitcher, gridId, events, getFilterObject, onDeleteCustomization) {

        this.onDelete = onDelete;
        this.onComplete = onComplete;
        this.onDataBinding = onDataBinding;
        this.create = create;
        this.edit = edit;
        this.refreshGrid = refreshGrid;
        this.init = init;
        this.onDeleteCustomization = onDeleteCustomization;

        var idToDelete = null;

        function onDelete(e) {
            idToDelete = e.dataItem.Id;
            if (onDeleteCustomization)
                onDeleteCustomization(e);
        }

        function onComplete(e) {
            if (e.name == "delete" && idToDelete != null) {
                events.deleted()(idToDelete);
                idToDelete = null;
                refreshGrid();
            }
        }

        function onDataBinding(e) {
            e.data = getFilterObject();
        }

        function refreshGrid(keepCurentPage) {
            common.refreshGrid(gridId, keepCurentPage);
        }

        function create() {
            tabSwitcher.selectDetailsTab();
            events.created()();
        }

        function edit(id) {
            tabSwitcher.selectDetailsTab();
            events.selected()(id);
        }

        function init() {
            common.hookOnFilter(function () { refreshGrid(false); });
        }
    }

    function Details() {
        
        this.hideForm = hideForm;
        this.showForm = showForm;

        function hideForm() {
            $("#formCell").toggle(false);
            emsg.map.updateSize();
        }

        function showForm() {
            $("#formCell").toggle(true);
            emsg.map.updateSize();
        }
    }

    var DirtyTracker = function (formSelector) {

        var isDirty = false;
        
        $(window).bind('beforeunload', function () {
            if (!isDirty)
                return undefined;

            return $(formSelector).data('cancel-confirmation');
        });

        emsg.form.prepareCancel = function () {
            return { unsavedChanges: isDirty, message: $(formSelector).data('cancel-confirmation') };
        };

        function trackChanges() {
            $(formSelector).on('change', setIsDirty);
        }

        function setIsDirty() {
            isDirty = true;
        }

        function resetIsDirty() {
            isDirty = false;
        }

        return {
            trackChanges: trackChanges,
            setIsDirty: setIsDirty,
            resetIsDirty: resetIsDirty
        };
    };

    return {
        MapOverviewSwitcher: MapOverviewSwitcher,
        OverviewModel: OverviewModel,
        DirtyTracker: DirtyTracker,
        Details: Details
    };
} ();