var digdinUpload = window.digdinUpload = {
    init: function() {

    },

    uploadCount: 0,
    formResp: null,
    
    upload: function(resp) {
        var uploads = $('[data-dgd-upload]');
        digdinUpload.formResp = resp;
        uploads.each(function () {
            var obj = $(this);
            var data = obj.data();
            var func = eval(data.dgdUploadPrimarykey);
            var id = func(resp);
            var iframe = obj.find('iframe');
            var form = $(iframe[0].contentWindow.document).find('form');

            if (form.find('input:file').val() == '') {

            } else {
                digdinUpload.uploadCount = digdinUpload.uploadCount + 1;
                iframe.load(digdinUpload.uploadCallbacking);

                form.find('#ReferenceId').val(id);
                form.submit();
            }

        });
    },
    
    uploadCallbacking: function () {
        var obj = $(this);
        var resp = $.parseJSON($(obj[0].contentWindow.document).find('body pre').html());
        if (resp.Status) {
            digdinUpload.uploadCount = digdinUpload.uploadCount - 1;
        }
        
        if (digdinUpload.uploadCount == 0)
            eval(obj.parent().data().dgdUploadCallback)(digdinUpload.formResp);

        $(this).off('load');
    }
}