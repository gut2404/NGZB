$(function ()
{
    $('#addBtn').linkbutton('disable');
    $('#eqtree').tree({
        onClick: function (node)
        {
            nonode = '';
            getpath(node);
            var searchkey = $('#searchkey').val();
            $('#dg').datagrid('getPanel').panel('setTitle', '当前选择设备：' + nonode + node.text);
            $('#dg').datagrid('load', {
                eqParendID: node.id,
                searchkey: searchkey
            });
            $('#editBtn').linkbutton('disable');
            $('#addBtn').linkbutton('disable');
            $.ajax({
                type: "POST",
                url: url_chek,
                data: "eqid=" + node.id + "&authorityusercode=" + loginuser,
                dataType: "text",
                success: function (response)
                {
                    if (response == "1")
                    {
                        $('#addBtn').linkbutton('enable');
                    }
                    else
                    {
                        $('#addBtn').linkbutton('disable');
                    }
                }
            });
        }
    });
    $('#dg').datagrid({
        onClickRow: function (index, data)
        {
            var row = $('#dg').datagrid('getSelected');
            var isPass = row.isPass;
            var usercode = row.createUserCode;
            var authorityUserCode = row.authorityUserCode;
            if (row)
            {
                if (isPass == "1")
                {
                    $('#editBtn').linkbutton('disable');
                }
                else
                {
                    if (authorityUserCode == loginuser)
                    {
                        $('#editBtn').linkbutton('enable');
                    }
                    else
                    {
                        if (usercode == longuser)
                        {
                            $('#editBtn').linkbutton('enable');
                        } else
                        {
                            $('#editBtn').linkbutton('disable');
                        }
                    }
                }
            }
        }
    });
    $('#editBtn').linkbutton('disable');
});
function formatlinbutton(data)
{
    $(".eqinfo").linkbutton({ plain: true, iconCls: 'icon-tip' });
    $(".ruleview").linkbutton({ plain: true, iconCls: 'icon-my_world' });
};
function searchKey()
{
    var searchkey = $('#searchkey').val();
    var selectNode = $('#eqtree').tree('getSelected');
    var eqID = 0;
    if (selectNode != null)
    {
        eqID = selectNode.id;
    };
    $('#dg').datagrid('load', {
        eqParendID: eqID,
        searchkey: searchkey
    });
    $('#editBtn').linkbutton('disable');
};
function addOpRule()
{
    var node = $('#eqtree').tree('getSelected');
    var opearterType = $('#addBtn').attr("opearteType");
    if (node == null)
    {
        layer.alert("未选择要添加作业文件的设备！");
        return;
    }
    layer.open({
        type: 2,
        title: '',
        area: ['95%', '95%'],
        content: url_managerule + '&optype=' + opearterType + '&eqid=' + node.id
    });
};
function editOpRule()
{
    var row = $('#dg').datagrid('getSelected');
    var opearterType = $('#editBtn').attr("opearteType");
    if (row)
    {
        layer.open({
            type: 2,
            title: '',
            area: ['95%', '95%'],
            content: url_managerule + '&optype=' + opearterType + '&eqid=' + row.eqID
        });
    }
    else
    {
        layer.alert("未选择要修改的作业文件！", { icon: 0 });
    }
};
function formatid(value, row, index)
{
    return "<a class='eqinfo c1' onclick=\"showeqinfo(" + row.eqID + ")\">" + row.eqID + "</a > ";
};
function showeqinfo(eqid)
{
    layer.open({
        type: 2,
        title: "设备详情浏览",
        area: ['60%', '420px'],
        content: url_eqinfo + '&eqid=' + eqid
    });
};
function getpath(nodes)
{
    var node1 = $('#eqtree').tree('getParent', nodes.target);
    if (node1 != null)
    {
        nonode = nonode + node1.text + ">>";
        getpath(node1);
        $('#eqid').val(nodes.id);
    };
};
function stateformat(value, row, index)
{
    if (value == "1")
    {
        return '<font color="Green">正式</font>';
    }
    else
    {
        return '<font color="Orange">草稿</font>';
    }
};
function userformat(value, row, index)
{
    return "<a class='ruleview' onclick=\"showrule('" + row.eqID + "')\"></a > ";
};
function showrule(eqid)
{
    layer.open({
        type: 2,
        title: "作业文件浏览",
        area: ['95%', '95%'],
        content: url_ruleinfo + '&eqid=' + eqid
    });
};