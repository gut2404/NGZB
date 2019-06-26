var nonode = "";
var eqgoback = new Array()
eqgoback[0] = 0;
var eqdatagridselectname = new Array();
$(function ()
{
    $('#eqtree').tree({
        onClick: function (node)
        {
            nonode = '';
            getpath(node);
            $('#dg').datagrid('getPanel').panel('setTitle', '当前选择设备：' + nonode + node.text);
            $('#dg').datagrid('load', {
                eqID: node.id
            });
            $('#nowselectNode').val(node.id);
        }
    });
    $("#dg").datagrid({
        onDblClickRow: function (index, row)
        {
            eqgoback.push(row.eqID);
            eqdatagridselectname.push(row.eqName);
            $('#dg').datagrid('getPanel').panel('setTitle', '当前选择设备：' + getRowName());
            $('#dg').datagrid('load', {
                eqID: row.eqID
            });
            $('#nowselectNode').val(row.eqID);
        }
    });
});
function getRowName()
{
    var eqname = '';
    for (var i = 0; i < eqdatagridselectname.length; i++)
    {
        if (i < eqdatagridselectname.length - 1)
        {
            eqname = eqname + eqdatagridselectname[i] + ">>";
        }
        else
        {
            eqname = eqname + eqdatagridselectname[i];
        }
    }
    return eqname;
}
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
function searchKey()
{
    var searchkey = $('#searchkey').val();
    var selectNode = $('#eqtree').tree('getSelected');
    var eqID = $('#nowselectNode').val();
    $('#dg').datagrid('load', {
        eqID: eqID,
        searchkey: searchkey
    });
};
function addEqTree()
{
    var node = $('#eqtree').tree('getSelected');
    var eqParentID = getSelectEqId();
    if (eqParentID == 'N')
    {
        eqParentID = '0';
    }
    layer.open({
        type: 2,
        title: '添加设备',
        area: ['640px', '400px'],
        content: url_add + '&eqParentID=' + eqParentID
    });
};
function editEqTree()
{
    var eqid = getSelectEqId();
    if (eqid == 'N')
    {
        layer.alert("没有选择要修改的设备!", { icon: 0 });
        return;
    }
    layer.open({
        type: 2,
        title: '修改设备',
        area: ['640px', '400px'],
        content: url_edit + '&eqID=' + eqid
    });
};
function upEqTree()
{
    showUpDown('u');
};
function downEqTree()
{
    showUpDown('d');
};
function showUpDown(optype)
{
    var eqid;
    var selectedrow = $('#dg').datagrid('getSelected');
    if (selectedrow)
    {
        eqid = selectedrow.eqID;
    }
    else
    {
        var node = $('#eqtree').tree('getSelected');
        if (node == null)
        {
            layer.alert("没有选择要上线的设备!", { icon: 0 });
            return;
        }
        eqid = node.id;
    }
    layer.open({
        type: 2,
        title: "设备上线操作",
        area: ['300px', '300px'],
        content: url_updown + '&deltype=' + optype + '&eqid=' + eqid
    });
}
function moveEqTree()
{
    var eqid = getSelectEqId();
    if (eqid == 'N')
    {
        layer.alert("没有选择要移动的设备!", { icon: 0 });
        return;
    }
    layer.open({
        type: 2,
        title: "设备移动操作",
        area: ['500px', '415px'],
        content: url_move + '&eqID=' + eqid
    });
};
function crappedEqTree()
{
    var eqid = getSelectEqId();
    if (eqid == 'N')
    {
        layer.alert("没有选择要报废的设备!", { icon: 0 });
        return;
    }
    layer.open({
        type: 2,
        title: "设备报废操作",
        area: ['300px', '300px'],
        content: url_updown + '&deltype=c&eqid=' + eqid
    });
};
function recoverEqTree()
{
    var eqid = getSelectEqId();
    if (eqid == 'N')
    {
        layer.alert("没有选择要恢复的设备!", { icon: 0 });
        return;
    }
    layer.open({
        type: 2,
        title: "设备恢复操作",
        area: ['300px', '300px'],
        content: url_updown + '&deltype=r&eqid=' + eqid
    });
};
function selectroot()
{
    $('#eqtree').find('.tree-node-selected').removeClass('tree-node-selected');
    $("#dg").datagrid("getPanel").panel("setTitle", "当前选择设备：");
    $('#nowselectNode').val('0');
    searchKey();
};
function treeReload()
{
    $("#eqtree").tree("reload");
};
function stateformat(value, row, index)
{
    if (value == '0')
    {
        return '在线';
    }
    else if (value == '-1')
    {
        return '<font color="Tomato">报废</font>';
    }
    else if (value == "1")
    {
        return '<font color="Orange">离线</font>';
    }
    else
    {
        return '<font color="red">错误</font>';
    }
};
function openformat(value, row, index)
{
    if (value == 'open')
    {
        return '展开';
    }
    else
    {
        return '折叠';
    }
};
function getSelectEqId()
{
    var eqid;
    var selectedrow = $('#dg').datagrid('getSelected');
    if (selectedrow)
    {
        eqid = selectedrow.eqID;
    }
    else
    {
        var node = $('#eqtree').tree('getSelected');
        if (node)
        {
            eqid = node.id;
        }
        else
        {
            eqid = 'N';
        }
    }
    return eqid;
};
function formatid(value, row, index)
{
    return "<a class='eqinfo c1' style=\"width:100%\" onclick=\"showeqinfo(" + row.eqID + ")\">" + row.eqID + "</a > ";
};
function formatlinbutton(data)
{
    $(".eqinfo").linkbutton({ plain: true, iconCls: 'icon-tip' });
};
function showeqinfo(eqid)
{
    layer.open({
        type: 2,
        title: "设备详情浏览",
        area: ['60%', '470px'],
        content: url_eqinfo + '&eqid=' + eqid
    });
};
function passABC()
{
    var eqid = getSelectEqId();
    if (eqid == 'N')
    {
        layer.alert("没有选择要评级的设备!", { icon: 0 });
        return;
    }
    layer.open({
        type: 2,
        title: "设备评级操作",
        area: ['335px', '390px'],
        content: url_passabc + '&eqid=' + eqid
    });
};
function goback()
{
    if (eqgoback.length >= 2)
    {
        gogoeqid = eqgoback.pop();
        eqdatagridselectname.pop();
        $('#dg').datagrid('getPanel').panel('setTitle', '当前选择设备：' + getRowName());
        $('#dg').datagrid('load', {
            eqID: eqgoback[eqgoback.length - 1]
        });
    }
};