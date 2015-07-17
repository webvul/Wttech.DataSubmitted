var type = 1;
var grid9 = {
    scrollOffset: 0,
    autoScroll: false,
    datatype: "local",
    viewrecords: true,
    height: "auto",
    viewrecords: true,
    colNames: ['数据日期', '总交通量(万辆)', '同比增幅(%)', '进京交通量(万辆)', '出京交通量(万辆)', '出进京比', '小型客车交通量(万辆)', '同比增幅(%)', '小型客车免收通行费(万元)', '收费车辆(万辆)'],
    colModel: [
        { name: 'CalculTime', index: 'CalculTime',width:60, align: "center", sortable: false, resizable: false },
        { name: 'LineSum', index: 'LineSum', width: 95, align: "center", title: false, sortable: false, resizable: false },
        { name: 'SumGrow', index: 'SumGrow', width: 70, align: "center", title: false, sortable: false, resizable: false },
        { name: 'LineEnSum', index: 'LineEnSum', width: 100, align: "center", title: false, sortable: false, resizable: false },
        { name: 'LineExSum', index: 'LineExSum', width: 100, align: "center", title: false, sortable: false, resizable: false },
        { name: 'ExEnPer', index: 'ExEnPer', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'SmaCarFeeNum', index: 'SmaCarFeeNum', width: 120, align: "center", title: false, sortable: false, resizable: false },
        { name: 'SmaCarCompGrow', index: 'SmaCarCompGrow', width: 70, align: "center", title: false, sortable: false, resizable: false },
        { name: 'SmaCarFee', index: 'SmaCarFee', width: 150, align: "center", title: false, sortable: false, resizable: false },
        { name: 'ChagSumNum', index: 'ChagSumNum', width: 80, align: "center", title: false, sortable: false, resizable: false }
    ],
    jsonReader: {
        // root: "ReportData",
        repeatitems: false
    }
}

$(document).ready(function () {
    var timeTemp = dateFormateYesterday2();
    $("#StartTime1").val(timeTemp);
    var holidayStart = $("#holidayStart").val();
    var holidayEnd = $("#holidayEnd").val();
    if (getIntDate(timeTemp) >= getIntDate(holidayStart) && getIntDate(timeTemp) <= getIntDate(holidayEnd)) {
        $("#StartTime").val(timeTemp);
    }
    resize();
    getReport();//获取报表数据
    //判断是否为管理员，操作日期选择
    if ($("#isAdmin").val() == "true") {
        $("#date").text($("#StartTime1").val());
        $("#StartTime1").show();
        $("#StartTime").hide();
        $("#LastYearStart1").show();
        $("#LastYearStart").hide();
    }
    else {
        $("#date").text($("#StartTime").val());
        $("#StartTime").show();
        $("#StartTime1").hide();
        $("#LastYearStart").show();
        $("#LastYearStart1").hide();
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
    //预测
    $("#forcast").click(function () {
        forecast();
    });
    //导出
    $("#export").click(function () {
        if (type != 1) {
            alert("请先保存后再导出！");
            return;
        }
        var StartTime;
        var LastYearStart;
        if ($("#isAdmin").val() == "true") {
            StartTime = $("#StartTime1").val();
            LastYearStart = $("#LastYearStart1").val();
        }
        else {
            StartTime = $("#StartTime").val();
            LastYearStart = $("#LastYearStart").val();
        }
        $.ajax({
            url: '/Report/ExportReport',
            data: { "StartTime": StartTime, "LastYearStart": LastYearStart, "ReportType": 9, "HolidayId": 8 },
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
                window.location = "/DownFile/Downfile?name=" + path + "&reporttype=9";
            }
        });
    });
});
//获取报表数据
function getReport() {
    //获取当前日期
    var StartTime;
    var LastYearStart;
    if ($("#isAdmin").val() == "true") {
        StartTime = $("#StartTime1").val();
        LastYearStart = $("#LastYearStart1").val();
    }
    else {
        StartTime = $("#StartTime").val();
        LastYearStart = $("#LastYearStart").val();
    }
    $.ajax({
        url: '/Report/Getinfo',
        data: { "StartTime": StartTime, "LastYearStart": LastYearStart, "ReportType": 9 },
        dataType: "json",
        type: "Post",
        beforeSend: function () {
            isShowload(1);
        },
        complete: function () {
            isShowload(0);
        },
        success: function (data) {
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            grid9.data = data.RoadRunSit;
            $("#report9").jqGrid(grid9).trigger("reloadGrid");
            $("#report9").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            $("#report9").closest(".ui-jqgrid-bdiv").css({ 'overflow-x': 'hidden' });
        }
    });

}
//查询报表
function reloadReportGrid() {
    var StartTime;
    var LastYearStart;
    if ($("#isAdmin").val() == "true") {
        StartTime = $("#StartTime1").val();
        LastYearStart = $("#LastYearStart1").val();
    }
    else {
        StartTime = $("#StartTime").val();
        LastYearStart = $("#LastYearStart").val();
    }
    if (StartTime == '') {
        alert("请选择数据日期");
        return false;
    }
    if (LastYearStart == '') {
        alert("请选择去年同期日期");
        return false;
    }
    $.ajax({
        type: "POST",
        url: '/Report/Getinfo',
        data: { "StartTime": StartTime, "LastYearStart": LastYearStart, "ReportType": 9 },
        dataType: "json",
        beforeSend: function () {
            isShowload(1);
        },
        complete: function () {
            isShowload(0);
        },
        success: function (data) {
            $("#date").text(StartTime);
            $('#report9').GridUnload();
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            grid9.data = data.RoadRunSit;
            $("#report9").jqGrid(grid9).trigger("reloadGrid");
            $("#report9").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            $("#report9").closest(".ui-jqgrid-bdiv").css({ 'overflow-x': 'hidden' });
        }
    });
}

