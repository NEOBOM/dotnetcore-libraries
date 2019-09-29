using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Library.Framework.JsonUtil
{
    public class ArrayConvert
    {
        public static List<List<String>> ConvertCustomList(string content)
        {
            Validate(content);

            return GetArrayWithStringBuilder(content);
        }

        public static List<object> DeserializeObject(string content)
        {
            Validate(content);

            return JsonHelper.DeserializeObject<List<object>>(content);
        }

        public static List<object> Deserialize(string content)
        {
            Validate(content);

            return JsonSerializer.Deserialize<List<object>>(content);
        }

        public static List<List<string>> GetArrayWithStringBuilder(string content)
        {
            var list = new List<List<string>>();

            List<string> contentList = null;

            var caracters = new StringBuilder();

            foreach (var lether in content)
            {
                switch (lether)
                {
                    case ' ':
                        if (caracters.Length > 0)
                            caracters.Append(lether);

                        break;

                    case '[':
                        if (contentList == null)
                            contentList = new List<string>();

                        break;
                    case ',':
                        if (contentList != null)
                        {
                            contentList.Add(caracters.ToString());
                            caracters.Clear();
                        }

                        break;
                    case ']':
                        if (caracters.Length > 0)
                        {
                            contentList.Add(caracters.ToString());
                            caracters.Clear();
                        }

                        if (contentList != null)
                            list.Add(contentList);

                        contentList = null;

                        break;

                    default:
                        caracters.Append(lether);
                        break;
                }
            }

            return list;
        }

        public static void Validate(string content)
        {
            if (!content.Contains("[") || !content.Contains("]"))
                throw new Exception("Format array invalid.");

        }
    }
}
