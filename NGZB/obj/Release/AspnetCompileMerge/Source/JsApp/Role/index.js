//给权限数量加超链接
function roleformater(value, row, index) {
    return "<a href=\"javascript:alergroleitem(" + row.roleID + ",'" + row.roleName + "');\">" + row.roleItemCount + "</a>";
};
function alergroleitem(rowID, rowName) {
    var url = url_getroleinfo + "&roleID=" + rowID;
    layer.open({
        type: 2,
        area: ['300px',"300px"],
        shift: 4,
        fix: true,
        maxmin: false,
        closeBtn: 2,
        content: url,
        title: rowName + '权限明细'
    });
};
//给用户数量加超链接
function userformater(value, row, index) {
    return "<a href=\"javascript:alergroleuser(" + row.roleID + ",'" + row.roleName + "');\">" + row.roleUserCount + "</a>";
};
function alergroleuser(rowID, rowName) {
    var url = url_getroleuser + "&roleID=" + rowID;
    layer.open({
        type: 2,
        area: ['250px',"300px"],
        shift: 4,
        fix: true,
        maxmin: false,
        closeBtn: 2,
        content: url,
        title: rowName + '用户明细'
    });
};
function add() {
    var url = url_add;
    layer.open({
        type: 2,
        area: ['303px', '530px'],
        fix: true,
        maxmin: false,
        closeBtn: 2,
        content: url,
        title: ''
    });
};
function edit() {
    var url = url_edit;
    layer.open({
        type: 2,
        area: ['80%', '540px'],
        fix: true,
        maxmin: false,
        closeBtn: 2,
        content: url,
        title: ''
    });
};
function delconfirm(index) {
    layer.confirm('删除角色后该角色对应的业务、用户都将被删除，确认吗？', {
        btn: ['确定', '取消'],
        icon: 3,
        title: '删除角色确认？'
    },
        function () {
            del();
        },
        function () {
            layer.close(index);
        });
};
function del() {
    var postUrl = url_del;
    var checkedItems = $('#dg').datagrid('getChecked');
    var names = [];
    $.each(checkedItems, function (index, item) {
        names.push(item.roleID);
    });
    var getvalue = names.join("|");
    if (getvalue != '') {
        $.ajax(
            {
                url: postUrl,
                type: 'post',
                data: {
                    roleID: getvalue
                },
                cache: false,
                dataType: 'text',
                success: function (data) {
                    if (data > 0) {
                        var str = data + "个角色被删除！"
                        layer.alert(str, { icon: 1 });
                        search();
                    }
                    else {
                        layer.alert("没有角色被删除！", { icon: 0 });
                    }
                },
                error: function () {
                    layer.alert('通讯错误，请检查网络!', { icon: 2 });
                }
            });
    }
    else {
        layer.alert('没有选择要删除的角色!', { icon: 2 });
    }
};
function search(keys) {
    if (keys == null) {
        keys = $('#searchkey').val();
    }
    $('#dg').datagrid('load', {
        key: keys,
        f: $('#f').combobox('getValue')
    });
};
$(function () {
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.iframeAuto(index);
    $('#dg').datagrid({ selectOnCheck: $(this).is(':checked') });
});