function resize() {
    $("body").height($(window).height()).width($(window).width());
    $("#big").height($("#maincontent").height());
    $("#creatReport").width($("#big").width() - 30);
    $(".tableDiv").width($("#big").width() - 30);
    $("#report9").jqGrid('setGridWidth', $("#maincontent").width() - 30);
    $(".report_title").width($("#big").width() - 30);
}
$(window).resize(function () {
    resize();
});
//预测弹窗
function forecast() {
    $(".forecastIcon").addClass("forecastIcon_select");
    $(".forecastIcon+span").css({ "color": "#5789DF" });
    $("#forecastWindow").frameDialog({
        src: "/BeijingReport/forecastWindow",
        onload: function () {
            if ($("#isAdmin").val() == "true") {
                $("iframe[name=dialog-frame]").contents().find("#foreDate").attr("disabled", false);
            }
            else {
                $("iframe[name=dialog-frame]").contents().find("#foreDate").attr("disabled", "disabled");
                $("iframe[name=dialog-frame]").contents().find("#foreDate").attr("readonly", "readonly");
                //$("iframe[name=dialog-frame]").contents().find("#foreDate").css("background", "#ccc");
            }
        },
        title: "报表预测",
        titleCloseIcon: true,
        resizable: false,
        dialog: {
            buttons: {
                "确定": function () {
                    var StartTime = $("iframe[name=dialog-frame]").contents().find("#foreDate").val();
                    var FloatingRange = $("iframe[name=dialog-frame]").contents().find("#fore_rank").val();
                    var val = $("iframe[name=dialog-frame]").contents().find('input:radio[name="checkRadio"]:checked').val();
                    if (val == 0) {
                        FloatingRange = FloatingRange * -1;
                    }
                    if ($("#isAdmin").val() == "true") {
                        if (StartTime == "") {
                            alert("请选择参考数据日期！");
                            return;
                        }
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
                        url: '/Report/ForecastData',
                        data: { "StartTime": StartTime, "ReportType": 9, "FloatingRange": FloatingRange },
                        success: function (data) {
                            if (data.ResultKey == 0) {
                                alert(data.ResultValue);
                            } else if (data.ResultKey == 1) {
                                var path = escape(data.ResultValue);
                                window.location = "/DownFile/Downfile?name=" + path + "&reporttype=9";
                                $("#forecastWindow").dialog("close");
                            } else {
                                alert(data.ResultValue);
                            }
                        }
                    });
                    $(this).dialog("close");
                },
                "取消": function () {
                    $(this).dialog("close");
                }
            },
            width: 260,
            height: 300
        }
    });
}