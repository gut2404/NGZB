$(function ()
{
    if (_ismy == "1")
    {
        $("#p").panel({
            tools: '#tt'
        })
    }
    $(function ()
    {
        var index = parent.layer.getFrameIndex(window.name);
        parent.layer.iframeAuto(index);
    });
});
function del()
{
    var msgid = $("#msgid").val();
    var url = url_delmessage;
    $.ajax({
        type: "POST",
        async: true,
        cache: false,
        url: url,
        data: "msgid=" + msgid,
        dataType: "text",
        success: function (response)
        {
            if (response == 1)
            {
                layer.alert("公告删除成功！", { icon: 1 }, function (index)
                {
                    try
                    {
                        parent.search();
                    }
                    catch (e) { };
                    parent.layer.closeAll();
                });
            } else
            {
                layer.alert("删除失败，请联系程序员！", { icon: 2 });
            }
        }
    });
};