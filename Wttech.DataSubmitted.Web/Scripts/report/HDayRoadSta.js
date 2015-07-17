var type = 1;
var grid18 = {
    scrollOffset: 0,
    autoScroll: false,
    datatype: "local",
    viewrecords: true,
    height: "auto",
    autowidth: true,
    shrinkToFit: false,
    autoFit: true,
    viewrecords: true,
    colNames: ['序号', '所在路线编号', '所在路线名称', '收费站及观测点简称', '收费站及观测点桩号', '收费站位置类型', '车道', '合计', '出京', '进京', '合计', '出京', '进京', '设计交通量', '拥挤度', '合计', '出京', '进京', '合计', '出京', '进京', '合计', '出京', '进京', '合计', '出京', '进京', '合计', '出京', '进京', '进出京货车数量', '客车货车比例(%)', '进出京大货车以上车型的数量', '大货车以上占货车交通量比例(%)',''],
    colModel: [
        { name: 'Num', index: 'Num', width: 25, align: "center", title: false, sortable: false, resizable: false, frozen: true },
        { name: 'LineNum', index: 'LineNum', width: 80, align: "center", title: false, sortable: false, resizable: false, frozen: true },
        {
            name: 'LineName', index: 'LineName', width: 120, align: "center", title: false, sortable: false, resizable: false, frozen: true, cellattr: function (rowId, tv, rawObject, cm, rdata) {
                //合并单元格
            return 'id=\'LineName' + rowId + "\'";
            } },
        { name: 'StaName', index: 'StaName', width: 110, align: "center", title: false, sortable: false, resizable: false, frozen: true },
        { name: 'StaNum', index: 'StaNum', width: 110, align: "center", title: false, sortable: false, resizable: false, frozen: true },
        { name: 'StaType', index: 'StaType', width: 90, align: "center", title: false, sortable: false, resizable: false, frozen: true },
        { name: 'LaneNum', index: 'LaneNum', width: 40, align: "center", title: false, sortable: false, resizable: false, frozen: true },
        { name: 'NatSum', index: 'NatSum', width: 60, align: "center", sortable: false, resizable: false, title: false, },
        { name: 'ExNat', index: 'ExNat', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'EnNat', index: 'EnNat', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'EquSum', index: 'EquSum', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'ExEqu', index: 'ExEqu', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'EnEqu', index: 'EnEqu', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'DeTra', index: 'DeTra', width: 120, align: "center", title: false, sortable: false, resizable: false },
        { name: 'CrowDeg', index: 'CrowDeg', width: 120, align: "center", title: false, sortable: false,  resizable: false },
        { name: 'SmaSum', index: 'SmaSum', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'SmaEx', index: 'SmaEx', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'SmaEn', index: 'SmaEn', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'MedSum', index: 'MedSum', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'MedEx', index: 'MedEx', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'MedEn', index: 'MedEn', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'LarSum', index: 'LarSum', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'LarEx', index: 'LarEx', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'LarEn', index: 'LarEn', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'HeaSum', index: 'HeaSum', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'HeaEx', index: 'HeaEx', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'HeaEn', index: 'HeaEn', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'SupSum', index: 'MedSum', width: 60, align: "center", title: false, sortable: false, resizable: false },
        { name: 'SupEx', index: 'SupEx', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'SupEn', index: 'SupEn', width: 60, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'EnExTrukNum', index: 'EnExTrukNum', width: 130, align: "center", title: false, sortable: false, editable: true, edittype: "text", resizable: false },
        { name: 'CarTrukPer', index: 'CarTrukPer', width: 130, align: "center", title: false, sortable: false,  resizable: false },
        { name: 'SupTruNum', index: 'SupTruNum', width: 160, align: "center", title: false, sortable: false,  resizable: false },
        { name: 'SupTruPer', index: 'SupTruPer', width: 180, align: "center", title: false, sortable: false,  resizable: false },
        { name: 'LineType', index: 'LineType', align: "center", title: false, sortable: false, resizable: false, hidden: true },
    ],

    gridComplete: function () {
        //②在gridComplete调用合并方法
        var gridName = "report18";
        Merger(gridName, 'LineName');
    },
    jsonReader: {
        // root: "ReportData",
        repeatitems: false
    }
}

