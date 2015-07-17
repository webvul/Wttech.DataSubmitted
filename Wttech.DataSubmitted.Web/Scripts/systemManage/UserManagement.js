$(document).ready(function () {
    resize();
    //加载用户列表
    getUserTable();
    //新增用户按钮显示添加区域
    $("#addUser_btn").click(function () {
        if (type != 1) {
            alert("请先保存或取消后再新增用户！");
            return;
        }
        $("#addUser").show();
        $("#operate_btn").hide();
        type = 0;
        $("#rolelist")[0].innerHTML = $("#tabel_rolelist")[0].innerHTML;//加载用户角色列表
        $("#rolelist input[value='数据报送员']").attr("checked", "checked");
    });
    //保存新增用户按钮
    $("#saveUser").click(function () {
        saveAddUser();

    });
    //取消新增用户
    $("#cancle").click(function () {
        $("#addUserName").attr("value", "");//将新增的用户名输入框清空
        $("#addUser").hide();
        $("#operate_btn").show();
        type = 1;
    });
    //加载角色单选列表
    loadRoleList();
    //查询功能
    $("#query").click(function () {
        var page = $("#userTable").jqGrid("getGridParam", "page");
        var rows = $("#userTable").jqGrid("getGridParam", "rowNum");
        var userName = $("#query_userName").val();
        queryTable(rows, page, userName);
    });
});
var type = 1;//type=1时，判断只可对当前行进行编辑保存，禁止其他行的编辑、初始化密码、新增用户以及查询列表信息
function resize() {
    $("#creatReport").width($("#big").width() - 30);
    $(".tableDiv").width($("#big").width() - 30);
    $("#userTable").jqGrid('setGridWidth', $("#maincontent").width() - 30);
   // $("#userTable_cb").width(24);
    $("#big").height($("#maincontent").height());
}
$(window).resize(function () {
    resize();
});
//加载用户管理列表
function getUserTable() {
    var page = $("#userTable").jqGrid("getGridParam", "page");
    var rows = $("#userTable").jqGrid("getGridParam", "rowNum");
    // var postdata='{ pageSize:"' + 10 + '", pageIndex: "' + 1 + '",userName:""}';
    jQuery("#userTable").jqGrid({
        mtype: "POST",
        url: "/SystemManage/UserManagement",
        postData: { "pageSize": 10, "pageIndex": 1, "userName": "" },
        scrollOffset: 0,
        autoScroll: false,
        datatype: "json",
        rowNum: 10,
        pager: '#pager2',
        viewrecords: true,
        height: '100%',
        multiselect: true,
        prmNames: { page: "pageIndex", rows: "pageSize", sort: null, order: null, search: null, nd: null, npage: null },
        colNames: ['', '登录名', '角色', '', '操作'],
        colModel: [
              { name: 'UserId', index: 'UserId', align: 'center', hidden: true, resizable: false },
              { name: 'UserName', index: 'UserName', align: 'center', title: false, sortable: false, editable: true, edittype: "text",resizable:false },
              { name: 'RoleName', index: 'RoleName', align: 'center', title: false, sortable: false, editable: true, edittype: "custom", editoptions: { custom_element: myelem, custom_value: myvalue }, formatter: listformatter, resizable: false },
              { name: 'RoleId', index: 'RoleId', align: 'center', hidden: true, editable: true, title: false, formatter: formatter, resizable: false },
              { name: 'Action', index: 'UserId', align: 'center', sortable: false, title: false, resizable: false }
        ],
        jsonReader: {
            root: "UserInfo",//数据源
            total: "PageCount",//总页数
            page: "PageIndex", //当前页
            records: "Count",  //数据总条数
            repeatitems: false
        },
        gridComplete: function () {
            var ids = jQuery("#userTable").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = "<a class='tabelhover' href='#' onclick=\"editRow('" + cl + "');\" >编辑</a>&nbsp;&nbsp;&nbsp;&nbsp;";
                ce = "<a class='tabelhover' href='#' onclick=\"clearPwd('" + cl + "');\"  >初始化密码</a>";
                jQuery("#userTable").jqGrid('setRowData', ids[i], { Action: be + ce });
            }
        }
    });
    $("#userTable").jqGrid('setGridWidth', $("#maincontent").width() - 30);
   // $("#userTable_cb").width(24);
    jQuery("#userTable").jqGrid('navGrid', "#pager2", { edit: false, add: false, del: false, search: false, refresh: false });

}
//解析rolelist角色列中RoleName及RoleId
function listformatter(cellvalue, options, rowObject) {
    var RoleName;
    if (cellvalue != undefined) {
        return cellvalue;
    }
    $.each(rowObject.RoleList, function (i, j) {
        RoleName = j.RoleName;
    });
    return RoleName;
}
function formatter(cellvalue, options, rowObject) {
    var RoleId;
    $.each(rowObject.RoleList, function (i, j) {
        RoleId = j.RoleId;
    });
    return RoleId;
}
//创建一个radio
function myelem(value, options) {
    $("#tabel_rolelist").find("input[value='" + value + "']").attr("checked", "checked");
    var el = "<div>" + $("#tabel_rolelist").eq(0).html() + "</div>";
    $("#tabel_rolelist").find("input[value='" + value + "']").removeAttr("checked");
    return el;
}
function myvalue(el, operation, value) {
    if (operation === 'get') {
        return $(el).find("input[type='radio']:checked").val();
    }
}
//行编辑
function editRow(id) {

    if (type == 1) {
        type = 0;
        jQuery("#userTable").jqGrid('editRow', id);
        var saveRow = "<a onclick=\"saveRow('" + id + "');\" title=\"保存\" class='tabelhover'>保存</a>&nbsp;&nbsp;&nbsp;&nbsp<a onclick=\"quit('" + id + "');\" title=\"取消\" class='tabelhover'>取消</a>";
        $("#userTable").jqGrid('setRowData', id, { Action: saveRow });
    } else {
        alert("请先保存或取消后再编辑!");
    }
}
//行保存
function saveRow(id) {
    var rowData = $("#userTable").jqGrid("getRowData", id);
    var userId = rowData.UserId;
    var userName = $("#" + id + "_UserName").val();
    var roleId = $("div[id='" + id + "_RoleName'] input[type='radio']:checked").attr('id');
    $.ajax({
        contentType: "application/json",
        type: "POST",
        url: "/SystemManage/Update",
        data: '{ userId:"' + userId + '", roleList: ["' + roleId + '" ], userName:"' + userName + '"}',
        success: function (data) {
            if (data.ResultKey == 1) {
                alert("修改成功！");
                ///*@cc_on @*/
                //alert(@Wttech.DataSubmitted.Web.Resources.TipInfo.NameIsNull);
                type = 1;
                jQuery("#userTable").jqGrid('saveRow', id);
                var publish = "<a onclick=\"editRow('" + id + "');\" title=\"编辑\" >编辑</a>&nbsp;&nbsp;&nbsp;&nbsp;<a class='tabelhover' href='#' onclick='clearPwd()' >初始化密码</a>";
                $("#userTable").jqGrid('setRowData', id, { Action: publish });
                $("#userTable").trigger("reloadGrid");
            }
            else {
                //else if (data == 2) {
                //    alert("请选择用户");
                //} else if (data == 3) {
                //    alert("请选择角色")
                //} else if (data == 4) {
                //    alert("请输入用户名");
                //} else {
                //    alert("修改失败");
                //}
                alert(data.ResultValue);
            }
        },
        error: function (data) {
            alert(data);
        }
    });
}
//行编辑取消
function quit(id) {
    type = 1;
    $("#userTable").jqGrid("restoreRow", id);
    var publish = "<a onclick=\"editRow('" + id + "');\" title=\"编辑\">编辑</a>&nbsp;&nbsp;&nbsp;&nbsp;<a class='tabelhover' href='#' onclick='clearPwd()' >初始化密码</a>";
    $("#userTable").jqGrid('setRowData', id, { Action: publish });
}

