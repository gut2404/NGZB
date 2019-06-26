function titleformat(value, row, index) {
    return '<a href="javascript:void(0)" onclick="readMsg(' + row.personMsgID + ',\'' + row.msgtitle + '\',' + row.readState + ',\'rec\')">' + value + '</a>';
};
function titleformatsend(value, row, index) {
    return '<a href="javascript:void(0)" onclick="readMsg(' + row.personMsgID + ',\'' + row.msgtitle + '\',' + row.readState + ',\'send\')">' + value + '</a>';
};
function textFormat(value, row, index) {
    if (value == '1') {
        return '已读';
    }
    else {
        return '未读';
    }
};
function cellStyler(value, row, index) {
    if (value == "0") {
        return 'color:red;';
    }
};
function selectDpid(dpid) {
    var url = url_getuser + '&groupid=' + dpid;
    $('#usercode').combobox('reload', url);
};
function sendmessage() {
    var msgTitle = $("#messagetitle").val();
    if (msgTitle == "") {
        layer.alert("消息标题不能为空!", { icon: 0 });
        return;
    };
    var recUsercode = $('#usercode').combobox('getValues');
    if (usercode == "") {
        layer.alert("没有选择接收用户!", { icon: 0 });
        return;
    }
    $.ajax({
        type: "POST",
        async: true,
        cache: false,
        url: url_sendmessage,
        data:
        {
            msgtitle: $("#messagetitle").val(),
            msginfo: KE[0].html(),
            recUsercode: recUsercode.join('|')
        },
        dataType: "text",
        success: function (response) {
            if (response > 0) {
                layer.alert(response + "条消息发送成功!", { icon: 1 },
                    function (index) {
                        searchSendBox();
                        layer.close(index);
                    });
            }
        }
    });
};
function clearmessage() {
    $("#messagetitle").val("");
    KE[0].html('');
};
function upMsgCount(noreadcount) {
    try {
        parent.msgcount(noreadcount);
    }
    catch (e) { };
};
function readMsg(personMsgID, msgtitle, readstate, boxtype) {
    var url = url_getmsginfo + '&personMsgID=' + personMsgID;
    layer.open({
        type: 2,
        area: ['600px', '400px'],
        shift: 4,
        fix: true,
        maxmin: false,
        content: url,
        closeBtn: 2,
        title: '“' + msgtitle + '”详细内容'
    });
    if (boxtype == 'rec') {
        if (readstate == 0) {
            var upReadUrl = url_getnoreadmsg + '&personMsgID=' + personMsgID;
            $.ajax({
                type: "GET",
                async: false,
                cache: false,
                url: upReadUrl,
                dataType: "text",
                success: function (response) {
                    upMsgCount(response);
                    searchRecBox();
                    searchSendBox();
                }
            });
        }
    }
};
function delMsg(deltype) {
    var checkedItems = "";
    var url = "";
    if (deltype == 'r') {
        url = url_delsendmsg;
        checkedItems = $('#recbox').datagrid('getChecked')
    }
    else {
        url = url_delsendmsg;
        checkedItems = $('#sendbox').datagrid('getChecked')
    }
    var _msgid = [];
    $.each(checkedItems, function (index, item) {
        _msgid.push(item.personMsgID);
    });
    var getvalue = _msgid.join("|");
    $.ajax({
        type: "POST",
        async: false,
        cache: false,
        url: url,
        data: { msgid: getvalue },
        dataType: "text",
        success: function (response) {
            if (response > 0) {
                layer.alert(response + "条消息删除成功!", { icon: 1 },
                    function (index) {
                        if (deltype == 'r') {
                            searchRecBox();
                        }
                        else {
                            searchSendBox();
                        }
                        layer.close(index);
                    });
            }
            else if (response == 0) {
                layer.alert("没有选择要删除的消息!", { icon: 0 });
            } else {
                layer.alert("数据格式错误，联系管理员!", { icon: 2 });
            }
        }
    });
};
function searchRecBox() {
    $('#recbox').datagrid('reload', {
        keys: $("#searchreckey").val(),
        readtype: $("#readtype1").combobox('getValue'),
        sdate: $("#st1").datebox("getValue"),
        edate: $("#et1").datebox("getValue")
    });
};
function searchSendBox() {
    $('#sendbox').datagrid('reload', {
        keys: $("#searchsendkey").val(),
        readtype: $("#readtype2").combobox('getValue'),
        sdate: $("#st2").datebox("getValue"),
        edate: $("#et2").datebox("getValue")
    });
};