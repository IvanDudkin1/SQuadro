if (typeof String.prototype.fulltrim != 'function') {
    String.prototype.fulltrim = function () {
        return this.replace(/(?:(?:^|\n)\s+|\s+(?:$|\n))/g, '').replace(/\s+/g, ' ');
    }
}

if (typeof String.prototype.right != 'function') {
    String.prototype.right = function (count) {
        return this.substring(this.length - count);
    }
}

if (typeof String.prototype.left != 'function') {
    String.prototype.left = function (count) {
        return this.substring(0, count);
    }
}

if (typeof String.prototype.startsWith != 'function') {
    String.prototype.startsWith = function (str) {
        return this.slice(0, str.length) == str;
    };
}

if (typeof String.prototype.isNullOrWhitespace != 'function') {
    String.prototype.isNullOrWhitespace = function (input) {
        if (input == null) return true;
        return input.replace(/\s/g, '').length < 1;
    }
}

// format yyyymmdd
Date.prototype.datetostring = function () {
    return String(this.getFullYear()) +
        ('0' + String(this.getMonth() + 1)).right(2) +
        ('0' + String(this.getDate())).right(2);
};

jQuery.fn.reverse = [].reverse;

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

(function ($) {
    $.fn.outerHTML = function () {
        return $(this).clone().wrap('<div></div>').parent().html();
    }
})(jQuery);

if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (searchElement) {
        "use strict";

        if (this === void 0 || this === null) throw new TypeError();

        var t = Object(this);
        var len = t.length >>> 0;
        if (len === 0) return -1;

        var n = 0;
        if (arguments.length > 0) {
            n = Number(arguments[1]);
            if (n !== n) // shortcut for verifying if it's NaN
                n = 0;
            else if (n !== 0 && n !== (1 / 0) && n !== -(1 / 0)) n = (n > 0 || -1) * Math.floor(Math.abs(n));
        }

        if (n >= len) return -1;

        var k = n >= 0 ? n : Math.max(len - Math.abs(n), 0);

        for (; k < len; k++) {
            if (k in t && t[k] === searchElement) return k;
        }
        return -1;
    };
}

var service = {
    viewport: {
        x: window.innerWidth || document.documentElement.clientWidth || document.getElementsByTagName('body')[0].clientWidth,
        y: window.innerHeight || document.documentElement.clientHeight || document.getElementsByTagName('body')[0].clientHeight
    },

    loadCssFile: function (filename) {
        var fileref = document.createElement("link");
        fileref.setAttribute("rel", "stylesheet");
        fileref.setAttribute("type", "text/css");
        fileref.setAttribute("href", filename);
        document.getElementsByTagName("head")[0].appendChild(fileref);
    },

    loadJsFile: function (filename) {
        var fileref = document.createElement("script");
        fileref.setAttribute("type", "text/javascript");
        fileref.setAttribute("src", filename);
        document.getElementsByTagName("head")[0].appendChild(fileref);
    },

    resizeTextArea: function (textArea) {
        textArea.style.height = 'auto';
        textArea.style.height = textArea.scrollHeight + 20 + 'px';
    },

    applySelect2: function ($container) {
        $('.select2', $container).each(function () {
            $this = $(this);

            var options = {};

            if ($this.attr('data-width'))
                options.width = $this.attr('data-width');
            else
                options.width = "100%";

            if ($this.attr('placeholder'))
                options.placeholder = $this.attr('placeholder');

            if ($this.is(".multi-choice"))
                options.multiple = true;

            if ($this.is(".allow-clear"))
                options.allowClear = true;

            if ($this.is(".ajax-select")) {
                options.ajax = {
                    url: $this.attr('data-feed'),
                    type: 'post',
                    dataType: 'json',
                    data: function (term, page) {
                        return {
                            term: term // search term
                        };
                    },
                    results: function (data, page) {
                        return { results: data };
                    }
                };

                options.initSelection = function (element, callback) {
                    var selection = element.val();
                    $.post(element.attr('data-init'), { selection: selection }, function (data) {
                        callback(data);
                    });
                };
            }

            if ($this.is(".add-new")) {
                options.createSearchChoice = function (term, data) {
                    if ($(data).filter(function () {
                        return this.text.localeCompare(term) === 0;
                    }).length === 0)
                        return { id: '@@_@@', text: term };
                }

                $this.on("select2-selecting", function (e) {
                    if (e.object.id === '@@_@@') {
                        $element = $(e.currentTarget);
                        var uriAddNew = $element.attr('data-add');
                        $.ajax({
                            cache: false,
                            url: encodeURI(uriAddNew),
                            type: 'post',
                            dataType: 'json',
                            data: { text: e.object.text },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                if (XMLHttpRequest.status === 401) {
                                    location.reload();
                                }
                            },
                            complete: function (data) {
                            },
                            success: function (data) {
                                if (!data.Result) {
                                    modalService.showAlert(data.Description, 'alert-error', 'Error');
                                }
                                else {
                                    var selectedItems = $element.select2("val");
                                    if (options.multiple) {
                                        var index = selectedItems.indexOf('@@_@@');
                                        if (index > -1) {
                                            selectedItems.splice(index, 1);
                                        }
                                        selectedItems.push(data.ID);
                                    }
                                    else
                                        selectedItems = data.ID;
                                    $element.select2("val", selectedItems);
                                }
                            }
                        });
                    }
                });
            }

            $this.select2(options);
        });
    }
}

