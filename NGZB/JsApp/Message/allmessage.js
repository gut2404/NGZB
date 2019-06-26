;
function formatlinbutton() {
	$(".msgbtn").linkbutton({ text: '详情', plain: true, iconCls: 'icon-my_msginfo' });
};
function info(msgid, msgtitle) {
	var url = url_getmessage + '&msgid=' + msgid;
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
function search() {
	var keys = $("#searchkey").val();
	$('#dgmsg').datagrid('load', {
		keys: keys,
		sdate: $("#st1").datebox("getValue"),
		edate: $("#et1").datebox("getValue")
	});
};

function msginfo(value, row, index) {
	return "<a class='msgbtn c7' onclick=\"info(" + row.msgID + ",'" + row.msgTitle + "')\">详情</a>";
}