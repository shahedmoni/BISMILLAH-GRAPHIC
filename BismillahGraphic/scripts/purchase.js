
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

    //load table data
    loadPurchaseCartData();
});

//get measurement unit data
let measurementDropDownData = [];
function getMeasurementUnitName() {
    $.ajax({
        url: "/Selling/GetMeasurementUnitNames",
        success: function (response) {
            measurementDropDownData = response;

            //load table data
            loadPurchaseCartData();
        },
        error: function (error) { console.log(error) }
    });
}

getMeasurementUnitName();


//create unit dropdown
function createUnitDropdown(selectedId) {
    let html = `<select name="MeasurementUnitId" class="measurement-unit form-control"><option value="">[ Select ]</option>`
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
        saveProductList(item);
        loadPurchaseCartData();
        return item;
    }
});

//build table
function productTable(products) {
    const row = $("#table-row");
    var html = '';

    products.forEach(function (data) {
        html += `<tr>
                <td><strong class="SN">${data.SN}</strong></td>
                <td class="text-left">${data.ProductName}</td>
                <td><input type="number" step="0.01" class="quantity form-control" value="${data.PurchaseQuantity}" name="PurchaseQuantity" placeholder="Quantity"/></td>
                <td>${createUnitDropdown(data.MeasurementUnitId)}</td>
                <td><input type="number" step="0.01" class="unitPrice form-control" value="${data.PurchaseUnitPrice}" name="PurchaseUnitPrice" placeholder="Unit Price"/></td>
                <td><input type="number" step="0.01" class="lineTotal form-control" value="${data.LineTotal}" name="LineTotal" placeholder="Line Total" disabled/></td>
                <td><a style="color:#ff0000" class="delete fas fa-trash-alt" href="/Products/Delete/${data.ProductID}"></a></td>
            </tr>`;
    });

    row.empty();
    row.append(html);
};

//save to purchaseCart
function saveProductList(product) {
    product.SN = (purchaseCart.length) + 1;
    product.PurchaseQuantity = 0;
    product.PurchaseUnitPrice = product.ProductPrice;
    product.LineTotal = 0;
    product.Details = '';

    purchaseCart.push(product);
    localStorage.setItem('purchaseCart', JSON.stringify(purchaseCart));
};

//load purchaseCart Data
function loadPurchaseCartData() {
    if (localStorage.getItem('purchaseCart')) {
        purchaseCart = JSON.parse(localStorage.getItem('purchaseCart'));
    };

    productTable(purchaseCart);
    calculateTotal();
};

//find table
var $tableBody = $('#table-row');

