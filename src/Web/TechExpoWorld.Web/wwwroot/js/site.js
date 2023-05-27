// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$.validator.addMethod("isDateTimeBefore", () => {
    const startDate = new Date(document.getElementById('StartDate').value);
    const endDate = new Date(document.getElementById('EndDate').value);

    return endDate == 'Invalid Date' || startDate <= endDate;
});

$.validator.unobtrusive.adapters.addBool("isDateTimeBefore");
