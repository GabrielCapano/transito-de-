var digdin = window.digdin = {
    formAjaxExecuting: false,


    init: function () {

        if (digdin.verifyDependencies()) {
            digdin.loadDateRegion();
            digdin.loadDatepicker();
            digdin.loadCheckToggle();
            digdin.loadMaskMoney();
            digdin.loadClear();
            digdin.loadMask();
            digdin.ajaxSetup();
            digdin.loadEvents();
            digdin.loadModal();
            //digdin.loadFormCache();
        }
    },
    loadEvents: function () {
        $(document).on('change', '[data-dgd-change]', digdin.eventHandle);
        $(document).on('click', '[data-dgd-click]', digdin.eventHandle);
        $(document).on('keydown', '[data-dgd-keydown]', digdin.eventHandle);
        $(document).on('keypress', '[data-dgd-keypress]', digdin.eventHandle);
        $(document).on('keyup', '[data-dgd-keyup]', digdin.eventHandle);
        $(document).on('dblclick', '[data-dgd-dblclick]', digdin.eventHandle);

        $('[data-dgd-change=change]').change();
        $('[data-dgd-click=click]').click();
        $('[data-dgd-keydown=keydown]').keydown();
        $('[data-dgd-keypress=keypress]').keypress();
        $('[data-dgd-keyup=keyup]').keyup();
    },

    eventHandle: function (e) {
        var data = $(this).data();
        var doNotAction = false;

        if (data.dgdKeypress != undefined && data.dgdKeypress != '' && e.keyCode != data.dgdKeypress) {
            doNotAction = true;
        }

        if (!doNotAction) {
            if (data.dgdCondition != undefined) {
                if (!eval(data.dgdCondition)) {
                    e.preventDefault();
                    eval(data.dgdConditionCallback);
                    return false;
                }
            }

            if (data.dgdAction == "parentFormSubmitAjax" || data.dgdPreventDefault != undefined)
                e.preventDefault();

            if (data.dgdConfirm != undefined && data.dgdConfirm != '') {

                $('#confirmbox').modal({
                    show: true,
                    backdrop: false,
                    keyboard: false,
                });

                $('#confirmMessage').html(data.dgdConfirm);

                $('#confirmFalse').click(function () {
                    $('#confirmbox').modal('hide');
                });

                var obj1 = $(this);

                $('#confirmTrue').click(function () {
                    $('#confirmbox').modal('hide');
                    if (data.dgdAction in digdin.actions)
                        digdin.actions[data.dgdAction](obj1);
                });

            }

            if (data.dgdConfirm == undefined) {
                if (!data.dgdSchedule)
                    if (data.dgdAction in digdin.actions)
                        digdin.actions[data.dgdAction]($(this));
                else {
                    clearTimeout($(this).attr('data-dgd-timeout-id'));
                    var obj = $(this);
                    var time = setTimeout(function () {
                        if (data.dgdAction in digdin.actions)
                            digdin.actions[data.dgdAction](obj);
                        if (data.dgdCallback != undefined && data.dgdCallback != '') {
                            eval(data.dgdCallback);
                        }
                    }, data.dgdSchedule);
                    $(this).attr('data-dgd-timeout-id', time);
                }
            }

            if (!data.dgdSchedule && data.dgdCallback != undefined && data.dgdCallback != '') {
                eval(data.dgdCallback);
            }
        }
    },

    actions: {
        filterList: function (obj) {
            var data = $(obj).data();
            var target = $(data.dgdTarget);
            var itens = $(data.dgdTargetParent, target);
            var value = $(obj).val().toLowerCase();
            itens.show();

            if (value != '') {
                itens.each(function () {
                    var toHide = true;
                    $('[data-dgd-filter]', $(this)).each(function () {
                        if ($(this).val().toLowerCase().indexOf(value) != -1) {
                            toHide = false;
                        }
                    });
                    if (toHide)
                        $(this).hide();
                });

            }

        },

        editInline: function (obj) {
            var data = $(obj).data();
            $(obj).removeAttr('readonly');
            $(obj).focus();
            $(obj).blur(function () {
                $(this).attr('readonly', 'readonly');
                $(this).unbind('change');
            });
        },

        addEditor: function (obj) {
            var data = $(obj).data();
            var target = $(data.dgdTarget);
            var source = $(data.dgdSource);
            var template = $(data.dgdTemplate);
            var count = $(data.dgdEditorParent, target).length;
           
            var objData = {};
            objData["count"] = count;

            source.find('[data-dgd-name]').each(function () {
                objData[$(this).attr('data-dgd-name')] = $(this).val();
            });

            var toAppend = $(digdin.addValueToTemplate(objData, template.html()));

            if (data.dgdAppend) {
                target.prepend(toAppend);
            } else {
                target.append(toAppend);
            }

            digdin.loadDatepicker(toAppend);
            digdin.loadMaskMoney(toAppend);
            digdin.loadClear(toAppend);
            digdin.loadMask(toAppend);
            toAppend.find('.chosen').chosen();
        },

        click: function (obj) {
            var data = $(obj).data();
            var target = $(data.dgdTarget);
            target.click();
        },

        ajaxCall: function (obj) {

            var data = $(obj).data();
            var ajaxData = {};
            if (data.dgdSourceData != null && data.dgdSourceData != undefined) {
                var func = eval(data.dgdSourceData);
                if (typeof func == "function") {
                    ajaxData = func(obj);
                } else
                    ajaxData = func;
            } else {
                ajaxData = data;
            }

            $.ajax({
                url: data.dgdSource,
                type: 'POST',
                data: ajaxData,
                dataType: 'json',
                complete: function () {

                },
                success: function (resp) {
                    if (data.dgdAlertCondition != undefined && data.dgdAlertCondition != '') {
                        if (eval(data.dgdAlertCondition))
                            digdin.callMessage(resp.Messages.join('</br>'), resp.Status, data.dgdAjaxCallback, resp, obj);
                    } else
                        digdin.callMessage(resp.Messages.join('</br>'), resp.Status, data.dgdAjaxCallback, resp, obj);
                }
            });
        },

        copyFormValues: function (obj) {
            var data = $(obj).data();
            var target = $(data.dgdTarget);
            var source = $(data.dgdSource);

            var sFields = source.find('[data-dgd-name]');
            sFields.each(function () {
                $('[data-dgd-name=' + $(this).attr('data-dgd-name') + ']', target).val($(this).val()).change();
            });
        },

        toggle: function (obj) {
            var data = $(obj).data();
            var target = $(data.dgdTarget);
            target.toggle();
        },

        append: function (obj) {
            var data = $(obj).data();
            var target = $(data.dgdTarget);
            var template = $(data.dgdTemplate).html();
            if (data.dgdSource != '' && data.dgdSource != undefined) {
                if (data.dgdSourceType == 'form') {
                    var obj = digdin.getJson($(data.dgdSource));
                    target.append(digdin.addValueToTemplate(obj, template));
                }
            }
            else
                target.append(template);
        },

        removeClosest: function (obj) {
            var data = $(obj).data();
            $(obj).closest(data.dgdTarget).remove();
        },

        parentFormSubmitAjax: function (obj) {
            if (!digdin.formAjaxExecuting) {
                digdin.formAjaxExecuting = true;
                var data = $(obj).data();
                var form = $(obj).closest('form');
                $.ajax({
                    url: form.attr('action'),
                    type: form.attr('method'),
                    data: form.serialize(),
                    complete: function () {
                        digdin.formAjaxExecuting = false;
                    },
                    success: function (resp) {
                        if (data.dgdAlertCondition != undefined && data.dgdAlertCondition != '') {
                            digdin.removeFormCache(form);
                            if (eval(data.dgdAlertCondition))
                                digdin.callMessage(resp.Messages.join('</br>'), resp.Status, data.dgdAjaxCallback, resp, obj);
                        } else
                            digdin.callMessage(resp.Messages.join('</br>'), resp.Status, data.dgdAjaxCallback, resp, obj);
                    }
                });
            }
        },

        fill: function (obj) {
            var data = obj.data();
            var target = $(data.dgdTarget);
            var template = $(data.dgdTemplate).html();
            target.html('');
            if (data.dgdUrl != undefined || data.dgdSource != undefined) {
                $.ajax({
                    url: data.dgdUrl != undefined ? data.dgdUrl : data.dgdSource,
                    type: 'post',
                    dataType: 'json',
                    data: {
                        id: obj.val()
                    },
                    success: function (resp) {
                        for (var e in resp) {
                            target.append(digdin.addValueToTemplate(resp[e], template));
                        }
                        if (data.dgdAjaxCallback != undefined && data.dgdAjaxCallback != '') {
                            var func = eval(data.dgdAjaxCallback);
                            func(resp);
                        }
                    }
                });
            }

        }
    },
    
    loadFormCache: function() {
        var url = window.location.href,
            forms = $('[data-dgd-action=parentFormSubmitAjax]').closest('form');

        forms.each(function() {
            var form = $(this);
            
            var inputs = $('input:text, input:checkbox, input:radio, textarea, select', form)
                .filter('[value!=]').filter('[value!=0]').not('select, [data-val-date]').length == 0;
            var cache = $.parseJSON(localStorage.getItem(form.attr('id') + url));
            if (cache != null && inputs) {
                for (var key in cache) {
                    var input = $('[name=' + key.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&") + ']'),
                        val = input.val();
                    if (val == '')
                        input.val(cache[key]);
                }
            }
        });

        window.onbeforeunload = digdin.saveFormCache;
    },
    
    removeFormCache: function(form) {
        var url = window.location.href;
        localStorage.setItem(form.attr('id') + url, null);
    },

    saveFormCache: function () {
        var url = window.location.href,
            forms = $('[data-dgd-action=parentFormSubmitAjax]').closest('form');

        forms.each(function() {
            var form = $(this);
            var inputs = $('input:text, input:checkbox, input:radio, textarea, select', form);
            var tmp = {};
            inputs.each(function() {
                var input = $(this);
                tmp[input.attr('name')] = input.val();  
            });

            localStorage.setItem(form.attr('id') + url, JSON.stringify(tmp));
        });
    },

    loadModal: function () {
        $("body").append($(digdin.templates.modal));
    },

    loadMaskMoney: function (obj) {
        if (obj != undefined) {

            $('[data-dgd-currency]', obj).maskMoney({
                symbol: 'R$ ',
                thousands: '.',
                decimal: ','
            });
        } else {

            $('[data-dgd-currency]').maskMoney({
                symbol: 'R$ ',
                thousands: '.',
                decimal: ','
            });
        }
    },

    loadCheckToggle: function () {
        $('[data-dgd-check-toggle]').each(function () {
            if ($(this).attr('data-dgd-checked') != undefined)
                $(this).find("input:checkbox").attr('checked', true);
            var data = $(this).data();
            $(this).toggleButtons({
                width: 60,
                label: {
                    enabled: data.dgdCheckOn,
                    disabled: data.dgdCheckOff
                }
            });
        });
    },

    loadDateRegion: function () {
        $.datepicker.regional['pt-BR'] = {
            closeText: 'Fechar',
            prevText: '&#x3c;Anterior',
            nextText: 'Pr&oacute;ximo&#x3e;',
            currentText: 'Hoje',
            monthNames: ['Janeiro', 'Fevereiro', 'Mar&ccedil;o', 'Abril', 'Maio', 'Junho',
            'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
            monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun',
            'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
            dayNames: ['Domingo', 'Segunda-feira', 'Ter&ccedil;a-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sabado'],
            dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'],
            dayNamesMin: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'],
            weekHeader: 'Sm',
            dateFormat: 'dd/mm/yy',
            firstDay: 0,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        };
        $.datepicker.setDefaults($.datepicker.regional['pt-BR']);
    },

    loadDatepicker: function (obj) {
        if (obj != undefined) {
            $('[data-dgd-datepicker]', obj).datepicker();
        } else {
            $('[data-dgd-datepicker]').each(function () {
                var data = $(this).data();
                var ob = $(this);
                ob.datepicker();
                var minDate;
                switch (data.dgdMinDateType) {
                    case 'date':
                        minDate = eval(data.dgdMinDate);
                        $(ob).datepicker('option', 'minDate', minDate);
                        break;
                    case 'selector':
                        $(data.dgdMinDate).change(function () {
                            var min = $.datepicker.parseDate('dd/mm/yy', $(this).val());
                            $(ob).datepicker('option', 'minDate', min);
                        });
                        break;
                }
            });
        }
    },

    loadClear: function () {
        $('[data-dgd-clear]').each(function () {
            var clear = $(this).attr('data-dgd-clear');
            if ($(this).val() == clear || clear == '')
                $(this).val('');
        });
    },

    loadMask: function (obj) {
        if (obj != undefined) {
            $('[data-dgd-mask]', obj).each(function () {
                $(this).mask($(this).attr('data-dgd-mask'));
            });
        } else {
            $('[data-dgd-mask]').each(function () {
                $(this).mask($(this).attr('data-dgd-mask'));
            });
        }
    },

    callMessage: function (message, status, callback, data, obj) {
        $.blockUI({
            baseZ: 2000,
            message: '<div style="background:' + (status ? "#1d943b" : "#bb2413") + ';color:white;padding:15px;line-height:20px;"><h4>' + message + '</h4></div>', css: { border: 'none' }
        });
        setTimeout(function () {
            $.unblockUI();
            if (callback != undefined && callback != '') {
                var func = eval(callback);
                func(data, obj);
            }
        }, 1000);
    },


    ajaxSetup: function () {
        setTimeout(function () {
            $(document).ajaxStart(function (a) {
                digdin.blockUi();
            }).ajaxComplete(function (a) {
                digdin.unblockUi();
            });

        }, 400);
    },

    unblockUi: function () {
        $('.block-ui-black').fadeOut(200, function () { $(this).remove(); });
    },

    blockUi: function () {
        var blockUi = $(digdin.templates.blockUi).hide();
        $('body').append(blockUi);
        blockUi.show();
    },

    addAction: function (name, func) {
        digdin.actions[name] = func;
    },

    addValueToTemplate: function (data, template) {
        if (typeof template === 'string') {
            for (var key in data) {
                if (typeof data[key] === "object" && data[key] != null && data[key] != undefined) {
                    for (var innerKey in data[key]) {
                        var innerPattern = new RegExp('{' + key + '.' + innerKey + '}', 'gi');
                        template = template.replace(innerPattern, data[key][innerKey]);
                    }
                } else {
                    if (data.hasOwnProperty(key)) {
                        var pattern = new RegExp('{' + key + ':.*?}|{' + key + '}', 'gi');
                        var match = template.match(pattern);
                        if (match != null) {
                            for (var i = 0; i < match.length; i++) {
                                var type = match[i].split(':')[1];
                                var val = data[key];
                                if (type != null) {
                                    type = type.replace('}', '');
                                    val = digdin.types[type](val);
                                }
                                var patt = new RegExp(match[i], 'gi');
                                template = template.replace(patt, val);

                            }
                        }
                    }

                }
            }
            return template;
        } else {
            return "";
        }
    },
    
    types: {
        currency: function (value) {
            var separadorMilhar = '.';
            var separadorDecimal = ',';

            var inteiros = parseInt(parseInt(value * (Math.pow(10, 2))) / parseFloat(Math.pow(10, 2)));
            var centavos = parseInt(parseInt(value * (Math.pow(10, 2))) % parseFloat(Math.pow(10, 2)));


            if (centavos % 10 == 0 && centavos + "".length < 2) {
                centavos = centavos + "0";
            } else if (centavos < 10) {
                centavos = "0" + centavos;
            }

            var milhares = parseInt(inteiros / 1000);
            inteiros = inteiros % 1000;

            var retorno = "";

            if (milhares > 0) {
                retorno = milhares + "" + separadorMilhar + "" + retorno;
                if (inteiros == 0) {
                    inteiros = "000";
                } else if (inteiros < 10) {
                    inteiros = "00" + inteiros;
                } else if (inteiros < 100) {
                    inteiros = "0" + inteiros;
                }
            }
            retorno += inteiros + "" + separadorDecimal + "" + centavos;
            

            var tmp = value + '';
            tmp = tmp.replace(/(\.[0-9]{2})$/g, ",$1");
            if (tmp.length > 6)
                tmp = tmp.replace(/([0-9]{3}),([0-9]{2}$)/g, ".$1,$2");

            return 'R$ ' + retorno;
        }  
    },
        

    getJson: function (obj) {
        var fmkName = '[data-dgd-name]';
        var fields = obj.find(fmkName);
        var ret = {};
        fields.each(function () {
            ret[$(this).attr(fmkName.replace('[', '').replace(']', ''))] = $(this).val();
        });

        return ret;
    },

    verifyDependencies: function () {

        if (jQuery == undefined) {
            console.error('jQuery library is not loaded, please include it in file or check for file\'s order');
            return false;
        }

        if ($.datepicker == undefined) {
            console.error('jQuery UI datepicker library is not loaded, please include it in file or check for file\'s order');
            return false;
        }

        if ($.mask == undefined) {
            console.error('jQuery MaskedInput plugin is not loaded, please include it in file or check for file\'s order');
            return false;
        }

        return true;

    },

    templates: {
        blockUi:
            '<div class="block-ui-black" ' +
            'style="position:fixed; top:0; left:0; width: 100%; ' +
            'height:100%; background-color: #000; opacity:0.8;"></div>',

        modal:
        '<div class="modal" id="confirmbox" style="display: none">' +
         '<div class="modal-header">' +
            '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
            '<h4 class="modal-title">Aviso</h4>' +
        '</div>' +
        '<div class="modal-body">' +
            '<p id="confirmMessage">Any confirmation message?</p>' +
        '</div>' +
        '<div class="modal-footer">' +
            '<button class="btn" id="confirmFalse">Cancelar</button>' +
            '<button class="btn btn-primary" id="confirmTrue">Confirmar</button>' +
        '</div>' +
        '</div>'
    }
};