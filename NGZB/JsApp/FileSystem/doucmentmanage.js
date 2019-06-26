var nonode = "";
$(function () {
    $('#tree').etree({
        onClick: function (node) {
            nonode = "";
            getpath(node);
            $("#dg").datagrid("getPanel").panel("setTitle", "当前选择目录：" + nonode + node.text);
            $('#dg').datagrid('load', {
                documentID: node.id
            });
            $("#selectducid").val(node.id);
        },
        isedit: false,
        dndUrl: url_movedocument,
        onDropSuccess: function (data) {
            if (data == 1) {
                $('#tree').etree("reload");
                search();
                layer.alert("移动成功！", { icon: 1 });
            }
            else {
                layer.alert("移动失败！", { icon: 2 });
            }
        },
        onContextMenu: function (e, node) {
            e.preventDefault();//阻止冒泡
            e.stopPropagation();//阻止事件传播
            $(this).etree('select', node.target);
            $('#mm').menu('show', { left: e.pageX, top: e.pageY });
        }
    });
    //$('#dg').datagrid('hideColumn', 'documentID');//用于目录操作权限判断，暂时预留
    //$('#dg').datagrid('hideColumn', '_del');
});
function getpath(nodes) {
    var node1 = $('#tree').tree('getParent', nodes.target);
    if (node1 != null) {
        nonode = nonode + node1.text + ">>";
        getpath(node1);
    }
};
function search() {
    var keys = $('#searchkey').val();
    var ducid = $("#selectducid").val();
    $('#dg').datagrid('load', {
        documentID: ducid,
        key: keys
    });
};
function urlformat(value, row, index) {
    return "<a class='downbtn c2' onclick=edit(" + row.documentID + ",'" + row.documentName + "','" + row.documentInfo + "'," + row.documentOrderBy + ")>修改</a>";
};
function addformat(value, row, index) {
    return "<a class='addbtn c2' onclick=add(" + row.documentID + ",'" + row.documentName + "','" + row.documentInfo + "')>添加</a>";
};
function delformat(value, row, index) {
    if (value == "1") {
        return '<font color="red">删除</font>';
    }
    else {
        return "正常";
    }
};
function edit(documetid, documentname, documentinfo, orderby) {
    layer.open({
        type: 1,
        title: "修改目录",
        area: ['450px', '420px'],
        content: $('#editdiv') //这里content是一个DOM
    });
    $("#parentid").textbox('setValue', '');
    $("#parentname").textbox('setValue', '');
    $("#editid").textbox('setValue', documetid);
    $("#editname").textbox('setValue', documentname);
    $("#editinfo").textbox('setValue', documentinfo == 'null' ? '' : documentinfo);
    $("#subeditbtn").show();
    $("#subaddbtn").hide();
    $("#subeditbtn").linkbutton("enable");
    $("#orderby").textbox('setValue', orderby);
};
function add(documetid, documentname, documentinfo) {
    layer.open({
        type: 1,
        title: "添加目录",
        area: ['450px', '420px'],
        content: $('#editdiv') //这里content是一个DOM
    });
    $("#parentid").textbox('setValue', documetid);
    $("#parentname").textbox('setValue', documentname);
    $("#editid").textbox('setValue', '');
    $("#editname").textbox('setValue', '');
    $("#editinfo").textbox('setValue', '');
    $("#subeditbtn").hide();
    $("#subaddbtn").show();
    $("#subaddbtn").linkbutton("enable");
    $("#orderby").textbox('setValue', '0');
};
function formatlinbutton() {
    $(".downbtn").linkbutton({ text: '修改', plain: true, iconCls: 'icon-my_edit' });
    $(".delbtn").linkbutton({ text: '删除', plain: true, iconCls: 'icon-undo' });
    $(".redobtn").linkbutton({ text: '恢复', plain: true, iconCls: 'icon-redo' });
    $(".addbtn").linkbutton({ text: '添加', plain: true, iconCls: 'icon-add' });
    $(".allowbtn").linkbutton({ text: '授权', plain: true, iconCls: 'icon-my_key' });
};
function formatOper(val, row, index) {
    if (row.isDel == "0") {
        return "<a class='delbtn c5' onclick=\"del(" + index + ")\">删除</a>";
    }
    else {
        return "<a class='redobtn c1' onclick=\"redo(" + row.documentID + ")\">恢复</a>";
    }
};
function formatAllow(val, row, index) {
    return "<a class='allowbtn c1' onclick=\"allow(" + row.documentID + ",'" + row.documentName + "')\">授权</a>";
};
function allow(documentid, documentname) {
    var url = url_adddocumentallow + '&ducid=' + documentid + "&ducname=" + documentname;
    layer.open({
        type: 2,
        area: ['320px', '310px'],
        shift: 4,
        fix: true,
        maxmin: false,
        content: url,
        title: '目录权限管理'
    });
};
function del(index) {
    $('#dg').datagrid('selectRow', index);// 手动选中行,可以选择行的其它对象，留作参考
    var row = $('#dg').datagrid('getSelected');//获取选中行对象
    if (row) {
        var url = url_deldocument;
        $.ajax({
            type: "POST",
            async: true,
            cache: false,
            url: url,
            data: { ducid: row.documentID },
            dataType: "text",
            success: function (response) {
                if (response == "1") {
                    search();
                    $("#tree").tree("reload");
                    layer.alert("删除成功", { icon: 1 });
                }
                else {
                    layer.alert("删除失败，联系程序员", { icon: 2 });
                }
            }
        });
    }
};
function redo(documentID) {
    var url = url_redocument;
    $.ajax({
        type: "POST",
        async: true,
        cache: false,
        url: url,
        data: { ducid: documentID },
        dataType: "text",
        success: function (response) {
            if (response == "1") {
                search();
                $("#tree").tree("reload");
                layer.alert("恢复成功", { icon: 1 });
            }
            else {
                layer.alert("恢复失败，联系程序员", { icon: 2 });
            }
        }
    });
};
function subedit() {
    var url = url_editdocument;
    $("#subeditbtn").linkbutton("disable");
    $.ajax({
        type: "POST",
        async: false,
        cache: false,
        url: url,
        data: {
            docid: $("#editid").textbox('getValue'),
            docname: $("#editname").textbox('getValue'),
            docinfo: $("#editinfo").textbox('getValue'),
            oderby: $("#orderby").textbox('getValue')
        },
        dataType: "text",
        success: function (response) {
            if (response == "1")
            {
                $("#tree").tree("reload");
                search();
                layer.alert("修改成功", { icon: 1 }, function (index)
                {
                    layer.closeAll();
                });
            }
            else {
                layer.alert("修改失败，联系程序员", { icon: 2 });
            }
        }
    });
};
function selectroot() {
    $('#tree').find('.tree-node-selected').removeClass('tree-node-selected');//取消tree选中项
    $("#dg").datagrid("getPanel").panel("setTitle", "当前选择目录：根目录");
    $("#selectducid").val("0");
    search();
};
function subadd() {
    $("#subaddbtn").linkbutton("disable");
    var url = url_adddocument;
    $("#subeditbtn").linkbutton("disable");
    $.ajax({
        type: "POST",
        async: false,
        cache: false,
        url: url,
        data: {
            docid: $("#parentid").textbox('getValue'),
            docname: $("#editname").textbox('getValue'),
            docinfo: $("#editinfo").textbox('getValue'),
            oderby: $("#orderby").textbox('getValue')
        },
        dataType: "text",
        success: function (response) {
            if (response == "1") {
                search();
                $("#tree").tree("reload");
                layer.alert("添加成功", { icon: 1 }, function (index)
                {
                    layer.closeAll();
                });
            }
            else {
                layer.alert("添加失败，联系程序员", { icon: 2 });
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
function setRoot() {
    var t = $('#tree');
    var node = t.etree('getSelected');
    var url = url_setroot;
    $.ajax({
        type: "POST",
        url: url,
        data: { ducid: node.id },
        dataType: "text",
        success: function (response) {
            if (response > 0) {
                layer.alert("设置成功！", { icon: 1 }, function (index) {
                    $('#tree').etree("reload");
                    layer.closeAll();
                });
            }
            else {
                layer.alert("设置失败，请联系程序员！", { icon: 2 });
            }
        }
    });
};