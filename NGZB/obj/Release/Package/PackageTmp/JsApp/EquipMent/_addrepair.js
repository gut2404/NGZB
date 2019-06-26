function repartimeformat(value, row, index)
{
    if (row.repairNum > 0)
    {
        return row.repairNum + '' + row.unitName;
    }
    return "不定期";
};
function timeformat(value, row, index)
{
    if (row.repairUnitName && row.repairTime > 0)
    {
        return row.repairTime + '' + row.repairUnitName;
    }
    return '未设置';
};
function formatlinbutton()
{
    $(".del").linkbutton({ text: '可作废', plain: true, iconCls: 'icon-redo' });
    $(".redo").linkbutton({ text: '可恢复', plain: true, iconCls: 'icon-undo' });
};
function opformat(value, row, index)
{
    if (row.isView == "1")
    {
        return "<a class='del c1' onclick=\"delOp(" + row.repairID + ")\">可作废</a>";
    }
    else
    {
        return "<a class='redo c5' onclick=\"redoOp(" + row.repairID + ")\">可启用</a>";
    }
}
function delOp(repairID)
{
    opFn(repairID, 0);
};
function redoOp(repairID)
{
    opFn(repairID, 1);
};
function opFn(repairID, doType)
{
    $.ajax({
        type: "POST",
        url: url_redo,
        data: "repairid=" + repairID + "&dotype=" + doType,
        dataType: "text",
        success: function (response)
        {
            if (response == "1")
            {
                layer.alert(__okMsg, { icon: 1 });
                $('#dg').datagrid('reload');
            }
            else
            {
                layer.alert(__errMsg, { icon: 2 });
                $('#dg').datagrid('reload');
            }
        }
    });
}
function saveRepair()
{
    var eqid = $('#eqid').val();
    var repairtitle = $('#repairtitle').textbox('getValue');

    var repairtypeid = $('#repairtypeid').combobox('getValue');

    var repairtime = $('#repairtime').textbox('getValue');
    var unitid = $('#unitselect1').combobox('getValue');

    var repaitinfo = $('#repaitinfo').textbox('getValue');

    var worknum = $('#worknum').textbox('getValue');
    var workUnitid = $('#unitselect2').combobox('getValue');

    var acceptstand = $('#acceptstand').textbox('getValue');
    var repairid = $('#repairid').val();

    if ($("#switchbtn").switchbutton("options").checked)
    {
        repairid = 0;
    }
    else
    {
        if (repairid == "0")
        {
            layer.alert("没有选择要保存的标准！", { icon: 2 });
            return;
        }
    }
    if (eqid == "")
    {
        layer.alert("没有选择设备！", { icon: 2 });
        return;
    }
    if (repairtitle == "")
    {
        layer.alert("章节标题不能为空！", { icon: 2 });
        return;
    }
    if (repairtypeid == "")
    {
        layer.alert("检修类型没有选择！", { icon: 2 });
        return;
    }
    var reg = new RegExp("^[0-9]*$");
    if (!reg.test(repairtime))
    {
        layer.alert("检修周期必须为数字！", { icon: 2 });
        return;
    }
    if (repairtime < 0)
    {
        layer.alert("检修周期不能小于0！", { icon: 2 });
        return;
    }
    if (unitid == "" && repairtime > 0)
    {
        layer.alert("非不定期检修必须选择检修周期单位！", { icon: 2 });
        return;
    }
    if (repaitinfo == "")
    {
        layer.alert("检修内容不能为空！", { icon: 2 });
        return;
    }
    if (acceptstand == "")
    {
        layer.alert("验收标准不能为空！", { icon: 2 });
        return;
    }
    if (worknum == "")
    {
        layer.alert("工作耗时如果忽略，请保持为0！", { icon: 2 });
        return;
    }
    else
    {
        if (!reg.test(worknum))
        {
            layer.alert("工作耗时必须为数字或者0！", { icon: 2 });
            return;
        }
        if (workUnitid == "" && worknum > 0)
        {
            layer.alert("设置了工作耗时就必须选择单位！", { icon: 2 });
            return;
        }
    }
    if (workUnitid == "")
    {
        workUnitid = 0;
    }
    if (unitid == "")
    {
        unitid = 0;
    }
    var repairtitle = $('#repairtitle').val();
    if (repairtitle == "")
    {
        layer.alert("章节标题不能为空！", { icon: 2 });
        return;
    }
    $.ajax({
        type: "POST",
        url: url_save,
        data: {
            repairid: repairid,
            eqid: eqid,
            repairtypeid: repairtypeid,
            repairtime: repairtime,
            unitid: unitid,
            repairinfo: repaitinfo,
            worknum: worknum,
            workUnitid: workUnitid,
            acceptstand: acceptstand,
            repairtitle: repairtitle
        },
        dataType: "text",
        success: function (response)
        {
            if (response == "1")
            {
                layer.alert(__okMsg, { icon: 1 });
                $('#dg').datagrid('reload');
            }
            else
            {
                layer.alert(__errMsg, { icon: 2 });
                $('#dg').datagrid('reload');
            }
            $('#repairid').val('0');
        }
    });
};
$(function ()
{
    $("#dg").datagrid({
        onDblClickRow: function (index, row)
        {
            $('#switchbtn').switchbutton({
                checked: false
            });
            $('#repaitinfo').textbox('setValue', row.repairInfo);
            $('#acceptstand').textbox('setValue', row.acceptStand);
            $('#repairid').val(row.repairID);
            $('#repairtime').textbox('setValue', row.repairNum);
            $('#worknum').textbox('setValue', row.repairTime);
            $('#repairtime').textbox('setValue', row.repairNum);
            $('#unitselect1').combobox('setValue', row.unitID);
            $('#unitselect2').combobox('setValue', row.timeUnitID);
            $('#repairtypeid').combobox('setValue', row.repairTypeID);
        }
    });
});