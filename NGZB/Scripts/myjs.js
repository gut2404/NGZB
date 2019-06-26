$(document).bind('contextmenu', function (e)
{
    if (enableRightBtn == '0')
    {
        e.preventDefault();//全文档阻止默认事件
    }
});
///以下datagrid 分页函数开始
function pagerFilter(data)
{
    if (typeof data.length == 'number' && typeof data.splice == 'function')
    {
        data = {
            total: data.length,
            rows: data
        };
    }
    var dg = $(this);
    var opts = dg.datagrid('options');
    var pager = dg.datagrid('getPager');
    pager.pagination({
        onSelectPage: function (pageNum, pageSize)
        {
            opts.pageNumber = pageNum;
            opts.pageSize = pageSize;
            pager.pagination('refresh', {
                pageNumber: pageNum,
                pageSize: pageSize
            });
            dg.datagrid('loadData', data);
        },
        onRefresh: function (pageNum, pageSize)
        {
            dg.datagrid('reload');
        }
    });
    if (!data.originalRows)
    {
        data.originalRows = (data.rows);
    }
    var start = (opts.pageNumber - 1) * parseInt(opts.pageSize);
    var end = start + parseInt(opts.pageSize);
    data.rows = (data.originalRows.slice(start, end));
    return data;
};
///datagrid 分页函数结束
//-----------------------华丽的分割线---------------------
///以下日期选择框格式化开始
function myformatterdate(date)
{
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    return y + '-' + (m < 10 ? ('0' + m) : m) + '-' + (d < 10 ? ('0' + d) : d);
};
function myparserdate(s)
{
    if (!s) return new Date();
    var ss = (s.split('-'));
    var y = parseInt(ss[0], 10);
    var m = parseInt(ss[1], 10);
    var d = parseInt(ss[2], 10);
    if (!isNaN(y) && !isNaN(m) && !isNaN(d))
    {
        return new Date(y, m - 1, d);
    } else
    {
        return new Date();
    }
};
///以上日期选择框格式化结束

var __errMsg = '操作失败,联系程序员！';
var __okMsg = '操作成功！';
var __dataMsg = '数据不全，无法操作！';
