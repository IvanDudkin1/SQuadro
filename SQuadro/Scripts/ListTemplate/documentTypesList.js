var documentTypesList = {
    listtemplate: null,

    getColumnsDefinition: function () {
        var actions = function (data, type, row) {
            var id = row.ID;
            var actions = [];
            if (!documentTypesList.listtemplate.readonly) {
                actions.push({ name: "Edit", click: "documentTypesList.edit('" + id + "')", html: '<i class="glyphicon glyphicon-edit"></i>' });
                actions.push({ name: "Delete", click: "documentTypesList.delete('" + id + "','" + row.Name + "')", html: '<i class="glyphicon glyphicon-remove"></i>' });
            }
            return dataTablesService.getInlineActions(actions);
        };
        return [
            { sName: "ID", bVisible: false, mData: "ID" },
            { sTitle: "Name", sName: "Name", sClass: "text-left", mData: "Name" },
            { sTitle: "Actions", bSortable: false, sClass: "text-right", mData: null, mRender: function (data, type, row) { return actions(data, type, row) } }
        ]
    },

    addNew: function () {
        documentTypesList.edit(null, true);
    },

    edit: function (id, addNew) {
        var uriDetails = documentTypesList.listtemplate.root + 'DocumentTypes/GetDocumentTypeDetails';

        var data = {};
        if (!addNew)
            data.id = id;

        $.ajax({
            cache: false,
            url: encodeURI(uriDetails),
            type: 'post',
            dataType: 'json',
            data: data,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (XMLHttpRequest.status === 401) {
                    location.reload();
                }
            },
            complete: function (data) {
                $ajaxLoader.remove();
            },
            success: function (data) {
                var modalName = addNew ? "Add New Document Type" : "Edit Document Type";
                var $modal = modalService.createModal(modalName, { Cancel: true, Save: { callback: function () {
                    var $form = $('form', $modal);
                    var data = $form.serializeObject();
                    $.ajax({
                        cache: false,
                        url: $form[0].action,
                        type: $form[0].method,
                        dataType: 'json',
                        data: data,
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            if (XMLHttpRequest.status === 401) {
                                location.reload();
                            }
                        },
                        complete: function (data) {
                            $ajaxLoader.remove();
                        },
                        success: function (data) {
                            if (!data.Result) {
                                $('.modal-body', $modal).html(data.Content).trigger('contentchanged');
                            }
                            else
                            {
                                documentTypesList.listtemplate.refresh();
                                $modal.modal('hide');
                            }
                        }
                    });
                    var $ajaxLoader = new ajaxLoader($modal);
                }
                }
                });

                $('.modal-body', $modal).html(data.Content);
                $modal.modal();
            }
        });
        var $ajaxLoader = new ajaxLoader($('body'));
    },

    'delete': function (id, name) {
        var uriDelete = documentTypesList.listtemplate.root + 'DocumentTypes/Delete';
        var $modal = modalService.createModal("Delete Document Type", { Cancel: true, Delete: { callback: function () {
             $.ajax({
                cache: false,
                url: encodeURI(uriDelete),
                type: 'post',
                dataType: 'json',
                data: { id: id },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (XMLHttpRequest.status === 401) {
                        location.reload();
                    }
                },
                complete: function (data) {
                    $ajaxLoader.remove();
                    $modal.modal('hide');
                },
                success: function (data) {
                    if (!data.Result) {
                        modalService.showAlert(data.Description, 'alert-error', "Error");
                    }
                    else
                        documentTypesList.listtemplate.refresh();
                }
            });
            var $ajaxLoader = new ajaxLoader($('body'));
        }
        }
        });
        $('.modal-body', $modal).html("<h4>Are you sure to delete Document Type " + name + "?</h4>");
        $modal.modal();
    }
}