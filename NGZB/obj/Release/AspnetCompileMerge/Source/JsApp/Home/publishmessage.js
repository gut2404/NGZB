function msginfo(value, row, index)
{
    return "<a class='msgbtn c7' onclick=\"info(" + row.msgID + ",'" + row.msgTitle + "')\">详情</a>";
};
function formatlinbutton()
{
    $(".downbtn").linkbutton({ text: '下载', plain: true, iconCls: 'icon-my_down' });
    $(".msgbtn").linkbutton({ text: '详情', plain: true, iconCls: 'icon-my_msginfo' });
};
