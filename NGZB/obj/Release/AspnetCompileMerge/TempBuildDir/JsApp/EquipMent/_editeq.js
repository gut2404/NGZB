$(function ()
{
    var v = $('#nodeselect').val();
    if (v == "open" || v == "closed")
    {
        $('#nodeopen').combobox('setValue', v);
    }
    var installdatevalue = $('#installdatevalue').val();
    $("#installdate").datebox("setValue", installdatevalue);
    var eqclassIdvalue = $('#eqclassIdvalue').val();
    if (eqclassIdvalue != "-1")
    {
        $('#eqclass').combobox('setValue', eqclassIdvalue);
    }
    var nodetype = $('#nodetype').val();
    $('#nodetypeselect').combobox('setValue', nodetype);
    var isparts = $('#isparts').val();
    $('#spareparts').combobox('setValue', isparts);
    var ismajor = $('#ismajor').val();
    if (ismajor == "1")
    {
        $('#major').switchbutton({
            checked: true
        });
    }
    else
    {
        $('#major').switchbutton({
            checked: false
        });
    }
    if ($('#eqid').val() == "")
    {
        layer.alert('非正常请求！', { icon: 2 }, function ()
        {
            parent.layer.closeAll();
        });
    }
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.iframeAuto(index);
});
function subEdit()
{
    var eqId = $('#eqid').val();
    var treeName = $('#treename').val();
    var codeName = $('#codename').val();
    var orderBy = $('#orderby').val();
    var installDate = $('#installdate').val();
    var getIcon = $('#geticon').combobox("getText");
    var editInfo = $('#editinfo').val();
    var nodeOpen = $('#nodeopen').combobox('getValue');
    var opearteType = parent.$('#editBtn').attr("opearteType");
    var opearterType = parent.$('#addBtn').attr("opearteType");
    var eqClassid = $('#eqclass').combobox('getValue');
    var eqTypeSpecification = $('#eqtype').val();
    var eqSupplier = $('#eqsupplier').val();
    var eqManufacturer = $('#eqmanufacturer').val();
    var eqIsVirtual = $('#nodetypeselect').combobox('getValue');
    var eqSpareparts = $('#spareparts').combobox('getValue');
    var eqMajor = 0;
    if ($("#major").switchbutton("options").checked)
    {
        eqMajor = 1;
    }
    if (treeName == "")
    {
        layer.alert("设备名称不能为空！", { icon: 0 });
        return;
    }
    if (eqIsVirtual == '')
    {
        eqIsVirtual = '1';
    }
    if (eqClassid == "")
    {
        eqClassid = "-1";
    }
    if (getIcon == '')
    {
        getIcon = $('#iconold').linkbutton('options')['iconCls'].replace('icon-', '');
    }
    if (orderBy == '')
    {
        orderBy = '0';
    }
    $.ajax({
        type: "POST",
        url: url_add,
        data: {
            eqid: eqId,
            treename: treeName,
            codename: codeName,
            px: orderBy,
            geticon: getIcon,
            treeinfo: editInfo,
            installdate: installDate,
            nodeopen: nodeOpen,
            opeartetype: opearteType,
            eqclassid: eqClassid,
            eqtypetpecification: eqTypeSpecification,
            eqsupplier: eqSupplier,
            eqmanufacturer: eqManufacturer,
            eqisvirtual: eqIsVirtual,
            eqspareparts: eqSpareparts,
            eqmajor: eqMajor
        },
        dataType: "text",
        success: function (response)
        {
            if (response == "1")
            {
                layer.alert("修改成功！", { icon: 1 }, function ()
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
                layer.alert("修改失败！", { icon: 2 });
            }
        }
    });
};