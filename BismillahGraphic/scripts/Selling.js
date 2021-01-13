
//global cart data
var cart = [];

$(function () {
    $('.datepicker').pickadate().val(moment(new Date()).format('DD MMMM, YYYY'));

    //clear id if refresh page
    $("#vendorId").val("");

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
    loadCartData();
});

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
        loadCartData();
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
                <td><input type="number" step="0.01" class="length form-control" value="${data.Length}" name="Length" placeholder="Length"/></td>
                <td><input type="number" step="0.01" class="width form-control" value="${data.Width}" name="Width" placeholder="Width"/></td>
                <td><input type="number" step="0.01" class="quantity form-control" value="${data.SellingQuantity}" name="SellingQuantity" placeholder="Square Inch" disabled/></td>
                <td><input type="number" step="0.01" class="unitPrice form-control" value="${data.SellingUnitPrice}" name="SellingUnitPrice" placeholder="Unit Price"/></td>
                <td><input type="number" step="0.01" class="lineTotal form-control" value="${data.LineTotal}" name="LineTotal" placeholder="Line Total" disabled/></td>
                <td><a style="color:#ff0000" class="delete fas fa-trash-alt" href="/Products/Delete/${data.ProductID}"></a></td>
            </tr>`;
    });

    row.empty();
    row.append(html);
};

//save to cart
function saveProductList(product) {
    product.SN = (cart.length) + 1;
    product.SellingQuantity = 0;
    product.SellingUnitPrice = product.ProductPrice;
    product.LineTotal = 0;
    product.Details = '';

    cart.push(product);
    localStorage.setItem('cart', JSON.stringify(cart));
};

//load Cart Data
function loadCartData() {
    if (localStorage.getItem('cart')) {
        cart = JSON.parse(localStorage.getItem('cart'));
    };

    productTable(cart);
    calculateTotal();
};

//find table
var $tableBody = $('#table-row');

//delete click
$tableBody.on("click", ".delete", function (evt) {
    evt.preventDefault();
    const row = $(this).closest('tr');
    var serialNumber = row.find('.SN').text();

    const filteredItems = cart.filter(function (item) {
        return item.SN !== parseInt(serialNumber);
    });


    localStorage.setItem('cart', JSON.stringify(filteredItems));
    loadCartData();
});

//input length, width
$tableBody.on('input', '.length, .width', function () {
    const row = $(this).closest('tr');
    const quantity = row.find('.quantity');
    const length = row.find('.length');
    const width = row.find('.width');

    const total = (parseNumber(length.val()) * parseNumber(width.val()));
    quantity.val(total.toFixed(2));
});

//input unit Price
$tableBody.on('input', '.unitPrice, .length, .width', function () {
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

//update data
$tableBody.on('change', 'input', function (e) {
    const row = $(this).closest('tr');
    const serialNumber = parseInt(row.find(".SN").text());
    const index = cart.findIndex((p => p.SN === serialNumber));

    row.find('input').each(function (i, element) {
        cart[index][element.name] = element.type === 'number' ? parseNumber(element.value) : element.value;
    });

    localStorage.setItem('cart', JSON.stringify(cart));
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

    $("#totalPrice").text(total);
    $("#grandTotal").text(total);
    $("#totalDue").text(total);

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
        row.find('input[type="number"]').each(function () {
            const input = $(this);

            if (input.val() === '' || input.val() === '0' || input.val() === 0) {
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

//vendor autocomplete
$("#inputFindVendor").typeahead({
    minLength: 1,
    displayText: function (item) {
        return `${item.VendorCompanyName} (${item.VendorName}, ${item.VendorPhone})`;
    },
    afterSelect: function (item) {
        this.$element[0].value = item.VendorCompanyName
    },
    source: function (request, result) {
        $.ajax({
            url: "/Selling/FindVendor",
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

//insert vendor
$("#CreateClick").on("click", function () {
    $.get($(this).data("url"), function (response) {
        $('#InsertModal').html(response).modal('show');
    });
});

function OnCreateSuccess(response) {
    if (response.Status) {
        $('#InsertModal').html(response).modal('hide');
        bindData(response.Data);
        $("#inputFindVendor").val('');
    } else {
        $('#InsertModal').html(response);
    }
}

function bindData(data) {
    const vendorInfo = $("#vendor-info");
    $("#vendorId").val(data.VendorID);

    const html = `<li class="list-group-item"><i class="fas fa-building"></i> ${data.VendorCompanyName}</li>
    <li class="list-group-item"><i class="fas fa-user-tie"></i> ${data.VendorName}</li>
    <li class="list-group-item"><i class="fas fa-phone"></i> ${data.VendorPhone}</li>
    <li class="list-group-item"><i class="fas fa-map-marker-alt"></i> ${data.VendorAddress}</li>`;

    if (vendorInfo.children)
        vendorInfo.empty();

    vendorInfo.append(html);
}

//Submit Sell
$("#btnSelling").on("click", function (evt) {
    const isValid = validation();
    const that = $(this);
    const vendorId = $("#vendorId").val();
    const totalPrice = $("#totalPrice").text() || 0;
    const discountAmount = $("#inputDiscount").val();
    const paidAmount = parseNumber($("#inputPaid").val());
    const date = $("#inputSellingDate").val();
    const paymentMethod = $("#selectPaymentMethod").children("option:selected").val();
    const vendorInfo = $("#vendor-info");


    if (!vendorId) {
        if (!vendorInfo.children().length) {
            $("#vendor-info").append(`<li class="list-group-item list-group-item-danger"><i class="fas fa-exclamation-triangle mr-1 red-text"></i>Select/add Vendor for selling!</li>`);
            return;
        }

        if (vendorInfo.children().length) return;
    }

    if ($("#payment-area em").length) return;


    const data = {
        VendorID: vendorId,
        SellingTotalPrice: totalPrice,
        SellingDiscountAmount: discountAmount,
        SellingPaidAmount: paidAmount,
        SellingDate: date,
        Payment_Situation: paymentMethod,
        SellingCarts: cart
    };


    if (isValid && data.SellingTotalPrice > 0) {
        $.ajax({
            url: "/Selling/PostSelling",
            data: JSON.stringify({ model: data }),
            type: "POST",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () { that.prop('disabled', true).val('Submitting..') },
            success: function (id) {
                localStorage.removeItem('cart');
                window.location.href = `/Selling/Receipt/${id}`;
            },
            error: function (error) { console.log(error) },
            complete: function () { that.prop('disabled', false).val('Submit') }
        });
    }
});