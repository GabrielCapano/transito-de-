var UIJQueryUI = function () {

    
    var handleDatePickers = function () {
        
        $("#ui_date_picker").datepicker();

        $("#ui_date_picker_with_button_bar").datepicker({
          showButtonPanel: true
        });

        $("#ui_date_picker_inline").datepicker();

        $("#ui_date_picker_change_year_month" ).datepicker({
	      changeMonth: true,
	      changeYear: true
	    });

	    $("#ui_date_picker_multiple").datepicker({
	    	numberOfMonths: 2,
      		showButtonPanel: true
	    });

	    $( "#ui_date_picker_range_from" ).datepicker({
	      defaultDate: "+1w",
	      changeMonth: true,
	      numberOfMonths: 2,
	      onClose: function( selectedDate ) {
	        $( "#ui_date_picker_range_to" ).datepicker( "option", "minDate", selectedDate );
	      }
	    });
	    $( "#ui_date_picker_range_to" ).datepicker({
	      defaultDate: "+1w",
	      changeMonth: true,
	      numberOfMonths: 2,
	      onClose: function( selectedDate ) {
	        $( "#ui_date_picker_range_from" ).datepicker( "option", "maxDate", selectedDate );
	      }
	    });

	    $("#ui_date_picker_week_year" ).datepicker({
	      showWeek: true,
	      firstDay: 1
	    });

	    $("#ui_date_picker_trigger input").datepicker();
	    $("#ui_date_picker_trigger .add-on").click(function(){
	    	$("#ui_date_picker_trigger input").datepicker("show");
	    });
    }

    return {
        //main function to initiate the module
        init: function () {
            handleDatePickers();
        }

    };

}();