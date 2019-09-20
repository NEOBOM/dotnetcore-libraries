using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Library.Framework.JsonUtil
{
    public class ArrayConvert
    {

        public static List<List<string>> GenerateArray(string content)
        {
            Validate(content);

            return GetArrayWithCharArray(content);
        }

        public static List<List<String>> GenerateArray2(string content)
        {
            Validate(content);

            return GetArrayWithStringBuilder(content);
        }

        public static List<List<string>> GenerateArray3(string content)
        {
            Validate(content);

            return GetArrayWithSpan(content);
        }


        public static List<string[]> GenerateArray4(string content)
        {
            Validate(content);

            return JsonHelper.DeserializeObject<List<string[]>>(content);
        }

        public static List<List<string>> GetArrayWithCharArray(string content)
        {
            var list = new List<List<string>>();

            List<string> contentList = null;

            char [] caracters = null;
            int count = 0;

            int i = 0;

            int arrayIndex = 0;
            int separeteIndex = 0;

            foreach (var lether in content)
            {
                switch (lether)
                {
                    case '[':
                        if (contentList == null)
                            contentList = new List<string>();

                        i++;
                        break;
                    case ',':
                        if (caracters.Length > 0)
                        {
                            contentList.Add(new string(caracters));
                            caracters = null;
                            count = 0;
                        }

                        i++;
                        break;
                    case ']':
                        if (caracters != null)
                        {
                            contentList.Add(new string(caracters));
                            caracters = null;
                            count = 0;
                        }

                        if (contentList != null)
                            list.Add(contentList);

                        contentList = null;

                        i++;
                        break;
                    default:
                        if (caracters == null)
                        {
                            arrayIndex = content.IndexOf("]", i);
                            separeteIndex = content.IndexOf(",", i);

                            if (separeteIndex > 0 && separeteIndex < arrayIndex)
                                caracters = new char[separeteIndex - i];
                            else
                                caracters = new char[arrayIndex - i];
                        }

                        caracters[count] = lether;
                        count++;

                        i++;
                        break;
                }
            }

            return list;
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
                    case '[':
                        if(contentList == null)
                            contentList = new List<string>();

                        break;
                    case ',':
                        if (caracters.Length > 0)
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

        public static List<List<string>> GetArrayWithSpan(string content)
        {
            var list = new List<List<string>>();

            List<string> contentList = null;

            Span<char> caracters = null;
            int count = 0;

            int arrayIndex = 0;
            int separeteIndex = 0;

            int i = 0;

            foreach (var lether in content)
            {
                switch (lether)
                {
                    case '[':
                        if (contentList == null)
                            contentList = new List<string>();

                        i++;
                        break;
                    case ',':
                        if (caracters.Length > 0)
                        {
                            contentList.Add(caracters.ToString());
                            caracters = null;
                            count = 0;
                        }

                        i++;
                        break;
                    case ']':
                        if (caracters != null)
                        {
                            contentList.Add(caracters.ToString());
                            caracters = null;
                            count = 0;
                        }

                        if (contentList != null)
                            list.Add(contentList);

                        contentList = null;

                        i++;
                        break;

                    default:
                        if (caracters == null)
                        {
                            arrayIndex = content.IndexOf("]", i);
                            separeteIndex = content.IndexOf(",", i);

                            if (separeteIndex > 0 && separeteIndex < arrayIndex)
                                caracters = new Span<char>(new char[separeteIndex - i]);
                            else
                                caracters = new Span<char>(new char[arrayIndex - i]);
                        }

                        caracters[count] = lether;
                        count++;

                        i++;

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
