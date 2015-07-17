var type = 1;
var reportgrid = {
   // scrollOffset: 0,
    autoScroll: true,
    datatype: "local",
    viewrecords: true,
    shrinkToFit: false,
  //  height:230,
    viewrecords: true,
    colNames: ['高速公路名称', '收费站名称', '', '0时(0-1)', '1时(1-2)', '2时(2-3)', '3时(3-4)', '4时(4-5)', '5时(5-6)', '6时(6-7)', '7时(7-8)', '8时(8-9)', '9时(9-10)', '10时(10-11)', '11时(11-12)', '12时(12-13)', '13时(13-14)', '14时(14-15)', '15时(15-16)', '16时(16-17)', '17时(17-18)', '18时(18-19)', '19时(19-20)', '20时(20-21)', '21时(21-22)', '22时(22-23)', '23时(23-24)','合计'],
    colModel: [
        {
            name: 'RoadName', index: 'RoadName', width: 110, align: "center", sortable: false, resizable: false,frozen: true,
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
        { name: 'TraName', index: 'TraName', width: 60, align: "center", title: false, sortable: false, resizable: false, frozen: true },
        { name: 'Count_0', index: 'Count_0', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_1', index: 'Count_1', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_2', index: 'Count_2', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_3', index: 'Count_3', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_4', index: 'Count_4', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_5', index: 'Count_5', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_6', index: 'Count_6', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_7', index: 'Count_7', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_8', index: 'Count_8', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_9', index: 'Count_9', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_10', index: 'Count_10', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_11', index: 'Count_11', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_12', index: 'Count_12', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_13', index: 'Count_13', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_14', index: 'Count_14', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_15', index: 'Count_15', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_16', index: 'Count_16', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_17', index: 'Count_17', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_18', index: 'Count_18', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_19', index: 'Count_19', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_20', index: 'Count_20', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_21', index: 'Count_21', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_22', index: 'Count_22', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_23', index: 'Count_23', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'Count_24', index: 'Count_24', width: 80, align: "center", title: false, sortable: false, resizable: false }
    ],

    gridComplete: function () {
        var gridName = "report14";
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
    //判断是否为管理员，操作日期选择
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
            data: { "StartTime": StartTime, "ReportType": 14, "HolidayId": 8 },
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
                window.location = "/DownFile/Downfile?name=" + path + "&reporttype=14";
            }
        });
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
        data: { "StartTime": StartTime, "ReportType": 14 },
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
            reportgrid.data = data.ReportData;
            $("#report14").jqGrid(reportgrid).trigger("reloadGrid");
            $("#report14").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            $("#report14").jqGrid('setGridHeight', $("#big").height() - 320);
            $("#report14").jqGrid('setFrozenColumns');
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
        data: { "StartTime": StartTime, "ReportType": 14 },
        dataType: "json",
        beforeSend: function () {
            isShowload(1);
        },
        complete: function () {
            isShowload(0);
        },
        success: function (data) {
            $("#date").text(StartTime);
            $('#report14').GridUnload();
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            reportgrid.data = data.ReportData;
            $("#report14").jqGrid(reportgrid).trigger("reloadGrid");
            $("#report14").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            $("#report14").jqGrid('setGridHeight', $("#big").height() - 320);
            $("#report14").jqGrid('setFrozenColumns');
        }
    });
}

function resize() {
    $("body").height($(window).height()).width($(window).width());
    $("#big").height($("#maincontent").height());
    $("#creatReport").width($("#big").width() - 30);
    $(".tableDiv").width($("#big").width() - 30);
    $("#report14").jqGrid('setGridWidth', $("#maincontent").width() - 30);
    $("#report14").jqGrid('setGridHeight', $("#big").height() - 320);
    $(".report_title").width($("#big").width() - 30);
}
$(window).resize(function () {
    resize();
});