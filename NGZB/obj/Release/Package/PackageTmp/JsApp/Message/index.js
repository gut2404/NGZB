function clearmessage() {
    $("#messagetitle").val("");
    KE[0].html('');
};
function addmessage() {
    $("#add").linkbutton("disable");
    $.ajax({
        async: true,
        cache: false,
        type: "POST",
        url: url_addmessage,
        data: {
            msgtitle: $("#messagetitle").val(),
            msg: KE[0].html()
        },
        dataType: "text",
        success: function (response) {
            if (response == 1) {
                layer.alert("发布成功！", { icon: 1 });
            } else if (response == -1) {
                layer.alert("数据格式不对！", { icon: 0 });
            } else {
                layer.alert("发布失败，请联系程序员！", { icon: 2 });
            }
        }
    });
}
$(function () {
    var url = url_messageaddcheck;
    $.ajax({
        type: "GET",
        async: false,
        cache: false,
        url: url,
        dataType: "text",
        success: function (response) {
            if (response == 1) {
                $("#add").linkbutton("enable");
                $("#cls").linkbutton("enable");
            }
            else {
                $("#add").linkbutton("disable");
                $("#cls").linkbutton("disable");
            }
        }
    });
});