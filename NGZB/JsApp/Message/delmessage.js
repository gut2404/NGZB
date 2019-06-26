function msginfo(value, row, index) {
    return "<a class='msgbtn c7' onclick=\"info(" + row.msgID + ",'" + row.msgTitle + "')\">详情</a>";
};
function delmsg(value, row, index) {
    return "<a class='delbtn c7' onclick=\"del(" + row.msgID + ")\">删除</a>";
};
function formatlinbutton() {
    $(".msgbtn").linkbutton({ text: '详情', plain: true, iconCls: 'icon-my_msginfo' });
    $(".delbtn").linkbutton({ text: '删除', plain: true, iconCls: 'icon-no' });
    var url = url_messagedelcheck;
    $.ajax({
        type: "GET",
        async: false,
        cache: false,
        url: url,
        dataType: "text",
        success: function (response) {
            if (response == 1) {
                $(".delbtn").linkbutton("enable");
            }
            else {
                $(".delbtn").linkbutton("disable");
                $(".delbtn").tooltip({ position: 'right', content: '你无权删除内部公告！' });
            }
        }
    });
};
function info(msgid, msgtitle) {
    var url = url_messageinfo + '&msgid=' + msgid;
    layer.open({
        type: 2,
        area: ['80%', '80%'],
        shift: 4,
        fix: true,
        maxmin: false,
        content: url,
        closeBtn: 2,
        title: ''
    })
};
function del(msgid) {
    $(this).attr("disable");
    var url = url_del + '&msgid=' + msgid;
    $.ajax({
        type: "POST",
        async: true,
        cache: false,
        url: url,
        data: "msgid=" + msgid,
        dataType: "text",
        success: function (response) {
            if (response == 1) {
                layer.alert("公告删除成功！", { icon: 1 });
                search();
            } else {
                layer.alert("删除失败，请联系程序员！", { icon: 2 });
            }
        }
    });
};
function search() {
    var keys = $("#searchkey").val();
    $('#dgmsg').datagrid('load', {
        keys: keys,
        sdate: $("#st1").datebox("getValue"),
        edate: $("#et1").datebox("getValue")
    });
}