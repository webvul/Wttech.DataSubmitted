var type = 1;
var grid2 = {
   // scrollOffset: 0,
    autoScroll: true,
    datatype: "local",
    viewrecords: true,
     height: "auto",
   // forceFit: true,
    viewrecords: true,
    colNames: ['出入口', '车辆类型', '车辆数(辆)', '去年同期', '收费/免征金额(万元)', '去年同期', '车辆数(辆)', '去年同期', '收费/免征金额(万元)', '去年同期'],
    colModel: [
        {
            name: 'ExEn', index: 'ExEn', width: 25, align: "center", sortable: false, resizable: false,
            cellattr: function (rowId, tv, rawObject, cm, rdata) {
                //合并单元格
                return 'id=\'ExEn' + rowId + "\'";
            }
        },
        { name: 'VehType', index: 'VehType', width: 25, align: "center", title: false, sortable: false, resizable: false },
        { name: 'VehNum', index: 'VehNum', width: 45, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'lastSame', index: 'lastSame', width: 25, align: "center", title: false, sortable: false, resizable: false },
        { name: 'CarChag', index: 'CarChag', width: 50, align: "center", title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false },
        { name: 'lastSame', index: 'lastSame', width: 25, align: "center", title: false, sortable: false, resizable: false },
        { name: 'OrdRoad', index: 'OrdRoad', width: 45, align: "center", title: false, sortable: false, resizable: false },
        { name: 'lastSame', index: 'lastSame', width: 25, align: "center", title: false, sortable: false, resizable: false },
        { name: 'OrdCarChag', index: 'OrdCarChag', width: 45, align: "center", title: false, sortable: false, resizable: false },
         { name: 'lastSame', index: 'lastSame', width: 25, align: "center", title: false, sortable: false, resizable: false }
    ],

    gridComplete: function () {
        //②在gridComplete调用合并方法
        var gridName = "report2";
        Merger(gridName, 'ExEn');
    },
    jsonReader: {
        // root: "ReportData",
        repeatitems: false
    }
}
function myelem(value, options) {
    if (value != "") {
        var el = document.createElement("input");
        el.type = "text";
        el.value = value;
        return el;
    } else {
        return "<span></span>";
    }
}
function myvalue(elem, operation, value) {
    if (operation === 'get') {
        return $(elem).val();
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
  //  getReport();//获取报表数据
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
        checkreport2();
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
            data: { "StartTime": StartTime, "ReportType": 2, "HolidayId": 8 },
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
                window.location = "/DownFile/Downfile?name=" + path + "&reporttype=2";
            }
        });
    });
    //修改
    $("#upd").click(function () {
        $(".updIcon").addClass("updIcon_select");
        $(".ipdIcon+span").css({ "color": "#5789DF" });
        var scroll = $('.ui-jqgrid-bdiv').scrollTop();
        var flag = 0;
        if (type == 1) {
            var ids = $("#report2").jqGrid("getDataIDs");
            $.each(ids, function (a, b) {
                var rowData = $("#report2").jqGrid('getRowData', b);
               // if (rowData.VehNum != "0" && b != "NaN") {
               if (b != "NaN") {
                    flag = 1;
                    jQuery("#report2").jqGrid('editRow', b);
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
        $(".saveIcon").addClass("saveIcon_select");
        $(".saveIcon+span").css({ "color": "#5789DF" });
        var DataDate;
        if ($("#isAdmin").val() == "true") {
            DataDate = $("#StartTime1").val();
        }
        else {
            DataDate = $("#StartTime").val();
        }
        var ids = $("#report2").jqGrid("getDataIDs");
        var dataP = new Array();
        var i = 0;
        $.each(ids, function (a, b) {
            rowData = $("#report2").jqGrid('getRowData', b);
            if (rowData.VehNum != "0") {
                dataP[i++] = {
                    "ExEn": rowData.ExEn,
                    "CarChag": $('#' + b + '_CarChag').val(),
                    "VehNum": $('#' + b + '_VehNum').val(),
                    "VehType": rowData.VehType
                }
            }
        });
        var ReportData = { "DataDate": DataDate, "DataInfo": dataP, "ReportType": 2 };
        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/TJReport/UpdateTollRoadDaily",
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
            var ids = $("#report2").jqGrid("getDataIDs");
            $.each(ids, function (a, b) {
                var rowData = $("#report2").jqGrid('getRowData', b);
                jQuery("#report2").jqGrid('restoreRow', b);
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
        data: { "StartTime": StartTime, "ReportType": 2 },
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
            grid2.data = data.ReportData;
            $("#report2").jqGrid(grid2).trigger("reloadGrid");
            $("#report2").jqGrid('setGridWidth', $("#maincontent").width() - 30);
     //       $("#report2").jqGrid('setGridHeight', $(window).height() - 575);
            $("#report2").closest(".ui-jqgrid-bdiv").css({ 'overflow-x': 'hidden' });
            changeHeader();
        }
    });

}
var isHb = false; //防止多次合并表头   
function changeHeader() {
    if (!isHb) {
        $("#report2").jqGrid('setGroupHeaders', {
            useColSpanStyle: true,
            shrinkToFit: true,
            groupHeaders: [
               { startColumnName: 'VehNum', numberOfColumns: 4, titleText: '高速公路' },
               { startColumnName: 'OrdRoad', numberOfColumns: 4, titleText: '普通公路' }
            ]
        });
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
        data: { "StartTime": StartTime, "ReportType": 2 },
        dataType: "json",
        beforeSend: function () {
            isShowload(1);
        },
        complete: function () {
            isShowload(0);
        },
        success: function (data) {
            $("#date").text(StartTime);
            $('#report2').GridUnload();
            isHb = false;
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            grid2.data = data.ReportData;
            $("#report2").jqGrid(grid2).trigger("reloadGrid");
            $("#report2").jqGrid('setGridWidth', $("#maincontent").width() - 30);
    //        $("#report2").jqGrid('setGridHeight', $(window).height() - 575);
            $("#report2").closest(".ui-jqgrid-bdiv").css({ 'overflow-x': 'hidden' });
            changeHeader();
        }
    });
}

function resize() {
    $("body").height($(window).height()).width($(window).width());
    $("#big").height($("#maincontent").height());
    $("#creatReport").width($("#big").width() - 30);
    $(".tableDiv").width($("#big").width() - 30);
    $("#report2").jqGrid('setGridWidth', $("#maincontent").width() - 30);
  //  $("#report2").jqGrid('setGridHeight', $(window).height() - 575);
    $(".report_title").width($("#big").width() - 30);
    if (type == 1) {
        reloadReportGrid();
    }
}
$(window).resize(function () {
    resize();
});

//报表1校正弹窗
function checkreport2() {
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
                            "ReportType": 2
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
                        data: { "StartTime": StartTime, "ReportType": 2, "FloatingRange": FloatingRange, "HolidayId": 8 },
                        success: function (data) {
                            if (data.ResultKey == 0) {
                                alert(data.ResultValue);
                            } else if (data.ResultKey == 1) {
                                var path = escape(data.ResultValue);
                                window.location = "/DownFile/Downfile?name=" + path + "&reporttype=2";
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