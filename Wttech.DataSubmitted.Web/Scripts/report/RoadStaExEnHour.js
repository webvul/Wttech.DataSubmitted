var type = 1;
var grid13 = {
    // scrollOffset: 0,
    autoScroll: true,
    datatype: "local",
    viewrecords: true,
    shrinkToFit: false,
    //  height:230,
    viewrecords: true,
    colNames: ['高速公路名称', '收费站名称', '小时交通量', '0时(0-1)', '1时(1-2)', '2时(2-3)', '3时(3-4)', '4时(4-5)', '5时(5-6)', '6时(6-7)', '7时(7-8)', '8时(8-9)', '9时(9-10)', '10时(10-11)', '11时(11-12)', '12时(12-13)', '13时(13-14)', '14时(14-15)', '15时(15-16)', '16时(16-17)', '17时(17-18)', '18时(18-19)', '19时(19-20)', '20时(20-21)', '21时(21-22)', '22时(22-23)', '23时(23-24)', '合计',''],
    colModel: [
        {
            name: 'RoadName', index: 'RoadName', width: 110, align: "center", sortable: false, resizable: false, frozen: true,
            cellattr: function (rowId, tv, rawObject, cm, rdata) {
                //合并单元格
                return 'id=\'RoadName' + rowId + "\'";
            }
        },
        {
            name: 'StaName', index: 'StaName', width: 80, align: "center", title: false, sortable: false, resizable: false, frozen: true,
            cellattr: function (rowId, tv, rawObject, cm, rdata) {
                //合并单元格
                return 'id=\'StaName' + rowId + "\'";
            }
        },
        { name: 'TraName', index: 'TraName', width: 80, align: "center", title: false, sortable: false, resizable: false, frozen: true },
        { name: 'Count_0', index: 'Count_0', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_1', index: 'Count_1', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_2', index: 'Count_2', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_3', index: 'Count_3', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_4', index: 'Count_4', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_5', index: 'Count_5', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_6', index: 'Count_6', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_7', index: 'Count_7', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_8', index: 'Count_8', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_9', index: 'Count_9', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_10', index: 'Count_10', width: 80, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_11', index: 'Count_11', width: 80, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_12', index: 'Count_12', width: 80, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_13', index: 'Count_13', width: 80, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_14', index: 'Count_14', width: 80, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_15', index: 'Count_15', width: 80, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_16', index: 'Count_16', width: 80, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_17', index: 'Count_17', width: 80, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_18', index: 'Count_18', width: 80, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_19', index: 'Count_19', width: 80, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_20', index: 'Count_20', width: 80, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_21', index: 'Count_21', width: 80, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_22', index: 'Count_22', width: 80, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_23', index: 'Count_23', width: 80, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'Count_24', index: 'Count_24', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'IsEdit', index: 'IsEdit', align: "center", title: false, sortable: false, resizable: false, hidden: true }
    ],

    gridComplete: function () {
        var gridName = "report13";
        Merger(gridName, 'RoadName');
        Merger(gridName, 'StaName');
    },
    jsonReader: {
        // root: "ReportData",
        repeatitems: false
    }
}
$(document).ready(function () {
    var timeTemp = dateFormate2();
    $("#StartTime1").val(timeTemp);
    var holidayStart = $("#holidayStart").val();
    var holidayEnd = $("#holidayEnd").val();
    if (getIntDate(timeTemp) >= getIntDate(holidayStart) && getIntDate(timeTemp) <= getIntDate(holidayEnd)) {
        $("#StartTime").val(timeTemp);
    }
    resize();
    getReport();//获取报表数据
    var StartTime = $("#StartTime").val();
    var StartTime1 = $("#StartTime1").val();
    var datelen = $("#rep_date").text().length - 5;
    //判断是否为管理员，操作日期选择
    if ($("#isAdmin").val() == "true") {
        $("#rep_date").text(StartTime1.substr(5, datelen));
        $("#date").text(StartTime1);
        $("#StartTime1").show();
        $("#StartTime").hide();
    }
    else {
        $("#rep_date").text(StartTime.substr(5, datelen));
        $("#date").text(StartTime);
        $("#StartTime").show();
        $("#StartTime1").hide();
    }
    //无数据列表弹出
    $("#noDataList").click(function () {
        if (type != 1) {
            alert("请先保存后再查看无数据收费站信息！");
            return;
        }
        window.parent.content("nodataList", "/DaliyReport/HourNoList", "无数据收费站信息", "500", "415");
    });
    //查询
    $("#query").click(function () {
        if (type != 1) {
            alert("请先保存后再查询！");
            return;
        }
        reloadReportGrid();
    });
    //校正
    $("#check").click(function () {
        $(".checkIcon").addClass("checkIcon_select");
        $(".checkIcon+span").css({ "color": "#5789DF" });
        if (type != 1) {
            alert("请先保存后再校正！");
            return;
        }
        checkreport13();
    });
    //导出
    $("#export").click(function () {
        if (type != 1) {
            alert("请先保存后再导出！");
            return;
        }
        var StartTime;
        if ($("#isAdmin").val() == "true") {
            StartTime = $("#StartTime1").val();
        }
        else {
            StartTime = $("#StartTime").val();
        }
        $.ajax({
            url: '/Report/ExportReport',
            data: { "StartTime": StartTime, "ReportType": 13, "HolidayId": 8 },
            dataType: "json",
            type: "Post",
            beforeSend: function () {
                isShowload(1);
            },
            complete: function () {
                isShowload(0);
            },
            success: function (data) {
                //获取到的文件名先进行转码再使用，否则可能乱码
                var path = escape(data.path);
                window.location = "/DownFile/Downfile?name=" + path + "&reporttype=13";
            }
        });
    });
    //记录滚动条当前位置
    var scroll_current = 0;
    var scrollLeft_current = 0;
    //记录最后一行编辑控件的id
    var textId = "";
    //监听编辑行状态的监听器
    var timer1;
    //修改
    $("#upd").click(function () {
        $(".updIcon").addClass("updIcon_select");
        $(".updIcon+span").css({ "color": "#5789DF" });
        var flag = 0;
        if (type == 1) {
            var ids = $("#report13").jqGrid("getDataIDs");
            $.each(ids, function (a, b) {
                var rowData = $("#report13").jqGrid('getRowData', b);
                if (rowData.IsEdit == 1 && b != "NaN" && rowData.Count_24 != "") {
                    flag = 1;
                    jQuery("#report13").jqGrid('editRow', b);                    
                }
                textId = '#' + b + '_Count_23';
            })
            if (flag == 1) {
                $("#upd").hide();
                $("#saveBtn").show();
                $("#cancleBtn").show();
                $("#export").attr("disabled", "disabled");
                $("#export").find("span").first().removeClass("expIcon").addClass("unexpIcon");
                $("#check").attr("disabled", "disabled");
                $("#check").find("span").first().removeClass("checkIcon").addClass("uncheckIcon");
                $("#forcast").attr("disabled", "disabled");
                $("#forcast").find("span").first().removeClass("forecastIcon").addClass("uncheckIcon");
                type = 0;
            }
            else {
                alert("当前数据日期无数据，不可以进行修改！");
                $(".updIcon").removeClass("updIcon_select");
                $(".updIcon+span").css({ "color": "#555" });
            }
            //启动监听器
            timer1 = setTimeout(setScroll, 1);
            //记录当前滚动条位置，该位置代表超出显示区域的高度
            scroll_current = $('.ui-jqgrid-bdiv').scrollTop();
            scrollLeft_current = $('.ui-jqgrid-bdiv').scrollLeft();
        }

    });
    //监听修改完成事件，并设置滚动条位置
    var setScroll = function () {
        if ($(textId).length != 0) {
            $('.ui-jqgrid-bdiv').scrollTop(scroll_current / $("#report13").height() * $("#report13").height());
            $('.ui-jqgrid-bdiv').scrollLeft(scrollLeft_current / $("#report13").width() * $("#report13").width());
            clearInterval(timer1);
        }
    }
    //保存
    $("#saveBtn").click(function () {
        $(".saveIcon").addClass("saveIcon_select");
        $(".saveIcon+span").css({ "color": "#5789DF" });
        var DataDate = $("#date").text();
        var ids = $("#report13").jqGrid("getDataIDs");
        var dataP = new Array();
        var i = 0;
        $.each(ids, function (a, b) {
            rowData = $("#report13").jqGrid('getRowData', b);
            if (rowData.IsEdit != 0) {
                dataP[i++] = {
                    "StaName": rowData.StaName,
                    "TraName":rowData.TraName,
                    "Count_0": $('#' + b + '_Count_0').val(),
                    "Count_1": $('#' + b + '_Count_1').val(),
                    "Count_2": $('#' + b + '_Count_2').val(),
                    "Count_3": $('#' + b + '_Count_3').val(),
                    "Count_4": $('#' + b + '_Count_4').val(),
                    "Count_5": $('#' + b + '_Count_5').val(),
                    "Count_6": $('#' + b + '_Count_6').val(),
                    "Count_7": $('#' + b + '_Count_7').val(),
                    "Count_8": $('#' + b + '_Count_8').val(),
                    "Count_9": $('#' + b + '_Count_9').val(),
                    "Count_10": $('#' + b + '_Count_10').val(),
                    "Count_11": $('#' + b + '_Count_11').val(),
                    "Count_12": $('#' + b + '_Count_12').val(),
                    "Count_13": $('#' + b + '_Count_13').val(),
                    "Count_14": $('#' + b + '_Count_14').val(),
                    "Count_15": $('#' + b + '_Count_15').val(),
                    "Count_16": $('#' + b + '_Count_16').val(),
                    "Count_17": $('#' + b + '_Count_17').val(),
                    "Count_18": $('#' + b + '_Count_18').val(),
                    "Count_19": $('#' + b + '_Count_19').val(),
                    "Count_20": $('#' + b + '_Count_20').val(),
                    "Count_21": $('#' + b + '_Count_21').val(),
                    "Count_22": $('#' + b + '_Count_22').val(),
                    "Count_23": $('#' + b + '_Count_23').val()
                }
            }
        });
        var ReportData = { "DataDate": DataDate, "UpdateData": dataP, "ReportType": 13 };
        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/BeijingReport/UpdateRoadStaExEnHour",
            data: ReportData,
            success: function (data) {
                if (data.ResultKey == 1) {
                    alert(data.ResultValue);
                    type = 1;
                    reloadReportGrid();
                    $("#upd").show();
                    $("#saveBtn").hide();
                    $("#cancleBtn").hide();
                    $("#export").removeAttr("disabled");
                    $("#export").find("span").first().removeClass("unexpIcon").addClass("expIcon");
                    $("#check").removeAttr("disabled");
                    $("#check").find("span").first().removeClass("uncheckIcon").addClass("checkIcon");
                    $("#forcast").removeAttr("disabled");
                    $("#forcast").find("span").first().removeClass("forecastIcon").addClass("forecastIcon");
                    $(".saveIcon").removeClass("saveIcon_select");
                    $(".saveIcon+span").css({ "color": "#555" });
                } else if (data.ResultKey == 0) {
                    alert(data.ResultValue);
                    $(".saveIcon").removeClass("saveIcon_select");
                    $(".saveIcon+span").css({ "color": "#555" });
                } else {
                    alert(data.ResultValue);
                    $(".saveIcon").removeClass("saveIcon_select");
                    $(".saveIcon+span").css({ "color": "#555" });
                }

            },
            error: function (data) {
                alert(data.ResultValue);
                $(".saveIcon").removeClass("saveIcon_select");
                $(".saveIcon+span").css({ "color": "#555" });
            }
        });
        $(".updIcon").removeClass("updIcon_select");
        $(".updIcon+span").css({ "color": "#555" });
    });
    //取消
    $("#cancleBtn").click(function () {
        $(".cancleIcon").addClass("cancleIcon_select");
        $(".cancleIcon+span").css({ "color": "#5789DF" });
        if (confirm("是否取消对当前报表的修改？")) {
            type = 1;
            var ids = $("#report13").jqGrid("getDataIDs");
            $.each(ids, function (a, b) {
                var rowData = $("#report13").jqGrid('getRowData', b);
                jQuery("#report13").jqGrid('restoreRow', b);
            })
            $("#upd").show();
            $("#saveBtn").hide();
            $("#cancleBtn").hide();
            $("#export").removeAttr("disabled");
            $("#export").find("span").first().removeClass("unexpIcon").addClass("expIcon");
            $("#check").removeAttr("disabled");
            $("#check").find("span").first().removeClass("uncheckIcon").addClass("checkIcon");
            $("#forcast").removeAttr("disabled");
            $("#forcast").find("span").first().removeClass("forecastIcon").addClass("forecastIcon");
        }
        $(".cancleIcon").removeClass("cancleIcon_select");
        $(".cancleIcon+span").css({ "color": "#555" });
        $(".updIcon").removeClass("updIcon_select");
        $(".updIcon+span").css({ "color": "#555" });
    });
});
//获取报表数据
function getReport() {
    var StartTime;
    if ($("#isAdmin").val() == "true") {
        StartTime = $("#StartTime1").val();
    }
    else {
        StartTime = $("#StartTime").val();
    }
    $.ajax({
        url: '/Report/Getinfo',
        data: { "StartTime": StartTime, "ReportType": 13 },
        dataType: "json",
        type: "Post",
        beforeSend: function () {
            isShowload(1);
        },
        complete: function () {
            isShowload(0);
        },
        success: function (data) {
            $("#rep_date").text(StartTime.substr(5, 14));
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            grid13.data = data.ReportData;
            $("#report13").jqGrid(grid13).trigger("reloadGrid");
            $("#report13").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            $("#report13").jqGrid('setGridHeight', $("#big").height() - 320);
            $("#report13").jqGrid('setFrozenColumns');
        }
    });

}
//查询报表
function reloadReportGrid() {
    var StartTime;
    if ($("#isAdmin").val() == "true") {
        StartTime = $("#StartTime1").val();
    }
    else {
        StartTime = $("#StartTime").val();
    }
    if (StartTime == '') {
        alert("请选择数据日期");
        return false;
    }
    $.ajax({
        type: "POST",
        url: '/Report/Getinfo',
        data: { "StartTime": StartTime, "ReportType": 13 },
        dataType: "json",
        beforeSend: function () {
            isShowload(1);
        },
        complete: function () {
            isShowload(0);
        },
        success: function (data) {
            $("#rep_date").text(StartTime.substr(5, 14));
            $("#date").text(StartTime);
            $('#report13').GridUnload();
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            grid13.data = data.ReportData;
            $("#report13").jqGrid(grid13).trigger("reloadGrid");
            $("#report13").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            $("#report13").jqGrid('setGridHeight', $("#big").height() - 320);
            $("#report13").jqGrid('setFrozenColumns');
        }
    });
}

