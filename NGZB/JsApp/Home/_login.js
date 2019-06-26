function sub(formUrl)
{
    var formUserCode, formPassWord, nomd5
    formUserCode = $('#userCode').val();
    formPassWord = $('#passWord').val();
    if (formUserCode === '' || formPassWord === '')
    {
        layer.alert("用户名和密码不能为空！", { icon: 0 });
        return;
    };
    nomd5 = formPassWord;
    formPassWord = $.md5(formPassWord);
    $.ajax(
        {
            url: formUrl,
            type: 'POST',
            data: {
                usercode: formUserCode,
                password: formPassWord,
                remember: $("#lg").switchbutton("options").checked
            },
            cache: false,
            dataType: 'text',
            success: function (data)
            {
                if (data == '1')
                {
                    window.top.location.reload();
                    //window.parent.parent.parent.parent.location.reload();
                }
                else if (data == "-1")
                {
                    layer.alert("数据错误！", { icon: 2 });
                }
                else
                {
                    layer.alert("密码错误，请重输！", { icon: 0 });
                }
            },
            error: function ()
            {
                layer.alert("通讯错误，联系程序员！", { icon: 2 });
            }
        });
};
$('#passWord').keypress(function (event)
{
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13')
    {
        sub(url_post);
    }
});  