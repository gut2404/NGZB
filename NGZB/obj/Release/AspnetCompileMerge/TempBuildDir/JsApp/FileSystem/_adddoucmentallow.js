function formattype(val, row, index) {
    if (val == "0") {
        return "全部";
    } else if (val == "1") {
        return "上传";
    }
    else {
        return "删除";
    }
};
function selectDpid(dpid) {
    var ducid = $('#allowducid').val();
    var url = url_getnoallowuser + '&ducid=' + ducid + '&dpid=' + dpid + '&allowtype=' + $("#allowtype").combobox("getValue");
    $('#getuser').combobox('clear');
    $('#getuser').combobox('reload', url);
};
function delallow() {
    var ducallowid = $('#allowuser').combogrid('getValue');
    var url = url_deldocumentallow;
    $.ajax({
        type: "POST",
        async: true,
        cache: false,
        url: url,
        data: { ducallowid: ducallowid },
        dataType: "text",
        success: function (response) {
            if (response == "1") {
                layer.alert("权限删除成功！", { icon: 1, title: '成功提示', closeBtn: 0 });
                upCombobox();
            } else if (response == "-1") {
                layer.alert("数据不合法！", { icon: 2, title: '错误提示' });
            } else {
                layer.alert("没有用户别删除！", { icon: 0, title: '操作提示' });
            }
        }
    });
};
function addallow() {
    var ducid = $('#allowducid').val();
    var usercode = $('#getuser').combobox('getValues');
    if (ducid == "") {
        layer.alert("没有选择要授权的目录！", { icon: 0 });
        return;
    }
    if (usercode != "") {
        $.ajax(
            {
                url: url_documentaddallow,
                type: 'post',
                data: {
                    ducid: ducid,
                    allowtype: $('#allowtype').combobox('getValue'),
                    usercode: usercode.join('|')
                },
                cache: false,
                dataType: 'text',
                success: function (data) {
                    if (data > 0) {
                        var str = data + "个用户被分配给<<" + $('#allowducname').val() + ">>";
                        layer.alert(str, { icon: 1, title: '成功提示!', closeBtn: 0 });
                        upCombobox();
                    }
                    else if (data == -1) {
                        layer.alert("数据不合法！", { icon: 2, title: '错误提示' });
                    }
                    else {
                        layer.alert("没有用户被分配！", { icon: 0, title: '操作提示' });
                    }
                },
                error: function () {
                    layer.alert('通讯错误，请检查网络!', { icon: 2, title: '错误提示' });
                }
            });
    }
    else {
        layer.alert("没有选择要授权的用户！", { icon: 0 });
    }
};
function upCombobox() {
    var ducid = $('#allowducid').val();
    var dpid = $('#groupID').combobox('getValue');
    var url = url_getnoallowuser + '&ducid=' + ducid + '&dpid=' + dpid + '&allowtype=' + $("#allowtype").combobox("getValue");
    $('#getuser').combobox('clear');
    $('#getuser').combobox('reload', url);
    var urlallow = url_getdocumentall + '&ducid=' + ducid;
    $('#allowuser').combogrid('clear');
    $('#allowuser').combogrid('grid').datagrid('reload', urlallow);
};
$(function ()
{
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.iframeAuto(index);
});