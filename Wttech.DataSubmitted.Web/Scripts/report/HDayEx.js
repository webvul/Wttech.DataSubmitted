var type = 1;
var grid12 = {
    //scrollOffset: 0,
    autoScroll: true,
    datatype: "local",
    viewrecords: true,
    shrinkToFit: false,
    //height: "auto",
    colModel: [
        { name: 'Num', index: 'Num', align: "center", sortable: false, resizable: false, frozen: true },
        { name: 'RoadName', index: 'RoadName', align: "center", title: false, sortable: false, resizable: false, frozen: true },
        { name: 'Tra1', index: 'Tra1', align: "center", title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Tra2', index: 'Tra2', align: "center", title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Tra3', index: 'Tra3', align: "center", sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Tra4', index: 'Tra4', align: "center", title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Tra5', index: 'Tra5', align: "center", title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Tra6', index: 'Tra6', align: "center", title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Tra7', index: 'Tra7', align: "center", sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Tra8', index: 'Tra8', align: "center", title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Tra9', index: 'Tra9', align: "center", title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Tra10', index: 'Tra10', align: "center", title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Tra11', index: 'Tra11', align: "center", sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Tra12', index: 'Tra12', align: "center", title: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, sortable: false, resizable: false, hidden: true },
        { name: 'Tra13', index: 'Tra13', align: "center", title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Tra14', index: 'Tra14', align: "center", title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Tra15', index: 'Tra15', align: "center", title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, resizable: false, hidden: true },
        { name: 'Sum', index: 'Sum', align: "center", title: false, sortable: false, resizable: false },
        { name: 'LastSum', index: 'LastSum', align: "center", title: false, sortable: false, resizable: false },
        { name: 'Growth', index: 'Growth', align: "center", title: false, sortable: false, resizable: false }
    ],
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
        $("#gly").hide();
        $("#StartTime1").hide();
        $("#EndTime1").hide();
        $("#StartTime").attr("disabled", true);
        $("#EndTime").attr("disabled", true);
        $("#LastYearStart").attr("disabled", "disabled");
        $("#LastYearStart").attr("readonly", "readonly");
        $("#LastYearEnd").attr("disabled", "disabled");
        $("#LastYearEnd").attr("readonly", "readonly");
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
        checkReport12();
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
        var LastYearStart = $("#LastYearStart").val();
        var LastYearEnd = $("#LastYearEnd").val();
        var HolidayId = $("#Holiday_List option:selected").val();
        $.ajax({
            url: '/Report/ExportReport',
            data: { "StartTime": StartTime, "EndTime": EndTime, "LastYearStart": LastYearStart, "LastYearEnd": LastYearEnd, "ReportType": 12, "HolidayId": HolidayId },
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
                window.location = "/DownFile/Downfile?name=" + path + "&reporttype=12";
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
            //var ids = $("#report12").jqGrid("getDataIDs");
            //$.each(ids, function (a, b) {
                var rowData = $("#report12").jqGrid('getRowData', 1);
                if (rowData.Tra1 != "") {
                    flag = 1;
                    jQuery("#report12").jqGrid('editRow', 1);
                    
                }
                textId = '#1_Tra15';
           // })
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
        if ($(textId).lenght != 0) {
            $('.ui-jqgrid-bdiv').scrollTop(scroll_current / $("#report12").height() * 34 * 2);
            $('.ui-jqgrid-bdiv').scrollLeft(scrollLeft_current / $("#report12").width() * $("#report12").width());
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
        var ids = $("#report12").jqGrid("getDataIDs");
        var dataP = new Array();
        var i = 0;
       // $.each(ids, function (a, b) {
            rowData = $("#report12").jqGrid('getRowData', 1);
            if (rowData.Tra1 != "") {
                dataP[i++] = {
                    "Tra1": $('#1_Tra1').val(),
                    "Tra2": $('#1_Tra2').val(),
                    "Tra3": $('#1_Tra3').val(),
                    "Tra4": $('#1_Tra4').val(),
                    "Tra5": $('#1_Tra5').val(),
                    "Tra6": $('#1_Tra6').val(),
                    "Tra7": $('#1_Tra7').val(),
                    "Tra8": $('#1_Tra8').val(),
                    "Tra9": $('#1_Tra9').val(),
                    "Tra10": $('#1_Tra10').val(),
                    "Tra11": $('#1_Tra11').val(),
                    "Tra12": $('#1_Tra12').val(),
                    "Tra13": $('#1_Tra13').val(),
                    "Tra14": $('#1_Tra14').val(),
                    "Tra15": $('#1_Tra15').val(),
                    "Num": rowData.Num
                }
            }
        //});
        var ReportData = { "StartTime": StartTime, "EndTime": EndTime, "DataInfo": dataP, "ReportType": 12 };
        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/TJReport/UpdateHDayEx",
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
            var ids = $("#report12").jqGrid("getDataIDs");
            $.each(ids, function (a, b) {
                var rowData = $("#report12").jqGrid('getRowData', b);
                jQuery("#report12").jqGrid('restoreRow', b);
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
    var LastYearStart = $("#LastYearStart").val();
    var LastYearEnd = $("#LastYearEnd").val();
    var HolidayId = $("#Holiday_List option:selected").val();
    $.ajax({
        url: '/Report/Getinfo',
        data: { "StartTime": StartTime, "EndTime": EndTime, "LastYearStart":LastYearStart, "LastYearEnd":LastYearEnd, "ReportType": 12, "HolidayId": HolidayId },
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
            //grid12.colNames = [data.TitleList[0], data.TitleList[1], data.TitleList[2], data.TitleList[3], data.TitleList[4], data.TitleList[5], data.TitleList[6], data.TitleList[7], data.TitleList[8], data.TitleList[9], data.TitleList[10], data.TitleList[11], data.TitleList[12], data.TitleList[13], data.TitleList[14], data.TitleList[15], data.TitleList[16], data.TitleList[17], data.TitleList[18], data.TitleList[19]];
            grid12.colNames = data.TitleList;
            grid12.data = data.ReportData;
            $("#report12").jqGrid(grid12).trigger("reloadGrid");
            //动态显示列
            if (data.CountDay == 1) {
                $("#report12").jqGrid('showCol', ["Tra1"]);
            } else if (data.CountDay == 2) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2"]);
            } else if (data.CountDay == 3) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3"]);
            } else if (data.CountDay == 4) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3","Tra4"]);
            } else if (data.CountDay == 5) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5"]);
            } else if (data.CountDay == 6) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6"]);
            } else if (data.CountDay == 7) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7"]);
            } else if (data.CountDay == 8) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7", "Tra8"]);
            } else if (data.CountDay == 9) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7", "Tra8", "Tra9"]);
            } else if (data.CountDay == 10) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7", "Tra8", "Tra9", "Tra10"]);
            } else if (data.CountDay == 11) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7", "Tra8", "Tra9", "Tra10","Tra11"]);
            } else if (data.CountDay == 12) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7", "Tra8", "Tra9", "Tra10", "Tra11","Tra12"]);
            } else if (data.CountDay == 13) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7", "Tra8", "Tra9", "Tra10", "Tra11", "Tra12", "Tra13"]);
            } else if (data.CountDay == 14) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7", "Tra8", "Tra9", "Tra10", "Tra11", "Tra12", "Tra13", "Tra14", "Tra15"]);
            } else if (data.CountDay == 15) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7", "Tra8", "Tra9", "Tra10", "Tra11", "Tra12", "Tra13", "Tra14"]);
            }
            $("#report12").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            var height = $("#big").height() - 300;
            if (height > $("#report12").height()) {
                height = $("#big").height() - 467;
                if (height < $("#report12").height() || height > $("#report12").height()) {
                    height = $("#report12").height()+17;
                }
            }
            $("#report12").jqGrid('setGridHeight', height);
            $("#report12").jqGrid('setFrozenColumns');
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
    var LastYearStart = $("#LastYearStart").val();
    var LastYearEnd = $("#LastYearEnd").val();
    var HolidayId = $("#Holiday_List option:selected").val();
    if (StartTime == '') {
        alert("请选择开始日期");
        return false;
    }
    if (EndTime == '') {
        alert("请选择结束日期");
        return false;
    }
    if (getIntDate(EndTime) - getIntDate(StartTime) != getIntDate(LastYearEnd) - getIntDate(LastYearStart)) {
        alert("去年同期天数与数据日期天数不一致！");
        return;
    }
    $.ajax({
        type: "POST",
        url: '/Report/Getinfo',
        data: { "StartTime": StartTime, "EndTime": EndTime, "LastYearStart": LastYearStart, "LastYearEnd": LastYearEnd, "ReportType": 12, "HolidayId": HolidayId },
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
            $('#report12').GridUnload();
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            grid12.colNames = data.TitleList;                  
            grid12.data = data.ReportData;
            $("#report12").jqGrid(grid12).trigger("reloadGrid");
            //动态显示列
            if (data.CountDay == 1) {
                $("#report12").jqGrid('showCol', ["Tra1"]);
            } else if (data.CountDay == 2) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2"]);
            } else if (data.CountDay == 3) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3"]);
            } else if (data.CountDay == 4) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4"]);
            } else if (data.CountDay == 5) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5"]);
            } else if (data.CountDay == 6) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6"]);
            } else if (data.CountDay == 7) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7"]);
            } else if (data.CountDay == 8) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7", "Tra8"]);
            } else if (data.CountDay == 9) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7", "Tra8", "Tra9"]);
            } else if (data.CountDay == 10) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7", "Tra8", "Tra9", "Tra10"]);
            } else if (data.CountDay == 11) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7", "Tra8", "Tra9", "Tra10", "Tra11"]);
            } else if (data.CountDay == 12) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7", "Tra8", "Tra9", "Tra10", "Tra11", "Tra12"]);
            } else if (data.CountDay == 13) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7", "Tra8", "Tra9", "Tra10", "Tra11", "Tra12", "Tra13"]);
            } else if (data.CountDay == 14) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7", "Tra8", "Tra9", "Tra10", "Tra11", "Tra12", "Tra13", "Tra14"]);
            } else if (data.CountDay == 15) {
                $("#report12").jqGrid('showCol', ["Tra1", "Tra2", "Tra3", "Tra4", "Tra5", "Tra6", "Tra7", "Tra8", "Tra9", "Tra10", "Tra11", "Tra12", "Tra13", "Tra14", "Tra15"]);
            }
            $("#report12").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            var height = $("#big").height() - 300;
            if (height > $("#report12").height()) {
                height = $("#big").height() - 467;
                if (height < $("#report12").height() || height > $("#report12").height()) {
                    height = $("#report12").height()+17;
                }
            }
            $("#report12").jqGrid('setGridHeight', height);
            $("#report12").jqGrid('setFrozenColumns');
        }
    });
}

function resize() {
    $("body").height($(window).height()).width($(window).width());
    $("#big").height($("#maincontent").height());
    $("#creatReport").width($("#big").width() - 30);
    $(".tableDiv").width($("#big").width() - 30);
    $("#report12").jqGrid('setGridWidth', $("#maincontent").width() - 30);
    var height = $("#big").height() - 300;
    if (height > $("#report12").height()) {
        height = $("#big").height() - 467;
        if (height < $("#report12").height() || height > $("#report12").height()) {
            height = $("#report12").height()+17;
        }
    }
    $("#report12").jqGrid('setGridHeight', height);
    $(".report_title").width($("#big").width() - 30);
}
$(window).resize(function () {
    resize();
});
//报表12校正弹窗
function checkReport12() {
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
                            "ReportType": 12
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