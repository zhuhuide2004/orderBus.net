function successmsg() {
    layer.msg('恭喜您，操作成功！', 1, 1);
}
function successmsg2(url) {
    layer.msg('恭喜您，操作成功！', 1, 1, function () {
        window.location.href = url;
    });
}
function successmsg3(txt) {
    layer.msg(txt, 1, 1);
}
function successmsg4(url,txt) {
    layer.msg(txt, 1, 1, function () {
        window.location.href = url;
    });
}
function errormsg(txt) {
    layer.msg(txt, 1, 3); return false;
}