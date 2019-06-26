function testConn()
{
    active(url_test, '测试', '0');
};
function saveConn()
{
    active(url_save, '保存', '1');
};
function active(url, msg,t)
{
    var server = $("#server").val();
    var uid = $("#uid").val();
    var pwd = $("#pwd").val();
    if (server == '' || uid == '' || pwd == '')
    {
        layer.alert('填写的信息不完整！', { icon: 0 });
        return;
    }
    $.ajax({
        type: "POST",
        url: url,
        data: {
            server: server,
            uid: uid,
            pwd: pwd
        },
        dataType: "text",
        success: function (response)
        {
            if (response == '1')
            {
                layer.alert(msg + "成功！", { icon: 1 }, function (index)
                {
                    if (t == '1')
                    {
                        parent.reload();
                        parent.layer.closeAll();
                    }
                    layer.close(index);
                });
            }
            else if (response == '0')
            {
                layer.alert(msg + "失败，检查服务器、用户名和密码！", { icon: 2 });
            }
            else
            {
                layer.alert('填写的信息不完整！', { icon: 0 });
            }
        }
    });
};