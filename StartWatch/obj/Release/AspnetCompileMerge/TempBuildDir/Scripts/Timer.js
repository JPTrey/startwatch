// add dynamic timers with current duration
var act = @ActNumber;
var time_today = 0;
var time_weekly = 0;
var time_total = 0;
var avg_session = 0;

if (act > -1) {       // if: not paused
    // initial values at page load
    time_today = @ActToday;
    time_weekly = @ActWeek;
    time_total = @ActTotal;
    time_remain = Math.max((@ActQuota - @ActToday), 0);
    avg_session = @(ActTotal/ActAvg);

    // update time divs
    //updateTime(time_today, "#time-today"); time_today++;
    //updateTime(time_total, "#time-total"); time_total++;
    //updateTime(time_remain, "#time-remaining"); time_remain--;
    //updateTime(avg_session, "#avg-session");

    // cycle every second
    setInterval(function() { updateTime(time_today, "#time-today"); time_today++; },1000);
    setInterval(function() { updateTime(time_total, "#time-total"); time_total++; },1000);
    setInterval(function() {
        if (time_remain > 0) {updateTime(time_remain, "#time-remaining"); time_remain--; }
        else {
            $("#time-remaining").text("0:00");
            $("#time-remaining").css("color","green");
        }
    },1000);
    setInterval(function() { updateTime(avg_session, "#avg-session"); },1000);

    function updateTime(display_time, id) {
        var msg = "";
        if (display_time > 3599) {
            if (display_time / 3600 < 10) { msg += "0"; }
            msg += ~~(display_time / 3600);
            display_time %= 3600;
            msg += ":";
        }
        if (display_time > 60) {
            if (display_time / 60 < 10) { msg += "0"; }
        }
        else { msg += "0"; }
        msg += ~~(display_time / 60) + ":";
        display_time %= 60;
        if (display_time < 10) { msg += ("0"); }
        msg += display_time;
        $(id).text(msg);
    }
}

// handle setting new current task, without refreshing
$(function() {
    $('#texthere').text("act = " + act);
    $('#act_list').change(function() {


        if (act === $("#act_list").val()) ; // if act doesn't change, do nothing
        else {
            // store time for outgoing activity
            act_todays[act] = time_today;
            act_weekly[act] = time_weekly;
            act_totals[act] = time_total;

            act = $("#act_list").val();
            $('#texthere').text("act = " + act);


            if (act === -1) {	// if: TimeLine was paused, refresh page
                $.get("/Activities/Set/" + act);
                window.location.reload();
            }


            else {      // else: update time for outgoing activity and set new activity in database
                $.get("/Activities/Set/?act=" + act + "&time=" + time_today);
                time_today = act_todays[act];
                time_weekly = act_weekly[act];
                time_total = act_totals[act];
                avg_session = ~~(act_totals[act]/act_sessions[act]);
                if (avg_session <= 0) { avg_session = 1 }
                $("#today").text(time_today);
                $('#texthere').text("act = " + act);

            }
        }
        return false;
    })
});