var grid = {
    autoScroll: true,
    datatype: "local",
    rowNum: 25,
    viewrecords: true,
    //height: "auto",
    scroll: 10,
    forceFit: true,
    colNames: ['序号', '时间', '道路名称', '日交通量合计', '进京方向日交通量', '出京方向日交通量', '运行状况', '备注'],
    colModel: [
          { name: 'Num', index: 'Num', align: 'center', title: false, sortable: false,resizable: false, width: 40},
          { name: 'TimeScope', index: 'TimeScope', align: 'center', title: false, sortable: false, resizable: false },
          { name: 'RoadName', index: 'RoadName', align: 'center', title: false, sortable: false, resizable: false },
          { name: 'DayTraffic', index: 'DayTraffic', align: 'center', title: false, sortable: false, resizable: false },
          { name: 'InDayTraffic', index: 'InDayTraffic', align: 'center', sortable: false, title: false, editable: true, edittype: "text", resizable: false },
          { name: 'OutDayTraffic', index: 'OutDayTraffic', align: 'center', title: false, sortable: false, editrules: { required: true, number: true }, editable: true, edittype: "text", resizable: false },
          { name: 'RunningStatus', index: 'RunningStatus', align: 'center', title: false, sortable: false, editable: true, edittype: "textarea", resizable: false },
          { name: 'Remark', index: 'Remark', align: 'center', title: false, sortable: false, editable: true, edittype: "textarea", resizable: false }
    ],
    //  footerrow:true,//显示尾行合计行
   // cellEdit: true,     //启用单元格可编辑
    cellsubmit: "clientArray",//编辑后的数据保存在客户端不提交至服务端
    jsonReader: {
        root: "ReportData",
        repeatitems: false
    },
    onCellSelect: function (rowid, iCol, cellcontent, e) {
        var rowdata = $("#report").jqGrid("getRowData", rowid);
        var id = rowid + "_" + name;
        if (name == "InDayTraffic") {
            $("#" + id).change(function (e) {
                var InDayTraffic = $(this).val();
                var OutDayTraffic = rowdata["OutDayTraffic"];
                var total = parseInt(InDayTraffic) + parseInt(OutDayTraffic);
                $("#report").jqGrid("setRowData", rowid, { "DayTraffic": total });
            });
        }
    }
}

