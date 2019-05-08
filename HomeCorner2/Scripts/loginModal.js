var $loginModalToggleButton = $('#login-modal-toggle-button');
var $loginModal = $('#logInModal');

$(document).ready(function (event) {
    console.log('no');
    $loginModalToggleButton.on('click', function (event) {
        console.log('yes');
        $loginModal.modal('show');
    });

    $loginModal.on('hidden.bs.modal', function (event) {
        $loginForm.trigger('reset');
    });
});
