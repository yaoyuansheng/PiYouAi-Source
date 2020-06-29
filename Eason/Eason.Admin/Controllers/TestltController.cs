using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Eason.Admin.Controllers
{
    [Authorize(Roles = "Administrots")]
    public class TestltController : Controller
    {
        public class MapList
        {
            public long id { get; set; }
            public string[] position { get; set; }
            public string position_x { get; set; }
            public string position_y { get; set; }
            public string markerLabel { get; set; }
            public string infoWinContent { get; set; }
            public string title { get; set; }
            public string telphone { get; set; }
            public string address { get; set; }
            public string image { get; set; }


        }
        // GET: Testlt
        public string Index(string search)
        {
            string sql = "select * from mapList where title like '%" + search + "%'";
            using (var conn = new SqlConnection("Server=.;Database=EasonDb; User ID=sa; Password=1q2w3e4r..;Connect Timeout=10;"))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);    //填充DataSet
                var result = DataSetToIList<MapList>(ds, 0);
                if (result != null && result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        item.position = new string[2] { item.position_x, item.position_y };
                    };
                }
                return JsonConvert.SerializeObject(result);
            }
            return null;
        }

        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="p_DataSet">DataSet</param> 
        /// <param name="p_TableIndex">待转换数据表索引</param> 
        /// <returns></returns> 
        /// 2008-08-01 22:46 HPDV2806 
        public static IList<T> DataSetToIList<T>(DataSet p_DataSet, int p_TableIndex)
        {
            if (p_DataSet == null || p_DataSet.Tables.Count < 0)
                return null;
            if (p_TableIndex > p_DataSet.Tables.Count - 1)
                return null;
            if (p_TableIndex < 0)
                p_TableIndex = 0;

            DataTable p_Data = p_DataSet.Tables[p_TableIndex];
            // 返回值初始化 
            IList<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理 
                            if (p_Data.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, p_Data.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }
    }
}