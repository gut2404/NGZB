function submitForm()
{
    $('#ff').form('submit', {
        onSubmit: function ()
        {
            if ($(this).form('enableValidation').form('validate'))
            {
                var pd = pass();
                if (checkPass == '0')
                {
                    $.ajax(
                        {
                            url: url_add,
                            type: 'post',
                            data: {
                                roleName: $("#roleName").val(),
                                roleInfo: $("#roleInfo").val()
                            },
                            async: false,
                            cache: false,
                            dataType: 'text',
                            success: function (data)
                            {
                                if (data == -1)
                                {
                                    layer.alert("数据不对，请不要伪造数据！", { icon: 2 });
                                } else if (data == 1)
                                {
                                    layer.alert("角色添加成功", { icon: 1 }, function (index)
                                    {
                                        try
                                        {
                                            parent.layer.closeAll();
                                            parent.search();
                                        } catch (e) { }
                                    });
                                } else if (data == 0)
                                {
                                    layer.alert("该角色已存在，不能强行添加！", { icon: 2 });
                                }
                                else
                                {
                                    layer.alert("添加失败，联系程序员！", { icon: 2 });
                                }
                            },
                            error: function ()
                            {
                                layer.alert("通讯失败，请检查服务器！", { icon: 2 });
                            }
                        });
                }
                else
                {
                    layer.alert("该角色已注册！", { icon: 0 });
                }
            }
        }
    });
};
function pass()
{
    var postUrl = url_pass;
    var roleName = $("#roleName").val();
    $.ajax(
        {
            url: postUrl,
            type: 'post',
            data: {
                roleName: roleName
            },
            async: false,
            cache: false,
            dataType: 'text',
            success: function (data)
            {
                checkPass = data;
            },
            error: function ()
            {
                layer.alert("通讯失败，请重试！", { icon: 2 });
            }
        });
};
function clearForm()
{
    $('#ff').form('clear');
};
$(function ()
{
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.iframeAuto(index);
});