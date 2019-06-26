var checkUser = "1";
function submitForm()
{
    $('#ff').form('submit', {
        onSubmit: function ()
        {
            if ($(this).form('enableValidation').form('validate'))
            {
                var pd = userPass();
                if (checkUser == 0)
                {
                    var password;
                    if ($("#password2").val() == "")
                    {
                        password = "";
                    }
                    else
                    {
                        password = $.md5($("#password2").val());
                    }
                    $.ajax(
                        {
                            url: url_edit,
                            type: 'post',
                            data: {
                                userCode: $("#userCode").val(),
                                userName: $("#userName").val(),
                                userPassWord: password,
                                groupId: $("#cc").combobox("getValue")
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
                                    layer.alert("用户编辑成功！", { icon: 1 });
                                    try
                                    {
                                        parent.search();
                                    } catch (e) { }
                                } else
                                {
                                    layer.alert("编辑失败，联系程序员！", { icon: 2 });
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
                    layer.alert("该用户名已注册！", { icon: 0 });
                }
            }
        }
    });
};
function userPass()
{
    var postUrl = url_pass;
    var userCode = $("#userCode").val();
    var pass1 = $("#password").val();
    var pass2 = $("#password2").val();
    if (pass1 != pass2)
    {
        layer.alert("两次密码不一致，请重新输入！", { icon: 0 });
        return;
    }
    if ($("#cc").combobox("getValue") == "")
    {
        layer.alert("部门没有选择！", { icon: 0 });
        return;
    }
    checkUser = 0;
};
function redoPass()
{
    $.ajax(
        {
            url: url_passwordredo,
            type: 'post',
            data: {
                userCode: $("#userCode").val(),
                userPassWord: $.md5('111111')
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
                    layer.alert("密码重置成功！", { icon: 1 });
                } else
                {
                    layer.alert("重置失败，联系程序员！", { icon: 2 });
                }
            },
            error: function ()
            {
                layer.alert("通讯失败，请检查服务器！", { icon: 2 });
            }
        });
};
$(function ()
{
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.iframeAuto(index);
    var _usercode = $('#userCode').textbox('getValue');
    if (_usercode == '')
    {
        $("#ok").linkbutton("disable");
        $("#passredo").linkbutton("disable");
    }
});