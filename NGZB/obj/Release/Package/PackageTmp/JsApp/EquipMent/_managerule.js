window.onload = function ()
{
    KE[0].html($('#oldeqoperateInfo').val());
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.iframeAuto(index);
};
$(function ()
{
    $("#eqoperateInfo").css("height", $("#lasttd").height() + 15);
});
function dayCheckFn()
{
    addTab(url_daycheck, '点检标准');
};
function debugFn()
{
    addTab(url_debug, '常见故障');
};
function oilFn()
{
    addTab(url_oil, '润滑标准');
};
function repairFn()
{
    addTab(url_repair, '检修标准');
};
function addTab(url, title)
{
    url = url + '&eqid=' + eqid;
    var content = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:99%;"></iframe>';
    if ($('#tt').tabs('exists', title))
    {
        $('#tt').tabs('select', title);
    } else
    {
        $('#tt').tabs('add', {
            title: title,
            content: content,
            closable: true
        });
    }
}
function saveRule()
{
    var companTitle = $('#compantitle').val();
    var operateName = $('#filename').val();
    var operateCode = $('#filecode').val();
    var operateNumber = $('#filenumber').val();
    var operateText = KE[0].html();
    $.ajax({
        type: "POST",
        url: url_save,
        data: {
            eqID: eqid,
            companTitle: companTitle,
            operateName: operateName,
            operateCode: operateCode,
            operateNumber: operateNumber,
            operateText: operateText,
            isPass: 0,
            createUserCode: '',
            uplastDate: ''
        },
        dataType: "text",
        success: function (response)
        {
            if (response == "1")
            {
                layer.alert("保存成功！", { icon: 1 }, function (index)
                {
                    layer.close(index);
                    parent.searchKey();
                });
            }
        }
    });
}