$.validator.addMethod("isDateTimeBefore", () => {
    let startDate = new Date(document.getElementById('StartDate').value);
    let endDate = new Date(document.getElementById('EndDate').value);
    let isBefore = false;

    startDate > endDate ? isBefore = false : isBefore = true;

    return isBefore;
});

$.validator.unobtrusive.adapters.addBool("isDateTimeBefore");
