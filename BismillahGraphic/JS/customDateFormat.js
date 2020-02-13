
function startEndOfMonth(from, to) {
    const startOfMonth = moment().startOf("month").format("DD MMMM, YYYY");
    const endOfMonth = moment().endOf("month").format("DD MMMM, YYYY");

    document.querySelector(from).value = startOfMonth;
    document.querySelector(to).value = endOfMonth;
}