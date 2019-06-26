var checkUser = "1";
function submitForm() {
    $('#ff').form('submit', {
        onSubmit: function () {
            if ($(this).form('enableValidation').form('validate')) {
                var pd = userPass();
                if (checkUser == 0) {
                    $.ajax(
                        {
                            url: url_add,
                            type: 'post',
                            data: {
                                userCode: $("#userCode").val(),
                                userName: $("#userName").val(),
                                userPassWord: $.md5($("#password2").val()),
                                groupId: $("#cc").combobox("getValue")
                            },
                            cache: false,
                            dataType: 'text',
                            success: function (data) {
                                if (data == -1) {
                                    layer.alert("数据不对，请不要伪造数据！", { icon: 2 });
                                } else if (data == 1) {
                                    layer.alert("用户注册成功", { icon: 1 }, function () {
                                        try {
                                            parent.search();
                                        } catch (e) { }
                                        parent.layer.closeAll();
                                    });
                                } else if (data == 0) {
                                    layer.alert("该用户已存在，不能强行注册！", { icon: 2 });
                                }
                                else {
                                    layer.alert("注册失败，联系程序员！", { icon: 2 });
                                }
                            },
                            error: function () {
                                layer.alert("通讯失败，请检查服务器！", { icon: 2 });
                            }
                        });
                }
                else {
                    layer.alert("该用户名已注册！", { icon: 0 });
                }
            }
        }
    });
};
function userPass() {
    var postUrl = url_pass;
    var userCode = $("#userCode").val();
    var pass1 = $("#password").val();
    var pass2 = $("#password2").val();
    if (pass1 !== pass2) {
        layer.alert("两次密码不一致，请重新输入！", { icon: 0 });
        return;
    }
    if ($("#cc").combobox("getValue") == "") {
        layer.alert("部门没有选择！", { icon: 0 });
        return;
    }
    $.ajax(
        {
            url: postUrl,
            type: 'post',
            data: {
                userCode: userCode
            },
            async: false,
            cache: false,
            dataType: 'text',
            success: function (data) {
                checkUser = data;
            },
            error: function () {
                layer.alert("通讯失败，请重试！", { icon: 2 });
            }
        });
};
function clearForm() {
    $('#ff').form('clear');
};
$(function () {
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.iframeAuto(index);
});