function add()
{
    var url = url_add;
    layer.open({
        type: 2,
        area: ['403px', '400px'],
        shift: 4,
        fix: true,
        maxmin: false,
        content: url,
        closeBtn: 2,
        title: ''
    });
};
function edit()
{
    var row = $('#dg').datagrid('getSelected');
    if (row == null)
    {
        layer.alert('先选择要修改的用户！', { icon: 0 });
        return;
    }
    var url = url_edit + '&userCode=' + row.userCode;
    layer.open({
        type: 2,
        area: ['405px', '400px'],
        shift: 4,
        fix: true,
        maxmin: false,
        content: url,
        closeBtn: 2,
        title: ''
    });
};
function delconfirm(index)
{
    layer.confirm('删除用户后该用户的所有权限将被取消，确认吗？', {
        btn: ['确定', '取消'],
        icon: 3,
        title: '删除用户确认'
    },
        function ()
        {
            layer.closeAll('dialog');
            del();
        },
        function ()
        {
            layer.closeAll('dialog');
        });
};
function del()
{
    var postUrl = url_del;
    var checkedItems = $('#dg').datagrid('getChecked');
    var names = [];
    $.each(checkedItems, function (index, item)
    {
        names.push(item.userCode);
    });
    var userCode = names.join("|");
    if (userCode != '')
    {
        $.ajax(
            {
                url: postUrl,
                type: 'post',
                data: {
                    userCode: userCode
                },
                cache: false,
                dataType: 'text',
                success: function (data)
                {
                    if (data > 0)
                    {
                        var str = data + "个用户被删除！"
                        layer.alert(str, { icon: 1 });
                        search();
                    }
                    else
                    {
                        layer.alert("没有用户被删除！", { icon: 0 });
                    }
                },
                error: function ()
                {
                    layer.alert('通讯错误，请检查网络!');
                }
            });
    }
    else
    {
        layer.alert('没有选择要删除的用户!', { icon: 2 });
    }
};
function redo()
{
    var postUrl = url_redo;
    var checkedItems = $('#dg').datagrid('getChecked');
    var names = [];
    $.each(checkedItems, function (index, item)
    {
        names.push(item.userCode);
    });
    var userCode = names.join("|");
    if (userCode != '')
    {
        $.ajax(
            {
                url: postUrl,
                type: 'post',
                data: {
                    userCode: userCode
                },
                cache: false,
                dataType: 'text',
                success: function (data)
                {
                    if (data > 0)
                    {
                        var str = data + "个用户被恢复！"
                        layer.alert(str, { icon: 1 });
                        search();
                    }
                    else
                    {
                        layer.alert("没有用户被恢复！", { icon: 0 });
                    }
                },
                error: function ()
                {
                    layer.alert('通讯错误，请检查网络!');
                }
            });
    }
    else
    {
        layer.alert('没有选择要恢复的用户!', { icon: 2 });
    }
};
function search(keys)
{
    if (keys == null)
    {
        keys = $('#searchkey').val();
    }
    $('#dg').datagrid('load', {
        key: keys,
        f: $('#f').combobox('getValue')
    });
};
$(function ()
{
    $('#dg').datagrid({ selectOnCheck: $(this).is(':checked') });
});
function delformat(value, row, index)
{
    if (value == "1")
    {
        return '<font color="red">已删除</font>';
    }
    else
    {
        return "正常";
    }
};