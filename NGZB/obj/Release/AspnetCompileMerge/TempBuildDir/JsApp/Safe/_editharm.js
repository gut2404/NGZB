function editHarm()
{
    var workTypeID = $('#worktypeid').combobox("getValue");
    var securityInfo = KE[0].html();
    var defendAgainst = KE[1].html();
    var safeHarmID = $('#safeharmid').val();
    if (safeHarmID == '')
    {
        layer.alert('非正常路径请求！', { icon: 0 });
        return;
    }
    if (workTypeID == '')
    {
        layer.alert('请选择作业类型！', { icon: 0 });
        return;
    } else if (KE[0].text() == '')
    {
        layer.alert('请填写风险描述！', { icon: 0 });
        return;
    }
    else if (KE[0].text() == '')
    {
        layer.alert('请填写预防措施！', { icon: 0 });
        return;
    }
    $.ajax({
        type: "POST",
        url: url_edit,
        data: { safeHarmID: safeHarmID, workTypeID: workTypeID, securityInfo: securityInfo, defendAgainst: defendAgainst },
        dataType: "text",
        success: function (response)
        {
            if (response == '1')
            {
                try
                {
                    parent.dgReload();
                } catch (e) { }
                layer.alert('添加成功！', { icon: 1 }, function ()
                {
                    parent.layer.closeAll();
                });
            } else
            {
                layer.alert('添加失败，联系管理员！', { icon: 2 });
            }
        }
    });
}
window.onload = function ()
{
    KE[0].html($('#oldsecurityinfo').val());
    KE[1].html($('#olddefendagainst').val());
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.iframeAuto(index);
    $('#worktypeid').combobox('setValue', $('#oldharmtypeid').val());
};