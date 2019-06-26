function ansyUser()
{
    $.ajax(
        {
            url: url_ansyuser,
            type: 'GET',
            cache: false,
            dataType: 'text',
            success: function (data)
            {
                if (data == '1')
                {
                    $('#dg').datagrid('reload');
                    layer.alert("同步成功！", { icon: 1 });
                }
                else
                {
                    layer.alert("同步失败，联系程序员！", { icon: 2 });
                }
            }
        });
}