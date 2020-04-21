var AttendanceService = function () {
    var createAttendance = function (gigId, done, fail) {
        $.post("/api/attendances", { gigId: gigId })
                  .done(done)
                  .fail(fail);
    };
    var deleteAttendance = function () {
        $.ajax({
            url: "/api/attendances/" + gigId,
            method: "DELETE"
        })
                    .done(done)
                    .fail(fail);

    };
    return {
        createAttendancr: createAttendance,
        deleteAttendance: deleteAttendance
    }
}();