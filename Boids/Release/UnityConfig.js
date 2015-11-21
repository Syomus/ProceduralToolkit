function CompatibilityCheck()
{
    // Identify user agent
    var browser = (function(){
        var ua= navigator.userAgent, tem, 
        M= ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
        if(/trident/i.test(M[1])){
            tem=  /\brv[ :]+(\d+)/g.exec(ua) || [];
            return 'IE '+(tem[1] || '');
        }
        if(M[1]=== 'Chrome'){
            tem= ua.match(/\bOPR\/(\d+)/)
            if(tem!= null) return 'Opera '+tem[1];
        }
        M= M[2]? [M[1], M[2]]: [navigator.appName, navigator.appVersion, '-?'];
        if((tem= ua.match(/version\/(\d+)/i))!= null) M.splice(1, 1, tem[1]);
        return M.join(' ');
    })();

    var hasWebGL = (function(){
        if (!window.WebGLRenderingContext) 
        {
          // Browser has no idea what WebGL is. Suggest they
          // get a new browser by presenting the user with link to
          // http://get.webgl.org
          return 0;   
        }

        var canvas = document.createElement('canvas'); 
        var gl = canvas.getContext("webgl");   
        if (!gl) 
        {
          gl = canvas.getContext("experimental-webgl");   
          if (!gl) 
          {
            // Browser could not initialize WebGL. User probably needs to
            // update their drivers or get a new browser. Present a link to
            // http://get.webgl.org/troubleshooting
            return 0;  
          }
        }
        return 1;
    })();

    // Check for mobile browsers
    var mobile = (function(a){
        return (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(a)||/1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0,4)))
    })(navigator.userAgent||navigator.vendor||window.opera);

    // Check for WebGL. Allow running without WebGL on development players for running tests on build farm.
    if (!0 && !hasWebGL)
    {
        alert("You need a browser which supports WebGL to run this content. Try installing Firefox.");
        window.history.back();                
    }
    // Show warnings if needed.
    else if (mobile)
    {
        if (!confirm("Please note that Unity WebGL is not currently supported on mobiles. Press Ok if you wish to continue anyways."))
            window.history.back();        
    }
    else if (browser.indexOf("Firefox") == -1 && browser.indexOf("Chrome") == -1 && browser.indexOf("Safari") == -1)
    {
        if (!confirm("Please note that your browser is not currently supported for this Unity WebGL content. Try installing Firefox, or press Ok if you wish to continue anyways."))
            window.history.back();
    }
}

CompatibilityCheck();

var didShowErrorMessage = false;
if (typeof window.onerror != 'function')
{
    window.onerror = function UnityErrorHandler(err, url, line)
    {
        console.log ("Invoking error handler due to\n"+err);
        if (typeof dump == 'function')
            dump ("Invoking error handler due to\n"+err);
        if (didShowErrorMessage)
            return;

        // Firefox has a bug where it's IndexedDB implementation will throw UnknownErrors, which are harmless, and should not be shown.
        if (err.indexOf("UnknownError") != -1)
            return;

        didShowErrorMessage = true;
        if (err.indexOf("DISABLE_EXCEPTION_CATCHING") != -1)
        {
            alert ("An exception has occured, but exception handling has been disabled in this build. If you are the developer of this content, enable exceptions in your project's WebGL player settings to be able to catch the exception or see the stack trace.");
            return;
        }
        if (err.indexOf("Cannot enlarge memory arrays") != -1)
        {
            alert ("Out of memory. If you are the developer of this content, try allocating more memory to your WebGL build in the WebGL player settings.");
            return;        
        }
        if (err.indexOf("Invalid array buffer length") != -1 || err.indexOf("out of memory") != -1 )
        {
            alert ("The browser could not allocate enough memory for the WebGL content. If you are the developer of this content, try allocating less memory to your WebGL build in the WebGL player settings.");
            return;                
        }
        if (err.indexOf("Script error.") != -1 && document.URL.indexOf("file:") == 0)
        {
            alert ("It seems your browser does not support running Unity WebGL content from file:// urls. Please upload it to an http server, or try a different browser.");
            return;
        } 
        alert ("An error occured running the Unity content on this page. See your browser's JavaScript console for more info. The error was:\n"+err);
}
}

function SetFullscreen(fullscreen)
{
    if (typeof JSEvents === 'undefined')
    {
        console.log ("Player not loaded yet.");
        return;
    }
    var tmp = JSEvents.canPerformEventHandlerRequests;
    JSEvents.canPerformEventHandlerRequests = function(){return 1;};
    Module.cwrap('SetFullscreen', 'void', ['number'])(fullscreen);
    JSEvents.canPerformEventHandlerRequests = tmp;
}
