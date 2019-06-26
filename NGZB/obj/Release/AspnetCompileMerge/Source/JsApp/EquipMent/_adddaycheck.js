function saveDayCheck()
{
    var eqid = $('#eqid').val();
    var checktitle = $('#checktitle').textbox('getValue');
    var daycheckpart = $('#daycheckpart').textbox('getValue');
    var daycheckstand = $('#daycheckstand').textbox('getValue');
    var daychecktime = $('#daychecktime').textbox('getValue');
    var unit = $('#unitselect').combobox('getValue');
    var daycheckmethod = $('#daycheckmethod').combobox('getText');
    var daycheckinfo = $('#daycheckinfo').textbox('getValue');
    var daycheckstate = $('#daycheckstate').combobox('getText');
    var checktypeid = $('#checktypeid').combobox('getValue');
    var daycheckid = $('#daycheckid').val();
    if ($("#switchbtn").switchbutton("options").checked)
    {
        daycheckid = 0;
    }
    else
    {
        if (daycheckid == "0")
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
    if (checktitle == "")
    {
        layer.alert("章节标题不能为空！", { icon: 2 });
        return;
    }
    if (daycheckpart == "")
    {
        layer.alert("点检部位不能为空！", { icon: 2 });
        return;
    }
    if (daycheckstand == "")
    {
        layer.alert("点检标准不能为空！", { icon: 2 });
        return;
    }
    var reg = new RegExp("^[0-9]*$");
    if (!reg.test(daychecktime))
    {
        layer.alert("点检周期必须为数字！", { icon: 2 });
        return;
    }
    if (unit == "")
    {
        layer.alert("没有选择周期单位！", { icon: 2 });
        return;
    }
    if (daycheckmethod == "")
    {
        layer.alert("点检方法不能为空！", { icon: 2 });
        return;
    }
    if (daycheckinfo == "")
    {
        layer.alert("点检内容不能为空！", { icon: 2 });
        return;
    }
    if (daycheckstate == "")
    {
        layer.alert("点检状态不能为空！", { icon: 2 });
        return;
    }
    if (checktypeid == "")
    {
        layer.alert("没有选择点检类型！", { icon: 2 });
        return;
    }
    $.ajax({
        type: "POST",
        url: url_save,
        data: {
            eqid: eqid,
            checktitle: checktitle,
            daycheckpart: daycheckpart,
            daycheckstand: daycheckstand,
            daychecktime: daychecktime,
            unit: unit,
            daycheckmethod: daycheckmethod,
            daycheckinfo: daycheckinfo,
            daycheckstate: daycheckstate,
            checktypeid: checktypeid,
            daycheckid: daycheckid
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
            $('#daycheckid').val('0');
        }
    });
};
function timeformat(value, row, index)
{
    return row.dayCheckTimeNum + '' + row.unitName;
};
function opformat(value, row, index)
{
    if (row.isView == "1")
    {
        return "<a class='del c1' onclick=\"delOp(" + row.dayCheckID + ")\">可作废</a>";
    }
    else
    {
        return "<a class='redo c5' onclick=\"redoOp(" + row.dayCheckID + ")\">可启用</a>";
    }
}
function formatlinbutton()
{
    $(".del").linkbutton({ text: '可作废', plain: true, iconCls: 'icon-redo' });
    $(".redo").linkbutton({ text: '可恢复', plain: true, iconCls: 'icon-undo' });
};
function delOp(dayCheckID)
{
    opFn(dayCheckID, 0);
};
function redoOp(dayCheckID)
{
    opFn(dayCheckID, 1);
};
function opFn(dayCheckID, doType)
{
    $.ajax({
        type: "POST",
        url: url_redo,
        data: "daycheckid=" + dayCheckID + "&dotype=" + doType,
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
$(function ()
{
    $("#dg").datagrid({
        onDblClickRow: function (index, row)
        {
            $('#daycheckpart').textbox('setValue', row.checkParts);
            $('#daycheckstand').textbox('setValue', row.dayCheckStand);
            $('#daycheckmethod').combobox('setText', row.dayCheckMothed);
            $('#daycheckinfo').textbox('setValue', row.dayCheckInfo);
            $('#daycheckstate').combobox('setText', row.dayCheckState);
            $('#daychecktime').textbox('setValue', row.dayCheckTimeNum);
            $('#checktypeid').combobox('setValue', row.checkTypeId);
            $('#unitselect').combobox('setValue', row.timeNumUnitID);
            $('#switchbtn').switchbutton({
                checked: false
            });
            $('#daycheckid').val(row.dayCheckID);
        }
    });
});