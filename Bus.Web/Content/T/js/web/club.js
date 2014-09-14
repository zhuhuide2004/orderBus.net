/**
 * User: dong.zhao
 * Date: 13-12-14
 * Time: 下午5:15
 */
function download(vtype) {
    var href = "";
    if (vtype == "android") {
        href = base + "/download/todownload.html";
        window.location.href = href;
    } else if (vtype == "iphone") {
        href = base + "/download/downios.html";
        window.location.href = href;
    } else if (vtype == "window8") {
        href = "http://www.windowsphone.com/zh-cn/store/app/%E9%AB%98%E5%BE%B7%E5%AF%BC%E8%88%AA/a8078319-139a-4238-a387-824b5d18c61d";
        window.open(href);
    } else if (vtype == "blackberry") {
        href = base + "/download/blackberry.html";
        window.location.href = href;
    }

}

function replaceHeaderCss(headerCss) {
    if (headerCss == "") {
        $("#index").addClass("current");
    } else if (headerCss == "function") {
        $("#function").addClass("current");
    } else if (headerCss == "myanav") {
        $("#myanav").addClass("current");
    } else if (headerCss == "download") {
        $("#download").addClass("current");
    } else if (headerCss == "playanav") {
        $("#playanav").addClass("current");
    }
}

function addOneCaiDan() {
    $("#one_caidan").html("<li class='ap1'><a id='index' href='" + base + "/'>首 页</a></li>" +
        "<li class='ap2'><a id='function' href='" + base + "/function.html'>功 能</a></li>" +
        "<li class='ap3'><a id='download' href='" + base + "/download/todownload.html'>下 载</a></li>" +
        "<li class='ap4'><a id='myanav' href='" + base + "/myanav.html'>我的导航</a></li>" +
        "<li class='ap5'><a id='playanav' href='" + base + "/help/playanav.portal'>玩转导航</a></li>");
}

function AddFavorite(sURL, sTitle) {
    try {
        window.external.addFavorite(sURL, sTitle);
    } catch (e) {
        try {
            window.sidebar.addPanel(sTitle, sURL, "");
        } catch (e) {
            alert("加入收藏失败，请使用Ctrl+D进行添加");
        }
    }
}

//鼠标滑过弹出层
function myanav_mouseover(obj, sType) {
    var oDiv = document.getElementById(obj);
    if (sType == 'show') {
        oDiv.style.display = 'block';
    }
    if (sType == 'hide') {
        oDiv.style.display = 'none';
    }
}

function goPhonesInfo(id, brandname) {
    if(id == null || id == "") {
        tipsAlert("手机相关信息不存在,请联系客服人员");
        return;
    }
    window.location.href = base + "/phone/phonesinfo.portal?id=" + id + "&brandname=" + brandname;
}

function selectTag(showContent, selfObj) {
    // 操作标签
    var tag = document.getElementById("tags")
        .getElementsByTagName("li");
    var taglength = tag.length;
    var i = 0;
    for (i = 0; i < taglength; i++) {
        tag[i].className = "";
    }
    selfObj.parentNode.className = "selectTag";
    // 操作内容
    for (i = 0; j = document.getElementById("tagContent" + i); i++) {
        j.style.display = "none";
    }
    document.getElementById(showContent).style.display = "block";
}

function download_two_navi(oneid, twoid) {
    var html = '<div class="and"><ul>' +
        '<li class="ar"><a id="android" href="' + base + '/download/todownload.html">Android</a></li>' +
        '<li class="ap"><a id="iphone" href="' + base + '/download/downios.html">iPhone</a></li>' +
        '<li class="wp"><a id="wp8" href="http://www.windowsphone.com/zh-cn/store/app/%E9%AB%98%E5%BE%B7%E5%AF%BC%E8%88%AA/a8078319-139a-4238-a387-824b5d18c61d" target="_blank">WP8</a></li>' +
        '<li class="bb"><a id="bb" href="' + base + '/download/blackberry.html">BlackBerry</a></li>' +
        '</ul>' +
        '<div class="crl"></div>' +
        '</div>';
    var html_two = '<div class="tit27">' +
        '<table width="100%" border="0" cellspacing="1" cellpadding="0">' +
        '<tr>' +
        '<td id="public"><a href="' + base + '/download/todownload.html">公众版下载</a> </td>' +
        '<td id="company"><a href="' + base + '/download/company.html">预装版下载</a></td>' +
        '<td id="history"><a href="' + base + '/download/history.html">历史版下载</a></td>' +
        '</tr>' +
        '</table>' +
        '</div>' +
        '<div class="crl"></div>';
    $('#two_navi').append(html);
    if(oneid != 'bb' && oneid != 'iphone') {
        $("#two_navi").append(html_two);
    }
    $("#" + oneid).addClass("current");
    if(twoid != null && twoid != '') {
        $("#" + twoid).addClass("current");
    }
}