var dataTablesService = {
    getInlineActions: function (actions) {
        var result = '';
        if (actions && actions.length) {
            var phoneLinks = '';
            result += '<div class="nowrap">';
            for (var i = 0; i < actions.length; i++) {
                result += (actions[i].type && actions[i].type == 'link' ? '<a' : '<button');
                result += ' class="btn btn-sm btn-info inline-action ' + (actions[i].class ? ' ' + actions[i].class : '') + '"';
                result += (actions[i].href ? ' href="' + actions[i].href + '"' : '');
                result += (actions[i].target ? ' target="' + actions[i].target + '"' : '');
                result += (actions[i].click ? ' onclick="' + actions[i].click + '"' : '');
                result += ' data-toggle="tooltip" title="' + actions[i].name + '">';
                result += actions[i].html;
                result += (actions[i].type && actions[i].type == 'link' ? '</a>' : '</button>');
            }
            result += '</div>';
        }
        return result;
    }
}

var modalService = {
    createModal: function (header, actionButtons, options) {

        var classModal = 'modal';
        var style = '';
        if (options) {
            if (options.fullWidth) style = 'width:100%';
            if (options.width) style = 'width:' + options.width;
        }
        if (style)
            style = ' style="' + style + '"';

        var content = "<div id='tmpModal' class='" + classModal + "' tabindex='-1' role='dialog' aria-labelledby='tmpModalLabel' aria-hidden='true' data-backdrop='static'>";
        content += "<div class='modal-dialog'" + style + ">";
        content += "<div class='modal-content'>";
        content += "<div class='modal-header'>";
        content += "<button type='button' class='close pull-right' data-dismiss='modal' aria-hidden='true'>&times;</button>";
        if (header) content += "<h4>" + header + "</h4>";
        content += "</div>";
        content += "<div class='modal-body'>";
        content += "</div>";
        if (actionButtons) {
            content += "<div class='modal-footer'>";
            if (actionButtons.Cancel)
                content += "<button class='btn' data-dismiss='modal' aria-hidden='true'>" + 'Cancel' + "</button>";
            if (actionButtons.Close)
                content += "<button class='btn btn-primary' data-dismiss='modal' aria-hidden='true'>" + 'Close' + "</button>";
            if (actionButtons.OK)
                content += "<button id='btnOK' class='btn btn-primary'>" + (actionButtons.OK.name ? actionButtons.OK.name : 'OK') + "</button>";
            if (actionButtons.Save)
                content += "<button id='btnSave' class='btn btn-primary'>" + (actionButtons.Save.name ? actionButtons.Save.name : 'Save') + "</button>";
            if (actionButtons.Action)
                content += "<button id='btnAction' class='btn btn-lg btn-primary'>" + (actionButtons.Action.name ? actionButtons.Action.name : "Action") + "</button>";
            if (actionButtons.Delete)
                content += "<button id='btnDelete' class='btn btn-danger'>" + 'Delete' + "</button>";
            content += "</div>";
        }
        content += "</div>"; //.modal-content
        content += "</div>"; //.modal-dialog
        content += "</div>"; //.modal

        var $return = $(content);

        if (actionButtons.Save && actionButtons.Save.callback) {
            $return.on('submit', 'form', function (event) {
                event.preventDefault();
                if ($(this).valid())
                    actionButtons.Save.callback();
                return false;
            });

            $("#btnSave", $return).click(function () {
                $('form', $return).submit();
            });
        }
        if (actionButtons.Action && actionButtons.Action.callback)
            $("#btnAction", $return).click(function () {
                actionButtons.Action.callback();
            });
        if (actionButtons.Delete && actionButtons.Delete.callback)
            $("#btnDelete", $return).click(function () {
                actionButtons.Delete.callback();
            });

        function format($target) {
            if (!$target)
                $target = $return;
            else
                $target = $target.jquery ? $target : $($target);

            $.validator.unobtrusive.parse($('form', $return));
            $return.find("[autofocus]:first").focus();
            $return.draggable({ handle: '.modal-header' });
            if (typeof responsiveTables != 'undefined')
                responsiveTables.updateTable($('table', $return));

            $('.date-picker', $return).datepicker({ autoclose: true }).next().on('click', function () {
                $(this).prev().focus();
            });

            //$('.time-picker', $return).mask('99:99').timepicker({
            //    minuteStep: 1,
            //    showSeconds: false,
            //    showMeridian: false
            //}).on('blur', function () {
            //    $(this).timepicker('hideWidget')
            //}).next().on('click', function () {
            //    $(this).prev().timepicker('showWidget');
            //});

            //setTimeout(function () { $('.wysihtml5', $return).wysihtml5(); }, 1000);

            $('.file-input', $return).each(function () {
                var $this = $(this);
                var no_file = $this.attr('data-no-file') ? $this.attr('data-no-file') : 'No File ...';
                $this.ace_file_input({
                    no_file: no_file,
                    btn_choose: 'Choose',
                    btn_change: 'Change',
                    droppable: false,
                    thumbnail: false, //| true | large
                    after_remove: function () {
                        $return.trigger('fileRemoved', this);
                        var $form = $(this).closest('form');
                        if ($form.length)
                            $form.validate().element($(this));
                    }
                }).on('change', function (event, data) {
                    $this = $(this);
                    var no_file = $this.attr('data-no-file') ? $this.attr('data-no-file') : 'No File ...';
                    $aceFileInput = $this.closest('.ace-file-input');
                    $aceFileLabel = $aceFileInput.find('.file-label');
                    $aceFileName = $aceFileLabel.find('.file-name');
                    var files = this.files;
                    if (files.length > 1)
                        $aceFileName.attr('data-title', '{0} files selected'.format(files.length));
                    else if (files.length == 1)
                        $aceFileName.attr('data-title', files[0].name);
                    else if ($this.attr('data-uploaded-file-name'))
                        $aceFileName.attr('data-title', $this.attr('data-uploaded-file-name'));
                    else
                        $aceFileName.attr('data-title', no_file);

                    if (files.length || $this.attr('data-uploaded-file-name')) {
                        $aceFileLabel.addClass('selected');
                        $aceFileLabel.attr('data-title', 'Change');
                    }
                    else {
                        $aceFileLabel.removeClass('selected');
                        $aceFileLabel.attr('data-title', 'Choose');
                    }

                    if (data && data.skipValidation)
                        return;

                    var $form = $(this).closest('form');
                    if ($form.length)
                        $form.validate().element($(this));
                }).trigger('change', { skipValidation: true });
            });

            service.applySelect2($return);

            //$('textarea[class*=autosize]', $target).autosize({ append: "\n" });

        }

        $return.on('contentchanged', function (event, $target) {
            format($target);
        });

        $return.on('shown.bs.modal', function () {
            $('body').addClass('modal-open');
            format();
        });

        $return.on('hidden.bs.modal', function () {
            $('body').removeClass('modal-open');
            $(this).remove();
        });

        return $return;
    },

    showAlert: function (text, alertClass, header) {
        var $modal = modalService.createModal(header, { Close: true });
        $(".modal-body", $modal).html(text);
        $modal.addClass(alertClass).modal();
    }
}

