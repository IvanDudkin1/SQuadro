var partnersList = {
    listtemplate: null,

    getColumnsDefinition: function () {
        var showContacts = function (data, type, row) {
            return '<a href="#" title="Show/Hide Contacts" onclick="partnersList.showContacts(this, ' + row.ID + '\')">' + row.FullName + '</a>';
        };
        var list = function (data, type, row) {
            var result = '';
            for (var i = 0; i < data.length; i++) {
                if (result != '') result += ', ';
                result += data[i].Name
            }
            return result;
        };

        return [
            { sName: "Selector", bSortable: false, mData: null, mRender: function (data, type, row) { return '<div class="checkbox"><label><input type="checkbox" class="select-row ace"/><span class="lbl"></span></label></div>'; } },
            { sName: "ID", bVisible: false, mData: "ID" },
            { sTitle: "Name", sName: "Name", sClass: "text-left hidden-sm hidden-xs", mData: "Name" },
            { sTitle: "Full Name", sName: "FullName", sClass: "text-left", mData: "FullName", mRender: function (data, type, row) { return showContacts(data, type, row); } },
            { sTitle: "Contacts", bVisible: false, sClass: "text-left", mData: "Contacts" },
            { sTitle: "Category", sName: "Categories", sClass: "text-left hidden-sm hidden-xs", mData: "Categories", mRender: function (data, type, row) { return list(data, type, row) } },
            { sTitle: "Area", sName: "Areas", sClass: "text-left hidden-sm hidden-xs", mData: "Areas", mRender: function (data, type, row) { return list(data, type, row) } },
            { sTitle: "Country", sName: "Country", sClass: "text-left hidden-sm hidden-xs", mData: "Country" },
            { sTitle: "Actions", bVisible: false, mData: null }
        ]
    },

    showContacts: function(target, id) {
        $tr = $(target).closest('tr');
        var data = this.listtemplate.$grid.fnGetData($tr[0]);

        var $trContacts = $tr.closest('table').find('#' + data.ID);
        if ($trContacts.length) {
            $container = $trContacts.find(".contacts-container");
            $containerRow = $container.closest('tr');
            if ($containerRow.is(":visible")) {
                $container.slideUp(500, function () { $containerRow.hide(); });
            }
            else {
               $containerRow.show();
               $container.slideDown(500);
            } 
        }
        else {
            $trContacts = $('<tr class="row-details"><td class="td-details-container" colspan=7><div class="contacts-container"></div></td></tr>').attr('hidden', 'hidden');
            $trContacts.find('.contacts-container').attr('hidden', 'hidden');
            $trContacts.attr('id', data.ID);
            $tr.after($trContacts);

            var uriContacts = this.listtemplate.root + 'Partners/GetContacts';

            $.ajax({
                cache: false,
                url: encodeURI(uriContacts),
                type: 'post',
                dataType: 'json',
                data: { id: id },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (XMLHttpRequest.status === 401) {
                        location.reload();
                    }
                },
                complete: function (data) {
                    document.body.style.cursor = "default";
                },
                success: function (data) {
                    if (!data.Result) {
                        modalService.showAlert(data.Description, 'alert-error', "Error");
                    }
                    else {
                        var $contacts = $trContacts.toggle().find(".contacts-container").html(data.Content);
                        var $respTable = $('table.responsive', $contacts);
                        if ($respTable.length) {
                            responsiveTables.updateTable($respTable);
                        }
                        $contacts.slideToggle(500);
                    }
                }
            });
            document.body.style.cursor = "wait";
        }
    }
}