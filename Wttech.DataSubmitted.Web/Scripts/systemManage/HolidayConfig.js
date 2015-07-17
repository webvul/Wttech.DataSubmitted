var dailygrid = {
    scrollOffset: 0,
    autoScroll: false,
    datatype: "local",
    viewrecords: true,
    height: "auto",
    //sortname: 'HolidayConfigId',
    grouping: true,
    groupingView: {
        groupField: ['Type'],
        groupColumnShow: [false],
        groupText: ['<b>{0}</b>']
    },
    colNames: ['', '报表名称', '假期名称', '假期开始时间', '假期截止时间', '同比开始时间', '同比截止时间', '预测日期', '预测百分比', '校正百分比', '备注', '操作', '',''],
    colModel: [
            { name: 'HolidayConfigId', index: 'HolidayConfigId', align: 'center', title: false, sortable: false, resizable: false, hidden: true },
            { name: 'ConfigName', index: 'ConfigName', align: 'left', title: true, sortable: false, resizable: false, width: 160 },
            { name: 'HolidayName', index: 'HolidayName', align: 'center', title: false, sortable: false, resizable: false, width: 60 },
            { name: 'HolidayStartTime', index: 'HolidayStartTime', align: 'center', title: false, sortable: false, resizable: false, hidden: true, formatter: dateFom },
            { name: 'HolidayEndTime', index: 'HolidayEndTime', align: 'center', title: false, sortable: false, resizable: false, hidden: true, formatter: dateFom },
            { name: 'ComparedStartTime', index: 'ComparedStartTime', align: 'center', sortable: false, title: false, resizable: false, hidden: true, formatter: dateFom },
            { name: 'ComparedEndTime', index: 'ComparedEndTime', align: 'center', sortable: false, title: false, resizable: false, hidden: true, formatter: dateFom },
            { name: 'ForecastDate', index: 'ForecastDate', align: 'center', title: false, sortable: false, resizable: false, hidden: true, formatter: dateFom },
            { name: 'ForecastFloat', index: 'ForecastFloat', align: 'center', title: false, sortable: false, resizable: false, hidden: true },
            { name: 'CheckFloat', index: 'CheckFloat', align: 'center', title: false, sortable: false, editable: true, edittype: "text", resizable: false, width: 50 },
            { name: 'ReportRemark', index: 'ReportRemark', align: 'left', title: true, sortable: false, editable: true, edittype: "textarea",editoptions:{rows:"4",cols:"20"}, resizable: false},
            { name: 'Action', index: 'HolidayConfigId', align: 'center', title: false, sortable: false, resizable: false, width: 40 },
            { name: 'Type', index: 'Type', align: 'center', title: false, sortable: true, resizable: false },
            { name: 'ConfigItem', index: 'ConfigItem', align: 'center', title: false, sortable: false, resizable: false, hidden: true }
    ],
    jsonReader: {
        repeatitems: false
    },
    gridComplete: function () {
        var ids = jQuery("#daily").jqGrid('getDataIDs');
        for (var i = 0; i < ids.length; i++) {
            var cl = ids[i];
            var rowdata = jQuery("#daily").jqGrid('getRowData', cl);
            //if (rowdata.ConfigItem.indexOf("CheckFloat") != -1) {
            //    jQuery("#daily").jqGrid('setCell', cl, 'CheckFloat', '', { background: '#e8eff9' });

            //}
            //if (rowdata.ConfigItem.indexOf("RptRemark") != -1) {
            //    jQuery("#daily").jqGrid('setCell', cl, 'ReportRemark', '', { background: '#e8eff9' });

            //}
            be = "<a class='tabelhover' href='#' onclick=\"editRow('" + cl + "');\" >编辑</a>";
            jQuery("#daily").jqGrid('setRowData', ids[i], { Action: be });
        }
    },
    //onSelectRow: function () {
    //    var ids = jQuery("#daily").jqGrid('getDataIDs');
    //    for (var i = 0; i < ids.length; i++) {
    //        var cl = ids[i];
    //        var rowdata = jQuery("#daily").jqGrid('getRowData', cl);
    //        if (rowdata.ConfigItem.indexOf("CheckFloat") != -1) {
    //            jQuery("#daily").jqGrid('setCell', cl, 'CheckFloat', '', { background: '#e8eff9', color: '#5b626d' });
    //        }
    //        if (rowdata.ConfigItem.indexOf("RptRemark") != -1) {
    //            jQuery("#daily").jqGrid('setCell', cl, 'ReportRemark', '', { background: '#e8eff9', color: '#5b626d' });
    //        }
    //    }
    //}
}
var bjGridConfig = {
    scrollOffset: 0,
    autoScroll: false,
    datatype: "local",
    viewrecords: true,
    height: "auto",
   // sortname: 'HolidayConfigId',
    grouping: true,
    groupingView: {
        groupField: ['Type'],
        groupColumnShow: [false],
        groupText: ['<span class="config_title">{0}</span>']
    },
    colNames: ['', '报表名称', '假期名称', '假期开始时间', '假期截止时间', '同比开始时间', '同比截止时间', '预测日期', '预测百分比', '校正百分比', '备注', '操作', '', ''],
    colModel : [
              { name: 'HolidayConfigId', index: 'HolidayConfigId', align: 'center', title: false, sortable: false, resizable: false, hidden: true },
              { name: 'ConfigName', index: 'ConfigName', align: 'left', title: true, sortable: true, resizable: false, width: 140 },
              { name: 'HolidayName', index: 'HolidayName', align: 'center', title: false, sortable: false, resizable: false, editable: true, edittype: "select", width: 60},
              { name: 'HolidayStartTime', index: 'HolidayStartTime', align: 'center', title: false, sortable: false, resizable: false, formatter: dateFom, width: 90 },
              { name: 'HolidayEndTime', index: 'HolidayEndTime', align: 'center', title: false, sortable: false, resizable: false, formatter: dateFom, width: 90 },
              { name: 'ComparedStartTime', index: 'ComparedStartTime', align: 'center', sortable: false, title: false, editable: true, edittype: "text", resizable: false, formatter: dateFom, width: 90 },
              { name: 'ComparedEndTime', index: 'ComparedEndTime', align: 'center', sortable: false, title: false, editable: true, edittype: "text", resizable: false, formatter: dateFom, width: 90 },
              { name: 'ForecastDate', index: 'ForecastDate', align: 'center', title: false, editable: true, sortable: false, resizable: false, formatter: dateFom, width: 90 },
              { name: 'ForecastFloat', index: 'ForecastFloat', align: 'center', title: false, sortable: false, editable: true, edittype: "textarea", resizable: false, width: 70 },
              { name: 'CheckFloat', index: 'CheckFloat', align: 'center', title: false, sortable: false, editable: true, edittype: "textarea", resizable: false, width: 70 },
              { name: 'ReportRemark', index: 'ReportRemark', align: 'center', title: true, sortable: false, editable: true, edittype: "textarea", resizable: false, hidden: true },
              { name: 'Action', index: 'HolidayConfigId', align: 'center', title: false, sortable: false, resizable: false, width: 40 },
              { name: 'Type', index: 'Type', align: 'center', title: false, sortable: true, resizable: false },
              { name: 'ConfigItem', index: 'ConfigItem', align: 'center', title: false, sortable: false, resizable: false, hidden: true }
    ],
    jsonReader: {
        repeatitems: false
    },
    gridComplete: gridCellAttr
    //onSelectRow: selscteRow
}
var tjGridConfig = {
    scrollOffset: 0,
    autoScroll: false,
    datatype: "local",
    viewrecords: true,
    height: "auto",
    sortname: 'Type',
    grouping: true,
    groupingView: {
        groupField: ['Type'],
        groupColumnShow: [false],
        groupText: ['<b>{0}</b>']
    },
    colNames: ['', '报表名称', '假期名称', '假期开始时间', '假期截止时间', '同比开始时间', '同比截止时间', '预测日期', '预测百分比', '校正百分比', '备注', '操作', '', ''],
    colModel : [
              { name: 'HolidayConfigId', index: 'HolidayConfigId', align: 'center', title: false, sortable: false, resizable: false, hidden: true },
              { name: 'ConfigName', index: 'ConfigName', align: 'left', title: true, sortable: false, resizable: false, width: 140 },
              { name: 'HolidayName', index: 'HolidayName', align: 'center', title: false, sortable: false, resizable: false, editable: true, edittype: "select", width: 60 },
              { name: 'HolidayStartTime', index: 'HolidayStartTime', align: 'center', title: false, sortable: false, resizable: false, formatter: dateFom, width: 90 },
              { name: 'HolidayEndTime', index: 'HolidayEndTime', align: 'center', title: false, sortable: false, resizable: false, formatter: dateFom, width: 90 },
              { name: 'ComparedStartTime', index: 'ComparedStartTime', align: 'center', sortable: false, title: false, editable: true, edittype: "text", resizable: false, formatter: dateFom, width: 90 },
              { name: 'ComparedEndTime', index: 'ComparedEndTime', align: 'center', sortable: false, title: false, editable: true, edittype: "text", resizable: false, formatter: dateFom, width: 90 },
              { name: 'ForecastDate', index: 'ForecastDate', align: 'center', title: false, editable: true, sortable: false, resizable: false, formatter: dateFom, width: 90 },
              { name: 'ForecastFloat', index: 'ForecastFloat', align: 'center', title: false, sortable: false, editable: true, edittype: "textarea", resizable: false, width: 70 },
              { name: 'CheckFloat', index: 'CheckFloat', align: 'center', title: false, sortable: false, editable: true, edittype: "textarea", resizable: false, width: 70 },
              { name: 'ReportRemark', index: 'ReportRemark', align: 'center', title: true, sortable: false, editable: true, edittype: "textarea", resizable: false, hidden: true },
              { name: 'Action', index: 'HolidayConfigId', align: 'center', title: false, sortable: false, resizable: false, width: 40 },
              { name: 'Type', index: 'Type', align: 'center', title: false, sortable: false, resizable: false },
              { name: 'ConfigItem', index: 'ConfigItem', align: 'center', title: false, sortable: false, resizable: false, hidden: true }
            ],
    jsonReader: {
        repeatitems: false
    },
    gridComplete: function () {
        var ids = jQuery("#tjConfig").jqGrid('getDataIDs');
        for (var i = 0; i < ids.length; i++) {
            var cl = ids[i];
            var rowdata = jQuery("#tjConfig").jqGrid('getRowData', cl);
            be = "<a class='tabelhover' href='#' onclick=\"editTJRow('" + cl + "');\" >编辑</a>";
            jQuery("#tjConfig").jqGrid('setRowData', ids[i], { Action: be });
        }
    }
}
$(document).ready(function () {
    resize();    
    //获取配置表信息
    getConfigInfo();
    getConfigInfo1();
    getTJConfigInfo();
    //获取假期名称列表
    getHolNameList();
    $("#editNameCog").show();
    //假期名称管理
    $("#holidayName").click(function () {
        $(".holIcon").addClass("holIcon_select");
        $(".holIcon+span").css({ "color": "#5789DF" });
        window.parent.content("holNameWindow", "/SystemManage/HolidayName", "假期名称管理", "480", "575");
    });
    $(".group_li:first").addClass("group_li_sel");
    $(".group_li").click(function () {
        $(this).addClass("group_li_sel").siblings().removeClass("group_li_sel");
        var tab = $(this).attr("id-title", $(this).attr("id"));
        var tab1 = $(this).attr("id-title");
        $("[class='" + tab1 + "']").show().siblings().hide();
    });
});
//日期格式化
function dateFom(cellvalue, options, rowObject) {
    return ChangeDateFormat(cellvalue);
}
function ChangeDateFormat(time) {
    if (time != null) {
        var date = new Date(parseInt(time.replace("/Date(", "").replace(")/", ""), 10));
        var month = changeInt(date.getMonth() + 1);
        var currentDate = changeInt(date.getDate());
        return date.getFullYear() + "-" + month + "-" + currentDate;
    }
    return "";
}
//表格加载后单元格属性设置
function gridCellAttr() {
    var ids = jQuery("#bj").jqGrid('getDataIDs');
    for (var i = 0; i < ids.length; i++) {
        var cl = ids[i];
        var rowdata = jQuery("#bj").jqGrid('getRowData', cl);
        be = "<a class='tabelhover' href='#' onclick=\"editBJRow('" + cl + "');\">编辑</a>";
        jQuery("#bj").jqGrid('setRowData', ids[i], { Action: be });           
    }
}
//修改默认假期名称
function xgDefault() {
    $("#defNameCog").hide();
    $("#editNameCog").show();
}
//取消修改名称
function quitDefault() {
    $("#editNameCog").show();
    //$("#defNameCog").show();
}
//保存名称修改
function saveDefault() {
    $.ajax({
        type: "POST",
        dataType: 'json',
        url: "/SystemManage/SetHoliday",
        data: { "HolidayId": $("#editHolName option:selected").attr("id") },
        success: function (data) {
            if (data.ResultKey == 1) {
                alert(data.ResultValue);
                $("#editNameCog").show();
               // $("#qd").hide();
                //$("#qxdef").hide();
                $("#defaultName").val($("#editHolName option:selected").val());
               // $("#defNameCog").show();
               // $("#xg").show();
                $("#daily").GridUnload();
                $("#bj").GridUnload();
                $("#tjConfig").GridUnload();
                getConfigInfo();
                getConfigInfo1();
                getTJConfigInfo();
            } else if (data.ResultKey == 2) {
                alert(data.ResultValue);
            } else {
                alert(data.ResultValue);
            }
        },
        error: function (data) {
            alert(data.ResultValue);
        }
    });
}
//每日报送
function getConfigInfo() {
    $.ajax({
        url: '/SystemManage/GetConfig',
        data:{"type": 1 },
        dataType: "json",
        type: "Post",
        success: function (data) {
            $.each(data, function (i, j) {
                j.ReportRemark = j.ReportRemark.replace("\r\n", "\r").replace("\n", "\r").replace("\r", "<br />");
                $("#defaultName").text(j.HolidayName);
            })
            dailygrid.data = data;
            $("#daily").jqGrid(dailygrid).trigger("reloadGrid");
            $("#daily").jqGrid('setGridWidth', $("#creatReport").width());
        }
    });
   // $("#daily").jqGrid('setGridHeight', $(window).height()-400);
}
//北京段
function getConfigInfo1() {
    $.ajax({
        url: '/SystemManage/GetConfig',
        data: { "type": 2 },
        dataType: "json",
        type: "Post",
        success: function (data) {
            bjGridConfig.data = data;
            $("#bj").jqGrid(bjGridConfig).trigger("reloadGrid");
            $("#bj").jqGrid('setGridWidth', $("#creatReport").width());
            $(".config_title").each(function (i,j) {
                j.innerText = j.innerText.substr(1, j.innerText.length - 1);
            });
        }
    });
}
//天津段
function getTJConfigInfo() {
    $.ajax({
        url: '/SystemManage/GetConfig',
        data: { "type": 3 },
        dataType: "json",
        type: "Post",
        success: function (data) {
            tjGridConfig.data = data;
            $("#tjConfig").jqGrid(tjGridConfig).trigger("reloadGrid");
            $("#tjConfig").jqGrid('setGridWidth', $("#creatReport").width());
        }
    });
}
//获取默认假期名称配置下拉列表
function getHolNameList() {
    $.ajax({
        url: '/SystemManage/GetHolidayList',
        dataType: "json",
        type: "Post",
        success: function (data) {
            var str = "";
            $.each(data, function (i, j) {
                str += "<option id=" + j.DictId + ">" + j.Name + "</option>";
            });
            $("#editHolName").append(str);
        }
    });
}
//每日报送编辑
function editRow(id) {
    $("#selId").val(id);
    $("#editHoliday").frameDialog({
        src: "/SystemManage/editHolConfig",
        onload: function () {
            var rowData = $("#daily").jqGrid('getRowData', id);
            var ConfigItem = rowData.ConfigItem;
            var dialogheight = ((ConfigItem.split(",").length) + 1) * 45;
            var dialogwidth = 465;
            if (ConfigItem.indexOf("CheckFloat") == -1){
            //var CheckFloat = rowData.CheckFloat;
                //if (CheckFloat == "") {
                dialogheight = 310;
                dialogwidth = 380;
                $("iframe[name=dialog-frame]").contents().find("#first_tr").hide();
                $("iframe[name=dialog-frame]").contents().find("#first_td").hide();
                $("iframe[name=dialog-frame]").contents().find("#four_tr").hide();
                $("iframe[name=dialog-frame]").contents().find("#second_tr").show();
                $("iframe[name=dialog-frame]").contents().find("#third_tr").show();
                $("iframe[name=dialog-frame]").contents().find("#fifth_tr").show();
            } else {
                dialogheight += 50;
            }
            $("iframe[name=dialog-frame]").height(dialogheight);
            $("iframe[name=dialog-frame]").width(dialogwidth);
            setWindowPos('editHoliday');//设置弹出框位置居中
        },
        title: "配置项编辑",
        titleCloseIcon: true,
        resizable: false,
        dialog: {           
            buttons: {
                "保存": function () {
                    var ReportRemark;
                    var rowData = $("#daily").jqGrid('getRowData', id);
                    var CheckFloat = rowData.CheckFloat;
                    var contents = $("iframe[name=dialog-frame]").contents();
                    if (CheckFloat == "") {
                        var span1 = contents.find("#span1").text();
                        var span2 = contents.find("#span2").text();
                        var span3 = contents.find("#span3").text();
                        var span4 = contents.find("#span4").text();
                        var span5 = contents.find("#span5").text();
                        var span6 = contents.find("#span6").text();
                        var charge = contents.find("#charge").val();
                        var charge_tel = contents.find("#charge_tel").val();
                        var charge_mobile = contents.find("#charge_mobile").val();
                        var tel = contents.find("#tel").val();
                        var mobile = contents.find("#mobile").val();
                        ReportRemark = span1 + charge + " " + span2 + charge_tel + " " + span3 + charge_mobile + ";" + span6 + " " + span4 + tel + " " + span5 + mobile;
                    } else {
                        ReportRemark = $("iframe[name=dialog-frame]").contents().find("#ReportRemark").val();
                    }
                    var queryData = {
                        "CheckFloat" : $("iframe[name=dialog-frame]").contents().find("#checkFloat").val(),
                        "ReportRemark": ReportRemark,
                        "HolidayConfigId": $("iframe[name=dialog-frame]").contents().find("#HolidayConfigId").val(),
                        "ConfigName": "",
                        "HolidayId": "",
                        "HolidayName": "",
                        "HolidayStartTime": "",
                        "HolidayEndTime": "",
                        "ComparedStartTime": "",
                        "ComparedEndTime": "",
                        "ForecastDate": "",
                        "ForecastFloat": "",
                        "Type": "",
                        "ConfigItem":""
                    }
                    $.ajax({
                        type: "POST",
                        dataType: 'json',
                        url: "/SystemManage/UpdateConfig",
                        data: queryData,
                        success: function (data) {
                            if (data.ResultKey == 1) {
                                alert(data.ResultValue);
                                $("#editHoliday").dialog("close");
                                $("#daily").GridUnload();
                                getConfigInfo();
                                } else if (data.ResultKey == 2) {
                                    alert(data.ResultValue);
                                }else {
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
            
            width: "auto",
            height: "auto"
        }
    });
}
//北京段编辑
function editBJRow(id) {
    $("#selId").val(id);
        $("#editBJHoliday").frameDialog({
            src: "/SystemManage/editBJHol",
            onload: function () {
                var rowData = $("#bj").jqGrid('getRowData', id);
                var ConfigItem= rowData.ConfigItem;
                var dialogheight = ((ConfigItem.split(",").length) + 1) * 45;

                var dialogwidth = 300;
                if (ConfigItem.indexOf("Start") != -1) {
                    dialogwidth = 390;
                }
                var frame=$("iframe[name=dialog-frame]").contents();
                //假期名称可配置
                if (ConfigItem.indexOf("HDayId") != -1) {
                    if (ConfigItem.indexOf("HDayStart") != -1 && ConfigItem.indexOf("CheckFloat") == -1) {
                        dialogheight += 25;
                        frame.find("#HolidayName").hide();
                        frame.find("#HolNameList").show();
                        frame.find("#fourth_tr").show();
                    } else if (ConfigItem.indexOf("CheckFloat") != -1 && ConfigItem.indexOf("HDayStart") == -1) {
                        dialogheight -= 30;
                        frame.find("#HolidayName").hide();
                        frame.find("#HolNameList").show();
                        frame.find("#third_tr").show();
                    } else if (ConfigItem.indexOf("CheckFloat") != -1 && ConfigItem.indexOf("HDayStart") != -1) {
                        dialogheight -= 30;
                        frame.find("#HolidayName").hide();
                        frame.find("#HolNameList").show();
                        frame.find("#third_tr").show();
                        frame.find("#fourth_tr").show();
                    }
                }
                else {
                    //假期名称不可配置
                    if (ConfigItem.indexOf("ForeDate") != -1 && ConfigItem.indexOf("CheckFloat") != -1 && ConfigItem.indexOf("ForeFloat") != -1) {
                        dialogheight -= 15;
                        frame.find("#first_tr").show();
                        frame.find("#second_tr").show();
                        frame.find("#third_tr").show();
                        frame.find("#fourth_tr").show();
                    } else if (ConfigItem.indexOf("CompStart") != -1 && ConfigItem.indexOf("CompEnd") != -1 && ConfigItem.indexOf("CheckFloat") == -1) {
                        dialogheight -= 75;
                        frame.find("#first_tr").show();
                        frame.find("#second_tr").show();
                        frame.find("#fourth_tr").show();
                        frame.find("#fifth_tr").show();
                    } else if (ConfigItem.indexOf("CheckFloat") != -1 && ConfigItem.indexOf("CompEnd") == -1 && ConfigItem.indexOf("ForeDate") == -1) {
                        dialogheight += 15;
                        frame.find("#third_tr").show();
                        frame.find("#fourth_tr").show();
                    } else if (ConfigItem.indexOf("HDayStart") != -1 && ConfigItem.indexOf("CompEnd") == -1 && ConfigItem.indexOf("CheckFloat") == -1) {
                        dialogheight += 65;
                        frame.find("#fourth_tr").show();
                    }
                }
                $("iframe[name=dialog-frame]").height(dialogheight);
                $("iframe[name=dialog-frame]").width(dialogwidth);
                setWindowPos('editBJHoliday');
            },
            title: "配置项编辑",
            titleCloseIcon: true,
            resizable: false,
            dialog: {
                buttons: {
                    "保存": function () {
                        var frame = $("iframe[name=dialog-frame]").contents();
                        var queryData = {
                            "CheckFloat": frame.find("#checkFloat").val(),
                            "ReportRemark": "",
                            "HolidayConfigId": frame.find("#HolidayConfigId").val(),
                            "ConfigName": "",
                            "HolidayId": frame.find("#HolNameList option:selected").attr("id"),
                            "HolidayName": frame.find("#HolidayName").val(),
                            "HolidayStartTime": frame.find("#HolidayStartTime").val(),
                            "HolidayEndTime": frame.find("#HolidayEndTime").val(),
                            "ComparedStartTime": frame.find("#ComparedStartTime").val(),
                            "ComparedEndTime": frame.find("#ComparedEndTime").val(),
                            "ForecastDate": frame.find("#ForecastDate").val(),
                            "ForecastFloat": frame.find("#ForecastFloat").val(),
                            "Type": "",
                            "ConfigItem": ""
                        }
                        $.ajax({
                            type: "POST",
                            dataType: 'json',
                            url: "/SystemManage/UpdateConfig",
                            data: queryData,
                            success: function (data) {
                                if (data.ResultKey == 1) {
                                    alert(data.ResultValue);
                                    $("#editBJHoliday").dialog("close");
                                    $("#bj").GridUnload();
                                    getConfigInfo1();
                                } else if (data.ResultKey == 2) {
                                    alert(data.ResultValue);
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
                width:"auto",
                height: "auto"
            }
        });
}
//天津段编辑
function editTJRow(id) {
    $("#selId").val(id);
    $("#editTemplate").frameDialog({
        src: "/SystemManage/editTemplate",
        onload: function () {
            var rowData = $("#tjConfig").jqGrid('getRowData', id);
            var ConfigItem = rowData.ConfigItem;
            var dialogheight = ((ConfigItem.split(",").length) + 1) * 45;
            var dialogwidth = 300;
            if (ConfigItem.indexOf("Start") != -1) {
                dialogwidth = 390;
            }
            var frame = $("iframe[name=dialog-frame]").contents();
            //假期名称可配置
            if (ConfigItem.indexOf("HDayId") != -1) {
                frame.find("#HolidayName").hide();
                frame.find("#HolNameList").show();
                if (ConfigItem.indexOf("HDayStart") != -1 && ConfigItem.indexOf("CompStart") != -1) {
                    dialogheight -= 110;                   
                    frame.find("#third_tr").show();
                    frame.find("#fourth_tr").show();
                    frame.find("#fifth_tr").show();
                } else {
                    dialogheight -= 30;
                    frame.find("#third_tr").show();
                    frame.find("#fourth_tr").show();
                }
            } else {
                if (ConfigItem.indexOf("ForeDate") != -1 && ConfigItem.indexOf("CheckFloat") != -1 && ConfigItem.indexOf("ForeFloat") != -1) {
                    dialogheight -= 15;
                    frame.find("#first_tr").show();
                    frame.find("#second_tr").show();
                    frame.find("#third_tr").show();
                    frame.find("#fourth_tr").show();
                }
            }

            $("iframe[name=dialog-frame]").height(dialogheight);
            $("iframe[name=dialog-frame]").width(dialogwidth);
            setWindowPos('editTemplate');
        },
        title: "配置项编辑",
        titleCloseIcon: true,
        resizable: false,
        dialog: {
            buttons: {
                "保存": function () {
                    var frame = $("iframe[name=dialog-frame]").contents();
                    var queryData = {
                        "CheckFloat": frame.find("#checkFloat").val(),
                        "ReportRemark": "",
                        "HolidayConfigId": frame.find("#HolidayConfigId").val(),
                        "ConfigName": "",
                        "HolidayId": frame.find("#HolNameList option:selected").attr("id"),
                        "HolidayName": frame.find("#HolidayName").val(),
                        "HolidayStartTime": frame.find("#HolidayStartTime").val(),
                        "HolidayEndTime": frame.find("#HolidayEndTime").val(),
                        "ComparedStartTime": frame.find("#ComparedStartTime").val(),
                        "ComparedEndTime": frame.find("#ComparedEndTime").val(),
                        "ForecastDate": frame.find("#ForecastDate").val(),
                        "ForecastFloat": frame.find("#ForecastFloat").val(),
                        "Type": "",
                        "ConfigItem": ""
                    }
                    $.ajax({
                        type: "POST",
                        dataType: 'json',
                        url: "/SystemManage/UpdateConfig",
                        data: queryData,
                        success: function (data) {
                            if (data.ResultKey == 1) {
                                alert(data.ResultValue);
                                $("#editTemplate").dialog("close");
                                $("#tjConfig").GridUnload();
                                getTJConfigInfo();
                            } else if (data.ResultKey == 2) {
                                alert(data.ResultValue);
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
            width: "auto",
            height: "auto"
        }
    });
}
function resize() {
    $("body").height($(window).height()).width($(window).width());
    $("#big").height($("#maincontent").height());
    $("#creatReport").width($("#big").width() - 30);
    $(".tableDiv").width($("#big").width() - 30);
    $("#daily").jqGrid('setGridWidth', $("#creatReport").width());
    $("#bj").jqGrid('setGridWidth', $("#creatReport").width());
    $("#tjConfig").jqGrid('setGridWidth', $("#creatReport").width());
}
$(window).resize(function () {
    resize();
})