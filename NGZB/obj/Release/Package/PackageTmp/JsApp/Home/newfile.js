function filesearch()
{
    $('#dgnew').datagrid('reload');
};
function urlformat(value, row, index)
{
    return "<a class='downbtn c2' href='" + url_downfile + "&fileid=" + row.fileID + "' target='_blank'> 下载d文件</a>";
};
function formatlinbutton()
{
    $(".downbtn").linkbutton({ text: '下载', plain: true, iconCls: 'icon-my_down' });
    $(".msgbtn").linkbutton({ text: '详情', plain: true, iconCls: 'icon-my_msginfo' });
};