var type = 1;
var grid6 = {
    scrollOffset: 0,
    autoScroll: false,
    datatype: "local",
    viewrecords: true,
    height: "auto",
    viewrecords: true,
    colNames: ['', '小型车辆', '其他客车', '货车', '绿色通道', '小型车辆', '其他客车', '货车', '绿色通道', '小型车辆', '其他客车', '货车', '绿色通道', '小型车辆', '其他客车', '货车', '绿色通道',''],
    colModel: [
        { name: 'Name', index: 'Name', width: 60, align: "center", sortable: false, resizable: false, title: false },
        { name: 'EnSmaCar', index: 'EnSmaCar', width: 45, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'EnOthCar', index: 'EnOthCar', width: 45, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'EnTruk', index: 'EnTruk', width: 35, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'EnGre', index: 'EnGre', width: 45, align: "center", title: false, sortable: false, resizable: false },
        { name: 'ExSmaCar', index: 'ExSmaCar', width: 45, align: "center", title: false, sortable: false, resizable: false },
        { name: 'ExOthCar', index: 'ExOthCar', width: 45, align: "center", title: false, sortable: false,resizable: false },
        { name: 'ExTruk', index: 'ExTruk', width: 35, align: "center", title: false, sortable: false, resizable: false },
        { name: 'ExGre', index: 'ExGre', width: 45, align: "center", title: false, sortable: false, resizable: false },
        { name: 'PEnSmaCar', index: 'PEnSmaCar', width: 45, align: "center", title: false, sortable: false, resizable: false },
        { name: 'PEnOthCar', index: 'PEnOthCar', width: 45, align: "center", title: false, sortable: false, resizable: false },
        { name: 'PEnTruk', index: 'PEnTruk', width: 35, align: "center", title: false, sortable: false, resizable: false },
        { name: 'PEnGre', index: 'PEnGre', width: 45, align: "center", title: false, sortable: false, resizable: false },
        { name: 'PExSmaCar', index: 'PExSmaCar', width: 45, align: "center", title: false, sortable: false, resizable: false },
        { name: 'PExOthCar', index: 'PExOthCar', width: 45, align: "center", title: false, sortable: false, resizable: false },
        { name: 'PExTruk', index: 'PExTruk', width: 35, align: "center", title: false, sortable: false, resizable: false },
        { name: 'PExGre', index: 'PExGre', width: 45, align: "center", title: false, sortable: false, resizable: false },
        { name: 'CalcuTime', index: 'CalcuTime', width: 45, align: "center", title: false, sortable: false, resizable: false, hidden: true }
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
   // getReport();//获取报表数据
    //判断是否为管理员，操作日期选择
    var StartTime = $("#StartTime").val();
    var StartTime1 = $("#StartTime1").val();
    if ($("#isAdmin").val() == "true") {
        $("#date").text(StartTime1);
        $("#StartTime1").show();
        $("#StartTime").hide();
    }
    else {
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
    //预测
    $("#forcast").click(function () {
        forecast();
    });
    //校正
    $("#check").click(function () {
        $(".checkIcon").addClass("checkIcon_select");
        $(".checkIcon+span").css({ "color": "#5789DF" });
        if (type != 1) {
            alert("请先保存后再校正！");
            return;
        }
        checkreport6();
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
            data: { "StartTime": StartTime, "ReportType": 6, "HolidayId": 8 },
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
                window.location = "/DownFile/Downfile?name=" + path + "&reporttype=6";
            }
        });
    });
    //修改
    $("#upd").click(function () {
        $(".updIcon").addClass("updIcon_select");
        $(".updIcon+span").css({ "color": "#5789DF" });
        var scroll = $('.ui-jqgrid-bdiv').scrollTop();
        var flag = 0;
        if (type == 1) {
            var ids = $("#report6").jqGrid("getDataIDs");
            $.each(ids, function (a, b) {
                var rowData = $("#report6").jqGrid('getRowData', b);
                if (rowData.EnSmaCar != "" && b != "NaN" && b != "2") {
                    flag = 1;
                    jQuery("#report6").jqGrid('editRow', b);
                }
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
                $('.ui-jqgrid-bdiv').scrollTop(scroll);
            }
            else {
                alert("当前数据日期无数据，不可以进行修改！");
                $(".updIcon").removeClass("updIcon_select");
                $(".updIcon+span").css({ "color": "#555" });
            }
        }

    })
    //保存
    $("#saveBtn").click(function () {
        //var DataDate;
        //if ($("#isAdmin").val() == "true") {
        //    DataDate = $("#StartTime1").val();
        //}
        //else {
        //    DataDate = $("#StartTime").val();
        //}
        $(".saveIcon").addClass("saveIcon_select");
        $(".saveIcon+span").css({ "color": "#5789DF" });
        var DataDate = $("#date").text();
        var ids = $("#report6").jqGrid("getDataIDs");
        var dataP = new Array();
        var i = 0;
        $.each(ids, function (a, b) {
            rowData = $("#report6").jqGrid('getRowData', b);
            if (rowData.EnSmaCar != "" && b != "2") {
                dataP[i++] = {
                    "EnSmaCar": $('#' + b + '_EnSmaCar').val(),
                    "EnOthCar": $('#' + b + '_EnOthCar').val(),
                    "EnTruk": $('#' + b + '_EnTruk').val(),
                   // "EnGre": $('#' + b + '_EnGre').val(),
                    "CalcuTime": rowData.CalcuTime
                }
            }
        });
        var ReportData = { "DataDate": DataDate, "DataInfo": dataP, "ReportType": 6 };
        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/TJReport/UpdateCityDailyEnEx",
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
            var ids = $("#report6").jqGrid("getDataIDs");
            $.each(ids, function (a, b) {
                var rowData = $("#report6").jqGrid('getRowData', b);
                jQuery("#report6").jqGrid('restoreRow', b);
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
        data: { "StartTime": StartTime, "ReportType": 6 },
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
            grid6.data = data.ReportData;
            $("#report6").jqGrid(grid6).trigger("reloadGrid");
            $("#report6").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            $("#report6").closest(".ui-jqgrid-bdiv").css({ 'overflow-x': 'hidden' });
            changeHeader();
        }
    });
}

var isHb = false; //防止多次合并表头   
function changeHeader() {
    var complexoption = {
        complexHeaders: {
            defaultStyle: true,
            threeLevel: [
                    { startColumnName: "EnSmaCar", numberOfColumns: 8, titleText: "高速公路" },
                    { startColumnName: "PEnSmaCar", numberOfColumns: 8, titleText: "普通公路" }
            ],
            twoLevel: [
               { startColumnName: "EnSmaCar", numberOfColumns: 4, titleText: "入境" },
               { startColumnName: "ExSmaCar", numberOfColumns: 4, titleText: "出境" },
               { startColumnName: "PEnSmaCar", numberOfColumns: 4, titleText: "入境" },
               { startColumnName: "PExSmaCar", numberOfColumns: 4, titleText: "出境" }
            ]
        }
    };
    if (!isHb) {
        $("#report6").jqGrid('setComplexHeaders',  complexoption);
    }
    isHb = true;
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
        data: { "StartTime": StartTime, "ReportType": 6 },
        dataType: "json",
        beforeSend: function () {
            isShowload(1);
        },
        complete: function () {
            isShowload(0);
        },
        success: function (data) {
            $("#date").text(StartTime);
            $('#report6').GridUnload();
            isHb = false;
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            grid6.data = data.ReportData;
            $("#report6").jqGrid(grid6).trigger("reloadGrid");
            $("#report6").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            $("#report6").closest(".ui-jqgrid-bdiv").css({ 'overflow-x': 'hidden' });
            changeHeader();
        }
    });
}