function resize() {
    $("body").height($(window).height()).width($(window).width());
    $("#big").height($("#maincontent").height());
    $("#creatReport").width($("#big").width() - 30);
    $(".tableDiv").width($("#big").width() - 30);
    $("#report13").jqGrid('setGridWidth', $("#maincontent").width() - 30);
    $(".report_title").width($("#big").width() - 30);
}
$(window).resize(function () {
    resize();
});

//报表13校正弹窗
function checkreport13() {
    $("#checkRight").frameDialog({
        src: "/DaliyReport/HourCheckRight",
        onload: function () {
        },
        title: "报表校正",
        titleCloseIcon: true,
        resizable: false,
        dialog: {
            buttons: {
                "确定": function () {
                    //var StartTime;
                    //if ($("#isAdmin").val() == "true") {
                    //    StartTime = $("#StartTime1").val();
                    //}
                    //else {
                    //    StartTime = $("#StartTime").val();
                    //}
                    var StartTime = $("iframe[name=dialog-frame]").contents().find("#StartTime").text();
                    var LastYearStart = $("iframe[name=dialog-frame]").contents().find("#LastYearStart").val();
                    var StartHour = $("iframe[name=dialog-frame]").contents().find("#StartHour").val();
                    var EndHour = $("iframe[name=dialog-frame]").contents().find("#EndHour").val();
                    var FloatingRange = $("iframe[name=dialog-frame]").contents().find("#rank").val();
                    var val = $("iframe[name=dialog-frame]").contents().find('input:radio[name="checkRadio"]:checked').val();
                    if (val == 0) {
                        FloatingRange = FloatingRange * -1;
                    }
                    if (LastYearStart == "") {
                        alert("请选择参考数据日期！");
                        return;
                    }
                    if (EndHour == "" || StartHour == "") {
                        alert("请输入时间范围！");
                        return;
                    }
                    if (parseInt(StartHour) > 23 || parseInt(StartHour) < 0) {
                        alert("开始时间必须介于00-23之间！");
                        return;
                    }
                    if (parseInt(EndHour) > 24 || parseInt(EndHour) < 1) {
                        alert("结束时间必须介于01-24之间！");
                        return;
                    }
                    if (parseInt(EndHour) <= parseInt(StartHour)) {
                        alert("开始时间必须小于结束时间！");
                        return;
                    }
                    if (FloatingRange == "") {
                        alert("请输入浮动范围！");
                        return;
                    }
                    var reg = new RegExp("^[0-9]*$");
                    if (!reg.test(parseInt(FloatingRange))) {
                        alert("请输入数字");
                        return;
                    }

                    $.ajax({
                        type: "POST",
                        dataType: 'json',
                        url: "/Report/CalibrationData",
                        data: {
                            "StartTime": StartTime,
                            "LastYearStart": LastYearStart,
                            "StartHour": StartHour,
                            "EndHour": parseInt(EndHour) - 1,
                            "FloatingRange": FloatingRange,
                            "ReportType": 13
                        },
                        success: function (data) {
                            if (data.ResultKey == 0) {
                                alert(data.ResultValue);
                            } else if (data.ResultKey == 1) {
                                alert(data.ResultValue);
                                $("#checkRight").dialog("close");
                                $("#query").click();
                            } else {
                                alert(data.ResultValue);
                            }
                        },
                        error: function (data) {
                            alert(data.ResultValue);
                        }

                    });
                    $(".checkIcon").removeClass("checkIcon_select");
                    $(".checkIcon+span").css({ "color": "#555" });
                },
                "取消": function () {
                    $(this).dialog("close");
                    $(".checkIcon").removeClass("checkIcon_select");
                    $(".checkIcon+span").css({ "color": "#555" });
                }
            },
            width: 280,
            height: 300
        }
    });
}