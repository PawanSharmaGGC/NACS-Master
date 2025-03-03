var today = new Date(); 
var dropSurveyCookie = true; // false disables the Cookie, allowing you to style the banner        
var SurveyCookieDuration = 1825; // Number of days before the cookie expires, and the banner reappears       
var SurveyCookieName = 'NSSurveyPopupCookie'; // Name of our cookie    
var SurveyCookieName_Counter = 'NSSurveyPopupCookie_Counter'; // Name of our cookie   
var SurveyCookieValue = today.toISOString(); 'on'; // Value of cookie
var SurveyCookieValue_Counter = 0; 'on'; // Value of cookie
        
//Check Expiration day
function CreateSurveyCookie(name, value, days, session) {
    if (days) {
        //var days = -1; // assign a specific date other than just days 
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toUTCString(); //date.toGMTString();
    }
    else var expires = "";

    if (window.dropSurveyCookie) {
        document.cookie = name + "=" + value + ";" + expires + "; path=/" + ";session=false";
    }
}

//Check cookie name
function CheckSurveyCookie(name) {
    var Name = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(Name) == 0) return c.substring(Name.length, c.length);
    }
    return null;
}     
       
function EraseSurveyCookie(name) {
    CreateSurveyCookie(name, "", -1);
}

//create cookie that will keep the popup hidden just for the session
function CloseSurvey() {

    var element = document.getElementById('top-tasks-survey-popup');
    element.parentNode.removeChild(element);

    //alert(CheckSurveyCookie(window.SurveyCookieName_Counter));

    if (CheckSurveyCookie(window.SurveyCookieName_Counter) == null) {

        //create new one for counting
        CreateSurveyCookie(window.SurveyCookieName_Counter, window.SurveyCookieValue_Counter + 1, window.SurveyCookieDuration, "false"); // New Cookie: Expires: 5 Years
        CreateSurveyCookie(window.SurveyCookieName, window.SurveyCookieValue, "", "true"); // Create the session cookie       
    }
    else {

        var count = parseInt(CheckSurveyCookie(window.SurveyCookieName_Counter));

        if (count < 2) {
            
            //update count
            CreateSurveyCookie(window.SurveyCookieName_Counter, count + 1, window.SurveyCookieDuration, "false"); // New Cookie: Expires: 5 Years
            CreateSurveyCookie(window.SurveyCookieName, window.SurveyCookieValue, "", "true"); // Create the session cookie    
        }
        else if (count >= 2) {
            //create a permanent cookie - they've closed 3 times
            CreateSurveyCookie(window.SurveyCookieName, window.SurveyCookieValue, window.SurveyCookieDuration, "false"); // New Cookie: Expires: 5 Years
        }
    }

    
}

//create cookie that will keep the popup hidden forever
function RemoveMeFromSurvey() {
    var element = document.getElementById('top-tasks-survey-popup');
    element.parentNode.removeChild(element);
    CreateSurveyCookie(window.SurveyCookieName, window.SurveyCookieValue, window.SurveyCookieDuration, "false"); // New Cookie: Expires: 5 Years
}

//create cookie that will keep the popup hidden forever
function TakeSurvey() {
    var element = document.getElementById('top-tasks-survey-popup');
    element.parentNode.removeChild(element);
    CreateSurveyCookie(window.SurveyCookieName, window.SurveyCookieValue, window.SurveyCookieDuration, "false"); // New Cookie: Expires: 5 Years
}

       
//permanent cookie does not exist yet
if (CheckSurveyCookie(window.SurveyCookieName) == null) {

    setTimeout(function () {
        $('#top-tasks-survey-popup').show();
    }, 20000); //wait 20 seconds
}
//cookie exists, do not show
else {
    $('#top-tasks-survey-popup').hide();
}