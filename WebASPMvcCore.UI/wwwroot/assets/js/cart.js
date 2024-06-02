
function addToCart(itemId, quantity) {
    //console.log(itemId, quantity)
    $.ajax({
        type: "POST",
        url: "/Cart/AddCart",
        data: {
            Id: itemId,
            Quantity: quantity
        },
        success: function (response) {
            if (response.success) {
                // Thêm mục vào giỏ hàng thành công
                updateCartCount();
                showAlertModal('success', response.message);
            } else {
                // Có lỗi xảy ra khi thêm mục vào giỏ hàng
                showAlertModal('danger', response.message);
            }
        },
    });
}

function deleteFromCart(itemId) {
    $.ajax({
        type: "POST",
        url: "/Cart/DeleteCart",
        data: { Id: itemId },
        success: function (response) {
            if (response.success) {
                // Thêm mục vào giỏ hàng thành công
                showAlertModal('success', response.message);
            } else {
                // Có lỗi xảy ra khi xóa mục khỏi giỏ hàng
                showAlertModal('danger', "Có lỗi xảy ra khi xóa mục khỏi giỏ hàng.");
            }
        },
        error: function () {
            // Xử lý lỗi
            console.log("Có lỗi xảy ra khi thực hiện yêu cầu!");
        }
    });
}

function updateCartContent() {
    $.ajax({
        url: '/json/cart',
        type: 'GET',
        success: function (data) {
            var cartItemsHtml = '';
            var totalPrice = data.total // Làm tròn đến 2 chữ số sau dấu thập phân

            if (data.results && data.results.length > 0) {
                data.results.forEach(function (item) {
                    console.log(item)
                    cartItemsHtml += '<div class="cart-bar__item position-relative d-flex">' +
                        '<div class="thumb">' +
                        '<img src="' + item.metaImage + '" alt="image_not_found">' +
                        '</div>' +
                        '<div class="content">' +
                        '<h4 class="title">' +
                        '<a href="#">' + item.name + '</a>' +
                        '</h4>' +
                        '<span class="Giá">' + item.price * item.quantity + ' đ</span>' +
                        //'<a href="javascript:void(0)" class="remove"><i class="fal fa-times"></i></a>' +
                        '</div>' +
                        '</div>';
                });
            } else {
                cartItemsHtml += '<tr><td colspan="5">Giỏ của bạn đang trống!</td></tr>';
                // cartTableHTMLmobie += '<tr><td colspan="5">Giỏ của bạn đang trống!</td></tr>';
            }

            // Thêm các phần tử sản phẩm vào div có class "cart-bar__lists"
            $('#listCart').html(cartItemsHtml);

            // Cập nhật tổng cộng
            $('.cart-bar__subtotal span:last-child').text(totalPrice + ' đ');
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
}

function updateCartCount() {
    $.ajax({
        url: '/json/cart',
        type: 'GET',
        success: function (data) {
            let totalQuantity = 0;
            if (data.success != false) {
                // Lặp qua mảng 'results' và tính tổng quantity
                for (let i = 0; i < data.results.length; i++) {
                    totalQuantity += data.results[i].quantity;
                }
            }
            console.log(totalQuantity)
            document.getElementById("txtCart").innerHTML = totalQuantity;
            document.getElementById("txtCartRe").innerHTML = totalQuantity;
            document.getElementById("txtCartPopup").innerHTML = totalQuantity;
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error: ' + errorThrown);
        }
    });
}
//$(document).ready(function () {
//    updateCartCount()
//});
