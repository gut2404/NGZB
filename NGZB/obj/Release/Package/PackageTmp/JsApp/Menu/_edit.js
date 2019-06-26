$(function ()
{
    var menuid = $('#menuid').val();
    if ($('#menuid').val() == '')
    {
        layer.alert('非正常请求！', { icon: 2 }, function ()
        {
            parent.layer.closeAll();
            return;
        });
    }
    if (menuid == "0")
    {
        $('#iconthemsbtn').linkbutton('disable');
    }
    $('#menutype').combobox("setValue", $('#menutypevalue').val());
});
function search(newValue)
{
    var posturl = "";
    var controller = $('#controller').combobox('getValue');
    var menugroup = $('#menugroup').combobox('getValue');
    if (controller != "" && menugroup != "")
    {
        posturl = url_getmenumodel + '&controller=' + controller + "&menugroup=" + menugroup;
        $('#action').combobox('clear');
        $('#action').combobox('reload', posturl);
    }
};
function editMenu()
{
    var menuid = $('#menuid').val();
    var menutype = $('#menutype').combobox("getValue");
    var modelid = $('#action').combobox("getValue");
    var menucolor = $("#getcolor").val();
    var menuimg = $("#iconselect").combobox('getText');
    var parentid = $('#menugroup').combobox("getValue");
    var menutypeold = $('#menutypevalue').val();
    var menuorderby = $('#menuorderby').val();
    if (menuid == '' || menuid == '0')
    {
        layer.alert('非正常路径请求，无法修改！', { icon: 2 });
        return;
    }
    if (menutype == menutypeold)
    {
        if (modelid == '' && menucolor == '' && menuimg == '' && parentid == '' && menuorderby == '')
        {
            layer.alert('数据没有变化，无需修改！', { icon: 0 });
            return;
        }
    }
    $.ajax({
        type: "POST",
        async: true,
        cache: false,
        url: url_edit,
        data: {
            menuid: menuid,
            menutype: menutype,
            modelid: modelid,
            menuimg: menuimg,
            menuorderby: menuorderby,
            menucolor: menucolor,
            parentid: parentid
        },
        dataType: "text",
        success: function (response)
        {
            if (response == "1")
            {
                parent.search();
                layer.alert("菜单修改成功!", { icon: 1 }, function ()
                {
                    parent.layer.closeAll();
                });
            }
            else if (response == '-1')
            {
                layer.alert("数据不合法，无法修改!", { icon: 0 });
            }
            else
            {
                layer.alert("修改失败，联系程序员!", { icon: 2 });
            }
        }
    });
};