var type = 1;
var grid7 = {
    scrollOffset: 0,
    autoScroll: false,
    datatype: "local",
    viewrecords: true,
    //height: 132,
    viewrecords: true,
    //shrinkToFit: false,
    colNames: ['报送日期', '交通量(辆次)', '其中客车数(辆次)', '旅客数(万人次)', '交通量(辆次)', '其中客车数(辆次)', '旅客数(万人次)',''],
    colModel: [
        { name: 'CalcuTime', index: 'CalcuTime',width:80, align: "center", sortable: false, resizable: false },
        { name: 'EnTra', index: 'EnTra', width: 80, align: "center", title: false, editable: true, edittype: "text", sortable: false, resizable: false },
        { name: 'EnCar', index: 'EnCar', width: 80, align: "center", title: false, editable: true, edittype: "text", sortable: false, resizable: false },
        { name: 'EnTrav', index: 'EnTrav', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'ExTra', index: 'ExTra', width: 80, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'ExCar', index: 'ExCar', width: 80, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'ExTrav', index: 'ExTrav', width: 80, align: "center", title: false, sortable: false, resizable: false },
        { name: 'CalcuTimeUpdate', index: 'CalcuTimeUpdate',align: "center", title: false, sortable: false, resizable: false,hidden:true }
    ],
    jsonReader: {
        // root: "ReportData",
        repeatitems: false
    }
}

