function searchmenu()
{
    var posturl = url_rolemenu + '&roleID=' + $('#roleID').combobox('getValue') + '&pid=' + $('#menuGrop').combobox('getValue');
    $('#menuID').combobox('reload', posturl);
};
function serchrolemenu()
{
    if ($('#roleID').combobox('getValue') != null)
    {
        $('#dg').datagrid('load', {
            roleid: $('#roleID').combobox('getValue')
        });
    }
};
function selectroleID()
{
    var data = $('#roleID').combobox('getData');
    if (data.length > 0)
    {
        $('#roleID').combobox('setValue', data[0].roleID);
        isnull = 1;
    }
    else
    {
        $('#roleID').combobox('setValue', "");
        isnull = 0;
    }
};
function selectmenugroup()
{
    var data = $('#menuGrop').combobox('getData');
    if (data.length > 0)
    {
        $('#menuGrop').combobox('setValue', data[0].menuID);
    }
    else
    {
        $('#menuGrop').combobox('setValue', "");
    }
};
function selectmenu()
{
    var data = $('#menuID').combobox('getData');
    if (data.length > 0)
    {
        $('#menuID').combobox('setValue', data[0].menuID);
        isnull = 1;
    }
    else
    {
        $('#menuID').combobox('setValue', "");
        isnull = 0;
    }
};
function add()
{
    $('#addallow').linkbutton('disable');
    var roleid = $('#roleID').combobox('getValue');
    var menuID = $('#menuID').combobox('getValues');
    if (isnull == 0)
    {
        layer.alert("数据选择不全，无法添加！", { icon: 0, title: '错误提示' });
        return;
    }
    $.ajax(
        {
            url: url_addroleitem,
            type: 'post',
            data: {
                roleid: roleid,
                menuid: menuID.join('|')
            },
            cache: false,
            dataType: 'text',
            success: function (data)
            {
                if (data > 0)
                {
                    var str = data + "个权限被分配给" + $('#roleID').combobox('getText');
                    try
                    {
                        parent.search();
                    } catch (e) { }
                    serchrolemenu();
                    searchmenu();
                    layer.alert(str, { icon: 1, title: '成功提示' });
                }
                else if (data = -1)
                {
                    layer.alert("数据不合法！", { icon: 2, title: '错误提示' });
                }
                else
                {
                    layer.alert("没有角色被删除！", { icon: 0, title: '操作提示' });
                }
            },
            error: function ()
            {
                layer.alert('通讯错误，请检查网络!', { icon: 2, title: '错误提示' });
            }
        });
    $('#addallow').linkbutton('enable');
};
function delconfirm(index)
{
    layer.confirm('删除该角色权限后该权限对应的用户都将被删除，确认吗？', {
        btn: ['确定', '取消'],
        icon: 3,
        title: '删除角色权限确认？'
    },
        function ()
        {
            del();
        },
        function ()
        {
            layer.close(index);
        });
};
function del()
{
    $('#delallow').linkbutton('disable');
    var postUrl = url_delroleitem;
    var checkedItems = $('#dg').datagrid('getChecked');
    var names = [];
    $.each(checkedItems, function (index, item)
    {
        names.push(item.roleItemID);
    });
    var getValeu = names.join("|");
    if (getValeu != '')
    {
        $.ajax(
            {
                url: postUrl,
                type: 'post',
                data: {
                    roleItemID: getValeu
                },
                cache: false,
                dataType: 'text',
                success: function (data)
                {
                    if (data > 0)
                    {
                        var str = data + "个权限被删除！"
                        layer.alert(str, { icon: 1 });
                        serchrolemenu();
                        searchmenu();
                        try
                        {
                            parent.search();
                        } catch (e) { }
                    }
                    else
                    {
                        layer.alert("没有角色权限被删除！", { icon: 0, title: '错误提示' });
                    }
                },
                error: function ()
                {
                    layer.alert('通讯错误，请检查网络!', { icon: 2, title: '错误提示' });
                }
            });
    }
    else
    {
        layer.alert('没有选择要删除的权限!', { icon: 2, title: '错误提示' });
    }
    $('#delallow').linkbutton('enable');
};
$(function ()
{
    $('#dg').datagrid({ selectOnCheck: $(this).is(':checked') });
    $('#menuGrop').combobox({
        onChange: function (n, o)
        {
            searchmenu();
        }
    });
    $('#roleID').combobox({
        onChange: function (n, o)
        {
            searchmenu();
            serchrolemenu();
        }
    });
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.iframeAuto(index);
});