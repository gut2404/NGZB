function subcchange() {
    var groupid = $("#txtGroupId").val();
    if (groupid == "") {
        layer.alert("没有选择要修改的节点！", { icon: 0 });
        return;
    }
    var groupname = $("#txtGroupName").val();
    if (groupname.length < 2) {
        layer.alert("组织名称必须不少于2个字符！", { icon: 0 });
        return;
    }
    var icon = $("#iconselect").combobox('getText');
    var orderby = $("#txtOrderby").val();
    $.ajax({
        type: "POST",
        url: url_edit,
        data: {
            groupid: groupid,
            groupname: groupname,
            icon: icon,
            orderby: orderby
        },
        dataType: "text",
        success: function (response) {
            if (response == "1") {
                layer.alert("编辑成功！", { icon: 1 }, function () {
                    try {
                        parent.reload();
                        parent.layer.closeAll();
                    }
                    catch (e) { };
                })
            }
            else if (response == "0") {
                layer.alert("编辑数据不合法，请检查！", { icon: 2 });
            }
            else {
                layer.alert("编辑失败，联系程序员！", { icon: 2 });
            }
        }
    });
};
$(function ()
{
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.iframeAuto(index);
});