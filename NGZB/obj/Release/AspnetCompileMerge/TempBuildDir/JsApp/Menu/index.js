function search()
{
    var posturl = "";
    var controller = "";
    var menugroup = "";
    if ($('#controller').combobox('getValue') != null)
    {
        controller = $('#controller').combobox('getValue');
    }
    if (controller != "")
    {
        controller = "controller=" + controller;
    }
    if ($('#menugroup').combobox('getValue') != null)
    {
        menugroup = $('#menugroup').combobox('getValue');
    }
    if (menugroup != "")
    {
        menugroup = "menugroup=" + menugroup;
    }
    //if (controller != "" && menugroup != "")
    //{
        posturl = url_getmenumodel + '&' + controller + "&" + menugroup;
        $('#action').combobox('clear');
        $('#action').combobox('reload', posturl);
    //}
    if ($('#menugroup').combobox('getValue') != null)
    {
        menugroup = $('#menugroup').combobox('getValue');
        $('#tt').datagrid('load', {
            parentID: menugroup
        });
    };
};
function addmenu()
{
    var menugroup = $("#menugroup").combobox("getValue");
    var modelid = $("#action").combobox("getValue");
    var color = $("#getcolor").val();
    var icon = $("#iconselect").combobox('getText');
    var menutype = $("#menutype").combobox("getValue");
    if (menugroup == "")
    {
        layer.alert("没有选择菜单分组！", { icon: 0 });
        return;
    }
    if (modelid == "")
    {
        layer.alert("没有选择菜功能！", { icon: 0 });
        return;
    }
    if (menutype == "")
    {
        menutype = "1";
    }
    var url = url_add;
    $.ajax({
        type: "POST",
        async: false,
        cache: false,
        url: url,
        data: {
            menuparentID: menugroup,
            menuImg: icon,
            modeid: modelid,
            colercssclass: color,
            menutype: menutype
        },
        dataType: "text",
        success: function (response)
        {
            if (response == "1")
            {
                menusearch(menugroup);
                search();
                layer.alert("添加成功！", { icon: 1 });
            } else
            {
                layer.alert("添加失败，请联系程序员！", { icon: 2 });
            }
        }
    });
};
function menusearch(parentID)
{
    $('#tt').datagrid('load', {
        parentID: parentID
    });
};
function themsformat(value, row, index)
{
    var color = row.colerCsscClass;
    var icon = 'icon-' + row.menuImg;
    return '<a href="#" class=\"themsbtn ' + color + '\" data-options=\"iconCls:\'' + icon + '\'\">' + row.menuName + '</a>';
};
function delformat(value, row, index)
{
    var color = row.colerCsscClass;
    var icon = 'icon-' + row.menuImg;
    return '<a href="#" class=\"delbtn\" data-options=\"iconCls:\'icon-no\'" onclick="delmenu(' + row.menuID + ');">删除</a>';
};
function formatlinbutton(data)
{
    $(".themsbtn").linkbutton();
    $(".delbtn").linkbutton();
};
function delmenu(menuid)
{
    var url = url_del;
    var menugroup = $("#menugroup").combobox("getValue");
    $.ajax({
        type: "POST",
        async: false,
        cache: false,
        url: url,
        data: { menuid: menuid },
        dataType: "text",
        success: function (response)
        {
            if (response == "1")
            {
                search();
                menusearch(menugroup);
                layer.alert("菜单删除成功!", { icon: 1 });
            }
            else
            {
                layer.alert("菜单删除失败!", { icon: 2 });
            }
        }
    });
};
function showAddMenuGroup()
{
    layer.open({
        type: 1,
        title: "添加分组",
        area: ['300px', '130px'],
        scrollbar: false,
        content: $('#addgroup') //这里content是一个DOM
    });
};
function addMenuGroup()
{
    var groupname = $("#groupname").val();
    if (groupname == "")
    {
        layer.alert("分组名称不能为空！", { icon: 2 });
        return;
    }
    var url = url_addmenugroup;
    var icon = $('#iconselect').combobox("getText");
    if (icon == "")
    {
        icon = 'my_any';
    }
    $.ajax({
        type: "POST",
        async: true,
        cache: false,
        url: url,
        data: { menugroupname: groupname, menuImg: icon },
        dataType: "text",
        success: function (response)
        {
            if (response == "1")
            {
                $("#menugroup").combobox("reload");
                search();
                menusearch();
                layer.alert("分组添加成功!", { icon: 1 }, function ()
                {
                    layer.closeAll();
                });
            }
            else
            {
                layer.alert("分组添加失败!", { icon: 2 });
            }
        }
    });
};
function showEdit(index, row)
{
    layer.open({
        type: 2,
        title: "修改菜单",
        area: ['450px', '350px'],
        scrollbar: false,
        content: url_edit + '&menuid=' + row.menuID + '&menutype=' + row.menuType
    });
}
function cellStyler(value, row, index)
{
    if (value=='1')
    {
        return '授权';
    }
    else
    {
        return '公共';
    }
}