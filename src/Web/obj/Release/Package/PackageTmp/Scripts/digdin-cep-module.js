var digdinCep = window.digdinCep = {
    init: function() {
        digdinCep.load.events();
        digdinCep.load.actions();
    },
    load: {
        events: function() {
            
        },
        actions: function() {
            digdin.addAction('loadCEP', digdinCep.actions.loadCEP);
        }
    },
    

    actions: {
        loadCEP: function(obj) {
            var data = $(obj).data();
            var target = data.dgdTarget;
            var source = data.dgdSource;
            var cep = obj.val().match(/\d+/gi).join('');

            $.ajax({
                url: source,
                data: {
                    cep: cep
                },
                success: function (resp) {
                    if (resp != null) {
                        $('[data-dgd-name=Street]', target).val(resp.Description);
                        $('[data-dgd-name=State]', target).val(resp.State).change();
                        $('[data-dgd-name=City]', target).val(resp.City);
                    }
                }
            });
        }
    }
}