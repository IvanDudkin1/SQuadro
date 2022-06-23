var documentsList = {
    listtemplate: null,

    getColumnsDefinition: function () {
        var actions = function (data, type, row) {
            var id = row.ID;
            var actions = [];
            actions.push({ name: "View", href: documentsList.listtemplate.root + 'Documents/View/' + id, target: "_blank", html: '<i class="glyphicon glyphicon-search"></i>', type: "link" });
            if (!documentsList.listtemplate.readonly) {
                actions.push({ name: "Edit", click: "documentsList.edit('" + id + "')", html: '<i class="glyphicon glyphicon-edit"></i>' });
                actions.push({ name: "Delete", click: "documentsList.delete('" + id + "','" + row.Name + "')", html: '<i class="glyphicon glyphicon-remove"></i>' });
            }
            return dataTablesService.getInlineActions(actions);
        };
        var showDetails = function (data, type, row) {
            return '<a href="#" title="Show/Hide Details" onclick="return documentsList.showDetails(this, \'' + row.ID + '\')">' + row.Name + '</a>' + ' <small>(' + row.FileName + ')</small    >';
        };

        return [
            { sName: "Selector", bSortable: false, mData: null, mRender: function (data, type, row) { return '<div class="checkbox"><label><input type="checkbox" class="select-row ace"/><span class="lbl"></span></label></div>'; } },
            { sName: "ID", bVisible: false, mData: "ID" },
            { sTitle: "Name", sName: "Name", sClass: "text-left", mData: "Name", mRender: function (data, type, row) { return showDetails(data, type, row); } },
            { sTitle: "Date", sName: "Date", sClass: "hidden-xs hidden-sm text-center", mData: "Date" },
            { sTitle: "Expiration", sName: "Expiration", sClass: "hidden-xs hidden-sm text-center", mData: "Expiration" },
            { sTitle: "Status", sName: "Status", sClass: "hidden-xs hidden-sm text-center", mData: "Status" },
            { sTitle: "Object", sName: "Object", sClass: "hidden-xs hidden-sm text-center", mData: "Object" },
            { sTitle: "Actions", bSortable: false, sClass: "text-right", mData: null, mRender: function (data, type, row) { return actions(data, type, row) } }
        ]
    },

    showDetails: function (target, id) {
        $tr = $(target).closest('tr');
        var data = this.listtemplate.$grid.fnGetData($tr[0]);

        var $trDetails = $tr.closest('table').find('#' + data.ID);
        if ($trDetails.length) {
            $container = $trDetails.find(".details-container");
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
            $trDetails = $('<tr class="row-details"><td class="td-details-container" colspan=7><div class="details-container"></div></td></tr>').attr('hidden', 'hidden');
            $trDetails.find('.details-container').attr('hidden', 'hidden');
            $trDetails.attr('id', data.ID);
            $tr.after($trDetails);

            var uriDetails = this.listtemplate.root + 'Documents/GetDetails';

            $.ajax({
                cache: false,
                url: encodeURI(uriDetails),
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
                        var $details = $trDetails.toggle().find(".details-container").html(data.Content);
                        var $respTable = $('table.responsive', $details);
                        if ($respTable.length) {
                            responsiveTables.updateTable($respTable);
                        }
                        $details.slideToggle(500);
                    }
                }
            });
            document.body.style.cursor = "wait";
        }
        return false;
    },

    addNew: function () {
        documentsList.edit(null, true);
    },

    edit: function (id, addNew) {
        var uriDetails = this.listtemplate.root + 'Documents/GetDocumentDetails';

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
                if (!data.Result) {
                    modalService.showAlert(data.Description, 'alert-error', "Error");
                }
                else {
                    var modalName = "Edit Document";
                    var $modal = modalService.createModal(modalName, { Cancel: true, Save: { callback: function () {
                        var $form = $('form', $modal);
                        var data = new FormData($form[0]);
                        $.ajax({
                            cache: false,
                            url: $form[0].action,
                            type: $form[0].method,
                            dataType: 'json',
                            data: data,
                            processData: false,
                            contentType: false,
                            timeout: 300000,
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
                                else {
                                    documentsList.listtemplate.refresh();
                                    $modal.modal('hide');
                                }
                            }
                        });
                        var $ajaxLoader = new ajaxLoader($modal);
                    }
                    }
                    });
                    $('.modal-body', $modal).html(data.Content);
                    $modal.on('fileRemoved', function (event, target) {
                        var $target = $(target);
                        $target.removeAttr('data-uploaded-file-name');
                        $target.closest('form').find($target.attr('data-uploaded-file-name-target')).val('');
                        $target.attr('data-custom-file-validation-required', 'required');
                    });
                    $modal.modal();
                }
            }
        });
        var $ajaxLoader = new ajaxLoader($('body'));
    },

    'delete': function (id, name) {
        var uriDelete = documentsList.listtemplate.root + 'Documents/Delete';
        var $modal = modalService.createModal("Delete Document", { Cancel: true, Delete: { callback: function () {
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
                        modalService.showAlert(data.Description, 'alert-error', 'Error');
                    }
                    else {
                        documentsList.listtemplate.refresh();
                    }
                }
            });
            var $ajaxLoader = new ajaxLoader($('body'));
        }
        }
        });
        $('.modal-body', $modal).html("Are you sure to delete document " + name + "?");
        $modal.modal();
    },

    deleteDocuments: function () {
        var documentsSelection = documentsList.listtemplate.$grid.fnGetSelectedIds();

        if (!documentsSelection.length) {
            modalService.showAlert('<h5>No documents selected, please select some</h5>', 'alert-info', 'Information');
            return;
        }

        var uriDelete = documentsList.listtemplate.root + 'Documents/DeleteDocuments';
        var $modal = modalService.createModal("Delete Document", { Cancel: true, Delete: { callback: function () {
            $.ajax({
                cache: false,
                url: encodeURI(uriDelete),
                type: 'post',
                dataType: 'json',
                data: { selectedDocuments: documentsSelection },
                traditional: true,
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
                        modalService.showAlert(data.Description, 'alert-error', 'Error');
                    }
                    else {
                        documentsSelection.fnClear();
                        documentsList.listtemplate.refresh();
                        alerts.show('Selected documents have been successfully deleted');
                    }
                }
            });
            var $ajaxLoader = new ajaxLoader($('body'));
        }
        }
        });
        $('.modal-body', $modal).html("Are you sure to delete all selected documents?");
        $modal.modal();
    },

    downloadDocuments: function () {
        var documentsSelection = documentsList.listtemplate.$grid.fnGetSelectedIds();

        if (!documentsSelection.length) {
            modalService.showAlert('<h5>No documents selected, please select some</h5>', 'alert-info', 'Information');
            return;
        }

        var uriDownload = documentsList.listtemplate.root + 'Documents/DownloadAsZip/?selectedDocuments=' + documentsSelection;
        window.open(uriDownload);
    },

    saveDocumentsSet: function () {
        var documentsSelection = documentsList.listtemplate.$grid.fnGetSelectedIds();

        if (!documentsSelection.length) {
            modalService.showAlert('<h5>No documents selected, please select some</h5>', 'alert-info', 'Information');
            return;
        }

        var uriDetails = documentsList.listtemplate.root + 'DocumentSets/GetDocumentSetDetails';

        $.ajax({
            cache: false,
            url: encodeURI(uriDetails),
            type: 'post',
            dataType: 'json',
            data: { selectedDocuments: documentsSelection },
            traditional: true,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (XMLHttpRequest.status === 401) {
                    location.reload();
                }
            },
            complete: function (data) {
                $ajaxLoader.remove();
            },
            success: function (data) {
                var modalName = "Add New Document Set";
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
                            else {
                                alerts.show("Documents set has been successfully saved");
                                $modal.modal('hide');
                            }
                        }
                    });
                    var $ajaxLoader = new ajaxLoader($modal);
                }
                }
                });

                $('.modal-body', $modal).html(data.Content);
                $modal.modal({ width: 800 });
            }
        });
        var $ajaxLoader = new ajaxLoader($('body'));
    },

    email: function () {
        var documentsSelection = documentsList.listtemplate.$grid.fnGetSelectedIds();

        if (!documentsSelection.length) {
            modalService.showAlert('<h4>No documents selected</h4>', 'alert-info', 'Information');
            return;
        }

        var uriPartners = documentsList.listtemplate.root + 'Partners/IndexPartial';
        $.ajax({
            cache: false,
            url: encodeURI(uriPartners),
            type: 'get',
            dataType: 'html',
            data: {},
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (XMLHttpRequest.status === 401) {
                    location.reload();
                }
            },
            complete: function (data) {
                $ajaxLoader.remove();
            },
            success: function (data) {
                var modalName = "Select Recipients";
                var $modal = modalService.createModal(modalName, { Cancel: true, Action: { name: "Select", callback: function () {
                    var $grid = $('table.dataTable', $modal);
                    var partnersSelection = partnersList.listtemplate.$grid.fnGetSelectedIds();

                    if (!partnersSelection.length) {
                        modalService.showAlert('<h4>No recipients selected</h4>', 'alert-info', 'Information');
                        return;
                    }

                    var $partnersModal = $modal;

                    var uriEmails = documentsList.listtemplate.root + 'Partners/Email';

                    $.ajax({
                        cache: false,
                        url: encodeURI(uriEmails),
                        type: 'post',
                        dataType: 'json',
                        data: { partners: partnersSelection, documents: documentsSelection },
                        traditional: true,
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
                                modalService.showAlert(data.Description, 'alert-error', 'Error');
                            }
                            else {
                                var modalName = "Send Documents";
                                var $modal = modalService.createModal(modalName, { Cancel: true, Save: { name: "Send", callback: function () {
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
                                            else {
                                                alerts.show('Documents have been successfully sent');
                                                $modal.modal('hide');
                                                $partnersModal.modal('hide');
                                            }
                                        }
                                    });
                                    var $ajaxLoader = new ajaxLoader($modal);
                                }
                                }
                                });

                                $('.modal-body', $modal).html(data.Content);
                                $modal.modal({ width: 800 });
                            }
                        }
                    });
                    var $ajaxLoader = new ajaxLoader($('body'));
                }
                }
                }, { fullWidth: true });

                $('.modal-body', $modal).html(data);
                $modal.modal();
            }
        });
        var $ajaxLoader = new ajaxLoader($('body'));
    },


    manageCategories: function () {
        documentsList.manageEntity('categories');
    },

    manageAreas: function () {
        documentsList.manageEntity('areas');
    },

    manageEntity: function (target) {
        var options = {};
        if (target === 'categories') {
            options.uri = documentsList.listtemplate.root + 'Categories/Index';
            options.title = 'Categories';
        }
        else if (target === 'areas') {
            options.uri = documentsList.listtemplate.root + 'Areas/Index';
            options.title = 'Areas';
        }
        else
            return;

        $.ajax({
            cache: false,
            url: encodeURI(options.uri),
            type: 'post',
            dataType: 'html',
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (XMLHttpRequest.status === 401) {
                    location.reload();
                }
            },
            complete: function (data) {
                $ajaxLoader.remove();
            },
            success: function (data) {
                var $modal = modalService.createModal(options.title, { Close: true }, { fullWidth: true });
                $('.modal-body', $modal).html(data);
                $modal.modal();
            }
        });
        var $ajaxLoader = new ajaxLoader($('body'));
    }

}