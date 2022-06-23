var userRolesList = {
    listtemplate: null,

    getColumnsDefinition: function () {
        var actions = function (data, type, row) {
            var id = row.ID;
            var actions = [];
            if (!userRolesList.listtemplate.readonly) {
                actions.push({ name: "Edit", click: "userRolesList.edit('" + id + "')", html: '<i class="glyphicon glyphicon-edit"></i>' });
                actions.push({ name: "Delete", click: "userRolesList.delete('" + id + "','" + row.Name + "')", html: '<i class="glyphicon glyphicon-remove"></i>' });
            }
            return dataTablesService.getInlineActions(actions);
        };
        return [
            { sName: "ID", bVisible: false, mData: "ID" },
            { sTitle: "Name", sClass: "text-left", mData: "Name" },
            { sTitle: "Readonly Access", sClass: "text-center", mData: "IsReadonly", mRender: function (data, type, row) {
                if (type === 'display') {
                    if (row.IsReadonly) return '<i class="glyphicon glyphicon-ok"></i>';
                }
                return '';
            } },
            { sTitle: "Companies Access", sClass: "text-left", mData: "CompaniesAccess" },
            { sTitle: "Documents Access", sClass: "text-left", mData: "DocumentsAccess" },
            { sTitle: "Actions", bSortable: false, sClass: "text-right", mData: null, mRender: function (data, type, row) { return actions(data, type, row) } }
        ]
    },

    addNew: function () {
        userRolesList.edit(null, true);
    },

    edit: function (id, addNew) {
        var uriDetails = userRolesList.listtemplate.root + 'UserRoles/GetUserRoleDetails';

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
                var modalName = addNew ? "Add New UserRole" : "Edit UserRole";
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
                                userRolesList.listtemplate.refresh();
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

    delete: function (id) {
        var uriDelete = userRolesList.listtemplate.root + 'UserRoles/Delete';
        var $modal = modalService.createModal("Delete UserRole", { Cancel: true, Delete: { callback: function () {
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
                        userRolesList.listtemplate.refresh();
                }
            });
            var $ajaxLoader = new ajaxLoader($('body'));
        }
        }
        });
        $('.modal-body', $modal).html("<h4>Are you sure to delete the User Role?</h4>");
        $modal.modal();
    }
}