function showAlertModal(state, message) {
    var modalBody = $('#alertModalBody');
    var alertClass = 'alert-primary'; // Default to primary color

    // Set alert color based on state
    if (state === 'success') {
        alertClass = 'alert-success';
    } else if (state === 'danger') {
        alertClass = 'alert-danger';
    }

    // Set alert message and color
    modalBody.html('<div class="alert ' + alertClass + '" role="alert">' + message + '</div>');

    $('#alertModal').modal('show'); // Show the modal
    setTimeout(function () {
        $('#alertModal').modal('hide'); // Hide the modal after 5 seconds
    }, 5000);
}