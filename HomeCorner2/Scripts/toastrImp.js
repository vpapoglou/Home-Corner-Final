$(document).ready(function () {

    displayToastr();

});

function displayToastr() {
    if ('@TempData["message"]' == "Success") {
        toastr.success('Success!');
    }
    else {
        if ('@TempData["message"]' == "Error") {
            toastr.error('Something went Wrong!');
        }
    }
}