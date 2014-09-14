var Common = {
    ValidMobile: function (mobile) {
        return mobile.length == 11 && /1[3|4|5|8][0-9]\d{8}/.test(mobile);
    },

    ValidIdNumber: function (idNumber) {
        return /^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$/.test(idNumber);
    },

    Format: function (format, args) {
        for (var i = 0; i < args.length; i++)
            format = format.replace(RegExp("\\{" + i.toString() + "\\}", "g"), args[i]);

        return format;
    },

    GetParam: function (paramName) {
        var tmp = location.search;

        if (tmp.length > 0) {
            tmp = decodeURI(tmp);

            var arr = tmp.substring(1).split("&");

            paramName += "=";

            for (var i = 0; i < arr.length; i++) {
                if (arr[i].indexOf(paramName) == 0)
                    return arr[i].substring(paramName.length);
            }
        }

        return undefined;
    }
}

function setTargetUrl(urlPart) {
    $.cookie("targeturl", urlPart, { path: '/' });
    //window.location.href = urlPart;
    window.location.href = "/index.aspx";
}

function getTargetUrl() {
    return $.cookie("targeturl");
}

function setTargetUrlVote(urlPart) {
    $.cookie("targeturl", urlPart, { path: '/' });
    window.location.href = urlPart;
    //window.location.href = "/index.aspx";
}