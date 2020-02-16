var salesData = null;

$(function () {
    loadData();
    $(".mdb-select").materialSelect();
    $(".datepicker").pickadate()
    $("#inputSellingDate").val(moment(new Date()).format("DD MMMM, YYYY"));
});

function loadData() {
    const from = $("#formDate").val();
    const to = $("#toDate").val();

    displayDates(from, to);

    const id = $("#Id").val();
    const url = "/Vendors/GetDetails";
    $.get(url, { id: id, fromDate: from, toDate: to }, function (data) {
        buildTable(data);
        summeryInfo(data);
        //assign to global
        salesData = data;
    });
};

function buildTable(data) {
    var html = "";
    const row = $("#t-row");

    if (row.children().length > 0)
        row.empty();

    $.each(data.Sales, function(i, item) {
        html += `<tr>
        <td>${moment(item.SellingDate).format("D MMM YYYY")}</td>
        <td>${item.SellingSN}</td>
        <td class="text-right">${item.SellingAmount}/-</td>
        <td class="text-right">${item.SellingDiscountAmount}/-</td>
        <td class="text-right">${item.SellingPaidAmount}/-</td> 
        <td class="text-right">${item.SellingDueAmount}</td>
        <td data-id="${item.SellingID}" class="text-right">0</td></tr>`;
    });

    row.append(html);
};

function summeryInfo(data) {
    const info = data.VendorInfo;

    $("#companyName").text(info.VendorCompanyName);
    $("#addressPhone").text(`${info.VendorAddress}, ${info.VendorPhone}`);
    $("#totalDue").text(info.VendorDue);

    $("#sales").text(`৳${data.DateToDateSale}`);
    $("#received").text(`৳${data.DateToDatePaid}`);
    $("#discount").text(`৳${data.DateToDateDiscount}`);
    $("#due").text(`৳${data.DateToDateDue}`);
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
};

function monthName(date) {
    const months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    return months[date.getMonth()];
}

$("#find").on("click", function () {
    loadData();
});

//paid area
document.querySelector("#inputPaid").addEventListener("input", updateValue);
const table = document.getElementById("DataTable");

function updateValue(e) {
    if (isNaN(e.target.value) || !e.target.value) return;

    const totalDue = salesData.DateToDateDue;
    var totalPaid = +e.target.value;

    this.setAttribute("max", totalDue);
    console.log(totalPaid);

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
}

//submit
document.getElementById("submitForm").addEventListener("submit", submitValue);

function submitValue(evt) {
    const data = {
        VendorID: salesData.VendorInfo.VendorID,
        PaidAmount: +document.querySelector("#inputPaid").value,
        Payment_Situation: document.querySelector("#selectPaymentMethod").value,
        Paid_Date: document.querySelector("#inputSellingDate").value,
        Invoices: []
    };

    for (var i = 1, row; (row = table.rows[i]); i++) {
        const paid = row.cells[6].innerHTML;

        if (paid > 0) {
            const invoice = {
                SellingID: row.cells[6].getAttribute("data-id"),
                SellingPaidAmount: paid
            };

            data.Invoices.push(invoice);
        }
    }

    const url = "/Vendors/PostReceipt";
    $.post(url, { model: data }, function(response) {
        if (response) {
            location.href = `/Vendors/PaidReceipt/${response}`;
        }
    });

    console.log(data)
    evt.preventDefault();
}