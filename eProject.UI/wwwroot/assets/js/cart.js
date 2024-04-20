function updateCartContent() {
    $.ajax({
        url: '/Cart/GetCart',
        type: 'GET',
        success: function (data) {
            var cartItemsHtml = '';
            var totalPrice = data.totalPrice.toFixed(0); // Làm tròn đến 2 chữ số sau dấu thập phân

            if (data.cartModels && data.cartModels.length > 0) {
                data.cartModels.forEach(function (item) {
                    console.log(item)
                     cartItemsHtml += '<div class="cart-bar__item position-relative d-flex">' +
                         '<div class="thumb">' +
                         '<img src="' + item.productModel.url + '" alt="image_not_found">' +
                         '</div>' +
                         '<div class="content">' +
                         '<h4 class="title">' +
                         '<a href="' + item.productModel.url + '">' + item.productModel.name + '</a>' +
                         '</h4>' +
                         '<span class="Giá">' + item.productModel.price.toFixed(0) * item.quantity + ' đ</span>' +
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
function addToCart(itemId, quantity) {
    console.log(itemId, quantity)
    $.ajax({
        type: "POST",
        url: "/Cart/AddCart",
        data: {
            Id: itemId,
            Quantity: quantity
        },
        success: function (response) {
            if (response > 0) {
                // Thêm mục vào giỏ hàng thành công
                alert("Mục đã được thêm vào giỏ hàng!");
            } else {
                // Có lỗi xảy ra khi thêm mục vào giỏ hàng
                alert("Có lỗi xảy ra khi thêm mục vào giỏ hàng.");
            }
        },
        error: function () {
            // Xử lý lỗi
            alert("Có lỗi xảy ra khi thực hiện yêu cầu!");
        }
    });
}

function deleteFromCart(itemId) {
    $.ajax({
        type: "POST",
        url: "/Cart/Delete",
        data: { Id: itemId },
        success: function (response) {
            if (response) {
                // Xóa mục khỏi giỏ hàng thành công
                alert("Mục đã được xóa khỏi giỏ hàng!");
            } else {
                // Có lỗi xảy ra khi xóa mục khỏi giỏ hàng
                alert("Có lỗi xảy ra khi xóa mục khỏi giỏ hàng.");
            }
        },
        error: function () {
            // Xử lý lỗi
            alert("Có lỗi xảy ra khi thực hiện yêu cầu!");
        }
    });
}

