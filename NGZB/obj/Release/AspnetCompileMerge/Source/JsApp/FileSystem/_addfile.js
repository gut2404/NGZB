$(function () {
    var doucmentid = _documentid;
    if (doucmentid == "0") {
        $("#up").linkbutton("disable");
        $("#documentID").textbox('setValue', '');
        $("#documentName").textbox('setValue', '');
    }
    else {
        $("#up").linkbutton("enable");
        $("#documentID").textbox('setValue', _documentid);
        $("#documentName").textbox('setValue', _documentid);
    }
    var upok = _upok;
    if (upok != "") {
        if (upok == "1") {
            layer.alert("上传成功!", { icon: 1 });
            try {
                parent.search();
            } catch (e) { };
        }
        else if (upok == "-1") {
            layer.alert("文件不能为空！", { icon: 0 });
        }
        else if (upok == "-2") {
            layer.alert("未选择上传目录！", { icon: 0 });
        }
        else {
            layer.alert("上传失败，联系程序员！", { icon: 2 });
        }
    }
    $("#up").attr("enable");
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.iframeAuto(index);
});
function submitForm() {
    if ($("#documentID").val() == "") {
        layer.alert("未选择上传目录！", { icon: 0 });
        return;
    }
    if ($("#documentInfo").val().length > 200) {
        layer.alert("文件描述不能超过200字，目前" + $("#documentInfo").val().length + "个字！", { icon: 2 });
        return;
    }
    if ($("#fileNameID").val() == "") {
        layer.alert("未选择文件！", { icon: 0 });
        return;
    }
    var path = url_addfile + "&doucmentID=" + $("#documentID").val() + "&fileinfo=" + $("#documentInfo").val() + "&documetName=" + $("#documentName").val();
    $("#up").attr("disable");
    var formData = new FormData();
    var name = $("input").val();
    formData.append("upfile", $("#f")[0].files[0]);
    formData.append("name", name);
    $.ajax({
        url: path,
        type: 'POST',
        data: formData,
        // 告诉jQuery不要去处理发送的数据
        processData: false,
        // 告诉jQuery不要去设置Content-Type请求头
        contentType: false,
        beforeSend: function () {
            layer.alert("正在上传，请稍候......");
        },
        success: function (responseStr) {
            if (responseStr == 1) {
                try {
                    parent.searchkey();
                } catch (e) { };
                layer.alert("上传成功!", { icon: 1 }, function () {
                    parent.layer.closeAll();
                });
            }
            else {
                layer.alert("上传失败，联系程序员！", { icon: 2 });
            }
        },
        error: function (responseStr) {
            layer.alert("系统错误，联系程序员！");
        }
    });
};