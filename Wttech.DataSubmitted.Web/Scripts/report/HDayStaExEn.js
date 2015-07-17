var type = 1;
var grid11 = {
    scrollOffset: 0,
    autoScroll: false,
    datatype: "local",
    viewrecords: true,
    //shrinkToFit: false,
    //height: "auto",
    colModel: [
        { name: 'Num', index: 'Num', align: "center",width:40, sortable: false, resizable: false, frozen: true },
        { name: 'Name', index: 'Name', align: "center", width: 120, title: false, sortable: false, resizable: false, frozen: true },
        {
            name: 'Belong', index: 'Belong', align: "center", width: 100, title: false, sortable: false, resizable: false, frozen: true, cellattr: function (rowId, tv, rawObject, cm, rdata) {
                //合并单元格
                return 'id=\'Belong' + rowId + "\'";
            }
        },
        { name: 'Date1', index: 'Date1', align: "center", width: 80, title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Date2', index: 'Date2', align: "center", width: 80, title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Date3', index: 'Date3', align: "center", width: 80, title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Date4', index: 'Date4', align: "center", width: 80, title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Date5', index: 'Date5', align: "center", width: 80, title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Date6', index: 'Date6', align: "center", width: 80, title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Date7', index: 'Date7', align: "center", width: 80, title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Date8', index: 'Date8', align: "center", width: 80, title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Date9', index: 'Date9', align: "center", width: 80, title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Date10', index: 'Date10', align: "center", width: 80, title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Date11', index: 'Date11', align: "center", width: 80, title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Date12', index: 'Date12', align: "center", width: 80, title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Date13', index: 'Date13', align: "center", width: 80, title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Date14', index: 'Date14', align: "center", width: 80, title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Date15', index: 'Date15', align: "center", width: 80, title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Total', index: 'Total', align: "center", width: 80, title: false, sortable: false, resizable: false }
    ],
    gridComplete: function () {
        var gridName = "report11";
        Merger(gridName, 'Belong');
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
    //var timeTemp = dateFormateYesterday2();
    //var endTimeTemp = dateFormate2();
    //$("#StartTime1").val(timeTemp);
    //$("#EndTime1").val(endTimeTemp);
    //var holidayStart = $("#holidayStart").val();
    //var holidayEnd = $("#holidayEnd").val();
    //if (getIntDate(timeTemp) >= getIntDate(holidayStart) && getIntDate(timeTemp) <= getIntDate(holidayEnd)) {
    //    $("#StartTime").val(timeTemp);
    //}
    resize();
    getReport();//获取报表数据
    $("#report9_name").text($("#Holiday_List option:selected").text());
    //判断是否为管理员，操作日期选择
    if ($("#isAdmin").val() == "true") {
        $("#date").text($("#StartTime1").val());
        $("#year").text($("#StartTime1").val().substr(0, 4));
        $("#gly").show();
        $("#StartTime1").show();
        $("#EndTime1").show();
        $("#sjy").hide();
        $("#StartTime").hide();
        $("#EndTime").hide();
    }
    else {
        $("#date").text($("#StartTime").val());
        $("#year").text($("#StartTime").val().substr(0, 4));
        $("#Holiday_List").attr("disabled", true);
        $("#sjy").show();
        $("#StartTime").show();
        $("#EndTime").show();
        $("#StartTime").attr("disabled", true);
        $("#EndTime").attr("disabled", true);
        $("#gly").hide();
        $("#StartTime1").hide();
        $("#EndTime1").hide();
    }

    //无数据列表弹出
    $("#noDataList").click(function () {
        if (type != 1) {
            alert("请先保存后再查看无数据收费站信息！");
            return;
        }
        window.parent.content("nodataList", "/TJReport/NoDataList", "无数据收费站信息", "500", "415");
    });
    //校正
    $("#check").click(function () {
        $(".checkIcon").addClass("checkIcon_select");
        $(".checkIcon+span").css({ "color": "#5789DF" });
        if (type != 1) {
            alert("请先保存后再校正！");
            return;
        }
        checkReport11();
    });

    //查询
    $("#query").click(function () {
        if (type != 1) {
            alert("请先保存后再查询！");
            return;
        }
        reloadReportGrid();
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
        var EndTime;
        if ($("#isAdmin").val() == "true") {
            EndTime = $("#EndTime1").val();
        }
        else {
            EndTime = $("#EndTime").val();
        }
        var HolidayId = $("#Holiday_List option:selected").val();
        $.ajax({
            url: '/Report/ExportReport',
            data: { "StartTime": StartTime, "EndTime": EndTime, "ReportType": 11, "HolidayId": HolidayId },
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
                window.location = "/DownFile/Downfile?name=" + path + "&reporttype=11";
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
            var ids = $("#report11").jqGrid("getDataIDs");
            $.each(ids, function (a, b) {
                var rowData = $("#report11").jqGrid('getRowData', b);
                if (b != "NaN" && rowData.Date1 != "") {
                    flag = 1;
                    jQuery("#report11").jqGrid('editRow', b);
                    textId = '#' + b + '_Date15';
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
            $('.ui-jqgrid-bdiv').scrollTop(scroll_current / $("#report11").height() * 34 * 9);
            $('.ui-jqgrid-bdiv').scrollLeft(scrollLeft_current / $("#report11").width() * $("#report11").width());
            clearInterval(timer1);
        }
    }
    //保存
    $("#saveBtn").click(function () {
        $(".saveIcon").addClass("saveIcon_select");
        $(".saveIcon+span").css({ "color": "#5789DF" });
        var StartTime = $("#date").text();
        var EndTime;
        if ($("#isAdmin").val() == "true") {
            EndTime = $("#EndTime1").val();
        }
        else {
            EndTime = $("#EndTime").val();
        }
        var ids = $("#report11").jqGrid("getDataIDs");
        var dataP = new Array();
        var i = 0;
        $.each(ids, function (a, b) {
            rowData = $("#report11").jqGrid('getRowData', b);
            if (rowData.EnTra != "") {
                dataP[i++] = {
                    "Date1": $('#' + b + '_Date1').val(),
                    "Date2": $('#' + b + '_Date2').val(),
                    "Date3": $('#' + b + '_Date3').val(),
                    "Date4": $('#' + b + '_Date4').val(),
                    "Date5": $('#' + b + '_Date5').val(),
                    "Date6": $('#' + b + '_Date6').val(),
                    "Date7": $('#' + b + '_Date7').val(),
                    "Date8": $('#' + b + '_Date8').val(),
                    "Date9": $('#' + b + '_Date9').val(),
                    "Date10": $('#' + b + '_Date10').val(),
                    "Date11": $('#' + b + '_Date11').val(),
                    "Date12": $('#' + b + '_Date12').val(),
                    "Date13": $('#' + b + '_Date13').val(),
                    "Date14": $('#' + b + '_Date14').val(),
                    "Date15": $('#' + b + '_Date15').val(),
                    "Num": rowData.Num,
                    "Name": rowData.Name,
                    "Belong": rowData.Belong,
                    "Total": rowData.Total
                }
            }
        });
        var ReportData = { "StartTime": StartTime,"EndTime":EndTime, "DataInfo": dataP, "ReportType": 11 };
        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/TJReport/UpdateHDayStaExEn",
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
            var ids = $("#report11").jqGrid("getDataIDs");
            $.each(ids, function (a, b) {
                var rowData = $("#report11").jqGrid('getRowData', b);
                jQuery("#report11").jqGrid('restoreRow', b);
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
    var EndTime;
    if ($("#isAdmin").val() == "true") {
        EndTime = $("#EndTime1").val();
    }
    else {
        EndTime = $("#EndTime").val();
    }
    var HolidayId = $("#Holiday_List option:selected").val();
    $.ajax({
        url: '/Report/Getinfo',
        data: { "StartTime": StartTime, "EndTime": EndTime, "ReportType": 11, "HolidayId": HolidayId },
        dataType: "json",
        type: "Post",
        beforeSend: function () {
            isShowload(1);
        },
        complete: function () {
            isShowload(0);
        },
        success: function (data) {
            $.each(data.TitleList, function (a, b) {
                grid11.colNames = [b.Num, b.Name, b.Belong, b.Date1, b.Date2, b.Date3, b.Date4, b.Date5, b.Date6, b.Date7, b.Date8, b.Date9, b.Date10, b.Date11, b.Date12, b.Date13, b.Date14, b.Date15, b.Total];
            });
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            grid11.data = data.ReportData;
            $("#report11").jqGrid(grid11).trigger("reloadGrid");
            //动态显示列
            if (data.DateTotal == 1) {
                $("#report11").jqGrid('showCol', ["Date1"]);
            } else if (data.DateTotal == 2) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2"]);
            } else if (data.DateTotal == 3) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3"]);
            } else if (data.DateTotal == 4) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4"]);
            } else if (data.DateTotal == 5) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5"]);
            } else if (data.DateTotal == 6) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6"]);
            } else if (data.DateTotal == 7) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7"]);
            } else if (data.DateTotal == 8) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7", "Date8"]);
            } else if (data.DateTotal == 9) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7", "Date8", "Date9"]);
            } else if (data.DateTotal == 10) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7", "Date8", "Date9", "Date10"]);
            } else if (data.DateTotal == 11) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7", "Date8", "Date9", "Date10", "Date11"]);
            } else if (data.DateTotal == 12) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7", "Date8", "Date9", "Date10", "Date11", "Date12"]);
            } else if (data.DateTotal == 13) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7", "Date8", "Date9", "Date10", "Date11", "Date12", "Date13"]);
            } else if (data.DateTotal == 14) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7", "Date8", "Date9", "Date10", "Date11", "Date12", "Date13", "Date14"]);
            } else if (data.DateTotal == 15) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7", "Date8", "Date9", "Date10", "Date11", "Date12", "Date13", "Date14", "Date15"]);
            }
            $("#report11").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            var height = $("#big").height() - 300;
            if (height > $("#report11").height()) {
                height = $("#big").height() - 466;
                if (height < $("#report11").height() || height > $("#report11").height()) {
                    height = $("#report11").height() + 18;
                }
            }
            $("#report11").jqGrid('setGridHeight', height);
            //$("#report11").jqGrid('setFrozenColumns');
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
    var EndTime;
    if ($("#isAdmin").val() == "true") {
        EndTime = $("#EndTime1").val();
    }
    else {
        EndTime = $("#EndTime").val();
    }
    var HolidayId = $("#Holiday_List option:selected").val();
    if (StartTime == '') {
        alert("请选择开始日期");
        return false;
    }
    if (EndTime == '') {
        alert("请选择结束日期");
        return false;
    }
    $.ajax({
        type: "POST",
        url: '/Report/Getinfo',
        data: { "StartTime": StartTime, "EndTime": EndTime,"ReportType": 11, "HolidayId": HolidayId },
        dataType: "json",
        beforeSend: function () {
            isShowload(1);
        },
        complete: function () {
            isShowload(0);
        },
        success: function (data) {
            $("#date").text(StartTime);
            $("#report9_name").text($("#Holiday_List option:selected").text());
            $("#year").text(StartTime.substr(0, 4));
            $('#report11').GridUnload();
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            $.each(data.TitleList, function (a, b) {
                grid11.colNames = [b.Num, b.Name, b.Belong, b.Date1, b.Date2, b.Date3, b.Date4, b.Date5, b.Date6, b.Date7, b.Date8, b.Date9, b.Date10, b.Date11, b.Date12, b.Date13, b.Date14, b.Date15, b.Total];
            })
            grid11.data = data.ReportData;
            $("#report11").jqGrid(grid11).trigger("reloadGrid");
            //动态显示列
            if (data.DateTotal == 1) {
                $("#report11").jqGrid('showCol', ["Date1"]);
            } else if (data.DateTotal == 2) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2"]);
            } else if (data.DateTotal == 3) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3"]);
            } else if (data.DateTotal == 4) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4"]);
            } else if (data.DateTotal == 5) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5"]);
            } else if (data.DateTotal == 6) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6"]);
            } else if (data.DateTotal == 7) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7"]);
            } else if (data.DateTotal == 8) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7", "Date8"]);
            } else if (data.DateTotal == 9) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7", "Date8", "Date9"]);
            } else if (data.DateTotal == 10) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7", "Date8", "Date9", "Date10"]);
            } else if (data.DateTotal == 11) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7", "Date8", "Date9", "Date10", "Date11"]);
            } else if (data.DateTotal == 12) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7", "Date8", "Date9", "Date10", "Date11", "Date12"]);
            } else if (data.DateTotal == 13) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7", "Date8", "Date9", "Date10", "Date11", "Date12", "Date13"]);
            } else if (data.DateTotal == 14) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7", "Date8", "Date9", "Date10", "Date11", "Date12", "Date13", "Date14"]);
            } else if (data.DateTotal == 15) {
                $("#report11").jqGrid('showCol', ["Date1", "Date2", "Date3", "Date4", "Date5", "Date6", "Date7", "Date8", "Date9", "Date10", "Date11", "Date12", "Date13", "Date14", "Date15"]);
            }
            $("#report11").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            var height = $("#big").height() - 300;
            if (height > $("#report11").height()) {
                height = $("#big").height() - 466;
                if (height < $("#report11").height() || height > $("#report11").height()) {
                    height = $("#report11").height() + 18;
                }
            }
            $("#report11").jqGrid('setGridHeight', height);
            //$("#report11").jqGrid('setFrozenColumns');
        }
    });
}

