/* *******************************************************
 * General JS, for all pages */
$('.dropdown-menu').on('click', function (e) {
    if ($(this).hasClass('dropdown-menu-checkboxes')) {
        e.stopPropagation();
    }
});

$(document).ready(function () {
    $(".body-content").show();
});

$('.navbar-toggle').on('click', function (e) {
    console.log("Pressed: ", e);
    $('.ccm-top-navbar-container').toggleClass('ccm-top-navbar-container--expanded');
});
