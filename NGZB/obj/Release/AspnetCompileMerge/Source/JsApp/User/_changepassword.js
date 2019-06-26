function subcchangepass()
{
    if ($("#oldpassword").val().length == 0)
    {
        layer.alert("原密码不能为空!", { icon: 0 });
        return;
    }
    if ($("#newpassword").val().length == 0)
    {
        layer.alert("新密码不能为空!", { icon: 0 });
        return;
    }
    var oldpass = $.md5($("#oldpassword").val());
    var newpass = $.md5($("#newpassword").val());
    if (newpass != $.md5($("#confirmpass").val()))
    {
        layer.alert("两次密码不一致!", { icon: 0 });
        return;
    }
    $.ajax(
        {
            url: url_ChangePassWord,
            type: 'post',
            data: {
                oldpassword: oldpass,
                userPassWord: newpass
            },
            cache: false,
            dataType: 'text',
            success: function (data)
            {
                if (data == -1)
                {
                    layer.alert("数据不对，请不要伪造数据！", { icon: 2 });
                } else if (data == 1)
                {
                    layer.alert("密码修改成功!", { icon: 1 }, function ()
                    {
                        try
                        {
                            parent.layer.closeAll();
                        } catch (e) { }
                    });
                } else if (data =="0")
                {
                    layer.alert("原密码不正确!", { icon: 2 });
                }
                else
                {
                    layer.alert("注册失败，联系程序员!", { icon: 2 });
                }
            },
            error: function ()
            {
                layer.alert("通讯失败，请检查服务器!", { icon: 2 });
            }
        });
};