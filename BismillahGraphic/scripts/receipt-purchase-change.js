
//global purchaseCart data
var purchaseCart = [];

$(function () {
    $('.datepicker').pickadate().val(moment(new Date()).format('DD MMMM, YYYY'));

    //clear id if refresh page
    $("#SupplierId").val("");

    // Material Select Initialization
    $('.mdb-select').materialSelect();

    //input number only
    $('#table-row').on('input keypress', '.quantity, .unitPrice, .lineTotal', function (event) {
        if ((event.which !== 46 || $(this).val().indexOf('.') !== -1) &&
            (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });

    //input Paid, input Discount
    $('#inputPaid,#inputDiscount').on('input keypress', function (event) {
        if ((event.which !== 46 || $(this).val().indexOf('.') !== -1) &&
            (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });
});

//get measurement unit data
let measurementDropDownData = [];
function getMeasurementUnitName() {
    $.ajax({
        url: "/Selling/GetMeasurementUnitNames",
        success: function (response) {
            measurementDropDownData = response;
        },
        error: function (error) { console.log(error) }
    });
}

getMeasurementUnitName();


//create unit dropdown
function createUnitDropdown(selectedId) {
    let html = `<select required name="MeasurementUnitId" class="measurement-unit form-control"><option value="">[ Select ]</option>`
    measurementDropDownData.forEach(item => {
        if (item.value !== +selectedId)
            html += `<option value="${item.value}">${item.label}</option>`
        else
            html += `<option selected value="${item.value}">${item.label}</option>`
    });
    html += ` </select>`;

    return html;
}


//product autocomplete
$("#inputProduct").typeahead({
    minLength: 1,
    displayText: function (item) {
        return item.ProductName;
    },
    afterSelect: function (item) {
        this.$element[0].value = ''
    },
    source: function (request, result) {
        $.ajax({
            url: "/Selling/FindProduct",
            data: JSON.stringify({ 'prefix': request }),
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (response) { result(response) }
        });
    },
    updater: function (item) {
        item.SN = purchaseCart.length + 1
        purchaseCart.push(item);
        productTable();

        return item;
    }
});

//build table
function productTable() {
    const row = $("#table-row");
    var html = '';

    purchaseCart.forEach(function (data) {
        const lineTotal = (data.PurchaseQuantity * data.PurchaseUnitPrice).toFixed(2)

        html += `<tr>
                <td><strong class="SN">${data.SN}</strong></td>
                <td class="text-left">${data.ProductName}</td>
                <td><input type="number" step="0.01" min="0" class="quantity form-control" value="${data.PurchaseQuantity}" name="PurchaseQuantity" placeholder="Quantity" required></td>
                <td>${createUnitDropdown(data.MeasurementUnitId)}</td>
                <td><input type="number" step="0.01" class="unitPrice form-control" value="${data.PurchaseUnitPrice}" name="PurchaseUnitPrice" placeholder="Unit Price" required></td>
                <td><input type="number" step="0.01" class="lineTotal form-control" value="${lineTotal}" name="LineTotal" placeholder="Line Total" disabled/></td>
                <td><a style="color:#ff0000" class="delete fas fa-trash-alt" href="/Products/Delete/${data.ProductID}"></a></td>
            </tr>`;
    });

    row.empty();
    row.append(html);
};


//find table
var $tableBody = $('#table-row');

//delete click
$tableBody.on("click", ".delete", function (evt) {
    evt.preventDefault();
    const row = $(this).closest('tr');
    var serialNumber = row.find('.SN').text();

    purchaseCart = purchaseCart.filter(function (item) {
        return item.SN !== parseInt(serialNumber);
    });


    productTable();
    calculateTotal();
});


//input unit Price
$tableBody.on('input', '.unitPrice, .quantity', function () {
    const row = $(this).closest('tr');
    const quantity = row.find('.quantity');
    const unitPrice = row.find('.unitPrice');
    const lineTotal = row.find('.lineTotal');

    const total = (parseNumber(quantity.val()) * parseNumber(unitPrice.val()));
    lineTotal.val(total.toFixed(2));
});

//convert to float number
function parseNumber(n) {
    const f = parseFloat(n);
    return isNaN(f) ? 0 : f;
}

//update inputted data in store
$tableBody.on('change', 'input, select', function (e) {
    const row = $(this).closest('tr');
    const serialNumber = parseInt(row.find(".SN").text());
    const index = purchaseCart.findIndex((p => p.SN === serialNumber));

    row.find('input, select').each(function (i, element) {
        purchaseCart[index][element.name] = element.type === 'number' ? parseNumber(element.value) : element.value;
    });

   
    calculateTotal();
    validation();
});

//sum table value
function calculateTotal(isDiscount = false) {
    const total = purchaseCart.reduce(function (prev, cur) {
        return prev + (cur.PurchaseQuantity * cur.PurchaseUnitPrice);
    }, 0);

    const discount = parseNumber(inputDiscount.value);
    const paid = parseNumber(paidAmount.textContent);

    totalPrice.textContent = total.toFixed(2);
    totalPayable.textContent = (total - discount).toFixed(2);

    const payable = parseNumber(totalPayable.textContent);
    totalDue.textContent = (payable - paid).toFixed(2);

    error.textContent = ""

    if (isDiscount) return;

    inputDiscount.setAttribute('max', totalDue.textContent);
};

//discount change
$("#inputDiscount").on("change", function () {
    calculateTotal(true);
});



//reset discount/paid amount
function resetPayment() {
    $("#inputDiscount").val('').next('em').remove();
}

//input field validation
function validation() {
    const row = $('#table-row tr');
    var isValid = true;

    if (row.length > 0) {
        row.find('input[type="number"], select').each(function () {
            const input = $(this);

            if (!input.val() || input.val() === '0' || input.val() === 0) {
                input.addClass("has-error");
                isValid = false;
            } else {
                input.removeClass("has-error");
            }
        });
    }

    return isValid;
}

//compare Validation
function compareValidation(total, inputted) {
    var isValid = true;

    if (total < inputted) {
        isValid = false;
    }
    return isValid;
}



//Submit Sell change
formProduct.addEventListener('submit', function (evt) {
    evt.preventDefault()

    error.textContent = ""
    const payable = +totalPayable.textContent;
    const paid = +paidAmount.textContent;

    const data = {
        PurchaseID: +document.getElementById('hiddenPurchaseId').value,
        PurchaseTotalPrice: +totalPrice.textContent,
        PurchaseDiscountAmount: +inputDiscount.value,
        Description: inputDescription.value,
        PurchaseCarts: purchaseCart
    };

    const btn = formProduct.btnPurchase;

    if (payable >= paid) {
        if (purchaseCart.length) {
            $.ajax({
                url: "/Supplier/PurchaseReceiptChange",
                data: JSON.stringify({ model: data }),
                type: "POST",
                contentType: "application/json; charset=utf-8",
                beforeSend: function () { btn.setAttribute('disabled', true) },
                success: function (id) { window.location.href = `/Supplier/Receipt/${id}` },
                error: function (error) { console.log(error) },
                complete: function () { btn.removeAttribute('disabled') }
            });
        }
    }
    else {
        error.textContent = "Payable amount must be greater than or equal paid amount"
    }
});