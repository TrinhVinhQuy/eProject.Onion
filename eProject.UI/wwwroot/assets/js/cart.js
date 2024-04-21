
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
            if (response.success) {
                // Thêm mục vào giỏ hàng thành công
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

