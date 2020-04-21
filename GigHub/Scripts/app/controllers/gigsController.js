var GigsController = function (attendanceService) {
    var button;
    var init = function (container) {
        $(container).on("click",".js-toggle-attendance",toggleAttendance)
      
    };


    var toggleAttendance = function (e) {
        button = $(e.target);
        var gigId = button.attr("data-gig-id");
        if (button.hasClass("btn-default"))
            var createAttendance = function (gigId,done, fail) {
                $.post("/api/attendances", { gigId: gigId })
                          .done(done)
                          .fail(fail);
            };
        else
            attendanceService.deleteAttendance(gigId,done, fail);
    };


    var done = function () {
        var text = (button.text() == "Going") ? "Going?" : "Going";
        button.toggleClass("btn-info").toggleClass("btn-default").text(text);
    };

    var fail = function () {
        alert("SOmething Wrong!!");
    };
    return {
        init: init,
        createAttendance: createAttendance
    }
}(AttendanceService);
var AttendanceService = function () {
   
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
