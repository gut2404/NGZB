function formatlink(val, row, index)
{
    return "<a class='allowbtn c1' onclick=\"active('" + row.modelactive + "','" + row.modelname + "')\">操作</a>";
}
function active(modelactive, modelname)
{
    var url = url_active + '?active=' + modelactive;
    layer.open({
        type: 2,
        area: ['95%', '90%'],
        shift: 4,
        fix: true,
        maxmin: false,
        content: url,
        closeBtn: 2,
        title: modelname
    });
};
function formatlinbutton()
{
    $(".allowbtn").linkbutton({ text: '初始化', plain: true, iconCls: 'icon-redo' });
};
function reload()
{
    $('#dg').datagrid('reload');
};