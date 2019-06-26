var nonode = "";
$(function () {
    $('#tree').tree({
        onClick: function (node) {
            nonode = "";
            getpath(node);
            $("#dg").datagrid("getPanel").panel("setTitle", "当前选择目录：" + nonode + node.text);
            $('#dg').datagrid('load', {
                documentID: node.id
            });
        }
    });
});
function getpath(nodes) {
    var node1 = $('#tree').tree('getParent', nodes.target);
    if (node1 != null) {
        nonode = nonode + node1.text + ">>";
        getpath(node1);
    }
};
function search() {
    var keys = $("#searchkey").val();
    var node = $('#tree').tree('getSelected');
    var ducid = 0;
    if (node != null) {
        ducid = node.id;
    };
    $('#dg').datagrid('load', {
        documentID: ducid,
        keys: keys
    });
};
function urlformat(value, row, index) {
    return "<a class='downbtn c2' href='" + url_downfile + "&fileid=" + row.fileID + "' target= '_blank' > 下载文件</a > ";
};
function formatlinbutton() {
    $(".downbtn").linkbutton({ text: '下载', plain: true, iconCls: 'icon-my_down' });
};
function parentNum(node) {
    var s = node.text;
    if (node.children) {
        s += '<span style=\'color:blue\'>(' + node.children.length + ')</span>';
    }
    return s;
};