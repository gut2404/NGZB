var nonode = "";
$(function () {
    $("#addbtn").linkbutton("disable");
    $("#addbtn").tooltip({ position: 'right', content: '请先选择上传的目录' });
    $("#delbtn").linkbutton("disable");
    $("#delbtn").tooltip({ position: 'right', content: '请先选择上传的目录' });
    $("#searchbtn").tooltip({ position: 'right', content: '请先选择要查询的目录' });
    $('#tree').tree({
        onClick: function (node) {
            nonode = "";
            getpath(node);
            $("#selectduc").html(nonode + node.text);
            $("#dg").datagrid("getPanel").panel("setTitle", "当前选择目录：" + nonode + node.text);
            $('#dg').datagrid('load', {
                documentID: node.id
            });
            var passary = node.attributes.pass.split('|');
            if (passary[0] == 1) {
                $("#addbtn").linkbutton("enable");
                $("#addbtn").tooltip({ position: 'right', content: '有权该目录上传文件' });
                $("#delbtn").linkbutton("enable");
                $("#delbtn").tooltip({ position: 'right', content: '有权删除该目录下文件' });
            }
            else {
                if (passary[1] == 1) {
                    $("#addbtn").linkbutton("enable");
                    $("#addbtn").tooltip({ position: 'right', content: '有权在该目录上传文件' });
                }
                else {
                    $("#addbtn").linkbutton("disable");
                    $("#addbtn").tooltip({ position: 'right', content: '无权在该目录上传文件' });
                }
                if (passary[2] == 1) {
                    $("#delbtn").linkbutton("enable");
                    $("#delbtn").tooltip({ position: 'right', content: '有权删除该目录下文件' });
                }
                else {
                    $("#delbtn").linkbutton("disable");
                    $("#delbtn").tooltip({ position: 'right', content: '无权删除该目录下文件' });
                }
            }
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
function searchkey() {
    var keys = $("#searchkey").val();
    var node = $('#tree').tree('getSelected');
    var ducid = 0;
    if (node != null) {
        ducid = node.id;
    }
    $('#dg').datagrid('load', {
        documentID: ducid,
        keys: keys
    });
};
function urlformat(value, row, index) {
    return "<a class='downbtn c2' href='" + url_downfile + "&fileid=" + row.fileID + "' target='_blank'></a>";
};
function formatlinbutton() {
    $(".downbtn").linkbutton({ text: '下载', plain: true, iconCls: 'icon-my_down' });
};
function add() {
    var node = $('#tree').tree('getSelected');
    var ducid = 0;
    if (node != null) {
        ducid = node.id;
    }
    var url = url_addfile + '&documentID=' + ducid;
    layer.open({
        type: 2,
        area: ['505px', '290px'],
        shift: 4,
        fix: true,
        maxmin: false,
        content: url,
        scrollbar: false,
        closeBtn: 2,
        title: '上传文件'
    });
};
function delconfirm(index) {
    layer.confirm('确认要删除选中的文件吗？', {
        btn: ['确定', '取消'],
        icon: 3,
        title: '删除文件确认'
    },
        function () {
            layer.closeAll('dialog');
            del();
        },
        function () {
            layer.closeAll('dialog');
        });
};
function del() {
    var postUrl = url_delfile;
    var checkedItems = $('#dg').datagrid('getChecked');
    var names = [];
    $.each(checkedItems, function (index, item) {
        names.push(item.fileID);
    });
    var deldoucmentID = names.join("|");
    var node = $('#tree').tree('getSelected');
    var ducid = 0;
    if (node != null) {
        ducid = node.id;
    }
    if (deldoucmentID != '') {
        deldoucmentID = ducid + "|" + deldoucmentID;
        $.ajax(
            {
                url: postUrl,
                type: 'post',
                data: {
                    deldoucment: deldoucmentID
                },
                cache: false,
                dataType: 'text',
                success: function (data) {
                    if (data > 0) {
                        var str = data + "个文件被删除！";
                        layer.alert(str, { icon: 1 });
                        searchkey();
                    }
                    else {
                        layer.alert("没有文件被删除！", { icon: 0 });
                    }
                },
                error: function () {
                    layer.alert('通讯错误，请检查网络!');
                }
            });
    }
    else {
        layer.alert('没有选择要删除的文件!', { icon: 2 });
    }
};
function parentNum(node) {
    var s = node.text;
    if (node.children) {
        s += '<span style=\'color:blue\'>(' + node.children.length + ')</span>';
    }
    return s;
};