function resize() {
    $("body").height($(window).height()).width($(window).width());
    $("#big").height($("#maincontent").height());
    $("#creatReport").width($("#big").width() - 30);
    $(".tableDiv").width($("#big").width() - 30);
    $("#report11").jqGrid('setGridWidth', $("#maincontent").width() - 30);
    var height = $("#big").height() - 300;
    if (height > $("#report11").height()) {
        height = $("#big").height() - 467;
        if (height < $("#report11").height() || height > $("#report11").height()) {
            height = $("#report11").height() + 17;
        }
    }
    $("#report11").jqGrid('setGridHeight', height);
    $(".report_title").width($("#big").width() - 30);
}
$(window).resize(function () {
    resize();
});

//报表11校正弹窗
function checkReport11() {
    $("#Check_rep7").frameDialog({
        src: "/BeijingReport/checkReport7",
        onload: function () {
        },
        title: "报表校正",
        titleCloseIcon: true,
        resizable: false,
        dialog: {
            buttons: {
                "确定": function () {
                    var StartTime = $("iframe[name=dialog-frame]").contents().find("#StartTime").text();
                    var EndTime = $("iframe[name=dialog-frame]").contents().find("#EndTime").text();
                    var LastYearStart = $("iframe[name=dialog-frame]").contents().find("#LastYearStart").val();
                    var LastYearEnd = $("iframe[name=dialog-frame]").contents().find("#LastYearEnd").val();
                    var FloatingRange = $("iframe[name=dialog-frame]").contents().find("#rank").val();
                    var val = $("iframe[name=dialog-frame]").contents().find('input:radio[name="checkRadio"]:checked').val();
                    if (val == 0) {
                        FloatingRange = FloatingRange * -1;
                    }
                    if (LastYearStart == "") {
                        alert("请选择数据参考开始日期！");
                        return;
                    }
                    if (LastYearEnd == "") {
                        alert("请选择数据参考结束日期！");
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
                            "EndTime": EndTime,
                            "LastYearStart": LastYearStart,
                            "LastYearEnd": LastYearEnd,
                            "FloatingRange": FloatingRange,
                            "ReportType": 11
                        },
                        success: function (data) {
                            if (data.ResultKey == 0) {
                                alert(data.ResultValue);
                            } else if (data.ResultKey == 1) {
                                alert(data.ResultValue);
                                $("#Check_rep7").dialog("close");
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
            width: 400,
            height: 300
        }
    });
}