function tipsAlert(content) {
    $.prompt(content, {buttons:{确认:true}});
}

//全局的ajax访问，处理ajax清求时sesion超时
$.ajaxSetup({
    contentType: "application/x-www-form-urlencoded;charset=utf-8",
    complete: function (XMLHttpRequest, textStatus) {
        var sessionstatus = XMLHttpRequest.getResponseHeader("sessionstatus"); //通过XMLHttpRequest取得响应头，sessionstatus，
        if (sessionstatus == "timeout") {  //如果超时就处理 ，指定要跳转的页面
            window.location.replace(base + "/tologin.portal?realPath=/scpoi/myanav.portal");
        }
    }
});

var num2city = {
    "1101": "北京市",
    "1102": "北京市",
    "1201": "天津市",
    "1202": "天津市",
    "1301": "石家庄市",
    "1302": "唐山市",
    "1303": "秦皇岛市",
    "1304": "邯郸市",
    "1305": "邢台市",
    "1306": "保定市",
    "1307": "张家口市",
    "1308": "承德市",
    "1309": "沧州市",
    "1310": "廊坊市",
    "1311": "衡水市",
    "1401": "太原市",
    "1402": "大同市",
    "1403": "阳泉市",
    "1404": "长治市",
    "1405": "晋城市",
    "1406": "朔州市",
    "1407": "晋中市",
    "1408": "运城市",
    "1409": "忻州市",
    "1410": "临汾市",
    "1411": "临汾市",
    "1501": "呼和浩特市",
    "1502": "包头市",
    "1503": "乌海市",
    "1504": "赤峰市",
    "1505": "通辽市",
    "1506": "鄂尔多斯市",
    "1507": "呼伦贝尔市",
    "1508": "巴彦淖尔市",
    "1509": "乌兰察布市",
    "1522": "兴安盟",
    "1525": "锡林郭勒盟",
    "1529": "阿拉善盟",
    "2101": "沈阳市",
    "2102": "大连市",
    "2103": "鞍山市",
    "2104": "抚顺市",
    "2105": "本溪市",
    "2106": "丹东市",
    "2107": "锦州市",
    "2108": "营口市",
    "2109": "阜新市",
    "2110": "辽阳市",
    "2111": "盘锦市",
    "2112": "铁岭市",
    "2113": "朝阳市",
    "2114": "葫芦岛市",
    "2201": "长春市",
    "2202": "吉林市",
    "2203": "四平市",
    "2204": "辽源市",
    "2205": "通化市",
    "2206": "白山市",
    "2207": "松原市",
    "2208": "白城市",
    "2224": "延边朝鲜族自治州",
    "2301": "哈尔滨市",
    "2302": "齐齐哈尔市",
    "2303": "鸡西市",
    "2304": "鹤岗市",
    "2305": "双鸭山市",
    "2306": "大庆市",
    "2307": "伊春市",
    "2308": "佳木斯市",
    "2309": "七台河市",
    "2310": "牡丹江市",
    "2311": "黑河市",
    "2312": "绥化市",
    "2327": "大兴安岭地区",
    "3101": "上海市",
    "3102": "上海市",
    "3201": "南京市",
    "3202": "无锡市",
    "3203": "徐州市",
    "3204": "常州市",
    "3205": "苏州市",
    "3206": "南通市",
    "3207": "连云港市",
    "3208": "淮安市",
    "3209": "盐城市",
    "3210": "扬州市",
    "3211": "镇江市",
    "3212": "泰州市",
    "3213": "宿迁市",
    "3301": "杭州市",
    "3302": "宁波市",
    "3303": "温州市",
    "3304": "嘉兴市",
    "3305": "湖州市",
    "3306": "绍兴市",
    "3307": "金华市",
    "3308": "衢州市",
    "3309": "舟山市",
    "3310": "台州市",
    "3311": "丽水市",
    "3401": "合肥市",
    "3402": "芜湖市",
    "3403": "蚌埠市",
    "3404": "淮南市",
    "3405": "马鞍山市",
    "3406": "淮北市",
    "3407": "铜陵市",
    "3408": "安庆市",
    "3410": "黄山市",
    "3411": "滁州市",
    "3412": "阜阳市",
    "3413": "宿州市",
    "3414": "巢湖市",
    "3415": "六安市",
    "3416": "亳州市",
    "3417": "池州市",
    "3418": "宣城市",
    "3501": "福州市",
    "3502": "厦门市",
    "3503": "莆田市",
    "3504": "三明市",
    "3505": "泉州市",
    "3506": "漳州市",
    "3507": "南平市",
    "3508": "龙岩市",
    "3509": "宁德市",
    "3601": "南昌市",
    "3602": "景德镇市",
    "3603": "萍乡市",
    "3604": "九江市",
    "3605": "新余市",
    "3606": "鹰潭市",
    "3607": "赣州市",
    "3608": "吉安市",
    "3609": "宜春市",
    "3610": "抚州市",
    "3611": "上饶市",
    "3701": "济南市",
    "3702": "青岛市",
    "3703": "淄博市",
    "3704": "枣庄市",
    "3705": "东营市",
    "3706": "烟台市",
    "3707": "潍坊市",
    "3708": "济宁市",
    "3709": "泰安市",
    "3710": "威海市",
    "3711": "日照市",
    "3712": "莱芜市",
    "3713": "临沂市",
    "3714": "德州市",
    "3715": "聊城市",
    "3716": "滨州市",
    "3717": "菏泽市",
    "4101": "郑州市",
    "4102": "开封市",
    "4103": "洛阳市",
    "4104": "平顶山市",
    "4105": "安阳市",
    "4106": "鹤壁市",
    "4107": "新乡市",
    "4108": "焦作市",
    "4109": "濮阳市",
    "4110": "许昌市",
    "4111": "漯河市",
    "4112": "三门峡市",
    "4113": "南阳市",
    "4114": "商丘市",
    "4115": "信阳市",
    "4116": "周口市",
    "4117": "驻马店市",
    "4190": "济源市",
    "4201": "武汉市",
    "4202": "黄石市",
    "4203": "十堰市",
    "4205": "宜昌市",
    "4206": "襄樊市",
    "4207": "鄂州市",
    "4208": "荆门市",
    "4209": "孝感市",
    "4210": "荆州市",
    "4211": "黄冈市",
    "4212": "咸宁市",
    "4213": "随州市",
    "4228": "恩施土家族苗族自治州",
    "4290": "潜江市",
    "4290": "神农架林区",
    "4290": "天门市",
    "4290": "仙桃市",
    "4301": "长沙市",
    "4302": "株洲市",
    "4303": "湘潭市",
    "4304": "衡阳市",
    "4305": "邵阳市",
    "4306": "岳阳市",
    "4307": "常德市",
    "4308": "张家界市",
    "4309": "益阳市",
    "4310": "郴州市",
    "4311": "永州市",
    "4312": "怀化市",
    "4313": "娄底市",
    "4331": "湘西土家族苗族自治州",
    "4400": "东沙群岛",
    "4401": "广州市",
    "4402": "韶关市",
    "4403": "深圳市",
    "4404": "珠海市",
    "4405": "汕头市",
    "4406": "佛山市",
    "4407": "江门市",
    "4408": "湛江市",
    "4409": "茂名市",
    "4412": "肇庆市",
    "4413": "惠州市",
    "4414": "梅州市",
    "4415": "汕尾市",
    "4416": "河源市",
    "4417": "阳江市",
    "4418": "清远市",
    "4419": "东莞市",
    "4420": "中山市",
    "4451": "潮州市",
    "4452": "揭阳市",
    "4453": "云浮市",
    "4501": "南宁市",
    "4502": "柳州市",
    "4503": "桂林市",
    "4504": "梧州市",
    "4505": "北海市",
    "4506": "防城港市",
    "4507": "钦州市",
    "4508": "贵港市",
    "4509": "玉林市",
    "4510": "百色市",
    "4511": "贺州市",
    "4512": "河池市",
    "4513": "来宾市",
    "4514": "崇左市",
    "4601": "海口市",
    "4602": "三亚市",
    "4690": "白沙黎族自治县",
    "4690": "保亭黎族苗族自治县",
    "4690": "昌江黎族自治县",
    "4690": "澄迈县",
    "4690": "定安县",
    "4690": "东方市",
    "4690": "乐东黎族自治县",
    "4690": "临高县",
    "4690": "陵水黎族自治县",
    "4690": "南沙群岛",
    "4690": "琼海市",
    "4690": "琼中黎族苗族自治县",
    "4690": "屯昌县",
    "4690": "万宁市",
    "4690": "文昌市",
    "4690": "五指山市",
    "4690": "西沙群岛",
    "4690": "中沙群岛的岛礁及其海域",
    "4690": "儋州市",
    "5001": "重庆市",
    "5002": "重庆市",
    "5101": "成都市",
    "5103": "自贡市",
    "5104": "攀枝花市",
    "5105": "泸州市",
    "5106": "德阳市",
    "5107": "绵阳市",
    "5108": "广元市",
    "5109": "遂宁市",
    "5110": "内江市",
    "5111": "乐山市",
    "5113": "南充市",
    "5114": "眉山市",
    "5115": "宜宾市",
    "5116": "广安市",
    "5117": "达州市",
    "5118": "雅安市",
    "5119": "巴中市",
    "5120": "资阳市",
    "5132": "阿坝藏族羌族自治州",
    "5133": "甘孜藏族自治州",
    "5134": "凉山彝族自治州",
    "5201": "贵阳市",
    "5202": "六盘水市",
    "5203": "遵义市",
    "5204": "安顺市",
    "5222": "铜仁地区",
    "5223": "黔西南布依族苗族自治州",
    "5224": "毕节地区",
    "5226": "黔东南苗族侗族自治州",
    "5227": "黔南布依族苗族自治州",
    "5301": "昆明市",
    "5303": "曲靖市",
    "5304": "玉溪市",
    "5305": "保山市",
    "5306": "昭通市",
    "5307": "丽江市",
    "5308": "普洱市",
    "5309": "临沧市",
    "5323": "楚雄彝族自治州",
    "5325": "红河哈尼族彝族自治州",
    "5326": "文山壮族苗族自治州",
    "5328": "西双版纳傣族自治州",
    "5329": "大理白族自治州",
    "5331": "德宏傣族景颇族自治州",
    "5333": "怒江傈僳族自治州",
    "5334": "迪庆藏族自治州",
    "5401": "拉萨市",
    "5421": "昌都地区",
    "5422": "山南地区",
    "5423": "日喀则地区",
    "5424": "那曲地区",
    "5425": "阿里地区",
    "5426": "林芝地区",
    "6101": "西安市",
    "6102": "铜川市",
    "6103": "宝鸡市",
    "6104": "咸阳市",
    "6105": "渭南市",
    "6106": "延安市",
    "6107": "汉中市",
    "6108": "榆林市",
    "6109": "安康市",
    "6110": "商洛市",
    "6201": "兰州市",
    "6202": "嘉峪关市",
    "6203": "金昌市",
    "6204": "白银市",
    "6205": "天水市",
    "6206": "武威市",
    "6207": "张掖市",
    "6208": "平凉市",
    "6209": "酒泉市",
    "6210": "庆阳市",
    "6211": "定西市",
    "6212": "陇南市",
    "6229": "临夏回族自治州",
    "6230": "甘南藏族自治州",
    "6301": "西宁市",
    "6321": "海东地区",
    "6322": "海北藏族自治州",
    "6323": "黄南藏族自治州",
    "6325": "海南藏族自治州",
    "6326": "果洛藏族自治州",
    "6327": "玉树藏族自治州",
    "6328": "海西蒙古族藏族自治州",
    "6401": "银川市",
    "6402": "石嘴山市",
    "6403": "吴忠市",
    "6404": "固原市",
    "6405": "中卫市",
    "6501": "乌鲁木齐市",
    "6502": "克拉玛依市",
    "6521": "吐鲁番地区",
    "6522": "哈密地区",
    "6523": "昌吉回族自治州",
    "6527": "博尔塔拉蒙古自治州",
    "6528": "巴音郭楞蒙古自治州",
    "6529": "阿克苏地区",
    "6530": "克孜勒苏柯尔克孜自治州",
    "6531": "喀什地区",
    "6532": "和田地区",
    "6540": "伊犁哈萨克自治州",
    "6542": "塔城地区",
    "6543": "阿勒泰地区",
    "6590": "阿拉尔市",
    "6590": "石河子市",
    "6590": "图木舒克市",
    "6590": "五家渠市",
    "7100": "台湾省",
    "8100": "香港特别行政区",
    "8200": "澳门特别行政区"
};

