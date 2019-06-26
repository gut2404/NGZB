function formatOper(val, row, index)
{
    var iconstr = "icon-" + row.iconClass
    return "<a class='imgbtn " + iconstr + "'></a>";
};
function formatDel(val, row, index)
{
    if (val == "0")
    {
        return "<a class=\"delbtn\" data-options=\"iconCls: 'icon-no'\" onclick=\"del(" + row.iconId + "); \">删除</a>";
    }
    else
    {
        return "<div style=\"color: blue\">内置图标</div>";
    }
};
function del(iconId)
{
    $.ajax({
        type: "POST",
        url: url_del,
        data: "iconid=" + iconId,
        dataType: "text",
        success: function (response)
        {
            if (response == "1")
            {
                if (response == "1")
                {
                    layer.alert("CSS删除成功！", { icon: 1 }, function (index)
                    {
                        reload();
                        writecss();
                        layer.close(index);
                    });
                }
                else
                {
                    layer.alert("删除失败，联系程序员！", { icon: 2 });
                }
            }
        }
    });
};
function formatlinbutton(data)
{
    $(".imgbtn").linkbutton({ text: '', plain: false });
    $(".delbtn").linkbutton({ text: '删除', plain: false });
};
function reload()
{
    $('#dg').datagrid('reload');
};
function writecss()
{
    var url = url_writecss;
    $.ajax({
        type: "post",
        async: false,
        cache: false,
        url: url,
        dataType: "text",
        success: function (response)
        {
            if (response == "1")
            {
                reload();
            }
        }
    });
    return 1;
}
function createcss()
{
    $('#createcss').linkbutton('disable');
    var url = url_writecss;
    var t = writecss();
    if (t == "1")
    {
        layer.alert("CSS文件保存成功！", { icon: 1 }, function (index)
        {
            layer.close(index);
        });
    }
    else
    {
        layer.alert(t, { icon: 2 });
    }
    $('#createcss').linkbutton('enable');
};
function submitForm()
{
    var url = url_addico;
    layer.open({
        type: 2,
        area: ['550px', '200px'],
        shift: 4,
        fix: true,
        maxmin: false,
        content: url,
        scrollbar: false,
        closeBtn: 2,
        title: '上传图标'
    });
};
function newcreate()
{
    $('#newcreatecss').linkbutton('disable');
    var t = writecss();
    if (t == "1")
    {
        layer.alert("CSS重新生成成功！", { icon: 1 }, function (index)
        {
            layer.close(index);
        });
    }
    else
    {
        layer.alert("生成失败，联系程序员2222！", { icon: 2 });
    }
    $('#newcreatecss').linkbutton('enable');
};
function doSearch(value, name)
{
    $('#dg').datagrid('load', {
        icontype: name,
        keys: value
    });
};