//加载角色权限列表
function loadRoleList() {
    $.ajax({
        contentType: "application/json",
        type: "POST",
        url: "/SystemManage/GetRoleList",
        success: function (data) {
            $.each(data, function (a, b) {
                var str = "";
                str += "<input name='roleradio' type='radio' value='" + b.RoleName + "' id=" + b.RoleId + " />" + b.RoleName;
                $("#tabel_rolelist").append(str);
            })

        }
    });
}
//查询用户列表
function queryTable(rows, page, userName) {
    if (type != 1) {
        alert("请先保存或取消后再查询！");
        return;
    }
    jQuery("#userTable").jqGrid('setGridParam', {
        url: "/SystemManage/UserManagement",
        postData: { "pageSize": rows, "pageIndex": page, "userName": userName }
    }).trigger("reloadGrid");
}
//新增用户后保存
function saveAddUser() {
    $(".saveIcon").addClass("saveIcon_select");
    $(".saveIcon+span").css({ "color": "#5789DF" });
    var userName = $("#addUserName").val();
    var roleId = $("input[type='radio']:checked").attr('id');
    var postdata = { roleList: [roleId], userName: userName };
    $.ajax({
        dataType: 'json',
        type: "POST",
        url: "/SystemManage/Add",
        data: postdata,
        success: function (data) {
            if (data == 1) {
                alert("添加成功!");
                type = 1;
                $("#addUserName").attr("value", "");
                $("#addUser").hide();
                $("#operate_btn").show();
                $("#userTable").trigger("reloadGrid");
                $(".saveIcon").removeClass("saveIcon_select");
                $(".saveIcon+span").css({ "color": "#555" });
            } else if (data == 2) {
                alert("添加失败，请选择角色!");
                $("#addUserName").focus();
                $(".saveIcon").removeClass("saveIcon_select");
                $(".saveIcon+span").css({ "color": "#555" });
                return;
            } else if (data == 3) {
                alert("添加失败，请输入用户名!");
                $("#addUserName").attr("value", "");
                $("#addUserName").focus();
                $(".saveIcon").removeClass("saveIcon_select");
                $(".saveIcon+span").css({ "color": "#555" });
                return;
            } else if (data == 7) {
                alert("用户名已被使用！");
                $("#addUserName").focus();
                $(".saveIcon").removeClass("saveIcon_select");
                $(".saveIcon+span").css({ "color": "#555" });
            }
            else if (data == 8) {
                alert("用户名为历史曾用名，不能重复使用！");
                $("#addUserName").focus();
                $(".saveIcon").removeClass("saveIcon_select");
                $(".saveIcon+span").css({ "color": "#555" });
            }
            else {
                alert("添加失败!");
                $("#addUserName").attr("value", "");
                $("#addUserName").focus();
                $(".saveIcon").removeClass("saveIcon_select");
                $(".saveIcon+span").css({ "color": "#555" });
                return;
            }

        },
        error: function (data) {
            alert("添加失败!");
            $(".saveIcon").removeClass("saveIcon_select");
            $(".saveIcon+span").css({ "color": "#555" });
            return;
        }
    });
}
//获取选中行userlist
function getSelUser() {
    var sels = $("#userTable").jqGrid('getGridParam', 'selarrrow');
    var len = sels.length;
    var userId = new Array(len);
    for (var i = 0; i < len ; i++) {
        var rowData = $("#userTable").jqGrid('getRowData', sels[i]);
        userId[i] = rowData.UserId;
    }
    return;
}
//删除用户
function delUser() {
    var sels = $("#userTable").jqGrid('getGridParam', 'selarrrow');
    $(".delIcon").addClass("delIcon_select");
    $(".delIcon+span").css({ "color": "#5789DF" });
    if (type != 1) {
        alert("请先保存或取消后再删除用户！");
        $(".delIcon").removeClass("delIcon_select");
        $(".delIcon+span").css({ "color": "#555" });
        return;
    }
    if (sels == "") {
        alert("请选择要删除的用户！");
        $(".delIcon").removeClass("delIcon_select");
        $(".delIcon+span").css({ "color": "#555" });
    } else {
        if (confirm("确定要删除此项吗？")) {
            // getSelUser();
            var len = sels.length;
            var userId = new Array(len);
            for (var i = 0; i < len ; i++) {
                var rowData = $("#userTable").jqGrid('getRowData', sels[i]);
                userId[i] = rowData.UserId;
            }
            var postdata = { userIdList: userId };
            $.ajax({
                type: "POST",
                dataType: 'json',
                url: "/SystemManage/Delete",
                data: postdata,
                error: function () {
                    alert("抱歉，删除失败！");
                    $(".delIcon").removeClass("delIcon_select");
                    $(".delIcon+span").css({ "color": "#555" });
                },
                success: function (data) {
                    if (data == 1) {
                        var len = sels.length;
                        for (var i = 0; i < len ; i++) {
                            $("#userTable").jqGrid('delRowData', sels[0]);
                        }
                        alert("成功删除!");
                        $("#cb_userTable").attr("checked", false);
                        $("#query").click();
                    } else if (data == 2) {
                        alert("删除失败，请选择用户！");
                        $(".delIcon").removeClass("delIcon_select");
                        $(".delIcon+span").css({ "color": "#555" });
                        return;
                    } else {
                        alert("删除失败！");
                        $(".delIcon").removeClass("delIcon_select");
                        $(".delIcon+span").css({ "color": "#555" });
                        return;
                    }
                }
            });
        }
        $(".delIcon").removeClass("delIcon_select");
        $(".delIcon+span").css({ "color": "#555" });
    }
}
//角色分配
function assignUser() {
    //修改点击后样式
    $(".assignIcon").addClass("assignIcon_select");
    $(".assignIcon+span").css({ "color": "#5789DF" });
    //getSelUser();
    var sels = $("#userTable").jqGrid('getGridParam', 'selarrrow');
    if (type != 1) {
        alert("请先保存或取消后再分配角色！");
        $(".assignIcon").removeClass("assignIcon_select");
        $(".assignIcon+span").css({ "color": "#555" });
        return;
    }
    if (sels == "") {
        alert("请选择要分配的用户！");
        $(".assignIcon").removeClass("assignIcon_select");
        $(".assignIcon+span").css({ "color": "#555" });
        return;
    }
    var len = sels.length;
    var assignUserId = new Array(len);
    for (var i = 0; i < len ; i++) {
        var rowData = $("#userTable").jqGrid('getRowData', sels[i]);
        assignUserId[i] = rowData.UserId;
    }
    var status = assignAlert(assignUserId);
}
//初始化密码
function clearPwd(id) {
    var rowData = $("#userTable").jqGrid('getRowData', id);
    var userId = rowData.UserId;
    $.ajax({
        type: "POST",
        dataType: 'json',
        url: "/SystemManage/StartPassword",
        data: { userId: userId },
        error: function () {
            alert("抱歉，初始化密码失败！");
        },
        success: function (data) {
            //if (data.ResultKey == 1) {
            //    alert(data.ResultValue);
            //} else if (data.ResultKey == 2) {
            //    alert(data.ResultValue);
            //    return;
            //} else {
            //    alert("初始化密码失败！");
            //    return;
            //}
            alert(data.ResultValue);
        }
    });
}
//回车键查询
function keyQuery() {
    if (window.event.keyCode == 13) {
        if (window.event.srcElement.id == "query_userName") {
            $("#query").click();
            window.event.keyCode = 9;
        }
    }

}

