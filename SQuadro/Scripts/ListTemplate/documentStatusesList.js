var documentStatusesList = {
    listtemplate: null,

    getColumnsDefinition: function () {
        var actions = function (data, type, row) {
            var id = row.ID;
            var actions = [];
            if (!documentStatusesList.listtemplate.readonly) {
                actions.push({ name: "Edit", click: "documentStatusesList.edit('" + id + "')", html: '<i class="glyphicon glyphicon-edit"></i>' });
                actions.push({ name: "Delete", click: "documentStatusesList.delete('" + id + "','" + row.Name + "')", html: '<i class="glyphicon glyphicon-remove"></i>' });
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
        documentStatusesList.edit(null, true);
    },

    edit: function (id, addNew) {
        var uriDetails = documentStatusesList.listtemplate.root + 'DocumentStatuses/GetDocumentStatusDetails';

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
                var modalName = addNew ? "Add New Document Status" : "Edit Document Status";
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
                                documentStatusesList.listtemplate.refresh();
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
        var uriDelete = documentStatusesList.listtemplate.root + 'DocumentStatuses/Delete';
        var $modal = modalService.createModal("Delete Document Status", { Cancel: true, Delete: { callback: function () {
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
                        documentStatusesList.listtemplate.refresh();
                }
            });
            var $ajaxLoader = new ajaxLoader($('body'));
        }
        }
        });
        $('.modal-body', $modal).html("<h4>Are you sure to delete Document Status " + name + "?</h4>");
        $modal.modal();
    }
}