function okActive()
{
    $.ajax({
        type: "POST",
        url: url,
        dataType: "text",
        success: function (response)
        {
            if (response == '1')
            {
                layer.alert("配置完成，欢迎使用，祝你愉快！", { icon: 6 }, function ()
                {
                    location.replace(recUrl);
                });
            }
            else if (response == '0')
            {
                layer.alert("操作失败，联系程序员！", { icon: 2 });
            }
            else
            {
                layer.alert('操作失败，联系程序员！', { icon: 0 });
            }
        }
    })
};