$.validator.addMethod("isdatetimebefore", function (value, element, parameters) {
    var startDate = new Date(value);
    var endDate = new Date($("#EndDate").val());

    if (startDate > endDate) {
        return false;
    }

    return true;
});

$.validator.unobtrusive.adapters.addBool("isdatetimebefore");
