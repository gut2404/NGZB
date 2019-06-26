$(function ()
{
    $('#tt').edatagrid({
        updateUrl: url_editmodelname,
        autoSave: true,
        onSuccess: function (index, row)
        {
            search();
            layer.alert("保存成功！", { icon: 1 });
        },
        onError: function (index, row)
        {
            layer.alert("通讯失败！", { icon: 2 });
        }
    });
    $(".datagrid-editable-input").tooltip({ position: 'right', content: '文本框之外右键取消编辑！' });
    $('.datagrid-view').bind('contextmenu', function (e)
    {
        e.preventDefault();
        $('#mm').menu('show', {
            left: e.pageX,
            top: e.pageY
        });
    });
});
function search()
{
    if ($('#f').combobox('getValue') == "")
    {
        $('#tt').datagrid('load', {
            key: $('#searchkey').val()
        });
    }
    else
    {
        $('#tt').datagrid('load', {
            key: $('#searchkey').val(),
            c: $('#f').combobox('getValue')
        });
    }
};
function cancle()
{
    $('#tt').edatagrid("cancelRow");
};
function delformat(value, row, index)
{
    if (value == "是")
    {
        return '<font color="red">是</font>';
    }
    else
    {
        return "否";
    }
};
function idformat(value, row, index)
{
    return '<a href="javascript:;" onclick="addHelp(' + value + '); ">' + value + '</a > ';
};
function addHelp(id)
{
    layer.open({
        type: 2,
        title: "添加、修改帮助",
        area: ['70%', '85%'],
        content: url_addedithelp + '&modelid=' + id
    });
}