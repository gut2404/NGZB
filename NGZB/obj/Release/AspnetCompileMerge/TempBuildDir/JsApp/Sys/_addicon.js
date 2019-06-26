function submitForm()
{
    if ($("#fileNameID").val() == "")
    {
        layer.alert("未选择文件！", { icon: 0 });
        return;
    }
    var path = url_upicon;
    $.ajaxFileUpload({
        url: path,
        secureuri: false,
        fileElementId: 'f',
        dataType: 'text',
        success: function (data)
        {
            if (data == 1)
            {
                layer.alert("上传成功，可生成CSS类！", { icon: 1 }, function ()
                {
                    parent.writecss()
                    parent.reload();
                    parent.layer.closeAll();
                });
            }
            else
            {
                layer.alert("上传失败，请重新上传！", { icon: 2 });
            }
        },
        error: function ()
        {
            layer.alert("上传失败，请重新上传！", { icon: 2 });
        }
    });
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.iframeAuto(index);
};