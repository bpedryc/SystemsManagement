$(document).ready(function () {
    $.ajax({
        type: 'GET',
        url: "/Faculties/GetAllFaculties",
        dataType: 'json',
        success: function (faculties) {
            faculties.forEach(function (f) {
                $("#Faculties").append($("<option/>").val(f.id).text(f.name));
            });
        }
    });
});

function GetSpecialties(faculty) {
    $.ajax({
        type: "GET",
        url: "/Authentication/GetSpecialties",
        data: { "facultyId": faculty },
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        async: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
        },
        success: function (specialties) {
            var dropdown = $('#Student_SpecialtyId');
            dropdown.empty();
            specialties.forEach(function (s) {
                dropdown.append(
                    $('<option></option>').val(s.id).html(s.name)
                );
            });
        }
    });
}