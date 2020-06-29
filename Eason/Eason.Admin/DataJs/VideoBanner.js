$(function () {

    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();

});

var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tb_informations').bootstrapTable({
            url: '/VideoBanner/List',         //请求后台的URL（*）
            method: 'get',                      //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: true,                  //是否显示所有的列
            showRefresh: true,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "id",                     //每一行的唯一标识，一般为主键列
            showToggle: true,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            columns:
            [
                {
                    checkbox: true
                },
                {
                    field: 'title',
                    title: '标题',
                    editable: true,
                },
                {
                    field: 'bpic',
                    title: '图片地址'
                },
                {
                    field: 'burl',
                    title: '链接地址'
                },
                  {
                      field: 'isActive',
                      title: '操作',
                      formatter: function (value, row, index) {
                          var c = " <a href='../videobanner/delete?id=" + row.id + "'>删除</a>";
                          var a = " <a href='../videobanner/Edit?id=" + row.id + "'>编辑</a>";
                          return  a+ c;

                      },
                  },

            ]
        });
    };

    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            limit: params.limit,   //页面大小
             offset: params.offset/10,  //页码
            search: $("#txt_search").val()
        };
        return temp;
    };
    return oTableInit;
};


var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};

    oInit.Init = function () {
        //初始化页面上面的按钮事件
    };

    return oInit;
};

$(function () {
    var $table = $('#tb_informations'),
        $delete = $('#btn_delete');
    $add = $('#btn_add');
    $update = $('#btn_edit');
    $btn_no_active = $('#btn_no_active');
    $btn_active = $('#btn_active');

    $btn_query = $('#btn_query');
    $delete.click(function () {
        if (confirm('确认需要进行删除操作吗？该操作不可恢复')) {
            var items = $table.bootstrapTable('getAllSelections');
            //alert(JSON.stringify(items));
            var ids = '';
            $.each(items, function (index, data) {
                ids += data.id + ",";
            });
            if (ids != '') {

                ids = ids.substring(0, ids.length - 1);
                $.post("/VideoBanner/Delete",
                    {
                        strid: ids,
                    },
                    function (data, status) {
                        $table.bootstrapTable('refresh')

                    });
            }
            else {
                alert("请先勾选数据在操作哦");
            }
        }

    });
    $update.click(function () {
        if (confirm('确认需要修改用户信息吗？')) {
            var items = $table.bootstrapTable('getAllSelections');
            //alert(JSON.stringify(items));
            var ids = 0;
            $.each(items, function (index, data) {
                ids = data.id;
            });
            if (ids != '') {
                window.location.href = "/VideoBanner/Edit?id=" + ids;
            }
            else {
                alert("请先勾选数据在操作哦");
            }
        }

    });
    $add.click(function () {

        window.location.href = "/VideoBanner/Create";

    });
    $btn_no_active.click(function () {
        if (confirm('确认需要进行限制该用户登陆吗？')) {
            var items = $table.bootstrapTable('getAllSelections');
            //alert(JSON.stringify(items));
            var ids = '';
            $.each(items, function (index, data) {
                ids += data.id + ",";
            });
            if (ids != '') {

                ids = ids.substring(0, ids.length - 1);
                $.post("/VideoBanner/Active",
                    {
                        strid: ids,
                        status: 1
                    },
                    function (data, status) {
                        $table.bootstrapTable('refresh')

                    });
            }
            else {
                alert("请先勾选数据在操作哦");
            }
        }

    });
    $btn_active.click(function () {
        if (confirm('确认允许该用户登陆?')) {
            var items = $table.bootstrapTable('getAllSelections');
            //alert(JSON.stringify(items));
            var ids = '';
            $.each(items, function (index, data) {
                ids += data.id + ",";
            });
            if (ids != '') {

                ids = ids.substring(0, ids.length - 1);
                $.post("/VideoBanner/Active",
                    {
                        strid: ids,
                        status: 0
                    },
                    function (data, status) {
                        $table.bootstrapTable('refresh')

                    });
            }
            else {
                alert("请先勾选数据在操作哦");
            }
        }

    });
    $btn_query.click(function () {
        $table.bootstrapTable('destroy');
        var oTable = new TableInit();
        oTable.Init();
        //2.初始化Button的点击事件
        var oButtonInit = new ButtonInit();
        oButtonInit.Init();
        // TableObj.oTableInit();
        // $table.bootstrapTable('refresh');
    });
});