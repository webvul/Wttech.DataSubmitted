//合并单元格公共调用方法
function Merger(gridName, CellName) {
    //得到显示到界面的id集合
    var mya = $("#" + gridName + "").getDataIDs();
    //当前显示多少条
    var length = mya.length;
    for (var i = 0; i < length; i++) {
        //从上到下获取一条信息
        var before = $("#" + gridName + "").jqGrid('getRowData', mya[i]);
        //定义合并行数
        var rowSpanTaxCount = 1;
        for (j = i + 1; j <= length; j++) {
            //和上边的信息对比 如果值一样就合并行数+1 然后设置rowspan 让当前单元格隐藏
            var end = $("#" + gridName + "").jqGrid('getRowData', mya[j]);
            if (before[CellName] == end[CellName]) {
                rowSpanTaxCount++;
                $("#" + gridName + "").setCell(mya[j], CellName, '', { display: 'none' });
            } else {
                rowSpanTaxCount = 1;
                break;
            }
            $("#" + CellName + "" + mya[i] + "").attr("rowspan", rowSpanTaxCount);
        }
    }
}
//日期格式转换公用方法
function changeInt(temp) {
    return temp < 10 ? "0" + temp : temp;
}
//日期格式转换为Int型字符串
function getIntDate(date1) {
    return parseInt(getdateYear(date1) + getdateMonth(date1) + getdateDay(date1));
}
function getdateYear(date1) {
    return parseInt(date1.substr(0, 4)).toString();
}
function getdateMonth(date1) {
    var temp = parseInt(date1.substr(date1.indexOf("年") + 1, 2).replace("月", ""))
    return temp > 9 ? temp.toString() : '0' + temp.toString();
}
function getdateDay(date1) {
    var temp = parseInt(date1.substr(date1.indexOf("月") + 1, 2).replace("日", ""));
    return temp > 9 ? temp.toString() : '0' + temp.toString();
}

//获取当前日期
function dateFormate() {
    var myDate = new Date();
   return myDate.getFullYear() + "."  + (myDate.getMonth() + 1) + "."+ myDate.getDate();
}
function dateFormate2() {
    var myDate = new Date();
    return myDate.getFullYear() + "年" + (myDate.getMonth() + 1) + "月" + myDate.getDate()+"日";
}
//获取昨天的日期
function dateFormateYesterday() {
    var myDate = new Date();
    return myDate.getFullYear() + "." + (myDate.getMonth() + 1) + "." + (myDate.getDate()-1);
}
function dateFormateYesterday2() {
    var myDate = new Date();
    return myDate.getFullYear() + "年" + (myDate.getMonth() + 1) + "月" + (myDate.getDate()-1) + "日";
}
//判断当前日期7点前为昨天，7点后为今天
function GetDate() {
    if ((new Date()).getHours() < 7) {
        return dateFormateYesterday2();
    }
    else {
        return dateFormate2()
    }
}
//获取当前日期
//var myDate = new Date();
//var StartTime = myDate.getFullYear() + "-" + ((myDate.getMonth() + 1) < 10 ? "0" : "") + (myDate.getMonth() + 1) + "-" + (myDate.getDate() < 10 ? "0" : "") + myDate.getDate();

/**
 * 终极hack列冻结导致的高度问题
 * @param  {[String]} listId [列表ID]
 */
var browser = {
    // 检测是否是IE浏览器
    isIE: function () {
        var _uaMatch = $.uaMatch(navigator.userAgent);
        var _browser = _uaMatch.browser;
        if (_browser == 'msie') {
            return true;
        } else {
            return false;
        }
    },
    // 检测是否是chrome浏览器
    isChrome: function () {
        var _uaMatch = $.uaMatch(navigator.userAgent);
        var _browser = _uaMatch.browser;
        if (_browser == 'chrome') {
            return true;
        } else {
            return false;
        }
    },
    // 检测是否是Firefox浏览器
    isMozila: function () {
        var _uaMatch = $.uaMatch(navigator.userAgent);
        var _browser = _uaMatch.browser;
        if (_browser == 'mozilla') {
            return true;
        } else {
            return false;
        }
    },
    // 检测是否是Firefox浏览器
    isOpera: function () {
        var _uaMatch = $.uaMatch(navigator.userAgent);
        var _browser = _uaMatch.browser;
        if (_browser == 'webkit') {
            return true;
        } else {
            return false;
        }
    }
};

function hackHeight(listId) {
    $(listId + '_frozen tr').slice(1).each(function () {

        var rowId = $(this).attr('id');

        var frozenTdHeight = parseFloat($('td:first', this).height());
        var normalHeight = parseFloat($(listId + ' #' + $(this).attr('id')).find('td:first').height());

        // 如果冻结的列高度小于未冻结列的高度则hack之
        if (frozenTdHeight < normalHeight) {

            $('td', this).each(function () {

                /*
				 浏览器差异高度hack
				 */
                var space = 0; // opera默认使用0就可以
                if (browser.isChrome()) {
                    space = 0.6;
                } else if (browser.isIE()) {
                    space = -0.2;
                } else if (browser.isMozila()) {
                    space = 0.5;
                }

                if (!$(this).attr('style') || $(this).attr('style').indexOf('height:') == -1) {
                    $(this).attr('style', $(this).attr('style') + ";height:" + (normalHeight + space) + "px !important");
                }
            });
        }
    });
}

//假期配置弹出框位置居中
function setWindowPos(dialogId) {
    var parentHeight = $("#" + dialogId).parent().height();
    var parentWidth = $("#" + dialogId).parent().width();
    var top = ($(window).height() - parentHeight) / 2;
    var left = ($(window).width() - parentWidth) / 2;
    var pos = $("#" + dialogId).parent().css({
        "top": top,
        "left": left
    });
    return pos;
}