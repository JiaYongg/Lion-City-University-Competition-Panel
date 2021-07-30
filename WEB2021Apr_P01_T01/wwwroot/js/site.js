// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('#selectAll').click(function (e) {
    var table = $(e.target).closest('table');
    $('td input:checkbox', table).prop('checked', this.checked);
});

$('#selectAll2').click(function (e) {
    var table = $(e.target).closest('table');
    $('td input:checkbox', table).prop('checked', this.checked);
});