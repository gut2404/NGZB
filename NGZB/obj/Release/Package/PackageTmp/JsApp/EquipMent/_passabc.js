function getChanges()
{
    var s = [];
    var rows = $('#pg').propertygrid('getChanges');
    var total = $('#pg').propertygrid('getData');
    for (var i = 0; i < rows.length; i++)
    {
        s.push(rows[i].name + '|' + rows[i].value);
    }
    if (total.total > s.length)
    {
        alert('所有项目都必须打分');
    }
    else
    {
        var eqID = $('#eqid').val();
        var opearterType = parent.$('#passabcBtn').attr("opearteType");
        $.ajax({
            type: "POST",
            url: url_passabc,
            data: {
                eqid: eqID,
                abc: s.join(','),
                opeartetype: opearterType
            },
            dataType: "text",
            success: function (response)
            {
                if (response == "1")
                {
                    layer.alert("评级成功！", { icon: 1 }, function ()
                    {
                        try
                        {
                            parent.searchKey();
                            parent.treeReload();
                        }
                        catch (e) { };
                        parent.layer.closeAll();
                    });
                } else if (response == "-1")
                {
                    layer.alert("数据错误，请检查！", { icon: 0 });
                }
                else
                {
                    layer.alert("评级失败！", { icon: 2 });
                }
            }
        });
    }
};
