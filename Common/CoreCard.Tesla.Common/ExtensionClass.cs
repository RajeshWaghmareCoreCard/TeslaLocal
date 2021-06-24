using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreCard.Tesla.Utilities
{
    public static class ExtensionClass
    {
        public static List<T> ConvertToObject<T>(this DataTable rd) where T : class, new()
        {
            Type type = typeof(T);
            List<T> list = new List<T>();

            PropertyInfo[] properties = type.GetProperties();

            var t = new T();


            foreach (DataRow row in rd.Rows)
            {
                T instance = Activator.CreateInstance<T>();
                for (int i = 0; i < rd.Columns.Count; i++)
                {
                    if (row.IsNull(rd.Columns[i]) == false)
                    {
                        properties.First(k => k.Name == rd.Columns[i].ColumnName).SetValue(instance, row[i]);
                    }
                    else
                    {
                        properties.First(k => k.Name == rd.Columns[i].ColumnName).SetValue(instance, default(T));
                    }
                }
                list.Add(instance);
            }
            return list;
        }
    }
}
