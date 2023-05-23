const isDateTimeBefore = () => {
    $.validator.addMethod("isDateTimeBefore", () => {
        const startDate = new Date(document.getElementById('StartDate').value);
        const endDate = new Date(document.getElementById('EndDate').value);

        return endDate == 'Invalid Date' || startDate <= endDate;
    });

    $.validator.unobtrusive.adapters.addBool("isDateTimeBefore");
};

export default isDateTimeBefore;
