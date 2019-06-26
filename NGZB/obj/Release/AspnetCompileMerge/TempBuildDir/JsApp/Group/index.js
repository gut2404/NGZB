function add() {
    var node = $('#tree').etree('getSelected');
    layer.open({
        type: 2,
        title: "添加组织节点",
        area: ['380px', '300px'],
        content: url_add + '&groupid=' + node.id
    });
};
function edit() {
    var node = $('#tree').etree('getSelected');
    layer.open({
        type: 2,
        title: "编辑组织节点",
        area: ['400px', '270px'],
        content: url_edit + '&groupid=' + node.id
    });
};
function reload() {
    $('#tree').etree("reload");
};
function redo() {
    var node = $('#tree').etree('getSelected');
    var url = url_redo;
    $.ajax({
        type: "POST",
        url: url,
        data: { groupid: node.id },
        dataType: "text",
        success: function (response) {
            if (response > 0) {
                layer.alert("恢复成功！", { icon: 1 }, function (index) {
                    reload();
                    layer.close(index);
                });
            }
            else {
                layer.alert("恢复失败，请联系程序员！", { icon: 2 });
            }
        }
    });
};
function confirmDel() {
    layer.confirm("确认删除吗？", { icon: 3, title: '提示' },
        function (intdex) {
            del();
            layer.close(index)
        },
        function (index) {
            layer.close(index)
        });
};
function confirmRedo() {
    layer.confirm("确认恢复吗？", { icon: 3, title: '提示' },
        function (intdex) {
            redo();
            layer.close(index)
        },
        function (index) {
            layer.close(index)
        });
};
function del() {
    var node = $('#tree').etree('getSelected');
    var url = url_del;
    $.ajax({
        type: "POST",
        url: url,
        data: { groupid: node.id },
        dataType: "text",
        success: function (response) {
            if (response > 0) {
                layer.alert("删除成功！", { icon: 1 }, function (index) {
                    reload();
                    layer.close(index);
                });
            }
            else {
                layer.alert("删除失败，请联系程序员！", { icon: 2 });
            }
        }
    });
};
function setRoot() {
    var t = $('#tree');
    var node = t.etree('getSelected');
    var url = url_setroot;
    $.ajax({
        type: "POST",
        url: url,
        data: { groupid: node.id },
        dataType: "text",
        success: function (response) {
            if (response > 0) {
                layer.alert("设置成功！", { icon: 1 }, function (index) {
                    reload();
                    layer.close(index);
                });
            }
            else {
                layer.alert("设置失败，请联系程序员！", { icon: 2 });
            }
        }
    });
};
function parentNum(node) {
    var s = node.text;
    if (node.children) {
        s += '<span style=\'color:blue\'>(' + node.children.length + ')</span>';
    }
    return s;
};
$(function () {
    $('#tree').etree({
        onClick: function (node) {
            nonode = "";
            getpath(node);
            $("#selectduc").html(nonode + node.text);
            $("#dg").datagrid("getPanel").panel("setTitle", "当前选择目录：" + nonode + node.text);
            $('#dg').datagrid('load', {
                documentID: node.id
            });
        },
        isedit: false,
        dndUrl: url_movegroup,
        onDropSuccess: function (data) {
            if (data == 1) {
                layer.alert("移动成功！", { icon: 1 }, function (index) {
                    reload();
                    layer.close(index);
                });
            }
            else {
                layer.alert("移动失败！", { icon: 2 });
            }
        },
        onContextMenu: function (e, node) {
            e.preventDefault();//阻止冒泡
            e.stopPropagation();//阻止事件传播
            $(this).etree('select', node.target);
            if (node.attributes.isdel == "1") {
                $("#btnAdd").css("display", "none");
                $("#btnEdit").css("display", "none");
                $("#del1").css("display", "none");
                $("#del2").css("display", "none");
                $("#redo2").css("display", "block");
                $("#redo3").css("display", "none");
                $("#btnroot").css("display", "none");
            } else {
                $("#btnAdd").css("display", "block");
                $("#btnEdit").css("display", "block");
                $("#del1").css("display", "block");
                $("#del2").css("display", "block");
                $("#redo2").css("display", "none");
                $("#redo3").css("display", "none");
                $("#btnroot").css("display", "block");
            }
            $('#mm').menu('show', { left: e.pageX, top: e.pageY });
        }
    });
});