$(document).ready(function () {
    alert("111");
   // $("#UserName").attr("value", "");
   // $("#UserName").focus();
    $("#login_btn").click(function () {
        alert("111");
        var userName = $("#UserName").val();
        var password = $("#Password").val();
        if (userName == "") {
            alert("用户名不能为空!");
            $("#UserName").focus();
            return;
        }
        if (password == "") {
            alert("密码不能为空!");
            $("#Password").focus();
            return;
        }
        $.ajax({
            type: 'Post',
            dataType: 'json',
            url: "/SystemManage/Login",
            data: { "UserName": userName, "Password": password },
            success: function (result) {
                if (result != "true") {
                    if (result == "用户名不存在，请重新输入！") {
                        $("#UserName").attr("value", "");
                        $("#Password").attr("value", "");
                        $("#UserName").focus();
                    }
                    if (result == "密码错误，请重新输入！") {
                        $("#Password").attr("value", "");
                        $("#Password").focus();
                    }
                    alert(result);
                    return;
                }
            },
            error: function (result) {
                alert(result);
            }
        });
    }); 
 });
   
   
       
       
        
           
            