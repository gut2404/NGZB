function add() {
    var usercode = $('#userCode').combobox('getValues');
    if (usercode != "") {
        $.ajax(
            {
                url: url_adduserrole,
                type: 'post',
                data: {
                    roleid: $('#roleID').combobox('getValue'),
                    usercode: usercode.join('|')
                },
                cache: false,
                dataType: 'text',
                success: function (data) {
                    if (data > 0) {
                        var str = data + "个用户被分配给" + $('#roleID').combobox('getText');
                        reloadtabe();
                        layer.alert(str, { icon: 1, title: '成功提示' });
                    }
                    else if (data = -1) {
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
        layer.alert("没有要添加的用户！", { icon: 0, title: '操作提示' });
    }
};
function del() {
    var checkedItems = $('#dg').datagrid('getChecked');
    var roleuserid = [];
    $.each(checkedItems, function (index, item) {
        roleuserid.push(item.roleUserID);
    });
    var postroleuserid = roleuserid.join("|");
    if (postroleuserid != "") {
        $.ajax(
            {
                url: url_deluserrole,
                type: 'post',
                data: {
                    roleUserID: postroleuserid
                },
                cache: false,
                dataType: 'text',
                success: function (data) {
                    if (data > 0) {
                        var str = data + "个用户被撤销角色：" + $('#roleID').combobox('getText');
                        search($("#groupID").combotree("getValue"));
                        reloadtabe();
                        layer.alert(str, { icon: 1, title: '成功提示' });
                    }
                    else if (data = -1) {
                        layer.alert("数据不合法！", { icon: 2, title: '错误提示' });
                    }
                    else {
                        layer.alert("没有用户角色被分配！", { icon: 0, title: '操作提示' });
                    }
                },
                error: function () {
                    layer.alert('通讯错误，请检查网络!', { icon: 2, title: '错误提示' });
                }
            });
    }
    else {
        layer.alert("没有要撤销的用户！", { icon: 0, title: '操作提示' });
    }
};
function search(groupid) {
    var posturl = url_getusergroup + '&roleID=' + $('#roleID').combobox('getValue') + '&groupID=' + groupid;
    $('#userCode').combobox('clear');
    $('#userCode').combobox('reload', posturl);
};
$(function () {
    $('#roleID').combobox({
        onChange: function (n, o) {
            search();
            reloadtabe();
        }
    });
});
function reloadtabe() {
    $('#dg').datagrid('load', {
        roleid: $('#roleID').combobox('getValue')
    });
};
