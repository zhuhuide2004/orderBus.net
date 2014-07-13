//标准空间检查输入
//控件中增加属性  required ,就会对这个控件进行检查
//提交之前调用即可    通过：true   不通过：false
function checkInput() {
    var retValue = true;

    $("[required]").each(
        function (i) {
            //文本框
            if ($(this).prop("type") == "text") {
                if ($.trim($(this).val()) == "") {
                    var itemName = $(this).parent().prev("th").text();
                    if (itemName == "") {
                        itemName = $(this).parent().parent().prev("th").text();
                    }
                    itemName = itemName.replace("*", "").replace("：", "");
                    alert("请输入【" + itemName + "】后再保存！");
                    $(this).focus();

                    retValue = false;
                    return false;
                }
            }

            //单选框
            if ($(this).prop("type") == "radio") {
                if ($("[name=" + $(this).prop("name") + "]:checked").size() == 0) {
                    var itemName = $(this).parent().prev("th").text();
                    itemName = itemName.replace("*", "").replace("：", "");
                    alert("请选择【" + itemName + "】后再保存！");
                    $(this).focus();

                    retValue = false;
                    return false;
                }
            }

            //下拉框
            if ($(this).prop("localName") == "select") {
                if ($(this).val() == "" || $(this).val() == "0") {
                    var itemName = $(this).parent().prev("th").text();
                    itemName = itemName.replace("*", "").replace("：", "");
                    alert("请选择【" + itemName + "】后再保存！");
                    $(this).focus();

                    retValue = false;
                    return false;
                }
            }
        });

    return retValue;
}