//delete click
$tableBody.on("click", ".delete", function (evt) {
    evt.preventDefault();
    const row = $(this).closest('tr');
    var serialNumber = row.find('.SN').text();

    const filteredItems = purchaseCart.filter(function (item) {
        return item.SN !== parseInt(serialNumber);
    });


    localStorage.setItem('purchaseCart', JSON.stringify(filteredItems));
    loadPurchaseCartData();
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

//update inputted data in local store
$tableBody.on('change', 'input, select', function (e) {
    const row = $(this).closest('tr');
    const serialNumber = parseInt(row.find(".SN").text());
    const index = purchaseCart.findIndex((p => p.SN === serialNumber));

    row.find('input, select').each(function (i, element) {
        purchaseCart[index][element.name] = element.type === 'number' ? parseNumber(element.value) : element.value;
    });

    localStorage.setItem('purchaseCart', JSON.stringify(purchaseCart));
    calculateTotal();
    validation();
});

//sum table value
function calculateTotal() {
    const row = $('#table-row tr');
    var total = 0;

    if (row.length > 0) {
        row.find('input').each(function(i, element) {
            if (element.name === 'LineTotal')
                total += parseNumber(element.value);
        });
    };

    $("#totalPrice").text(total.toFixed(2));
    $("#grandTotal").text(total.toFixed(2));
    $("#totalDue").text(total.toFixed(2));

    resetPayment();
};

//discount change
$("#inputDiscount").on("change", function () {
    const totalPrice = parseNumber($("#totalPrice").text());
    const discount = parseNumber($(this).val());
    const isValid = compareValidation(totalPrice, discount);
    const grandTotal = (totalPrice - discount).toFixed(2);

    $(this).next('em').remove();

    if (!isValid) {
        $(this).after(`<em class="d-block red-text text-right">Discount within ৳${totalPrice}</em>`);
        return;
    }

    $("#grandTotal").text(grandTotal);
    $("#totalDue").text(grandTotal);

    const inputPaid = $("#inputPaid");
    if (inputPaid.val())
        inputPaid.val('');
});

//paid change
$("#inputPaid").on("change", function () {
    const grandTotal = parseNumber($("#grandTotal").text());
    const paid = parseNumber($(this).val());
    const isValid = compareValidation(grandTotal, paid);

    $(this).next('em').remove();

    if (!isValid) {
        $(this).after(`<em class="d-block red-text text-right">Pay within ৳${grandTotal}</em>`);
        return;
    }

    $("#totalDue").text((grandTotal - paid).toFixed(2));

    const paymentMethod = $("#selectPaymentMethod");
    paymentMethod.next('em').remove();

    if (paid > 0)
        paymentMethod.after('<em class="d-block red-text text-right">Select payment method</em>');
});

//remove error
$("#selectPaymentMethod").on("change", function () {
    $(this).next('em').remove();
});

//reset discount/paid amount
function resetPayment() {
    $("#inputDiscount").val('').next('em').remove();
    $("#inputPaid").val('').next('em').remove();
    $("#payment-method-error").text('');
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

//Supplier autocomplete
$("#inputFindSupplier").typeahead({
    minLength: 1,
    displayText: function (item) {
        return `${item.SupplierCompanyName} (${item.SupplierName}, ${item.SupplierPhone})`;
    },
    afterSelect: function (item) {
        this.$element[0].value = item.SupplierCompanyName
    },
    source: function (request, result) {
        $.ajax({
            url: "/Supplier/FindSupplier",
            data: JSON.stringify({ 'prefix': request }),
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (response) { result(response); }
        });
    },
    updater: function (item) {
        bindData(item);
        return item;
    }
});

//insert Supplier
$("#CreateClick").on("click", function () {
    $.get($(this).data("url"), function (response) {
        $('#InsertModal').html(response).modal('show');
    });
});

function OnCreateSuccess(response) {
    if (response.Status) {
        $('#InsertModal').html(response).modal('hide');
        bindData(response.Data);
        $("#inputFindSupplier").val('');
    } else {
        $('#InsertModal').html(response);
    }
}

function bindData(data) {
    const SupplierInfo = $("#Supplier-info");
    $("#SupplierId").val(data.SupplierID);

    const html = `<li class="list-group-item"><i class="fas fa-building"></i> ${data.SupplierCompanyName}</li>
    <li class="list-group-item"><i class="fas fa-user-tie"></i> ${data.SupplierName}</li>
    <li class="list-group-item"><i class="fas fa-phone"></i> ${data.SupplierPhone}</li>
    <li class="list-group-item"><i class="fas fa-map-marker-alt"></i> ${data.SupplierAddress}</li>`;

    if (SupplierInfo.children)
        SupplierInfo.empty();

    SupplierInfo.append(html);
}

//Submit Purchase
$("#btnPurchase").on("click", function (evt) {
    const isValid = validation();
    const that = $(this);
    const SupplierId = $("#SupplierId").val();
    const totalPrice = $("#totalPrice").text() || 0;
    const discountAmount = $("#inputDiscount").val();
    const paidAmount = parseNumber($("#inputPaid").val());
    const description = $("#inputDescription").val();
    const date = $("#inputPurchaseDate").val();
    const paymentMethod = $("#selectPaymentMethod").children("option:selected").val();
    const SupplierInfo = $("#Supplier-info");


    if (!SupplierId) {
        if (!SupplierInfo.children().length) {
            $("#Supplier-info").append(`<li class="list-group-item list-group-item-danger"><i class="fas fa-exclamation-triangle mr-1 red-text"></i>Select/add Supplier for Purchase!</li>`);
            return;
        }

        if (SupplierInfo.children().length) return;
    }

    if ($("#payment-area em").length) return;


    const data = {
        SupplierID: SupplierId,
        PurchaseTotalPrice: totalPrice,
        PurchaseDiscountAmount: discountAmount,
        PurchasePaidAmount: paidAmount,
        PurchaseDate: date,
        Description: description,
        Payment_Situation: paymentMethod,
        PurchaseCarts: purchaseCart
    };


    if (isValid && data.PurchaseTotalPrice > 0) {
        $.ajax({
            url: "/Supplier/PostPurchase",
            data: JSON.stringify({ model: data }),
            type: "POST",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () { that.prop('disabled', true).val('Submitting..') },
            success: function (id) {
                localStorage.removeItem('purchaseCart');
                window.location.href = `/Supplier/Receipt/${id}`;
            },
            error: function (error) { console.log(error) },
            complete: function () { that.prop('disabled', false).val('Submit') }
        });
    }
});