function okActive()
{
    var usercode = $("#userCode").val();
    var username = $("#userName").val();
    var pwd1 = $("#password").val();
    var pwd2 = $("#password2").val();
    var groupid = $("#cc").combobox("getValue");
    if (usercode == '' || username == '' || groupid == '' || pwd1 == '')
    {
        layer.alert("信息填写不全！", { icon: 2 });
        return;
    }
    if (pwd1 != pwd2)
    {
        layer.alert("两次密码不一致！", { icon: 2 });
        return;
    }
    $.ajax(
        {
            url: url,
            type: 'post',
            data: {
                usercode: usercode,
                username: username,
                password: $.md5(pwd2),
                groupid: groupid
            },
            cache: false,
            dataType: 'text',
            success: function (data)
            {
                if (data == "-1")
                {
                    layer.alert("数据不对，请不要伪造数据！", { icon: 2 });
                }
                else if (data == "1")
                {
                    layer.alert("初始化成功", { icon: 1 }, function ()
                    {
                        parent.reload();
                        parent.layer.closeAll();
                    });
                }
                else
                {
                    layer.alert("初始化失败，联系程序员！", { icon: 2 });
                }
            }
        });
};