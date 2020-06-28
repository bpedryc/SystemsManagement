function GetAllFaculties() {
    $.ajax({
        type: "GET",
        url: "/Faculties/GetAllFaculties",
        dataType: "json",
        async: false,
        success: function (faculties) {
            faculties.forEach(function (f) {
                $("#FacultyId").append($("<option/>").val(f.id).text(f.name));
            });
        },
        error: function (e) {
            console.log(e);
        }
    });
}