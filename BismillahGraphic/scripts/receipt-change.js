
//global cart data
var productsList = [];

$(function () {
    //input number only
    $('#table-row').on('input keypress', '.quantity, .unitPrice, .lineTotal', function (event) {
        if ((event.which !== 46 || $(this).val().indexOf('.') !== -1) &&
            (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });

    //input Paid, input Discount
    $('#inputDiscount').on('input keypress', function (event) {
        if ((event.which !== 46 || $(this).val().indexOf('.') !== -1) &&
            (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });
});

//selectors
const $tableBody = $('#table-row');
const formProduct = document.getElementById('formProduct')
const totalPrice = document.getElementById('totalPrice')
const inputDiscount = document.getElementById('inputDiscount')
const totalPayable = document.getElementById('totalPayable')
const paidAmount = document.getElementById('paidAmount')
const totalDue = document.getElementById('totalDue')

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
        item.SN = productsList.length+1
        productsList.push(item);
        appendDataTable();
        console.log(item)
        return item;
    }
});

//build table
function appendDataTable() {
    let html = '';
    productsList.forEach(function (data,i) {
    html += `<tr>
            <td><strong class="SN">${i+1}</strong></td>
            <td class="text-left">${data.ProductName}</td>
            <td><input type="number" step="0.01" class="length form-control" value="${data.Length}" name="Length" placeholder="Length" required/></td>
            <td><input type="number" step="0.01" class="width form-control" value="${data.Width}" name="Width" placeholder="Width" required/></td>
            <td><input type="number" step="0.01" class="quantity form-control" value="${data.SellingQuantity}" name="SellingQuantity" placeholder="Square Inch" disabled/></td>
            <td><input type="number" step="0.01" class="unitPrice form-control" value="${data.SellingUnitPrice}" name="SellingUnitPrice" placeholder="Unit Price" required/></td>
            <td><input type="number" step="0.01" class="lineTotal form-control" value="${(data.SellingQuantity*data.SellingUnitPrice).toFixed(2)}" name="LineTotal" placeholder="Line Total" disabled/></td>
            <td><a style="color:#ff0000" class="delete fas fa-trash-alt" href="/Products/Delete/${data.ProductID}"></a></td>
        </tr>`;
    });

    $tableBody.empty()
    $tableBody.append(html);
};

//delete click
$tableBody.on("click", ".delete", function (evt) {
    evt.preventDefault();

    const row = $(this).closest('tr');
    const serialNumber = row.find('.SN').text();

    productsList = productsList.filter(function (item) {
        return item.SN !== parseInt(serialNumber);
    });

    appendDataTable();
    calculateTotal();
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

//update product info
$tableBody.on('change', 'input', function (e) {
    const row = $(this).closest('tr');
    const serialNumber = parseInt(row.find(".SN").text());
    const index = productsList.findIndex(p => p.SN === serialNumber);

    row.find('input').each(function (i, element) {
        productsList[index][element.name] = element.type === 'number' ? parseNumber(element.value) : element.value;
    });

    calculateTotal();
});

//discount change
$("#inputDiscount").on("change", function () {
    calculateTotal();
});

//convert to float number
function parseNumber(n) {
    const f = parseFloat(n);
    return isNaN(f) ? 0 : f;
}

//sum table value
function calculateTotal() {
    const total = productsList.reduce(function (prev, cur) {
        return prev + (cur.SellingQuantity * cur.SellingUnitPrice);
    }, 0);

    const discount = +inputDiscount.value | 0;
    const paid = +paidAmount.textContent | 0;

    totalPrice.textContent = Math.ceil(total);
    totalPayable.textContent = Math.ceil(total - discount);

    const payable = +totalPayable.textContent | 0;
    totalDue.textContent = Math.ceil(payable - paid);

    inputDiscount.setAttribute('max', totalDue.textContent);
};

//Submit Sell change
formProduct.addEventListener('submit', function(evt) {
    evt.preventDefault()

    const data = {
        SellingID: +document.getElementById('hiddenSellingId').value,
        SellingTotalPrice: +totalPrice.textContent | 0,
        SellingDiscountAmount: +inputDiscount.value | 0,
        SellingCarts: productsList
    };
    console.log(data)
    const btn = formProduct.btnSelling;

    if (productsList.length) {
        $.ajax({
            url: "/Selling/ReceiptChange",
            data: JSON.stringify({ model: data }),
            type: "POST",
            contentType: "application/json; charset=utf-8",
            beforeSend: function () { btn.setAttribute('disabled', true) },
            success: function(id) { window.location.href = `/Selling/Receipt/${id}` },
            error: function(error) { console.log(error) },
            complete: function () { btn.removeAttribute('disabled')}
        });
    }
});