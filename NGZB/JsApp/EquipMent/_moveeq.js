$(function ()
{
    var eqid = $('#eqid').val();
    if (eqid== '')
    {
        layer.alert("非正确路径请求!", { icon: 0 }, function (index)
        {
            parent.layer.closeAll();
        });
        return;
    }
    var selecteq = "当前选择设备:" + $('#selecteqname').val();
    $('#selecteq').html(selecteq);
    $('#moveBtn').linkbutton('disable');
    $('#eqtree').tree({
        onClick: function (node)
        {
            if (node.id == eqid)
            {
                $('#moveBtn').linkbutton('disable');
                layer.alert("目标位置不能与原位置相同!", { icon: 0 });
                return;
            }
            else
            {
                $('#moveBtn').linkbutton('enable');
                $('#parentid').val(node.id);

            }
        }
    });
});
function moveEqTree()
{
    var targetID = $('#parentid').val();
    if (targetID == "")
    {
        layer.alert("未选择目标位置!", { icon: 0 });
        return;
    }
    var moveInfo = $('#moveinfo').val();
    if (moveInfo == ''){
        layer.alert("未填写原因!", { icon: 0 });
        return;
    };
    var eqID = $('#eqid').val();
    if (eqID == targetID)
    {
        layer.alert("目标位置不能与原位置相同!", { icon: 0 });
        return;
    }
    var opearterType = $('#moveBtn').attr("opearteType");
    $.ajax({
        type: "POST",
        url: url_move,
        data: {
            eqid: eqID,
            targetId: targetID,
            moveinfo: moveInfo,
            op: opearterType
        },
        dataType: "text",
        success: function (response)
        {
            if (response == "1")
            {
                layer.alert("移动成功！", { icon: 1 }, function ()
                {
                    try
                    {
                        parent.searchKey();
                        parent.treeReload();
                    }
                    catch (e) { };
                    parent.layer.closeAll();
                });
            } else if (response == "-1")
            {
                layer.alert("数据不完整，请检查！", { icon: 0 });
            }
            else
            {
                layer.alert("移动失败！", { icon: 2 });
            }
        }
    });
};
function selectRoot()
{
    $('#parentid').val('0');
    $('#eqtree').find('.tree-node-selected').removeClass('tree-node-selected');
    $('#moveBtn').linkbutton('enable');
    layer.alert("已选择根节点！", { icon: 1 });
} window.onload = function ()
{
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.iframeAuto(index);
}