var alerts = {
    show: function (message) {
        if (typeof message === 'string') {
            message = [message];
        }
        if (message) {
            for (var i = 0; i < message.length; i++) {
                $.bootstrapGrowl(message[i], {
                    align: 'center',
                    width: 'auto',
                    type: 'success'
                });
            }
        }
    }
}

function ajaxLoader(el, options) {
    // Becomes this.options
    var defaults = {
        bgColor: '#fff',
        duration: 800,
        opacity: 0.7,
        classOveride: false
    };
    this.options = jQuery.extend(defaults, options);
    if (el)
        this.container = $(el);
    else
        this.container = $('body');

    this.init = function () {
        var container = this.container;
        // Delete any other loaders
        this.remove();
        // Create the overlay
        var width;
        var height;
        var position;
        if (container.is('body')) {
            width = '100%';
            height = '100%';
            position = 'fixed';
        }
        else {
            width = container.width();
            height = container.height();
            position = 'absolute';
        }
        var overlay = $('<div class="widget-box-overlay"><i class="icon-spinner icon-spin icon-2x white"></i></div>').css({
            'background-color': this.options.bgColor,
            'opacity': this.options.opacity,
            /*'width': width,
            'height': height,*/
            'position': position,
            'top': '0px',
            'left': '0px',
            'bottom': '0px',
            'right': '0px',
            'z-index': 99999
        }).addClass('ajax_overlay');
        // add an overiding class name to set new loader style 
        if (this.options.classOveride) {
            overlay.addClass(this.options.classOveride);
        }
        // insert overlay and loader into DOM 
        container.append(
			overlay.append(
				$('<div></div>').addClass('ajax_loader')
			).fadeIn(this.options.duration)
		);
    };

    this.remove = function () {
        var overlay = this.container.children(".ajax_overlay");
        if (overlay.length) {
            overlay.fadeOut(this.options.classOveride, function () {
                overlay.remove();
            });
        }
    }

    this.init();
}

jQuery.validator.addMethod("custom_file_validation", function (fileName, element) {
    fileName = fileName.replace(/^.*[\\\/]/, '');
    $element = $(element);
    var expr = $element.attr("data-custom-file-validation-expression");
    var validformat = $element.attr("data-custom-file-validation-validformat");
    var required = $element.attr("data-custom-file-validation-required");

    if (required && !fileName) {
        $.validator.messages.custom_file_validation = "File is required.";
        return false;
    }

    if (expr && fileName) {
        var patt = new RegExp(expr, 'i');
        if (!patt.test(fileName)) {
            $.validator.messages.custom_file_validation = resources.val_FileNameIsInvalid.format(validformat);
            return false;
        }
    }

    return true;
}, "File name is invalid.");	