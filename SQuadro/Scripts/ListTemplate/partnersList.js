var partnersList = {
    listtemplate: null,

    getColumnsDefinition: function () {
        var actions = function (data, type, row) {
            var id = row.ID;
            var actions = [];
            if (!partnersList.listtemplate.readonly) {
                actions.push({ name: "Edit", click: "partnersList.edit('" + id + "')", html: '<i class="glyphicon glyphicon-edit"></i>' });
                actions.push({ name: "Delete", click: "partnersList.delete('" + id + "','" + row.Name + "')", html: '<i class="glyphicon glyphicon-remove"></i>' });
            }
            return dataTablesService.getInlineActions(actions);
        };
        var showContacts = function (data, type, row) {
            //var contacts = JSON.stringify(row.Contacts); debugger;
            var contacts = '';
            return '<a href="#" title="Show/Hide Contacts" onclick="return partnersList.showContacts(this, \'' + contacts + '\', \'' + row.ID + '\', \'' + row.FullName + '\')">' + row.FullName + '</a>';
        };
        var contacts = function (data, type, row) {
            var result = '';
            for (var i = 0; i < data.length; i++) {
                var item = '';
                if (data[i].ID == 1) { //phone
                    item += '<i class="glyphicon glyphicon-large glyphicon glyphicon-phone"></i>';
                    item += '<a href="tel:' + data[i].Name + '">' + data[i].Name + '</a>';
                }
                else if (data[i].ID == 3) { //email
                    item += '<i class="icon-large icon-envelope"></i>';
                    item += '<a href="mailto:' + data[i].Name + '">' + data[i].Name + '</a>';
                }
                else if (data[i].ID == 4) { //mobile
                    item += '<i class="icon-large icon-iphone"></i>';
                    item += '<a href="tel:' + data[i].Name + '">' + data[i].Name + '</a>';
                }
                else if (data[i].ID == 5) { //skype
                    item += '<i class="icon-large icon-skype"></i>';
                    item += '<a href="skype:' + data[i].Name + '">' + data[i].Name + '</a>';
                }
                else
                {
                    item += '<a href="#">' + data[i].Type + ':' + data[i].Name + '</a>';
                }                
                
                if (result != '') result += ' ';
                result += '<div class="nowrap">' + item + '</div>';   
            }
            return result;
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
            { sName: "Selector", bSortable: false, mData: null, mRender: function(data, type, row) { return '<div class="checkbox"><label><input type="checkbox" class="select-row ace"/><span class="lbl"></span></label></div>'; } },
            { sName: "ID", bVisible: false, mData: "ID" },
            { sTitle: "Name", sName: "Name", sClass: "text-left hidden-sm hidden-xs", mData: "Name" },
            { sTitle: "Full Name", sName: "FullName", sClass: "text-left", mData: "FullName", mRender: function (data, type, row) { return showContacts(data, type, row); } },
            { sTitle: "Contacts", bVisible: false, sClass: "text-left hidden-xs", mData: "Contacts" },
            { sTitle: "Category", sName: "Categories", sClass: "text-left hidden-sm hidden-xs", mData: "Categories", mRender: function (data, type, row) { return list(data, type, row) } },
            { sTitle: "Area", sName: "Areas", sClass: "text-left hidden-sm hidden-xs", mData: "Areas", mRender: function (data, type, row) { return list(data, type, row) } },
            { sTitle: "Country", sName: "Country", sClass: "text-left hidden-sm hidden-xs", mData: "Country" },
            { sTitle: "Actions", bSortable: false, sClass: "text-right", mData: null, mRender: function (data, type, row) { return actions(data, type, row) } }
        ]
    },

    showContacts: function(target, contacts, id, name) {
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
        return false;
    },

    addNew: function () {
        partnersList.edit(null, true);
    },

    edit: function (id, addNew) {
        var uriDetails = this.listtemplate.root + 'Partners/GetPartnerDetails';

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
                    var modalName = addNew ? "Add New Partner" : "Edit Partner";
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
                                    partnersList.listtemplate.refresh();
                                    $modal.modal('hide');
                                }
                            }
                        });
                        var $ajaxLoader = new ajaxLoader($modal);
                    }
                    }
                    }, { fullWidth: true });

                    $('.modal-body', $modal).html(data.Content);
                    $modal.modal();
                }
            }
        });
        var $ajaxLoader = new ajaxLoader($('body'));
    },

    'delete': function (id) {
        var uriDelete = partnersList.listtemplate.root + 'Partners/Delete';
        var $modal = modalService.createModal("Delete Partner", { Cancel: true, Delete: { callback: function () {
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
                        partnersList.listtemplate.refresh();
                    }
                }
            });
            var $ajaxLoader = new ajaxLoader($('body'));
        }
        }
        });
        $('.modal-body', $modal).html("<h5>Are you sure to delete the Partner?</h5>");
        $modal.modal();
    },

    email: function() {
        var partnersSelection = partnersList.listtemplate.$grid.fnGetSelectedIds();

        if (!partnersSelection.length) {
            modalService.showAlert('<h6>No partners selected, please select some</h6>', 'alert-info', 'Information');
            return;
        }

        var uriDocuments = partnersList.listtemplate.root + 'Documents/IndexPartial';
        $.ajax({
            cache: false,
            url: encodeURI(uriDocuments),
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
                var modalName = "Select Documents";
                var $modal = modalService.createModal(modalName, { Cancel: true, Action: { name: "Select", callback: function () {
                    var $grid = $('table.dataTable', $modal);
                    var documentsSelection = documentsList.listtemplate.$grid.fnGetSelectedIds();

                    if (!documentsSelection.length) {
                        modalService.showAlert('<h5>No documents selected, please select some</h5>', 'alert-info', 'Information');
                        return;
                    }

                    var $documentsModal = $modal;

                    var uriEmails = partnersList.listtemplate.root + 'Partners/Email';

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
                                modalService.showAlert(data.Description, 'alert-error', "Error");
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
                                                $documentsModal.modal('hide');
                                            }
                                        }
                                    });
                                    var $ajaxLoader = new ajaxLoader($modal);
                                }
                                }
                                });

                                $('.modal-body', $modal).html(data.Content);
                                $modal.modal({ width: 800    });
                            }
                        }
                    });
                    var $ajaxLoader = new ajaxLoader($('body'));
                }
                }
                },  { fullWidth: true } );

                $('.modal-body', $modal).html(data);
                $modal.modal();
            }
        });
        var $ajaxLoader = new ajaxLoader($('body'));
    }
}