$(document).ready(function () {
    var timeTemp = dateFormateYesterday2();
    $("#StartTime1").val(timeTemp);
    $("#date").text(timeTemp);
    var holidayStart = $("#holidayStart").val();
    var holidayEnd = $("#holidayEnd").val();
    if (getIntDate(timeTemp) >= getIntDate(holidayStart) && getIntDate(timeTemp) <= getIntDate(holidayEnd)) {
        $("#StartTime").val(timeTemp);
    }else {
        $("#StartTime").val(holidayStart);
        $("#date").text(holidayStart);
    }
    resize();
    getReport();//获取报表数据
    $("#report18_name").text($("#HolidayId_List option:selected").text());
    //判断是否为管理员，操作日期选择
    if ($("#isAdmin").val() == "true") {
        $("#StartTime1").show();
        $("#StartTime").hide();
    }
    else {
        $("#HolidayId_List").attr("disabled", true);
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
        checkreport18();
    });
    //导出
    $("#export").click(function () {
        if (type != 1) {
            alert("请先保存后再导出！");
            $(".expIcon").removeClass("expIcon_select");
            $(".expIcon+span").css({ "color": "#555" });
            return;
        }
        var StartTime;
        if ($("#isAdmin").val() == "true") {
            StartTime = $("#StartTime1").val();
        }
        else {
            StartTime = $("#StartTime").val();
        }
        var HolidayId = $("#HolidayId_List option:selected").val();
        $.ajax({
            url: '/Report/ExportReport',
            data: { "StartTime": StartTime, "ReportType": 18, "HolidayId": HolidayId },
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
                window.location = "/DownFile/Downfile?name=" + path + "&reporttype=18";
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
        var scrollLeft = $('.ui-jqgrid-bdiv').scrollLeft();
        var flag = 0;
        if (type == 1) {
            var ids = $("#report18").jqGrid("getDataIDs");
            $.each(ids, function (a, b) {
                var rowData = $("#report18").jqGrid('getRowData', b);
                if ($("#IsEdit").val() == 1) {
                    if (b != "NaN" && rowData.LineType != "0") {
                        flag = 1;
                        jQuery("#report18").jqGrid('editRow', b);
                    }
                    textId = '#' + b + '_EnExTrukNum';
                }
            });
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
                $('.ui-jqgrid-bdiv').scrollLeft(scrollLeft);
            }
            else {
                alert("当前数据日期无数据，不可以进行修改！");
                $(".updIcon").removeClass("updIcon_select");
                $(".updIcon+span").css({ "color": "#555" });
                return;
            }
            //启动监听器
            timer1 = setTimeout(setScroll, 1);
            //记录当前滚动条位置，该位置代表超出显示区域的高度
            scroll_current = $('.ui-jqgrid-bdiv').scrollTop();
            scrollLeft_current = $('.ui-jqgrid-bdiv').scrollLeft();
        }

    })
    //监听修改完成事件，并设置滚动条位置
    var setScroll = function () {
        if ($(textId).length != 0) {
            $('.ui-jqgrid-bdiv').scrollTop(scroll_current / $("#report18").height() * 34 * 7);
            $('.ui-jqgrid-bdiv').scrollLeft(scrollLeft_current / $("#report18").width() * $("#report18").width());
            clearInterval(timer1);
        }
    }
    //保存
    $("#saveBtn").click(function () {
        $(".saveIcon").addClass("saveIcon_select");
        $(".saveIcon+span").css({ "color": "#5789DF" });
        var DataDate = $("#date").text();
        var ids = $("#report18").jqGrid("getDataIDs");
        var dataP = new Array();
        var i = 0;
        var flag = true;
        $.each(ids, function (a, b) {
            if (flag) {
                rowData = $("#report18").jqGrid('getRowData', b);

                //验证
                var ExNat = $('#' + b + '_ExNat').val();
                var EnNat = $('#' + b + '_EnNat').val();
                var ExEqu = $('#' + b + '_ExEqu').val();
                var EnEqu = $('#' + b + '_EnEqu').val();
                var SmaEx = $('#' + b + '_SmaEx').val();
                var SmaEn = $('#' + b + '_SmaEn').val();
                var MedEx = $('#' + b + '_MedEx').val();
                var MedEn = $('#' + b + '_MedEn').val();
                var LarEx = $('#' + b + '_LarEx').val();
                var LarEn = $('#' + b + '_LarEn').val();
                var HeaEx = $('#' + b + '_HeaEx').val();
                var HeaEn = $('#' + b + '_HeaEn').val();
                var SupEx = $('#' + b + '_SupEx').val();
                var SupEn = $('#' + b + '_SupEn').val();
                var EnExTrukNum = $('#' + b + '_EnExTrukNum').val();
                if (rowData.LineType != "0"&& rowData.LineType != "6") {
                    var reg = new RegExp("^[0-9]*$");
                    if (!reg.test(parseInt(ExNat)) || !reg.test(parseInt(EnNat)) || !reg.test(parseInt(ExEqu)) || !reg.test(parseInt(EnEqu)) || !reg.test(parseInt(SmaEx)) || !reg.test(parseInt(SmaEn)) || !reg.test(parseInt(MedEx)) || !reg.test(parseInt(MedEn)) || !reg.test(parseInt(LarEx)) || !reg.test(parseInt(LarEn)) || !reg.test(parseInt(HeaEx)) || !reg.test(parseInt(HeaEn)) || !reg.test(parseInt(SupEx)) || !reg.test(parseInt(SupEn)) || !reg.test(parseInt(EnExTrukNum))) {
                        alert("请输入数字!");
                        flag = false;
                        return;
                    }
                }

                if ($("#IsEdit").val() == 1) {
                    dataP[i++] = {
                        "LineType": rowData.LineType,
                        "ExNat": $('#' + b + '_ExNat').val(),
                        "EnNat": $('#' + b + '_EnNat').val(),
                        "ExEqu": $('#' + b + '_ExEqu').val(),
                        "EnEqu": $('#' + b + '_EnEqu').val(),
                        "SmaEx": $('#' + b + '_SmaEx').val(),
                        "SmaEn": $('#' + b + '_SmaEn').val(),
                        "MedEx": $('#' + b + '_MedEx').val(),
                        "MedEn": $('#' + b + '_MedEn').val(),
                        "LarEx": $('#' + b + '_LarEx').val(),
                        "LarEn": $('#' + b + '_LarEn').val(),
                        "HeaEx": $('#' + b + '_HeaEx').val(),
                        "HeaEn": $('#' + b + '_HeaEn').val(),
                        "SupEx": $('#' + b + '_SupEx').val(),
                        "SupEn": $('#' + b + '_SupEn').val(),
                        "EnExTrukNum": $('#' + b + '_EnExTrukNum').val()
                    }
                }
                flag = true;
            }
        });
        if (flag) {
            var ReportData = { "DataDate": DataDate, "DataInfo": dataP, "ReportType": 18 };
            $.ajax({
                dataType: "json",
                type: "POST",
                url: "/BeijingReport/UpdateHDayRoadSta",
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
        }
        $(".updIcon").removeClass("updIcon_select");
        $(".updIcon+span").css({ "color": "#555" });
    });
    //取消
    $("#cancleBtn").click(function () {
        $(".cancleIcon").addClass("cancleIcon_select");
        $(".cancleIcon+span").css({ "color": "#5789DF" });
        if (confirm("是否取消对当前报表的修改？")) {
            type = 1;
            var ids = $("#report18").jqGrid("getDataIDs");
            $.each(ids, function (a, b) {
                var rowData = $("#report18").jqGrid('getRowData', b);
                jQuery("#report18").jqGrid('restoreRow', b);
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
        data: { "StartTime": StartTime, "ReportType": 18 },
        dataType: "json",
        type: "Post",
        beforeSend: function () {
            isShowload(1);
        },
        complete: function () {
            isShowload(0);
        },
        success: function (data) {
            $("#IsEdit").val(data.IsEdit);
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            grid18.data = data.ReportData;
            $("#report18").jqGrid(grid18).trigger("reloadGrid");
            $("#report18").jqGrid('setGridWidth', $("#maincontent").width() - 30);
         //   $("#report18").jqGrid('setGridHeight', $("#big").height() - 450);
            changeHeader();
            $("#report18").jqGrid('setFrozenColumns');
        }
    });

}
var isHb = false; //防止多次合并表头   
function changeHeader() {
    if (!isHb) {
        $("#report18").jqGrid('setGroupHeaders', {
            useColSpanStyle: true,
            groupHeaders: [
               { startColumnName: 'Num', numberOfColumns: 1, titleText: '' },
               { startColumnName: 'LineNum', numberOfColumns: 1, titleText: '' },
               //{ startColumnName: 'StaName', numberOfColumns: 1, titleText: '' },
               //{ startColumnName: 'StaNum', numberOfColumns: 1, titleText: '' },
               { startColumnName: 'NatSum', numberOfColumns: 3, titleText: '交通量(自然交通辆)' },
               { startColumnName: 'EquSum', numberOfColumns: 3, titleText: '交通量(当量交通辆)' },
               { startColumnName: 'SmaSum', numberOfColumns: 3, titleText: '小型车' },
               { startColumnName: 'MedSum', numberOfColumns: 3, titleText: '中型车' },
               { startColumnName: 'LarSum', numberOfColumns: 3, titleText: '大型车' },
               { startColumnName: 'HeaSum', numberOfColumns: 3, titleText: '重型车' },
               { startColumnName: 'SupSum', numberOfColumns: 3, titleText: '超大型车' }

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
        data: { "StartTime": StartTime, "ReportType": 18 },
        dataType: "json",
        beforeSend: function () {
            isShowload(1);
        },
        complete: function () {
            isShowload(0);
        },
        success: function (data) {
            $("#IsEdit").val(data.IsEdit);
            $("#date").text(StartTime);
            $("#report18_name").text($("#HolidayId_List option:selected").text());
            $('#report18').GridUnload();
            isHb = false;
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            grid18.data = data.ReportData;
            $("#report18").jqGrid(grid18).trigger("reloadGrid");
            $("#report18").jqGrid('setGridWidth', $("#maincontent").width() - 30);
          //  $("#report18").jqGrid('setGridHeight', $("#big").height() - 450);
            changeHeader();
            $("#report18").jqGrid('setFrozenColumns');
        }
    });
}

function resize() {
    $("body").height($(window).height()).width($(window).width());
    $("#big").height($("#maincontent").height());
    $("#creatReport").width($("#big").width() - 30);
    $(".tableDiv").width($("#big").width() - 30);
    $("#report18").jqGrid('setGridWidth', $("#maincontent").width() - 30);
   // $("#report18").jqGrid('setGridHeight', $("#big").height() - 450);
    $(".report_title").width($("#big").width() - 30);
}
$(window).resize(function () {
    resize();
});

//报表18校正弹窗
function checkreport18() {
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
                            "ReportType": 18
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