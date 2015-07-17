var shousuo = 1;
$(document).ready(function () {
    ///////////等待效果
    var windowHeight = $(window).height();
    var windowWidth = $(window).width();
    var marginLeft = (windowWidth - 220) * 0.5;
    var marginTop = (windowHeight - 20) * 0.5;
    var marginLeft1 = (windowWidth - 160) * 0.5;
    var marginTop1 = (windowHeight - 110) * 0.5;    
    $("#shadow").css({ "width": windowWidth + "px", "height": windowHeight + "px" });
    $("#shadow img").css({ "left": marginLeft + "px", "top": marginTop + "px" });
    $("#shadow span").css({ "left": marginLeft1 + "px", "top": marginTop1 + "px" });
    
    isShowload(1);
    setTimeout("isShowload(0)", 2500);
    /////////////

    resizes();
    openhref('/DaliyReport/NaturalHour');
    //加载用户信息
    loadUserInfo();
    //$("#rightframe").contents()[0].location.reload();
   // $("#rightframe")[0].src = "/DaliyReport/NaturalHour";
    $("#dailyReport").addClass("beijing_select");
    $("#shousuo").click(function () {
        closeboxleft();
    });
    // 收缩后点击弹出菜单栏,下拉相应的菜单项
    $(".menu-small-Icon").click(function () {
        $("#left").removeClass("left1");
        $("#left").addClass("left");
        $("#shousuo").removeClass("open").addClass("shensuo");
        $("#menu_small").hide();
        $("#c1").show();
        $("#c1").siblings(".allsecond").hide();
        $("#dailyReport").addClass("beijing_select").siblings().removeClass("beijing_select");
        $("#dailyReport").find("span:eq(0)").addClass("menu-icon_sel");
        $("#dailyReport").siblings().each(function () {
            var $icon = $(this).find("span:eq(0)");
            $icon.removeClass($icon.attr("data-icon") + "_sel");
        });
        shousuo = 1;
        resize();
    });
    $(".beijing-small-Icon").click(function () {
        $("#left").removeClass("left1");
        $("#left").addClass("left");
        $("#shousuo").removeClass("open").addClass("shensuo");
        $("#menu_small").hide();
        $("#c2").show();
        $("#c2").siblings(".allsecond").hide();
        $("#beijing").addClass("beijing_select").siblings().removeClass("beijing_select");
        $("#beijing").find("span:eq(0)").addClass("beijingIcon_sel");
        $("#beijing").siblings().each(function () {
            var $icon = $(this).find("span:eq(0)");
            $icon.removeClass($icon.attr("data-icon") + "_sel");
        });
        shousuo = 1;
        resize();
    });
    $(".tj-small-Icon").click(function () {
        $("#left").removeClass("left1");
        $("#left").addClass("left");
        $("#shousuo").removeClass("open").addClass("shensuo");
        $("#menu_small").hide();
        $("#c3").show();
        $("#c3").siblings(".allsecond").hide();
        $("#tj").addClass("beijing_select").siblings().removeClass("beijing_select");
        $("#tj").find("span:eq(0)").addClass("tjIcon_sel");
        $("#tj").siblings().each(function () {
            var $icon = $(this).find("span:eq(0)");
            $icon.removeClass($icon.attr("data-icon") + "_sel");
        });
        shousuo = 1;
        resize();
    });
    $(".system-small-Icon").click(function () {
        $("#left").removeClass("left1");
        $("#left").addClass("left");
        $("#shousuo").removeClass("open").addClass("shensuo");
        $("#menu_small").hide();
        $("#c4").show();
        $("#c4").siblings(".allsecond").hide();
        $("#sys").addClass("beijing_select").siblings().removeClass("beijing_select");
        $("#sys").find("span:eq(0)").addClass("systemIcon_sel");
        $("#sys").siblings().each(function () {
            var $icon = $(this).find("span:eq(0)");
            $icon.removeClass($icon.attr("data-icon") + "_sel");
        });
        shousuo = 1;
        resize();
    });
    //修改密码
    $("#updPwd").click(function () {
        initPassWord();
    });
    $("#updPwd").mouseover(function () {
        $(".pwd").addClass("pwd_hover");
        $(this).css({ "color": "#003a71" });
    });
    $("#updPwd").mouseout(function () {
        $(".pwd").removeClass("pwd_hover");
        $(this).css({ "color": "#fff" });
    });
    //退出
    $("#exit").click(function () {
        $.ajax({
            type: 'Post',
            dataType: 'json',
            url: "/SystemManage/Exit",
            success: function (data) {
                //alert("退出成功！");
            }
        });
        window.location = "/SystemManage/Login";
    });
    $("#exit").mouseover(function () {
        $(".exit").addClass("exit_hover");
        $(this).css({ "color": "#003a71" });
    });
    $("#exit").mouseout(function () {
        $(".exit").removeClass("exit_hover");
        $(this).css({ "color": "#fff" });
    });
   
    //点击左侧根节点任何地方均可展开列表
    $(".beijing").each(function () {
        var $icon = $(this).find("span:eq(0)");
        $icon.attr("data-icon", $icon.attr("class"));
    });
   // $("#dailyReport").click();
    $("#dailyReport").find("span:eq(0)").addClass("menu-icon_sel");
    $(".beijing").click(function () {
        var $icon = $(this).find("span:eq(0)");
        if (!$(this).hasClass("beijing_select")) {
            $icon.addClass($icon.attr("data-icon") + "_sel");
        }
        
        $(this).siblings().each(function () {
            var $icon = $(this).find("span:eq(0)");
            $icon.removeClass($icon.attr("data-icon") + "_sel");
        });

        $(this).addClass("beijing_select").siblings().removeClass("beijing_select");
        if ($(this).next().css("display") == 'none') {
            $(this).next().slideDown(200).siblings(".allsecond").slideUp(200);
        }
    });
    //鼠标滑过提示信息
    $(function(){
        var x=10;
        var y=20;
        $(".beijing-small span").mouseover(function (e) {
 //           this.myTitle=this.title;
  //          this.title="";
            var tooltip = "<div id='tooltip'>"+每日报送+"</div>";
            $("body").append(tooltip);
            $(".beijing-small span").css({
                "top":(e.pageY+y)+"px",
                "left":(e.pageX+x)+"px"
            }).show("fast");
        })
    });
    //二级菜单展开与关闭
    $(".allsecond .second-list").click(function () {
        if ($(this).next().css("display") == 'block') {
            $(this).removeClass("second-list-select");
            $(this).find(".second_icon_sel").removeClass("second_icon_sel").addClass("second_icon");
            $(this).find(".secondColor").removeClass("secondColor");
            $(this).next().slideUp(200);
        } else {
            $(this).next().slideDown(200);            
            $(this).addClass("second-list-select");
            $(this).find(".second_icon").removeClass("second_icon").addClass("second_icon_sel");
            $(this).find(".menu-titleName").addClass("secondColor");
            
        }
    });
    //三级菜单选中
    $(".datalist li").click(function () {
        $(".datalist li a").removeClass("selected-li");
        $(".datalist li span").removeClass("span_select");
        $(this).find("a").addClass("selected-li");
        $(this).find("span").addClass("span_select");
        $("#userManage").find(".second_icon_sel").removeClass("second_icon_sel").addClass("second_icon");
        $("#userManage").find("a").css("color", "#5b626d");
        $("#logManage").find(".second_icon_sel").removeClass("second_icon_sel").addClass("second_icon");
        $("#logManage").find("a").css("color", "#5b626d");
    });
    //用户权限管理选中
    $("#userManage").click(function () {
        $(this).find("a").css("color", "#578adf");
        $(this).find(".second_icon").removeClass("second_icon").addClass("second_icon_sel");
        $(".datalist li a").removeClass("selected-li");
        $("#logManage").find(".second_icon_sel").removeClass("second_icon_sel").addClass("second_icon");
        $("#logManage").find("a").css("color", "#5b626d");
    });
    //日志查询选中
    $("#logManage").click(function () {
        $(this).find("a").css("color", "#578adf");
        $(this).find(".second_icon").removeClass("second_icon").addClass("second_icon_sel");
        $(".datalist li a").removeClass("selected-li");
        $("#userManage").find(".second_icon_sel").removeClass("second_icon_sel").addClass("second_icon");
        $("#userManage").find("a").css("color", "#5b626d");
    });
    $(window).resize(function () {
        resizes();
    });
    
});
//加载等待效果
function isShowload(num) {
    if (num == 1) {
        $("#shadow").show();
    } else {
        $("#shadow").hide();
    }
}
function resizes() {
    $("#all").height($(window).height());
    $(".header").width($(window).width());
    $(".middle").height($(window).height() - $(".header").height() - $(".bottom").height());
    $(".left").height($(window).height() - $(".header").height() - $(".bottom").height());
    $(".right").height($(window).height() - $(".header").height() - $(".bottom").height());
    $("#maincontent").height($(window).height() - $("#header").height() - $(".bottom").height());
    $("iframe[name='right-middle']").height($("#maincontent").height());
    $("#menu").height($(window).height() - $(".header").height() - $(".bottom").height());
    $("#menu_small").height($(window).height() - $(".header").height() - $(".bottom").height());
    var shensuoTop = ($(window).height()) * 0.7 + 50;
    $(".shensuo").css({ "margin-top": shensuoTop + "px"});
}

