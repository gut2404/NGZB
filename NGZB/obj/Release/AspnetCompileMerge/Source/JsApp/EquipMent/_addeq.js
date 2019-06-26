function subAdd()
{
    var parentId = $('#parentid').val();
    var treeName = $('#treename').val();
    var codeName = $('#codename').val();
    var orderBy = $('#orderby').val();
    var installDate = $('#installdate').val();
    var getIcon = $('#geticon').combobox("getText");
    var editInfo = $('#editinfo').val();
    var nodeOpen = $('#nodeopen').combobox('getValue');
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
    if (treeName == '')
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
    if (orderBy == '')
    {
        orderBy = '0';
    }
    $.ajax({
        type: "POST",
        url: url_add,
        data: {
            parentid: parentId,
            treename: treeName,
            codename: codeName,
            px: orderBy,
            geticon: getIcon,
            treeinfo: editInfo,
            installdate: installDate,
            nodeopen: nodeOpen,
            opeartetype: opearterType,
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
                layer.alert(__okMsg, { icon: 1 }, function ()
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
                layer.alert(__dataMsg, { icon: 0 });
            }
            else
            {
                layer.alert(__errMsg, { icon: 2 });
            }
        }
    });
};
$(function ()
{
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.iframeAuto(index);
    if ($('#parentid').val() == '')
    {
        layer.alert('非正常请求！', { icon: 2 }, function ()
        {
            parent.layer.closeAll();
        });
    }
});