$(document).ready(function () {
    resize();
    //getReport();//获取报表数据
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
    //校正
    $("#check").click(function () {
        $(".checkIcon").addClass("checkIcon_select");
        $(".checkIcon+span").css({ "color": "#5789DF" });
        if (type != 1) {
            alert("请先保存后再校正！");
            return;
        }
        checkReport7();
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
            data: { "StartTime": StartTime, "EndTime": EndTime, "ReportType": 7, "HolidayId": HolidayId },
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
                window.location = "/DownFile/Downfile?name=" + path + "&reporttype=7";
            }
        });
    });
    //记录滚动条当前位置
    var scroll_current = 0;
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
            var ids = $("#report7").jqGrid("getDataIDs");
            $.each(ids, function (a, b) {
                var rowData = $("#report7").jqGrid('getRowData', b);
                if (rowData.EnTra != "" && b != "NaN" && rowData.CalcuTime != "合计") {
                    flag = 1;
                    jQuery("#report7").jqGrid('editRow', b);
                }
                textId = '#' + b + '_ExCar';
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
        }
    });
    //监听修改完成事件，并设置滚动条位置
    var setScroll = function () {
        if ($(textId).length != 0) {
            $('.ui-jqgrid-bdiv').scrollTop(scroll_current / $("#report7").height() * $("#report7").height());
            clearInterval(timer1);
        }
    }
    //保存
    $("#saveBtn").click(function () {
        $(".saveIcon").addClass("saveIcon_select");
        $(".saveIcon+span").css({ "color": "#5789DF" });
        var DataDate = $("#date").text();
        var ids = $("#report7").jqGrid("getDataIDs");
        var dataP = new Array();
        var i = 0;
        $.each(ids, function (a, b) {
            rowData = $("#report7").jqGrid('getRowData', b);
            if (rowData.EnTra != "") {
                dataP[i++] = {
                    "EnTra": $('#' + b + '_EnTra').val(),
                    "EnCar": $('#' + b + '_EnCar').val(),
                    "ExTra": $('#' + b + '_ExTra').val(),
                    "ExCar": $('#' + b + '_ExCar').val(),
                    "CalcuTime": rowData.CalcuTime,
                    "CalcuTimeUpdate": rowData.CalcuTimeUpdate
                }
            }
        });
        var ReportData = { "DataDate": DataDate, "DataInfo": dataP, "ReportType": 7 };
        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/BeijingReport/UpdateJJTHighWay",
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
            var ids = $("#report7").jqGrid("getDataIDs");
            $.each(ids, function (a, b) {
                var rowData = $("#report7").jqGrid('getRowData', b);
                jQuery("#report7").jqGrid('restoreRow', b);
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
        data: { "StartTime": StartTime, "EndTime": EndTime, "ReportType": 7, "HolidayId": HolidayId },
        dataType: "json",
        type: "Post",
        beforeSend: function () {
            isShowload(1);
           // $("#big").hide();
        },
        complete: function () {
            isShowload(0);
            //$("#big").show();
        },
        success: function (data) {
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            grid7.data = data.ReportData;
            $("#report7").jqGrid(grid7).trigger("reloadGrid");
            $("#report7").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            var height = $("#big").height() - 350;
            if (height > $("#report7").height()) {
                height = $("#big").height() - 467;
                if (height < $("#report7").height() || height > $("#report7").height()) {
                    height = $("#report7").height();
                }
            }
            $("#report7").jqGrid('setGridHeight', height);
            $("#report7").closest(".ui-jqgrid-bdiv").css({ 'overflow-x': 'hidden' });
            changeHeader();
            $(".reportRemark").children().remove();
            $(".reportRemark").append("<table class='report17'><tr><td>统计人："+data.CrtBy+"</td></tr></table>");
        }
    });

}
var isHb = false; //防止多次合并表头   
function changeHeader() {
    if (!isHb) {
        $("#report7").jqGrid('setGroupHeaders', {
            useColSpanStyle: true,
            //shrinkToFit: true,
            groupHeaders: [
               { startColumnName: 'EnTra', numberOfColumns: 3, titleText: '进京方向' },
               { startColumnName: 'ExTra', numberOfColumns: 3, titleText: '出京方向' }
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
    if (StartTime != '' && EndTime != '') {
        if (parseInt(StartTime) > parseInt(EndTime)) {
            alert("开始时间不能大于结束时间！");
            return false;
        }
    }
    $.ajax({
        type: "POST",
        url: '/Report/Getinfo',
        data: { "StartTime": StartTime, "EndTime": EndTime, "ReportType": 7, "HolidayId": HolidayId },
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
            $('#report7').GridUnload();
            isHb = false;
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            grid7.data = data.ReportData;
            $("#report7").jqGrid(grid7).trigger("reloadGrid");
            $("#report7").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            var height = $("#big").height() - 350;
            if (height > $("#report7").height()) {
                height = $("#big").height() - 467;
                if (height < $("#report7").height() || height > $("#report7").height()) {
                    height = $("#report7").height();
                }
            }
            $("#report7").jqGrid('setGridHeight', height);
            $("#report7").closest(".ui-jqgrid-bdiv").css({ 'overflow-x': 'hidden' });
            changeHeader();
            $(".reportRemark").children().remove();
            $(".reportRemark").append("<table class='report17'><tr><td>统计人：" + data.CrtBy + "</td></tr></table>");
        }
    });
}

function resize() {
    $("body").height($(window).height()).width($(window).width());
    $("#big").height($("#maincontent").height());
    $("#creatReport").width($("#big").width() - 30);
    $(".tableDiv").width($("#big").width() - 30);
    $("#report7").jqGrid('setGridWidth', $("#maincontent").width() - 30);
    var height = $("#big").height() - 350;
    if (height > $("#report7").height()) {
        height = $("#big").height() - 467;
        if (height < $("#report7").height() || height > $("#report7").height()) {
            height = $("#report7").height();
        }
    }
    $("#report7").jqGrid('setGridHeight', height);
    $(".report_title").width($("#big").width() - 30);
    if (type == 1) {
        reloadReportGrid();
    }
}
$(window).resize(function () {
    resize();
});

//报表7校正弹窗
function checkReport7() {
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
                            "EndTime":EndTime,
                            "LastYearStart": LastYearStart,
                            "LastYearEnd": LastYearEnd,
                            "FloatingRange": FloatingRange,
                            "ReportType": 7
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