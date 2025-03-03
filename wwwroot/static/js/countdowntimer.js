
$(document).ready(function () {
    var hfEventDate = $('#hfEventDate').val(); // Use the Razor variable
    var eventDate = new Date(hfEventDate); 
    
    var countdownInterval = setInterval(function () {
        var currentTime = new Date().getTime();
        var remainingTime = eventDate.getTime() - currentTime;

        if (remainingTime <= 0) {
            remainingTime = 0;
            clearInterval(countdownInterval);
            //alert("Countdown complete!");
        }

        var seconds = Math.floor((remainingTime / 1000) % 60);
        var minutes = Math.floor((remainingTime / (1000 * 60)) % 60);
        var hours = Math.floor((remainingTime / (1000 * 60 * 60)) % 24);
        var days = Math.floor(remainingTime / (1000 * 60 * 60 * 24));

        $('#days span:first').text(days.toString().padStart(2, '0'));
        $('#hours span:first').text(hours.toString().padStart(2, '0'));
        $('#minutes span:first').text(minutes.toString().padStart(2, '0'));
        $('#seconds span:first').text(seconds.toString().padStart(2, '0'));

    }, 1000);
});