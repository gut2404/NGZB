function okActive()
{
    var groupname = $("#groupname").val();
    if (groupname == '')
    {
        layer.alert('填写的信息不完整！', { icon: 0 });
        return;
    }
    $.ajax({
        type: "POST",
        url: url,
        data: {
            groupname: groupname
        },
        dataType: "text",
        success: function (response)
        {
            if (response == '1')
            {
                layer.alert("初始化成功！", { icon: 1 }, function (index)
                {
                    parent.reload();
                    parent.layer.closeAll();
                });
            }
            else if (response == '0')
            {
                layer.alert("初始化失败，联系程序员！", { icon: 2 });
            }
            else
            {
                layer.alert('填写的信息不完整！', { icon: 0 });
            }
        }
    })
};