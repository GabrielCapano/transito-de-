var digdinList = window.digdinList = {
    init: function () {
        digdinList.addActions();
        digdinList.loadLists();
        digdinList.loadEvents();
    },
    
    loadEvents: function () {
        
    },
    
    addActions: function () {
        digdin.addAction('nextPage', digdinList.actions.nextPage);
        digdin.addAction('goToPage', digdinList.actions.goToPage);
        digdin.addAction('prevPage', digdinList.actions.prevPage);
        digdin.addAction('filterChange', digdinList.actions.filterChange);
        digdin.addAction('dataList', digdinList.actions.list); 
    },
    
    loadLists: function() {
        $('[data-dgd-data-list]').each(function () {
            var data = $(this).data();
            digdinList.lists[data.dgdDataListId] = [];
            digdinList.actions.list($(this));
        });
    },

    lists: {
        
    },

    actions: {
        filterChange: function(obj) {
            var data = obj.data();
            var holder = $(data.dgdDataListHolder);
            var dataList = holder.find('[data-dgd-data-list]');
            var form = $(dataList.data().dgdDataListForm);
            form.find('[name=' + obj.attr('name') + ']').val(obj.val());
            digdinList.actions.list(dataList);
        },

        goToPage: function(obj) {
            var data = obj.data();
            var holder = $(data.dgdDataListHolder);
            var dataList = holder.find('[data-dgd-data-list]');
            var form = $(dataList.data().dgdDataListForm);
            var page = form.find('[data-dgd-data-list-page]');
            page.val(+data.dgdDataListToPage);
            digdinList.actions.list(dataList);
        },
        
        nextPage: function (obj) {
            var data = obj.data();
            var holder = $(data.dgdDataListHolder);
            var dataList = holder.find('[data-dgd-data-list]');
            var form = $(dataList.data().dgdDataListForm);
            var pagesCount = form.find('[data-dgd-data-list-pagescount]');
            var page = form.find('[data-dgd-data-list-page]');
            var registerPerPage = form.find('[data-dgd-data-list-registerperpage]');

            if (+page.val() + 1 < pagesCount.val()) {
                page.val(+page.val() + 1);
                digdinList.actions.list(dataList);
            }
        },
        
        prevPage: function (obj) {
            var data = obj.data();
            var holder = $(data.dgdDataListHolder);
            var dataList = holder.find('[data-dgd-data-list]');
            var form = $(dataList.data().dgdDataListForm);
            var page = form.find('[data-dgd-data-list-page]');

            if (!(+page.val() - 1 < 0)) {
                page.val(+page.val() - 1);
                digdinList.actions.list(dataList);
            }
        },
        
        createPagination: function(obj, paginationResponse) {
            var data = obj.data();
            var holder = $(data.dgdDataListHolder);
            var paginationHolder = $('[data-dgd-data-list-pagination-holder]', holder);
            var pageData = paginationHolder.data();
            var paginationTemplate = $(pageData.dgdTemplate).html();
            var pagesCount = holder.find('[data-dgd-data-list-pagescount]');
            var registerCount = holder.find('[data-dgd-data-list-registercount]');
            var page = holder.find('[data-dgd-data-list-page]');
            pagesCount.val(paginationResponse.PagesCount).not('[type=hidden]').html(paginationResponse.PagesCount);
            registerCount.val(paginationResponse.RegisterCount).not('[type=hidden]').html(paginationResponse.RegisterCount);
            page.val(paginationResponse.Page).not('[type=hidden]').html(paginationResponse.Page);
            paginationHolder.html('');
            var highPage = +page.val() + 5;
            highPage = (highPage > paginationResponse.PagesCount ? paginationResponse.PagesCount : highPage);
            var lowPage = +page.val() - 5;
            lowPage = (lowPage < 0 ? 0 : lowPage);
            for (var i = lowPage; i < highPage; i++) {
                var dt = {
                    page: i,
                    pageLabel: i+1
                };
                paginationHolder.append(digdin.addValueToTemplate(dt, paginationTemplate));
            }
        },
        
        
        list: function (obj) {
            var data = obj.data();
            if (data.dgdDataListTarget != undefined) {
                obj = $(data.dgdDataListTarget);
                data = obj.data();
            }
            
            var template = $(data.dgdTemplate).html();
            var source =data.dgdSource;
            var sourceType =data.dgdSourceType;
            var target = $(data.dgdTarget);
            var form = $(data.dgdDataListForm);
            
            //Para implementação futura de cache local (id do registro + data de modificação)
            var idAttr =data.dgdDataListIdattr;
            var dateAttr =data.dgdDataListDateattr;
            
            var paginationReponse =data.dgdDataListPaginationObject;


            var objectBase = data.dgdDataListBaseObject;
            

            var row =
                '<tr>' +
                    '<td style="text-align:center;" colspan="' + target.closest('table').find('th').length + '">' +
                        '<i>' +
                            'Consultando o servidor, aguarde, isso não costuma demorar.' +
                        '</i>' +
                    '</td>' +
                '</tr>';
            target.html(row);

            if (sourceType == 'url') {
                $.ajax({
                    url: source,
                    dataType: 'json',
                    type: 'post',
                    data: form.serialize(),
                    success: function (resp) {
                        target.html('');
                        var objs = resp;
                        if (objectBase != '' && objectBase != undefined)
                            objs = resp[objectBase];


                        if (objs.length == 0) {
                            var row = $('<tr>' +
                                '<td style="text-align:center;" colspan="' + target.closest('table').find('th').length + '"><i>' +
                                'Não foram encontrados registros' +
                                '</i></td>' +
                                '</tr>');
                            target.append(row);
                        } else {
                            var out = [];

                            for (var i = 0; i < objs.length; i++) {
                                var reg = objs[i];
                                out.push($(digdin.addValueToTemplate(reg, template)).data(reg));
                            }

                            target.append(out);
                        }
                        
                        
                        

                        if (paginationReponse)
                            digdinList.actions.createPagination(obj, resp[paginationReponse]);
                        
                        if (data.dgdAjaxCallback) {
                            var func = eval(data.dgdAjaxCallback);
                            if(typeof func == "function")
                                func();
                        }
                    }
                });
            }
        }
    }
};