function resize() {
    $("body").height($(window).height()).width($(window).width());
    $("#big").height($("#maincontent").height());
    $("#creatReport").width($("#big").width() - 30);
    $(".tableDiv").width($("#big").width() - 30);
    $("#report6").jqGrid('setGridWidth', $("#maincontent").width() - 30);
    $(".report_title").width($("#big").width() - 30);
    if (type == 1) {
        reloadReportGrid();
    }
}
$(window).resize(function () {
    resize();
});

//报表6校正弹窗
function checkreport6() {
    $("#Check_noTime").frameDialog({
        src: "/BeijingReport/BJCheckWindow",
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
                    var FloatingRange = $("iframe[name=dialog-frame]").contents().find("#rank").val();
                    var val = $("iframe[name=dialog-frame]").contents().find('input:radio[name="checkRadio"]:checked').val();
                    if (val == 0) {
                        FloatingRange = FloatingRange * -1;
                    }
                    if (LastYearStart == "") {
                        alert("请选择参考数据日期！");
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
                            "FloatingRange": FloatingRange,
                            "ReportType": 6
                        },
                        success: function (data) {
                            if (data.ResultKey == 0) {
                                alert(data.ResultValue);
                            } else if (data.ResultKey == 1) {
                                alert(data.ResultValue);
                                $("#Check_noTime").dialog("close");
                                $("#query").click();
                            } else {
                                alert(data.ResultValue);
                            }
                        },
                        error: function (data) {
                            alert(data.ResultValue);
                        }

                    });
                },
                "取消": function () {
                    $(this).dialog("close");
                }
            },
            width: 280,
            height: 300
        }
    });
}
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
                        data: { "StartTime": StartTime, "ReportType": 6, "FloatingRange": FloatingRange, "HolidayId": 8 },
                        success: function (data) {
                            if (data.ResultKey == 0) {
                                alert(data.ResultValue);
                            } else if (data.ResultKey == 1) {
                                var path = escape(data.ResultValue);
                                window.location = "/DownFile/Downfile?name=" + path + "&reporttype=6";
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