$(function () {
    $('#tt').edatagrid({
        updateUrl: url_setsave,
        autoSave: true,
        onSuccess: function (index, row) {
            search();
            layer.alert("保存成功！", { icon: 1 });
        },
        onError: function (index, row) {
            layer.alert("保存失败！", { icon: 2 });
        }
    });
    $(".datagrid-editable-input").tooltip({ position: 'right', content: '文本框之外右键取消编辑！' });
    $('.datagrid-view').bind('contextmenu', function (e) {
        e.preventDefault();
        $('#mm').menu('show', {
            left: e.pageX,
            top: e.pageY
        });
    });
});
function search() {
    $('#tt').datagrid('load', {
        key: $('#searchkey').val()
    });
};
function cancle() {
    $('#tt').edatagrid("cancelRow");
};