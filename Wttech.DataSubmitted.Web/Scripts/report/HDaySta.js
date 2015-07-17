var type = 1;
var grid8 = {
    scrollOffset: 0,
    autoScroll: false,
    datatype: "local",
    viewrecords: true,
    height: "auto",
    shrinkToFit: false,
    viewrecords: true,
    autowidth: true,
    autoFit:true,
    colNames: ['序号', '路线名称', '合计', '出京', '进京', '免、收费总金额(万元)', '合计', '出京', '进京', '小型客车免费金额(万元)', '合计', '出京', '进京', '收费额度(万元)', '绿色通道(辆)', '绿色通道免收费金额(万元)', '主收费站', '合计', '出京', '进京', '累计加班值班（人次）', '发布交通服务信息数量', '公路阻断和处置情况'],
    colModel: [
        { name: 'Num', index: 'Num', align: "center", width: 40 ,sortable: false, resizable: false, frozen: true },
{ name: 'RoadName', index: 'RoadName', align: "center", width: 90, title: false, sortable: false, resizable: false, frozen: true},
        { name: 'LineSum', index: 'LineSum', align: "center", width: 70, title: false, sortable: false, resizable: false },
        { name: 'LineExSum', index: 'LineExSum', align: "center", width: 70, title: false, sortable: false, resizable: false },
        { name: 'LineEnSum', index: 'LineEnSum', align: "center", width: 70, title: false, sortable: false, resizable: false },
        { name: 'FeeSum', index: 'FeeSum', align: "center", width: 140,title: false, sortable: false, resizable: false },
        { name: 'SmaCarFeeNum', index: 'SmaCarFeeNum', align: "center", width: 70, title: false, sortable: false,resizable: false },
        { name: 'ExSmaCarFee', index: 'ExSmaCarFee', align: "center", width: 70, title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'EnSmaCarFee', index: 'EnSmaCarFee', align: "center", width: 70, title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'SmaCarFee', index: 'SmaCarFee', align: "center", width: 140, title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'ChagSumNum', index: 'ChagSumNum', align: "center", width: 70, title: false, sortable: false,  resizable: false },
        { name: 'ExChagNum', index: 'ExChagNum', align: "center", width: 70, title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'EnChagNum', index: 'EnChagNum', align: "center", width: 70, title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'ChagAmount', index: 'ChagAmount', align: "center", width: 90, title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'GreNum', index: 'GreNum', align: "center", width: 90, title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'GreFee', index: 'GreFee', align: "center", width: 140, title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'TollStaName', index: 'TollStaName', width: 90, align: "center", title: false, sortable: false, resizable: false },
        { name: 'StaSum', index: 'StaSum', align: "center", width: 70, title: false, sortable: false, resizable: false },
        { name: 'StaExSum', index: 'StaExSum', align: "center", width: 70, title: false, sortable: false, editable: true, edittype: "text",resizable: false },
        { name: 'StaEnSum', index: 'StaEnSum', align: "center", width: 70, title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'WorkPeoNum', index: 'WorkPeoNum', align: "center", width: 140, title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'InfoNum', index: 'InfoNum', align: "center", width: 140, title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'SitState', index: 'SitState', align: "center", width: 140, title: true, sortable: false, editable: true, edittype: "text", resizable: false },
    ],
   // gridComplete: hackHeight("#report8"),
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
    var StartTime = $("#StartTime").val().replace("年", ".").replace("月", ".").replace("日", "");
    var StartTime1 = $("#StartTime1").val().replace("年", ".").replace("月", ".").replace("日", "");
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
        checkreport8();
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
            data: { "StartTime": StartTime, "ReportType": 8, "HolidayId": 8 },
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
                window.location = "/DownFile/Downfile?name=" + path + "&reporttype=8";
            }
        });
    });
    //记录滚动条当前位置
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
            var rowData = $("#report8").jqGrid('getRowData', 1);
            if (rowData.LineSum != "") {
                flag = 1;
                jQuery("#report8").jqGrid('editRow', 1);
            }
            textId = '#1_SitState';
            //var ids = $("#report8").jqGrid("getDataIDs");
            //$.each(ids, function (a, b) {
            //    var rowData = $("#report8").jqGrid('getRowData', b);
            //    flag = 1;
            //    jQuery("#report8").jqGrid('editRow', b);
            //})
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
            scrollLeft_current = $('.ui-jqgrid-bdiv').scrollLeft();
        }

    })
    //监听修改完成事件，并设置滚动条位置
    var setScroll = function () {
        if ($(textId).length != 0) {
            $('.ui-jqgrid-bdiv').scrollLeft(scrollLeft_current / $("#report8").width() * $("#report8").width());
            clearInterval(timer1);
        }
        }
    //保存
    $("#saveBtn").click(function () {
        $(".saveIcon").addClass("saveIcon_select");
        $(".saveIcon+span").css({ "color": "#5789DF" });
        var DataDate = $("#date").text();
        var rowData = $("#report8").jqGrid('getRowData', 1);
        //总交通量//
        var LineExSum = rowData.LineExSum;
        var LineEnSum = rowData.LineEnSum;
        var LineSum = rowData.LineSum;
        ////////////
        //大羊坊总交通量//
        var StaSum = rowData.StaSum;
        var StaExSum = $('#1_StaExSum').val();
        var StaEnSum = $('#1_StaEnSum').val();
        //////////
        //小型客车免费交通量//
        var ExSmaCarFee = $('#1_ExSmaCarFee').val();
        var EnSmaCarFee = $('#1_EnSmaCarFee').val();
        var SmaCarFee = $('#1_SmaCarFee').val();
        //////////
        //收费车辆//
        var ExChagNum = $('#1_ExChagNum').val();
        var EnChagNum = $('#1_EnChagNum').val();
        var ChagAmount = $('#1_ChagAmount').val();//收费额度
        var GreNum = $('#1_GreNum').val();//绿通车辆
        var GreFee = $('#1_GreFee').val();//绿通免收费金额
        var WorkPeoNum = $('#1_WorkPeoNum').val();//加班人次

        if (parseInt(StaExSum) > parseInt(LineExSum)) {
            alert("大羊坊站总交通量(出京)必须小于或等于总交通量（出京）！");
            $(".saveIcon").removeClass("saveIcon_select");
            $(".saveIcon+span").css({ "color": "#555" });
            return;
        }
        if (parseInt(StaEnSum) > parseInt(LineEnSum)) {
            alert("大羊坊站总交通量(进京)必须小于或等于总交通量（进京）！");
            $(".saveIcon").removeClass("saveIcon_select");
            $(".saveIcon+span").css({ "color": "#555" });
            return;
        }
        if (parseInt(StaSum) > parseInt(LineSum)) {
            alert("大羊坊站总交通量(合计)必须小于或等于总交通量（合计）！");
            $(".saveIcon").removeClass("saveIcon_select");
            $(".saveIcon+span").css({ "color": "#555" });
            return;
        }
        var reg = new RegExp("^[0-9]*$");
        if (!reg.test(parseInt(StaExSum)) || !reg.test(parseInt(StaEnSum)) || !reg.test(parseInt(ExSmaCarFee)) || !reg.test(parseInt(EnSmaCarFee)) || !reg.test(parseInt(ExChagNum)) || !reg.test(parseInt(EnChagNum)) || !reg.test(parseInt(GreNum))) {
            alert("请输入数字!");
            $(".saveIcon").removeClass("saveIcon_select");
            $(".saveIcon+span").css({ "color": "#555" });
            return;
        }
        //验证有两位小数的正实数：^[0-9]+(.[0-9]{2})?$ 
        var reg1 = new RegExp("^[0-9]+(.[0-9]{1,2})?$");
        if (!reg1.test(parseFloat(SmaCarFee)) || !reg1.test(parseFloat(ChagAmount)) || !reg1.test(parseFloat(GreFee))) {
            alert("请保留两位小数!");
            $(".saveIcon").removeClass("saveIcon_select");
            $(".saveIcon+span").css({ "color": "#555" });
            return;
        }
        var ids = $("#report8").jqGrid("getDataIDs");
        var dataP = new Array();
        var i = 0;
        //$.each(ids, function (a, b) {
             dataP[i++] = {
                    "ExSmaCarFee": $('#1_ExSmaCarFee').val(),
                    "EnSmaCarFee": $('#1_EnSmaCarFee').val(),
                    "SmaCarFee": $('#1_SmaCarFee').val(),
                    "ExChagNum": $('#1_ExChagNum').val(),
                    "ChagAmount": $('#1_ChagAmount').val(),
                    "GreNum": $('#1_GreNum').val(),
                    "GreFee": $('#1_GreFee').val(),
                    "StaExSum": $('#1_StaExSum').val(),
                    "StaEnSum": $('#1_StaEnSum').val(),
                    "WorkPeoNum": $('#1_WorkPeoNum').val(),
                    "InfoNum": $('#1_InfoNum').val(),
                    "SitState": $('#1_SitState').val(),
                    "EnChagNum": $('#1_EnChagNum').val()
             }
       // });
        var ReportData = { "DataDate": DataDate, "DataInfo": dataP, "ReportType": 8 };
        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/BeijingReport/UpdateHDaySta",
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
                alert(data);
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
            var ids = $("#report8").jqGrid("getDataIDs");
            $.each(ids, function (a, b) {
                var rowData = $("#report8").jqGrid('getRowData', b);
                jQuery("#report8").jqGrid('restoreRow', b);
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
        data: { "StartTime": StartTime, "ReportType": 8 },
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
            grid8.data = data.ReportData;
            $("#report8").jqGrid(grid8).trigger("reloadGrid");
            $("#report8").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            changeHeader();
            $("#report8").jqGrid('setFrozenColumns');
            
        }
    });

}
var isHb = false; //防止多次合并表头   
function changeHeader() {
    if (!isHb) {
        $("#report8").jqGrid('setGroupHeaders', {
            useColSpanStyle: true,
            groupHeaders: [
               { startColumnName: 'Num', numberOfColumns: 1, titleText: '' },
               { startColumnName: 'RoadName', numberOfColumns: 1, titleText: '' },
               { startColumnName: 'LineSum', numberOfColumns: 3, titleText: '总交通量(辆)' },
               { startColumnName: 'SmaCarFeeNum', numberOfColumns: 3, titleText: '小型客车免费通行交通量(辆)' },
               { startColumnName: 'ChagSumNum', numberOfColumns: 3, titleText: '收费车辆(辆)' },
               { startColumnName: 'StaSum', numberOfColumns: 3, titleText: '总交通量(辆)' }
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
        data: { "StartTime": StartTime, "ReportType": 8 },
        dataType: "json",
        beforeSend: function () {
            isShowload(1);
        },
        complete: function () {
            isShowload(0);
        },
        success: function (data) {
            var startTime1 = StartTime.replace("年", ".").replace("月", ".").replace("日", "");
            $("#date").text(startTime1);
            $('#report8').GridUnload();
            isHb = false;
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            grid8.data = data.ReportData;
            $("#report8").jqGrid(grid8).trigger("reloadGrid");
            $("#report8").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            changeHeader();
            $("#report8").jqGrid('setFrozenColumns');
        }
    });
}

function resize() {
    $("body").height($(window).height()).width($(window).width());
    $("#big").height($("#maincontent").height());
    $("#creatReport").width($("#big").width() - 30);
    $(".tableDiv").width($("#big").width() - 30);
    $("#report8").jqGrid('setGridWidth', $("#maincontent").width() - 30);
    $(".report_title").width($("#big").width() - 30);
    if (type == 1) {
        reloadReportGrid();
    }   
}
$(window).resize(function () {
    resize();
});

//报表8校正弹窗
function checkreport8() {
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
                            "ReportType": 8
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
                        data: { "StartTime": StartTime, "ReportType": 8, "FloatingRange": FloatingRange, "HolidayId": 8 },
                        success: function (data) {
                            if (data.ResultKey == 0) {
                                alert(data.ResultValue);
                            } else if (data.ResultKey == 1) {
                                var path = escape(data.ResultValue);
                                window.location = "/DownFile/Downfile?name=" + path + "&reporttype=8";
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