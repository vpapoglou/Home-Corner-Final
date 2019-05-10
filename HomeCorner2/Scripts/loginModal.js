var $loginModalToggleButton = $('#login-modal-toggle-button');
var $loginModal = $('#logInModal');
var $loginForm = $('#loginForm').find('form');

$(document).ready(function (event) {
    $loginModalToggleButton.on('click', function (event) {
        $loginModal.modal('show');
    });

    $loginModal.on('hidden.bs.modal', function (event) {
        $loginForm.trigger('reset');
    });



    //$loginForm.on('submit', function (event) {
    //    event.preventDefault();
    //    var email = $loginForm.find('#Email').val();
    //    var password = $loginForm.find('#Password').val();

    //    console.log(email);
    //    console.log(password);
    //});
});