//分配角色弹窗

var tempAssignUserId;

function assignAlert(assignUserId) {
    tempAssignUserId = assignUserId;
    $("#assignUser").frameDialog({
        src: "/SystemManage/UserRoleAssign",
        title: '用户角色分配',
        titleCloseIcon: true,
        resizable: false,
        dialog: {
            buttons: {
                "保存": function () {
                    //////////////////////////////
                    var roleId = $("iframe[name=dialog-frame]").contents().find("input[name='rolelist'][type='radio']:checked").attr("id");
                    $.ajax({
                        type: "POST",
                        dataType: 'json',
                        url: "/SystemManage/UserRoleAssign",
                        data: { "UserIdList": tempAssignUserId, "RoleIdList": [roleId] },
                        success: function (data) {
                            if (data == 1) {
                                alert("分配成功!");
                                $(".assignIcon").removeClass("assignIcon_select");
                                $(".assignIcon+span").css({ "color": "#555" });
                            } else if (data == 2) {
                                alert("分配失败，请选择用户！");
                                $(".assignIcon").removeClass("assignIcon_select");
                                $(".assignIcon+span").css({ "color": "#555" });
                            } else {
                                alert("分配角色失败！");
                                $(".assignIcon").removeClass("assignIcon_select");
                                $(".assignIcon+span").css({ "color": "#555" });
                            }
                        },
                        error: function (data) {
                            alert("抱歉，分配角色失败！");
                            $(".assignIcon").removeClass("assignIcon_select");
                            $(".assignIcon+span").css({ "color": "#555" });
                        }

                    });
                    //window.frames["dialog-frame"].save(tempAssignUserId);

                    //////////////////////////////////
                    $(this).dialog("close");
                    $("#query").click();
                    //$(window.frames["right-middle"].document).contents().find("#query").click();

                },
                "取消": function () {
                    $(this).dialog("close");
                    $(".assignIcon").removeClass("assignIcon_select");
                    $(".assignIcon+span").css({ "color": "#555" });
                }
            },
            //modal: true,
            modal: true,
            width: '300',
            height: '200'

        }
    });
}