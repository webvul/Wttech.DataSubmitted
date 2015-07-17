var type = 1;
var grid10 = {
    //scrollOffset: 0,
    autoScroll: true,
    datatype: "local",
    viewrecords: true,
    //height: "auto",
    viewrecords: true,
    colNames: ['数据日期', '进京流量', '出京流量', '合计'],
    colModel: [
        { name: 'DataDate', index: 'DataDate', align: "center", sortable: false, resizable: false },
        { name: 'LineEnSum', index: 'LineEnSum', align: "center", title: false, sortable: false, resizable: false },
        { name: 'LineExSum', index: 'LineExSum', align: "center", title: false, sortable: false, resizable: false },
        { name: 'Total', index: 'Total', align: "center", title: false, sortable: false, resizable: false }
    ],
    jsonReader: {
        // root: "ReportData",
        repeatitems: false
    }
}

$(document).ready(function () {
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
        $("#StartTime").attr("disabled", false);
        $("#StartTime").attr("readonly", false);
        $("#EndTime").attr("disabled", false);
        $("#EndTime").attr("readonly", false);
    }
    else {
        $("#date").text($("#StartTime").val());
        $("#year").text($("#StartTime").val().substr(0, 4));
        $("#Holiday_List").attr("disabled", true);
        $("#sjy").show();
        $("#StartTime").show();
        $("#EndTime").show();
        $("#gly").hide();
        $("#StartTime1").hide();
        $("#EndTime1").hide();
        $("#StartTime").attr("disabled", "disabled");
        $("#StartTime").attr("readonly", "readonly");
        $("#EndTime").attr("disabled", "disabled");
        $("#EndTime").attr("readonly", "readonly");

    }

    //无数据列表弹出
    $("#noDataList").click(function () {
        if (type != 1) {
            alert("请先保存后再查看无数据收费站信息！");
            return;
        }
        window.parent.content("nodataList", "/TJReport/NoDataList", "无数据收费站信息", "500", "415");
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
            data: { "StartTime": StartTime, "EndTime": EndTime, "ReportType": 10, "HolidayId": HolidayId },
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
                window.location = "/DownFile/Downfile?name=" + path + "&reporttype=10";
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
        data: { "StartTime": StartTime, "EndTime": EndTime, "ReportType": 10, "HolidayId": HolidayId },
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
            grid10.data = data.HdayExEn;
            $("#report10").jqGrid(grid10).trigger("reloadGrid");
            $("#report10").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            var height = $("#big").height() - 300;
            if (height > $("#report10").height()) {
                height= $("#big").height() - 467;
                if (height<$("#report10").height() ||height>$("#report10").height())
                {
                    height=$("#report10").height();
                }
            }
            $("#report10").jqGrid('setGridHeight', height);
            $("#report10").closest(".ui-jqgrid-bdiv").css({ 'overflow-x': 'hidden' });
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
        data: { "StartTime": StartTime, "EndTime": EndTime, "ReportType": 10, "HolidayId": HolidayId },
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
            $('#report10').GridUnload();
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            grid10.data = data.HdayExEn;
            $("#report10").jqGrid(grid10).trigger("reloadGrid");
            $("#report10").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            var height = $("#big").height() -300;
            if(height>$("#report10").height()) {
                height=$("#big").height() -467;
                if (height<$("#report10").height() ||height>$("#report10").height())
                {
                    height=$("#report10").height();
                }
            }
            $("#report10").jqGrid('setGridHeight', height);
            $("#report10").closest(".ui-jqgrid-bdiv").css({ 'overflow-x': 'hidden' });
        }
    });
}

function resize() {
    $("body").height($(window).height()).width($(window).width());
    $("#big").height($("#maincontent").height());
    $("#creatReport").width($("#big").width() - 30);
    $(".tableDiv").width($("#big").width() - 30);
    $("#report10").jqGrid('setGridWidth', $("#maincontent").width() - 30);
    var height = $("#big").height() - 300;
    if (height>$("#report10").height()){
        height=$("#big").height() -467;
        if (height<$("#report10").height()||height>$("#report10").height())
        {
            height=$("#report10").height();
        }
        }
    $("#report10").jqGrid('setGridHeight', height);
    $(".report_title").width($("#big").width() - 30);
}
$(window).resize(function () {
    resize();
});