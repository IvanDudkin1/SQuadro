var documentsList = {
    listtemplate: null,

    getColumnsDefinition: function () {
        var actions = function (data, type, row) {
            var id = row.ID;
            var actions = [];
            actions.push({ name: "View", click: "documentsList.view('" + id + "')", html: '<i class="glyphicon glyphicon-search"></i>' });
            return dataTablesService.getInlineActions(actions);
        };

        return [
            { sName: "Selector", bSortable: false, mData: null, mRender: function (data, type, row) { return '<div class="checkbox"><label><input type="checkbox" class="select-row ace"/><span class="lbl"></span></label></div>'; } },
            { sName: "ID", bVisible: false, mData: "ID" },
            { sTitle: "Name", sName: "Name", sClass: "text-left", mData: "Name" },
            { sTitle: "Date", sName: "Date", sClass: "hidden-sm text-center hidden-xs", mData: "Date" },
            { sTitle: "Expiration", sName: "Expiration", sClass: "hidden-sm text-center hidden-xs", mData: "Expiration" },
            { sTitle: "Status", sName: "Status", sClass: "hidden-sm text-center hidden-xs", mData: "Status" },
            { sTitle: "Object", sName: "Object", sClass: "hidden-sm text-center hidden-xs", mData: "Object" },
            { sTitle: "Actions", bSortable: false, sClass: "text-right", mData: null, mRender: function (data, type, row) { return actions(data, type, row) } }
        ]
    },

    view: function (id) {
        var uriView = documentsList.listtemplate.root + 'documents/View/' + id;
        window.open(uriView);
    }
}