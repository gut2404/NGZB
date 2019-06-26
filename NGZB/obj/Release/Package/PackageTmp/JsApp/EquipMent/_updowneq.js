function upDown()
{
    var op = $('#opstate').val();
    var opStr = "";
    var eqID = $('#eqid').val();
    var opwhy = $('#whyupdown').val();
    var opearterType;
    var delType = 0;
    if (op == "u")
    {
        opStr = "上线";
        opearterType = parent.$('#upBtn').attr("opearteType");
        delType = 0;
    }
    else if (op == "d")
    {
        opStr = "下线";
        opearterType = parent.$('#downBtn').attr("opearteType");
        delType = 1;
    }
    else if (op == "c")
    {
        opStr = "报废";
        opearterType = parent.$('#crappedBtn').attr("opearteType");
        delType = -1;
    }
    else if (op == "r")
    {
        opStr = "恢复";
        opearterType = parent.$('#recoverBtn').attr("opearteType");
        delType = 0;
    }
    else
    {
        layer.alert('没有对应的操作，无法完成', { icon: 0 });
        return;
    }
    if (opwhy == "")
    {
        layer.alert('请填写' + opStr + '原因', { icon: 0 });
        return;
    }
    $.ajax({
        type: "POST",
        url: url_updown,
        data:
        {
            eqid: eqID,
            deltype: delType,
            opwhy: opwhy,
            optype: opearterType
        },
        dataType: "text",
        success: function (response)
        {
            if (response == "1")
            {
                layer.alert(opStr + "操作成功！", { icon: 1 }, function (index)
                {
                    try
                    {
                        parent.searchKey();
                        parent.layer.closeAll();
                    } catch (e) { }
                });
            } else if (response == "-1")
            {
                layer.alert("数据不完整！", { icon: 0 });
            } else
            {
                layer.alert("操作失败！", { icon: 2 });
            }
        },
        onError: function (index, row)
        {
            layer.alert("通讯失败！", { icon: 2 });
        }
    });
};
$(function ()
{
    var op = $('#opstate').val();
    if (op == 'd')
    {
        $('#updowntitle').html('设备下线原因');
        $('#alertInfo').html('特别注意：设备下线后，附属的所有子设备同时全部下线!');
        $('#opbtn').linkbutton({ iconCls: 'icon-my_down', text: '下线' });
        $('#whyupdown').textbox('setValue', '');
    } else if (op == 'u')
    {
        $('#updowntitle').html('设备上线原因');
        $('#alertInfo').html('特别注意：设备上线后，所有父级设备全部上线!');
        $('#opbtn').linkbutton({ iconCls: 'icon-my_up', text: '上线' });
        $('#whyupdown').textbox('setValue', '');
    }
    else if (op == 'c')
    {
        $('#updowntitle').html('设备报废原因');
        $('#alertInfo').html('特别注意：设备报废后，附属的所有子设备同时全部报废!');
        $('#opbtn').linkbutton({ iconCls: 'icon-my_eqdel', text: '报废' });
        $('#whyupdown').textbox('setValue', '');
    }
    else if (op == 'r')
    {
        $('#updowntitle').html('设备恢复原因');
        $('#alertInfo').html('特别注意：设备恢复后，所有父级设备全部恢复!');
        $('#opbtn').linkbutton({ iconCls: 'icon-my_eqerr', text: '恢复' });
        $('#whyupdown').textbox('setValue', '');
    }
    else
    {
        layer.alert('非正常请求！', { icon: 2 }, function ()
        {
            parent.layer.closeAll();
        });
    }
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.iframeAuto(index);
});