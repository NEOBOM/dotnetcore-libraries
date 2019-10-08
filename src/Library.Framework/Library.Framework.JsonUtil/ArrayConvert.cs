using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Library.Framework.JsonUtil
{
    public class ArrayConvert
    {
        public static List<T> ConvertCustomList<T>(string content) where T : new()
        {
            Validate(content);

            return GetArrayWithStringBuilder<T>(content);
        }

        public static List<object> DeserializeObject(string content)
        {
            Validate(content);

            return JsonHelper.DeserializeObject<List<object>>(content);
        }

        public static List<T> GetArrayWithStringBuilder<T>(string content) where T : new()
        {
            var list = new List<T>();

            T obj = default(T);

            PropertyInfo[] properties = null;

            int index = 0;

            var caracters = new StringBuilder();

            foreach (var lether in content)
            {
                if (obj != null && properties == null)
                    properties = obj.GetType().GetProperties();

                switch (lether)
                {
                    case ' ':
                        if (caracters.Length > 0)
                            caracters.Append(lether);

                        break;

                    case '[':

                        obj = new T();
                        index = 0;

                        break;
                    case ',':
                        if (caracters.Length > 0)
                        {
                            properties[index].SetValue(obj, caracters.ToString());
                            caracters.Clear();
                            index++;
                        }

                        break;
                    case ']':
                        if (caracters.Length > 0)
                        {
                            properties[index].SetValue(obj, caracters.ToString());
                            caracters.Clear();
                            
                            list.Add(obj);

                            index++;
                        }

                        break;

                    default:
                        caracters.Append(lether);
                        break;
                }
            }

            caracters = null;
            properties = null;

            return list;
        }

        public static void Validate(string content)
        {
            if (!content.Contains("[") || !content.Contains("]"))
                throw new Exception("Format array invalid.");
        }
    }
}