//左侧菜单的收缩
function closeboxleft() {
    if (shousuo == 1) {
        $("#left").removeClass("left");
        $("#left").addClass('left1');
        $("#shousuo").removeClass("shensuo").addClass("open");
        $("#menu_small").show();
        shousuo = 2;
        resize();
    } else {
        $("#left").removeClass("left1");
        $("#left").addClass("left");
        $("#shousuo").removeClass("open").addClass("shensuo");
        $("#menu_small").hide();
        shousuo = 1;
        resize();
    }
}


//修改密码弹出框
function initPassWord() {
    var PassWordUserId = $("#userId").val();
    $("#PassWordAlter").frameDialog({
        src: "/SystemManage/UpdatePassWord",
        onload: function () {
        },
        title: '修改密码',
        titleCloseIcon: false,
        modal: true,
        resizable: false,
        dialog: {
            buttons: {
                "确认": function () {
                    //window.frames["dialog-frame"].save(PassWordUserId);

                    var password = $("iframe[name=dialog-frame]").contents().find("#password").val();
                    var oldpassword = $("iframe[name=dialog-frame]").contents().find("#oldpassword").val();
                   var checkpassword = $("iframe[name=dialog-frame]").contents().find("input[name='checkPassword']").val();
                    if (oldpassword == '') {
                        alert("请输入原密码！");
                        $("iframe[name=dialog-frame]").contents().find("#oldpassword").focus();
                        return false;
                    }
                    if (password == '') {
                        alert("请输入新密码！");
                        return false;
                    }
                    if (checkpassword == '') {
                        alert("请再输入一次新密码！");
                        return false;
                    }

                    if (password !== checkpassword) {
                        alert("两次输入的密码不一致！");
                        return false;
                    }
                    
                    $.ajax({
                        dataType: "json",
                        type: "POST",
                       
                        url: "/SystemManage/UpdatePassWord",
                        data: { "userId": PassWordUserId, "password": password, "oldpassword": oldpassword },
                        success: function (data) {
                            alert(data.ResultValue);
                            if (data.ResultKey == 1) {
                                //alert(data.ResultValue);
                                window.location = "/SystemManage/Login";
                                //$("iframe[name=dialog-frame]").contents().find("#password").attr("value", "");
                                //$("iframe[name=dialog-frame]").contents().find("#oldpassword").attr("value", "");
                                //$("iframe[name=dialog-frame]").contents().find("input[name='checkPassword']").attr("value", "");
                                //$("iframe[name=dialog-frame]").contents().find("#password").focus();
                              //  $(this).dialog("close");
                                }
                           // else if (data.ResultKey == 0) {
                            //    alert(data.ResultValue);
                                //}
                            else if (data.ResultKey == 3) {
                                //alert(data.ResultValue);
                                $("iframe[name=dialog-frame]").contents().find("#password").attr("value", "");
                                $("iframe[name=dialog-frame]").contents().find("input[name='checkPassword']").attr("value", "");
                                $("iframe[name=dialog-frame]").contents().find("#password").focus();
                            }

                        },
                        error: function (data) {
                            alert("抱歉，修改失败！");
                        }
                    });

                    $(this).dialog("close");
                },
                "取消": function () {
                    $(this).dialog("close");
                }
            },

            width: '400',
            height: '260'

        }
    });
}
//关闭弹窗
function closeDialog(id) {
    $("#dialog_"+id).dialog("close");
}
//加载页面信息
function loadUserInfo() {
    $.ajax({
        type: 'Post',
        dataType: 'json',
        url: "/SystemManage/GetUserInfo",
        //beforeSend: function () {
        //    isShowload(1);
        //},
        //complete: function () {
        //    isShowload(0);
        //},
        success: function (data) {
            $("#userId").val(data.UserId);
            if (data.UserName == '') {
                window.location = "/SystemManage/Login";
            }
            $("#user").text(data.UserName);//登录用户名
            var RoleId;
            $.each(data.RoleList, function (a, b) {
                RoleId = b.RoleId;                 
            });
            //角色权限判断是否显示系统管理菜单
            if (RoleId != "F4711D45-F6AA-46EE-B4B8-60144100C460" && RoleId != "f4711d45-f6aa-46ee-b4b8-60144100c460") {
                $("#sys").hide();
                $("#isAdmin").val(false);
            } else {
                $("#isAdmin").val(true);
                $("#sys").show();
            }
            $("#userId").val(data.UserId);
            $("#RoleId").val(data.RoleId);
        },
        error: function (data) {
            window.location = "/SystemManage/Login";

        }
    });
}



//无数据列表弹窗公用方法
function content(id, url, title, width, height) {
    $("#"+ id).frameDialog({
        src: url,
        onload: function () {
        },
        title: title,
        titleCloseIcon: true,
        resizable: false,
        dialog: {
            modal: true,
            width: width,
            height: height
        }
    });
}
//校正弹窗公用方法
function checkWindow(id, url, title, width, height) {
    $("#" + id).frameDialog({
        src: url,
        onload: function () {
        },
        title: title,
        titleCloseIcon: true,        
        resizable: false,
        dialog: {
            buttons: {
                "确定": function () {
                    $(this).dialog("close");
                },
                "取消": function () {
                    $(this).dialog("close");
                }
            },
            modal: true,
            width: width,
            height: height
        }
    });
}
function openhref(url) {
    $("#maincontent").children().remove();
    $("#maincontent").load(url);
}