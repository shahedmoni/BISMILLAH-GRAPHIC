var salesData = null;

$(function () {
    loadData();
    $(".mdb-select").materialSelect();
    $(".datepicker").pickadate()
    $("#inputPurchaseDate").val(moment(new Date()).format("DD MMMM, YYYY"));
});

$("#find").on("click", function () {
    loadData();
})

//selectors
const tBody = document.getElementById('t-row')
const inputTotalPaid = document.getElementById("inputPaid")
const table = document.getElementById("DataTable");
const dueContent = document.getElementById("due");
const formReceipt = document.getElementById("submitForm");

function loadData() {
    const from = $("#formDate").val();
    const to = $("#toDate").val();

    displayDates(from, to);

    const id = $("#Id").val();
    const url = "/Supplier/GetDetails";
    $.get(url, { id: id, fromDate: from, toDate: to }, function (data) {
        buildTable(data);
        summeryInfo(data);
        //assign to global
        salesData = data;
    });
}

function buildTable(data) {
    var html = "";
    const row = $("#t-row");

    if (row.children().length > 0)
        row.empty();

    $.each(data.Dues, function(i, item) {
        html += `<tr>
        <td>${moment(item.PurchaseDate).format("D MMM YYYY")}</td>
        <td>${item.PurchaseSN}</td>
        <td class="text-right">${item.PurchaseAmount}/-</td>
        <td class="text-right" style="max-width:100px"><input type="number" data-due="${item.PurchaseDueAmount}" class="form-control inputDiscount" min="0" max="${item.PurchaseDueAmount}" step="0.01" placeholder="Discount amount" value="${item.PurchaseDiscountAmount}" /></td>
        <td class="text-right">${item.PurchasePaidAmount.toFixed(2)}/-</td> 
        <td class="text-right dueAmount">${item.PurchaseDueAmount}</td>
        <td class="text-right paidAmount" data-id="${item.PurchaseID}">0</td></tr>`;
    });

    row.append(html);
}

function summeryInfo(data) {
    const info = data.SupplierInfo;

    $("#companyName").text(info.SupplierCompanyName);
    $("#addressPhone").text(`${info.SupplierAddress}, ${info.SupplierPhone}`);
    $("#totalDue").text(info.SupplierDue);

    $("#sales").text(`৳${data.DateToDateSale}`);
    $("#received").text(`৳${data.DateToDatePaid}`);
    $("#discount").text(`৳${data.DateToDateDiscount}`);
    $("#due").text(data.DateToDateDue);
}

function displayDates(from, to) {
    if (from || to) {
        const shoDate = combineDate(from, to);
        $("#showDate").text(shoDate);
    }
}

function combineDate(from, to) {
    var label = "";
    if (from && !to)
        label = `from ${from}`;

    if (to && !from)
        label = `upto ${to}`;

    if (from && to)
        label = `from (${from}) upto (${to})`;

    return label;
}

//show month name
function monthName(date) {
    const months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    return months[date.getMonth()];
}

//convert to float
function parseNumber(n) {
    const f = parseFloat(n);
    return isNaN(f) ? 0 : f;
}

//reset paid amount
function resetPaidAmount(updatedDue) {
    dueContent.textContent = updatedDue
    inputTotalPaid.setAttribute("max", updatedDue);
    inputTotalPaid.value = '';

    const rows = tBody.querySelectorAll("tr")
    rows.forEach(row => {
        row.cells[6].textContent = 0;
    })
}


//paid area
inputTotalPaid.addEventListener("input", function(e) {
    var totalDue = parseNumber(dueContent.textContent);
    var totalPaid = parseNumber(e.target.value);
    this.setAttribute("max", totalDue);

    for (var i = 1, row; (row = table.rows[i]); i++) {
        const due = row.cells[5].innerHTML;

        if (totalPaid < due && totalPaid > 0) {
            row.cells[6].innerHTML = totalPaid;
        } else if (totalPaid >= due) {
            row.cells[6].innerHTML = due;
        } else {
            row.cells[6].innerHTML = 0;
        }
        totalPaid -= due;
    }

    if (totalDue < totalPaid) {
        this.value = totalDue;
        return;
    }
})

//submit
formReceipt.addEventListener("submit", function(evt) {
    evt.preventDefault();

    const data = {
        SupplierID: salesData.SupplierInfo.SupplierID,
        PaidAmount: +document.querySelector("#inputPaid").value,
        Payment_Situation: document.querySelector("#selectPaymentMethod").value,
        Paid_Date: document.querySelector("#inputPurchaseDate").value,
        Description: document.querySelector("#inputDescription").value,
        Invoices: []
    };

    for (var i = 1, row; (row = table.rows[i]); i++) {
        const paid = +row.cells[6].innerHTML;
        const discount = +row.cells[3].querySelector(".inputDiscount").value;

        if (paid > 0 || discount > 0) {
            const invoice = {
                PurchaseID: row.cells[6].getAttribute("data-id"),
                PurchasePaidAmount: paid,
                PurchaseDiscountAmount: discount
            };

            data.Invoices.push(invoice);
        }
    }

    const url = "/Supplier/PostReceipt";
    $.post(url, { model: data }, function (response) {
        if (response) {
            location.href = `/Supplier/PaidReceipt/${response}`;
        }
    });
})

//on discount change
tBody.addEventListener('input', function (evt) {
    const input = evt.target
    const due = +input.getAttribute('data-due')
    const discount = +input.value

    if (input.classList.contains("inputDiscount")) {
        input.parentElement.parentElement.cells[5].textContent = (due - discount)

        //reset paid amount
        resetPaidAmount(salesData.DateToDateDue - discount)
    }
})