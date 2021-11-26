using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace ConsoleLib.Data
{
    public static class Entitiy
    {
        public static string ToLine(this object data)
        {
            Type type = data.GetType();
            PropertyInfo[] properties = type.GetProperties();
            string builder = "";
            for(int i = 0; i<properties.Length; i++)
            {
                if (properties[i].PropertyType == typeof(DateTime))
                    builder+=((DateTime)properties[i].GetValue(data)).ToString("MM/dd/yyyy")+"#";
                else
                    builder+= properties[i].GetValue(data).ToString()+"#";
            }
            return builder.ToString().TrimEnd('#');
        }

        
    }
}
