$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: "/Faculties/GetAllFaculties",
        dataType: 'json',
        success: function (faculties) {
            faculties.forEach(function (f) {
                $("#Faculty_Id").append($("<option/>").val(f.id).text(f.name));
            });
        }
    });
});