var phones = [
    {"nameen": "samsung", "id": 1, "name": "三星", "phones": [
        {"osid": 2, "yuzhuang": "yes", "name": "S8530"},
        {"osid": 2, "yuzhuang": "yes", "name": "YP-G70"},
        {"osid": 2, "name": "Bigfoot"},
        {"osid": 2, "name": "D600 Conquer 4G"},
        {"osid": 2, "name": "Dart t499"},
        {"osid": 2, "name": "Droid Charge"},
        {"osid": 2, "name": "Galaxy 2"},
        {"osid": 2, "name": "Galaxy Indulge"},
        {"osid": 2, "name": "GALAXY Neo"},
        {"osid": 2, "name": "Galaxy Prevail"},
        {"osid": 2, "name": "Galaxy Pro B7510"},
        {"osid": 2, "name": "GALAXY S Plus"},
        {"osid": 2, "name": "Galaxy S Pro"},
        {"osid": 2, "name": "Gravity Touch 2"},
        {"osid": 2, "name": "Gravity Smart"},
        {"osid": 2, "yuzhuang": "yes", "name": "GT-P1000"},
        {"osid": 2, "yuzhuang": "yes", "name": "GT-P1010"},
        {"osid": 2, "name": "i510"},
        {"osid": 2, "name": "i5500"},
        {"osid": 2, "name": "i5508"},
        {"osid": 2, "name": "i5700"},
        {"osid": 2, "name": "i6500U"},
        {"osid": 2, "yuzhuang": "yes", "name": "i809"},
        {"osid": 2, "name": "i8520"},
        {"osid": 2, "name": "i899"},
        {"osid": 2, "name": "i9000"},
        {"osid": 2, "yuzhuang": "yes", "name": "i9003"},
        {"osid": 2, "yuzhuang": "yes", "name": "i9008"},
        {"osid": 2, "name": "i9010"},
        {"osid": 2, "name": "i9020"},
        {"osid": 2, "name": "i9023"},
        {"osid": 2, "name": "i9088"},
        {"osid": 2, "yuzhuang": "yes", "name": "i9100"},
        {"osid": 2, "yuzhuang": "yes", "name": "i9108"},
        {"osid": 2, "name": "i9103"},
        {"osid": 2, "name": "i9300"},
        {"osid": 2, "name": "i997"},
        {"osid": 2, "name": "M100S"},
        {"osid": 2, "name": "M190S"},
        {"osid": 2, "yuzhuang": "yes", "name": "M8910U"},
        {"osid": 2, "name": "S5570"},
        {"osid": 2, "name": "S5670"},
        {"osid": 2, "yuzhuang": "yes", "name": "S5830"},
        {"osid": 2, "yuzhuang": "yes", "name": "S8000C"},
        {"osid": 2, "yuzhuang": "yes", "name": "SCH-F859"},
        {"osid": 2, "name": "SCH-i559"},
        {"osid": 2, "name": "SCH-i569"},
        {"osid": 2, "name": "SCH-i579"},
        {"osid": 2, "name": "SCH-I909"},
        {"osid": 2, "name": "SCH-T959"},
        {"osid": 2, "name": "SCH-W899"},
        {"osid": 2, "name": "SGH-T759"},
        {"osid": 2, "name": "SMT-i9100"},
        {"osid": 2, "name": "SPH-m580"},
        {"osid": 2, "name": "Stealth"},
        {"osid": 2, "yuzhuang": "yes", "name": "YP-G1"},
        {"osid": 2, "yuzhuang": "yes", "name": "S8500"}
    ]},
    {"nameen": "moto", "id": 2, "name": "摩托罗拉", "phones": [
        {"osid": 2, "name": "Droid 2"},
        {"osid": 2, "name": "XT883"},
        {"osid": 2, "name": "i420"},
        {"osid": 2, "name": "MB611"},
        {"osid": 2, "name": "MB810"},
        {"osid": 2, "name": "MB855"},
        {"osid": 2, "name": "ME502"},
        {"osid": 2, "name": "ME511"},
        {"osid": 2, "name": "ME525"},
        {"osid": 2, "yuzhuang": "yes", "name": "ME722"},
        {"osid": 2, "name": "ME811"},
        {"osid": 2, "name": "ME860"},
        {"osid": 2, "yuzhuang": "yes", "name": "MT870"},
        {"osid": 2, "name": "Sholes"},
        {"osid": 2, "name": "Titanium"},
        {"osid": 2, "name": "Triumph"},
        {"osid": 2, "name": "XPRT"},
        {"osid": 2, "name": "XT300"},
        {"osid": 2, "name": "XT301"},
        {"osid": 2, "name": "XT316"},
        {"osid": 2, "name": "XT500"},
        {"osid": 2, "name": "XT502"},
        {"osid": 2, "name": "XT701"},
        {"osid": 2, "name": "XT702"},
        {"osid": 2, "yuzhuang": "yes", "name": "XT711"},
        {"osid": 2, "name": "XT720"},
        {"osid": 2, "name": "XT800"},
        {"osid": 2, "name": "XT806"},
        {"osid": 2, "name": "XT860"},
        {"osid": 2, "name": "XT875"},
        {"osid": 2, "name": "XT882"},
        {"osid": 2, "name": "Droid X2"}
    ]},
    {"nameen": "lenovo", "id": 3, "name": "联想", "phones": []},
    {"nameen": "htc", "id": 4, "name": "HTC", "phones": [
        {"osid": 2, "name": "A315c"},
        {"osid": 2, "name": "Wildfire S"},
        {"osid": 2, "name": "A3380"},
        {"osid": 2, "name": "A510c"},
        {"osid": 2, "name": "A6390"},
        {"osid": 2, "name": "A9292"},
        {"osid": 2, "name": "Aria"},
        {"osid": 2, "name": "Bee"},
        {"osid": 2, "name": "Desire HD"},
        {"osid": 2, "name": "Desire S"},
        {"osid": 2, "name": "Desire Z"},
        {"osid": 2, "name": "Doubleshot"},
        {"osid": 2, "name": "EVO 3D"},
        {"osid": 2, "name": "EVO Shift"},
        {"osid": 2, "name": "Holiday"},
        {"osid": 2, "name": "Incredible HD"},
        {"osid": 2, "name": "Incredible S"},
        {"osid": 2, "name": "Incredible"},
        {"osid": 2, "name": "Inspire"},
        {"osid": 2, "name": "Kingdom"},
        {"osid": 2, "name": "Legend"},
        {"osid": 2, "name": "Magic"},
        {"osid": 2, "name": "Merge"},
        {"osid": 2, "name": "MyTouch"},
        {"osid": 2, "name": "Pyramid"},
        {"osid": 2, "name": "S710d.gif"},
        {"osid": 2, "name": "Salsa"},
        {"osid": 2, "name": "Sensation"},
        {"osid": 2, "name": "Tata"},
        {"osid": 2, "name": "Thunderbolt"},
        {"osid": 2, "name": "Touch Slide"},
        {"osid": 2, "name": "Vision"},
        {"osid": 2, "name": "A3360"}
    ]},
    {"nameen": "sharp", "id": 5, "name": "夏普", "phones": [
        {"osid": 2, "yuzhuang": "yes", "name": "SH8iUC"},
        {"osid": 2, "yuzhuang": "yes", "name": "SH7228U"},
        {"osid": 2, "yuzhuang": "yes", "name": "SH7218U"}
    ]},
    {"nameen": "lg", "id": 12, "name": "LG", "phones": [
        {"osid": 2, "name": "Ally"},
        {"osid": 2, "name": "Revolution"},
        {"osid": 2, "name": "C660"},
        {"osid": 2, "name": "Genesis"},
        {"osid": 2, "name": "KH5200"},
        {"osid": 2, "name": "LU2300"},
        {"osid": 2, "name": "LU3000"},
        {"osid": 2, "name": "Optimus 3D"},
        {"osid": 2, "name": "Optimus Big"},
        {"osid": 2, "name": "Optimus C"},
        {"osid": 2, "name": "Optimus"},
        {"osid": 2, "name": "P350"},
        {"osid": 2, "name": "P503"},
        {"osid": 2, "name": "P610s"},
        {"osid": 2, "name": "P690"},
        {"osid": 2, "name": "P970"},
        {"osid": 2, "name": "P993"},
        {"osid": 2, "name": "Phoenix"},
        {"osid": 2, "name": "C550"}
    ]},
    {"nameen": "zte", "id": 6, "name": "中兴", "phones": [
        {"osid": 2, "yuzhuang": "yes", "name": "N880"},
        {"osid": 2, "yuzhuang": "yes", "name": "N880"},
        {"osid": 2, "name": "N600"},
        {"osid": 2, "name": "R750"},
        {"osid": 2, "name": "X950"},
        {"osid": 2, "name": "U810"},
        {"osid": 2, "name": "U880"},
        {"osid": 2, "name": "V880"},
        {"osid": 2, "name": "X850"},
        {"osid": 2, "name": "U802"}
    ]},
    {"nameen": "dell", "id": 9, "name": "戴尔", "phones": [
        {"osid": 2, "yuzhuang": "yes", "name": "M01M"},
        {"osid": 2, "yuzhuang": "yes", "name": "Streak"},
        {"osid": 2, "yuzhuang": "yes", "name": "Streak-10"}
    ]},
    {"nameen": "philips", "id": 7, "name": "飞利浦", "phones": [
        {"osid": 2, "yuzhuang": "yes", "name": "D813"},
        {"osid": 2, "yuzhuang": "yes", "name": "D812"}
    ]},
    {"nameen": "okwap", "id": 10, "name": "英华达", "phones": [
        {"osid": 2, "yuzhuang": "yes", "name": "C670"},
        {"osid": 2, "yuzhuang": "yes", "name": "C370"},
        {"osid": 2, "yuzhuang": "yes", "name": "C680"}
    ]},
    {"nameen": "oppo", "id": 8, "name": "OPPO", "phones": [
        {"osid": 2, "yuzhuang": "yes", "name": "X903"}
    ]},
    {"nameen": "huawei", "id": 13, "name": "华为", "phones": [
        {"osid": 2, "name": "C8500S"},
        {"osid": 2, "name": "X6"},
        {"osid": 2, "name": "C8650"},
        {"osid": 2, "name": "C8800"},
        {"osid": 2, "name": "CHT8000"},
        {"osid": 2, "name": "IDEOS X3"},
        {"osid": 2, "name": "Pulse Mini"},
        {"osid": 2, "name": "T8100"},
        {"osid": 2, "name": "T8301"},
        {"osid": 2, "name": "U8100"},
        {"osid": 2, "name": "U8110"},
        {"osid": 2, "name": "U8150"},
        {"osid": 2, "name": "U8160"},
        {"osid": 2, "name": "U8300"},
        {"osid": 2, "name": "U8500"},
        {"osid": 2, "name": "U8800"},
        {"osid": 2, "name": "X5"},
        {"osid": 2, "name": "C8600"}
    ]},
    {"nameen": "se", "id": 14, "name": "索爱", "phones": [
        {"osid": 2, "name": "E16i"},
        {"osid": 2, "name": "LT15i"},
        {"osid": 2, "name": "MK16i"},
        {"osid": 2, "name": "MT15i"},
        {"osid": 2, "name": "SK17i"},
        {"osid": 2, "name": "ST15i"},
        {"osid": 2, "name": "Xperia ray"},
        {"osid": 2, "name": "WT19i"},
        {"osid": 2, "name": "X10i"},
        {"osid": 2, "name": "Xperia active"},
        {"osid": 2, "name": "Xperia duo"},
        {"osid": 2, "name": "Xperia Play"},
        {"osid": 2, "name": "ST18i"}
    ]},
    {"nameen": "huaheng", "id": 15, "name": "华恒智器", "phones": [
        {"osid": 2, "yuzhuang": "yes", "name": "华恒智器¨ G7"}
    ]}
]