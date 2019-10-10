using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Library.Framework.JsonUtil
{
    public class ArrayConvert
    {
        public static T ConvertCustoObject<T>(string content) where T : new()
        {
            ValidateForObject(content);

            return CreateObjectByString<T>(content);
        }

        public static List<T> ConvertCustomList<T>(string content) where T : new()
        {
            ValidateForList(content);

            return CreateListObjectByString<T>(content);
        }

        private static T CreateObjectByString<T>(string content) where T : new()
        {
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

                        if (obj == null || index > 0)
                        {
                            obj = new T();
                            index = 0;
                        }

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

            return obj;
        }

        private static List<T> CreateListObjectByString<T>(string content) where T : new()
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

                        if(obj == null || index > 0)
                        {
                            obj = new T();
                            index = 0;
                        }

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

        private static void ValidateForObject(string content)
        {
            if (!content.Contains("[") || !content.Contains("]"))
                throw new Exception("Format array invalid.");
        }

        private static void ValidateForList(string content)
        {
            if (!content.Contains("[[") || !content.Contains("]]"))
                throw new Exception("Format array invalid.");
        }
    }
}
