function addHarm()
{
    layer.open({
        type: 2,
        title: "添加风险因素",
        area: ['90%', '90%'],
        content: url_addharm
    });
};
function dgReload()
{
    $('#tt').datagrid('reload');
}
function editHarm()
{
    var row = $('#tt').datagrid('getSelected');
    if (row == null)
    {
        layer.alert('先选择要修改的行!', { icon: 0 });
        return;
    }
    layer.open({
        type: 2,
        title: "修改风险因素",
        area: ['90%', '90%'],
        content: url_editharm + '&safeHarmID=' + row.safeHarmID
    });
};
function doSearch(value)
{
    var harmTypeID = $('#harmtypeid').combobox('getValue');
    if (harmTypeID == "")
    {
        harmTypeID = "0";
    }
    $('#tt').datagrid('load', {
        serchkey: value,
        harmtypeid: harmTypeID
    });
}