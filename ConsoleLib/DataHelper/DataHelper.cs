using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleLib.Interfaces;
using System.IO;
using System.Reflection;
namespace ConsoleLib.Data
{
    public class DataHelper : IDataHelper
    {
        public List<T> Convert<T>(List<string> data)
        {
            try
            {
                List<T> result = new List<T>();
                Type type = typeof(T);
                for(int i = 0; i<data.Count; i++)
                {
                    object newItem = Activator.CreateInstance(typeof(T));
                    PropertyInfo[] properties = type.GetProperties();
                    string[] line = data[i].Split("#");
                    for(var j = 0; j<properties.Length; j++)
                    {
                        if (properties[j].PropertyType == typeof(DateTime))
                            properties[j].SetValue(newItem, DateTime.Parse(line[j]));
                        else if (properties[j].PropertyType == typeof(string))
                            properties[j].SetValue(newItem, line[j]);
                        else
                        {
                            object val = line[j];
                            properties[j].SetValue(newItem, System.Convert.ChangeType(val, properties[j].PropertyType)); 
                        }
                    }
                    result.Add((T)newItem);
                }
                return result;
            } catch(Exception e)
            {
                return null;
            }
        }

        public string CreateID()
        {
            Guid guid =  Guid.NewGuid();
            return guid.ToString();
        }

        public List<string> ReadFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                    File.Create(path).Close();
                List<string> builder = new List<string>();
                using (StreamReader reader = new StreamReader(path))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        builder.Add(line);
                    }
                    reader.Close();
                    return builder;
                }
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool WriteFile<T>(string path, List<T> data)
        {
            try
            {
                if (!File.Exists(path))
                    File.Create(path).Close();
                using (StreamWriter writer = new StreamWriter(path))
                {
                    for (int i = 0; i < data.Count; i++)
                        writer.WriteLine(data[i].ToLine());
                    writer.Flush();
                    writer.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