var type = 1;
$(document).ready(function () {
    resize();
    //获取当前日期
    var StartTime = GetDate();
    $("#date").text(StartTime);
    $("#StartTime").val(StartTime);
    $("#StartTime1").val(StartTime);
    getReport();//获取报表数据
    //判断是否为管理员，操作日期选择
    if ($("#isAdmin").val() == "true") {
        $("#StartTime1").show();
        $("#StartTime").hide();
    }
    else {
        $("#StartTime").show();
        $("#StartTime1").hide();
    }

    //无数据列表弹出
    $("#noDataList").click(function () {
        if (type != 1) {
            alert("请先保存或取消后再查看无数据收费站信息！");
            return;
        }
        window.parent.content("nodataList", "/DaliyReport/HourNoList", "无数据收费站信息", "500", "415");
    });
    //查询
    $("#query").click(function () {
        if (type != 1) {
            alert("请先保存或取消后再查询！");
            return;
        }
        reloadReportGrid();
    });
    //校正
    $("#check").click(function () {
        $(".checkIcon").addClass("checkIcon_select");
        $(".checkIcon+span").css({ "color": "#5789DF" });
        if (type != 1) {
            alert("请先保存或取消后再校正！");
            return;
        }
        checkReport15();
    });
    //导出
    $("#export").click(function () {
        if (type != 1) {
            alert("请先保存或取消后再导出！");
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
            data: { "StartTime": StartTime, "ReportType": 15, "HolidayId": 8 },
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
                window.location = "/DownFile/Downfile?name=" + path + "&reporttype=15";
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
            var ids = $("#report").jqGrid("getDataIDs");
            $.each(ids, function (a, b) {
                var rowData = $("#report").jqGrid('getRowData', b);
                if (rowData.InDayTraffic != "" && b!="NaN") {
                    flag = 1;
                    jQuery("#report").jqGrid('editRow', b);
                }                
                if (rowData.Num == "合计") {
                    textId='#' + b + '_InDayTraffic';
                    $('#' + b + '_InDayTraffic').attr("disabled", "disabled");
                    $('#' + b + '_OutDayTraffic').attr("disabled", "disabled");
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
    })
    //监听修改完成事件，并设置滚动条位置
    var setScroll = function ()
    {
        //通过判断最后一行控件的状态，确定所有编辑行已加载完成
        if ($(textId).attr('disabled') == 'disabled')
        {
            //alert(scroll_current / 825 * 36 * 25);
            //计算“超出区域的高度scroll_current”占“编辑前总高度825”的百分比，再乘以“编辑后的总高度（行高36 * 行數25）”得到应该设置的滚动条位置
            $('.ui-jqgrid-bdiv').scrollTop(scroll_current / 825 * 36 * 25);
            clearInterval(timer1);
        }
    }

    //保存
    $("#saveBtn").click(function () {
        $(".saveIcon").addClass("saveIcon_select");
        $(".saveIcon+span").css({ "color": "#5789DF" });
        var DataDate = $("#date").text();
        var ids = $("#report").jqGrid("getDataIDs");
        var dataP = new Object();
        var i = 0;
        $.each(ids, function (a, b) {
            rowData = $("#report").jqGrid('getRowData', b);
            if (rowData.InDayTraffic != "") {
                var Num = rowData.Num;
                if (Num == "合计") {
                    Num = 25;
                }
                dataP[i++] = {
                    "Num": Num,
                    "InDayTraffic": $('#' + b + '_InDayTraffic').val(),
                    "OutDayTraffic": $('#' + b + '_OutDayTraffic').val(),
                    "RunningStatus": $('#' + b + '_RunningStatus').val(),
                    "Remark": $('#' + b + '_Remark').val()
                }
            }
        });
        var ReportData = { "DataDate": DataDate, "UpdateData": dataP };
        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/DaliyReport/UpdateNaturalHour",
            data: ReportData,
            success: function (data) {
                if (data.ResultKey == 1) {
                    alert(data.ResultValue);
                    //$.each(ids, function (a, b) {
                    //    jQuery("#report").jqGrid('saveRow', b);
                    //});
                    type = 1;
                    reloadReportGrid();
                    $("#upd").show();
                    $("#saveBtn").hide();
                    $("#cancleBtn").hide();
                    $("#export").removeAttr("disabled");
                    $("#export").find("span").first().removeClass("unexpIcon").addClass("expIcon");
                    $("#check").removeAttr("disabled");
                    $("#check").find("span").first().removeClass("uncheckIcon").addClass("checkIcon");
                    $(".saveIcon").removeClass("saveIcon_select");
                    $(".saveIcon+span").css({ "color": "#555" });
                } else if (data.ResultKey == 0) {
                    alert(data.ResultValue);
                    $(".saveIcon").removeClass("saveIcon_select");
                    $(".saveIcon+span").css({ "color": "#555" });
                } else {
                    alert(data.ResultValue);
                    $(".saveIcon").remvoeClass("saveIcon_select");
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
            var ids = $("#report").jqGrid("getDataIDs");
            $.each(ids, function (a, b) {
                var rowData = $("#report").jqGrid('getRowData', b);
                jQuery("#report").jqGrid('restoreRow', b);
            })
            $("#upd").show();
            $("#saveBtn").hide();
            $("#cancleBtn").hide();
            $("#export").removeAttr("disabled");
            $("#export").find("span").first().removeClass("unexpIcon").addClass("expIcon");
            $("#check").removeAttr("disabled");
            $("#check").find("span").first().removeClass("uncheckIcon").addClass("checkIcon");
        }
        $(".cancleIcon").removeClass("cancleIcon_select");
        $(".cancleIcon+span").css({ "color": "#555" });
        $(".updIcon").removeClass("updIcon_select");
        $(".updIcon+span").css({ "color": "#555" });
    });
});

//获取报表数据
function getReport() {
    //获取当前日期
    //var myDate = new Date();
    //var StartTime = myDate.getFullYear() + "-" + ((myDate.getMonth() + 1) < 10 ? "0" : "") + (myDate.getMonth() + 1) + "-" + (myDate.getDate() < 10 ? "0" : "") + myDate.getDate();
    var StartTime;
    if ($("#isAdmin").val() == "true") {
        StartTime = $("#StartTime1").val();
    }
    else {
        StartTime = $("#StartTime").val();
    }
    $.ajax({
        url: '/Report/Getinfo',
        data: { "StartTime": StartTime, "ReportType": 15 },
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
            grid.data = data.ReportData;            
            $("#report").jqGrid(grid).trigger("reloadGrid");
            $("#report").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            $("#report").jqGrid('setGridHeight', $(window).height() - 460);
            $("#report").closest(".ui-jqgrid-bdiv").css({ 'overflow-x': 'hidden' });
            //$("#report_Num").width($("#report td[aria-describedby='report_Num']").width() + 3);
            $(".reportRemark").children().remove();
            $(".reportRemark").append("<span id='row_remark'>备注</span><span id='reportRemark'></span>");
            $("#reportRemark").html(data.ReportRemark.replace("\r\n", "\r").replace("\n", "\r").replace("\r", "<br />"));
            $("#row_remark").width($("#report_Num").width() + 4);



            //var ids = $("#report").jqGrid('getDataIDs');
            //如果jqgrid中没有数据 定义行号为1 ，否则取当前最大行号+1  
           // var newInd = NaN;
            //(ids.length == 0 ? 1 : Math.max.apply(Math, ids) + 1);
            // 插入一行

            //$("#report").addRowData(newInd, {}, newInd);
            //alert(aa);
            //$('#' + newInd).children().remove();
            //$('#' + newInd).addClass("reportRemark");
            //$('#' + newInd).html("<td style='text-align:center;'>备注</td><td colspan=7 style='word-break:break-all; word-wrap:break-word;WHITE-SPACE: normal'>" + data.ReportRemark + "</td>");
            //$('#' + newInd + ' td[aria-describedby="report_TimeScope"]').attr("colspan", 7);
            //$('#' + newInd + ' td[aria-describedby="report_RoadName"]').remove();
            //$('#' + newInd + ' td[aria-describedby="report_DayTraffic"]').remove();
            //$('#' + newInd + ' td[aria-describedby="report_InDayTraffic"]').remove();
            //$('#' + newInd + ' td[aria-describedby="report_OutDayTraffic"]').remove();
            //$('#' + newInd + ' td[aria-describedby="report_RunningStatus"]').remove();
            //$('#' + newInd + ' td[aria-describedby="report_Remark"]').remove();
        },
        error: function (data) {
            alert(aa);
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
        data: { "StartTime": StartTime, "ReportType": 15 },
        dataType: "json",
        beforeSend: function () {
            isShowload(1);
        },
        complete: function () {
            isShowload(0);
        },
        success: function (data) {
            $("#date").text(StartTime);
            $('#report').GridUnload();
            if (data.IsFull == 0) {
                $("#noDataList").show();
            } else {
                $("#noDataList").hide();
            }
            grid.data = data.ReportData;
           // $("#reportRemark").html(data.ReportRemark.replace("\r\n", "<br />"));
            $("#report").jqGrid(grid).trigger("reloadGrid");
            $("#report").jqGrid('setGridWidth', $("#maincontent").width() - 30);
            $("#report").jqGrid('setGridHeight', $(window).height() - 460);
            $("#report").closest(".ui-jqgrid-bdiv").css({ 'overflow-x': 'hidden' });
            $(".reportRemark").children().remove();
            $(".reportRemark").append("<span id='row_remark'>备注</span><span id='reportRemark'></span>");
            $("#reportRemark").html(data.ReportRemark.replace("\r\n", "\r").replace("\n", "\r").replace("\r", "<br />"));
            $("#row_remark").width($("#report_Num").width() + 4);

            //var newInd = NaN;
            //// 插入一行
            //$("#report").addRowData(newInd, {}, newInd);
            //$('#' + newInd).addClass("reportRemark");
            //$('#' + newInd).html("<td style='text-align:center;'>备注</td><td colspan=7 style='word-break:break-all; word-wrap:break-word;WHITE-SPACE: normal'>" + data.ReportRemark + "</td>");
        },
            error: function (data) {
                alert(aa);
            }
    });
}

function resize() {
    $("body").height($(window).height()).width($(window).width());
    $("#big").height($("#maincontent").height());
    $("#creatReport").width($("#big").width() - 30);
    $(".tableDiv").width($("#big").width() - 30);
    $("#report").jqGrid('setGridWidth', $("#maincontent").width() - 30);
    $("#report").jqGrid('setGridHeight', $(window).height() - 460);
    $(".report_title").width($("#big").width() - 30);
    $("#row_remark").width($("#report_Num").width() + 4);
}
$(window).resize(function () {
    resize();
});

//报表15校正弹窗
function checkReport15() {
    $("#checkRight").frameDialog({
        src: "/DaliyReport/HourCheckRight",
        onload: function () {
        },
        title: "报表校正",
        titleCloseIcon: true,
        resizable: false,
        dialog: {
            buttons: {
                "确定": function () {
                    //var StartTime = $("#StartTime").val();
                    // var checkStartTime = $("iframe[name=dialog-frame]").contents().find("#StartTime").val(StartTimes);
                    var StartTime = $("iframe[name=dialog-frame]").contents().find("#StartTime").text();
                    var LastYearStart = $("iframe[name=dialog-frame]").contents().find("#LastYearStart").val();
                    var StartHour = $("iframe[name=dialog-frame]").contents().find("#StartHour").val();
                    var EndHour = $("iframe[name=dialog-frame]").contents().find("#EndHour").val();
                    var FloatingRange = $("iframe[name=dialog-frame]").contents().find("#rank").val();
                    var val = $("iframe[name=dialog-frame]").contents().find('input:radio[name="checkRadio"]:checked').val();
                    if (val == 0) {
                        FloatingRange = FloatingRange * -1;
                    }
                    if (LastYearStart == "") {
                        alert("请选择参考数据日期！");
                        return;
                    }
                    if (EndHour ==""|| StartHour=="") {
                        alert("请输入时间范围！");
                        return;
                    }
                    if (parseInt(StartHour) > 23 || parseInt(StartHour) < 0) {
                        alert("开始时间必须介于00-23之间！");
                        return;
                    }
                    if (parseInt(EndHour) > 24 || parseInt(EndHour) < 1) {
                        alert("结束时间必须介于01-24之间！");
                        return;
                    }
                    if (parseInt(EndHour) <= parseInt(StartHour)) {
                        alert("开始时间必须小于结束时间！");
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
                            "StartHour": StartHour,
                            "EndHour": parseInt(EndHour) - 1,
                            "ReportType": 15
                        },
                        success: function (data) {
                            if (data.ResultKey == 0) {
                                alert(data.ResultValue);
                            } else if (data.ResultKey == 1) {
                                alert(data.ResultValue);
                                $("#checkRight").dialog("close");
                                $("#query").click();
                            } else {
                                alert(data.ResultValue);
                            }
                        },
                        error: function (data) {
                        }

                    });
                    
                },
                "取消": function () {
                    $(this).dialog("close");
                }

            },
            width: 300,
            height: 330
        },
                
    });
}