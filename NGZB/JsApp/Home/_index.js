$(function ()
{
    reloadmsgcount();
});
function changepassword()
{
    //$('#password').window('open');
    layer.open({
        type: 2,
        title: "修改用户密码",
        area: ['300px', '270px'],
        content: url_ChangePassWord
    });
};
function addTab(title, url, icon)
{//添加tabs的函数，title为tabs名称，url为载入页面的url
    $('#tt').css("display", "block");
    $('#tt').tabs({//给tabs添加关闭事件
        onClose: function (title, index)
        {
            var t = $('#tt');
            var tabNum = t.tabs('tabs');
            if (tabNum.length == 1)
            {
                $("#tt").tabs("select", 0);
            }
        }
    })
    var content = '<div class="easyui-tabs" style="width:100%;border:none;"data-options="fit:true"><iframe scrolling="auto" frameborder="0" src="' + url + '?tokenkey=' + tokenkey + '" style="width:100%;height:100%;"></iframe></div>';//添加一个iframe来载入页面
    if ($('#tt').tabs('exists', title))
    {//判断要添加的tabs是否存在，如果存在则选择该tabs
        $('#tt').tabs('close', title);
        $('#tt').tabs('add', {//添加tabs
            title: title,//tabs标题
            content: content,//添加的内容
            closable: true,//显示关闭按钮
            iconCls: 'icon-' + icon
        });
    } else
    {
        var t = $('#tt');
        var tabNum = t.tabs('tabs');
        if (tabNum.length <= tabAllowNum)
        {
            $('#tt').tabs('add', {//添加tabs
                title: title,//tabs标题
                content: content,//添加的内容
                closable: true,//显示关闭按钮
                iconCls: 'icon-' + icon
            });
        }
        else
        {
            layer.alert("为了提高页面的访问速度，系统设置最多只能加载" + tabAllowNum + "个页面，请关闭一些不常用的页面再打开！", { icon: 0 });
        }
    }
    getMsg();
};
function removePanel()
{//关闭tabs函数
    var tab = $('#tt').tabs('getSelected');//获取当前已选择的tabs
    if (tab)
    {//如果存在
        var index = $('#tt').tabs('getTabIndex', tab);//获取该tabs的索引
        $('#tt').tabs('close', index);//关闭tabs
    }
};
function help()
{
    var url = url_Help;
    addTab("系统帮助", url);
};
function openmsg()
{
    var url = url_MyMsg;
    addTab("我的消息", url);
};
function msgcount(noreadcount)
{
    $("#msgcount").html(noreadcount);
};
$(window).bind('beforeunload', function ()
{
    $.ajax(
        {
            url: url_closebrower,
            type: 'GET',
            cache: false,
            dataType: 'text'
        });
});
function getMsg()
{
    $.ajax({
        type: "GET",
        url: url_getmsg,
        dataType: "text",
        success: function (response)
        {
            var count = parseInt(response);
            if (count > 0)
            {
                $("#msgcount").text(count).css("color", "red");
            }
        }
    });
}
function reloadmsgcount()
{
    getMsg();
    if (msgtime > 0)
    {
        setTimeout('reloadmsgcount()', msgtime);
    }
};