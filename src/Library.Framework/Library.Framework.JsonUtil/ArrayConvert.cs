using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Library.Framework.JsonUtil
{
    public class ArrayConvert
    {

        public static List<string> GenerateArray(string content)
        {
            Validate(content);

            return GetArrayWithCharArray(content);
        }

        public static List<String> GenerateArray2(string content)
        {
            Validate(content);

            return GetArrayWithStringBuilder(content);
        }

        public static List<String> GenerateArray3(string content)
        {
            Validate(content);

            return GetArrayWithSpan(content);
        }


        public static List<string[]> GenerateArray4(string content)
        {
            Validate(content);

            return JsonHelper.DeserializeObject<List<string[]>>(content);
        }

        public static List<string> GetArrayWithCharArray(string content)
        {
            var lethers = content.ToCharArray();

            var list = new List<string>();

            char [] caracters = null;
            int count = 0;

            int start = 0;
            int length = lethers.Length;

            if (content.StartsWith("[["))
            {
                start++;
                length--;
            }

            //for (int i = start; i < length; i++)
            //{
            //    switch (lethers[i])
            //    {
            //        case '[':
            //            if(caracters != null && count > 0)
            //            {
            //                list.Add(new string(caracters));
            //                caracters = null;
            //                count = 0;
            //            }

            //            if (caracters == null)
            //            {
            //                int arrayIndex = content.IndexOf("]", i);
            //                int separeteIndex = content.IndexOf(",", i);

            //                if (separeteIndex > 0 && separeteIndex < arrayIndex)
            //                    caracters = new char[(separeteIndex - i) - 1];
            //                else
            //                    caracters = new char[arrayIndex - i];
            //            }
            //            break;
            //        case ',':
            //        case ']':
            //            if(caracters != null)
            //            {
            //                list.Add(new string(caracters));
            //                caracters = null;
            //                count = 0;
            //            }
            //            break;
            //        default:
            //            if (caracters == null)
            //            {
            //                int arrayIndex = content.IndexOf("]", i);
            //                int separeteIndex = content.IndexOf(",", i);

            //                if (separeteIndex > 0 && separeteIndex < arrayIndex)
            //                    caracters = new char[separeteIndex - i];
            //                else
            //                    caracters = new char[arrayIndex - i];
            //            }

            //            caracters[count] = lethers[i];
            //            count++;

            //            break;
            //    }
            //}

            int i = start;

            foreach (var lether in content)
            {
                switch (lether)
                {
                    case '[':
                        if (caracters != null && count > 0)
                        {
                            list.Add(new string(caracters));
                            caracters = null;
                            count = 0;
                        }

                        if (caracters == null)
                        {
                            int arrayIndex = content.IndexOf("]", i);
                            int separeteIndex = content.IndexOf(",", i);

                            if (separeteIndex > 0 && separeteIndex < arrayIndex)
                                caracters = new char[(separeteIndex - i) - 1];
                            else
                                caracters = new char[arrayIndex - i];
                        }
                        break;
                    case ',':
                    case ']':
                        if (caracters != null)
                        {
                            list.Add(new string(caracters));
                            caracters = null;
                            count = 0;
                        }
                        break;
                    default:
                        if (caracters == null)
                        {
                            int arrayIndex = content.IndexOf("]", i);
                            int separeteIndex = content.IndexOf(",", i);

                            if (separeteIndex > 0 && separeteIndex < arrayIndex)
                                caracters = new char[separeteIndex - i];
                            else
                                caracters = new char[arrayIndex - i];
                        }

                        caracters[count] = lether;
                        count++;

                        break;
                }
            }

            return list;
        }

        public static List<string> GetArrayWithStringBuilder(string content)
        {
            var lethers = content.ToCharArray();

            var list = new List<string>();

            StringBuilder caracters = null;
            int count = 0;

            int start = 0;
            int length = lethers.Length;

            if (content.StartsWith("[["))
            {
                start++;
                length--;
            }

            for (int i = start; i < length; i++)
            {
                switch (lethers[i])
                {
                    case '[':

                        if (caracters != null && count > 0)
                        {
                            list.Add(caracters.ToString());
                            caracters = null;
                            count = 0;
                        }

                        if (caracters == null)
                        {
                            int arrayIndex = content.IndexOf("]", i);
                            int separeteIndex = content.IndexOf(",", i);

                            if (separeteIndex > 0 && separeteIndex < arrayIndex)
                                caracters =  new StringBuilder((separeteIndex - i) - 1);
                            else
                                caracters = new StringBuilder(arrayIndex - i);
                        }

                        break;

                    case ',':
                    case ']':
                        if (caracters != null)
                        {
                            list.Add(caracters.ToString());
                            caracters = null;
                            count = 0;
                        }

                        break;

                    default:

                        if (caracters == null)
                        {
                            int arrayIndex = content.IndexOf("]", i);
                            int separeteIndex = content.IndexOf(",", i);

                            if (separeteIndex > 0 && separeteIndex < arrayIndex)
                                caracters = new StringBuilder(separeteIndex - i);
                            else
                                caracters = new StringBuilder(arrayIndex - i);
                        }

                        caracters.Append(lethers[i]);
                        count++;
                        break;
                }
            }

            return list;
        }

        public static List<string> GetArrayWithSpan(string content)
        {
            var lethers = content.AsSpan();

            var list = new List<string>();

            Span<char> caracters = null;
            int count = 0;

            int start = 0;
            int length = lethers.Length;

            if (content.StartsWith("[["))
            {
                start++;
                length--;
            }

            for (int i = start; i < length; i++)
            {
                switch (lethers[i])
                {
                    case '[':

                        if (caracters != null && count > 0)
                        {
                            list.Add(caracters.ToString());
                            caracters = null;
                            count = 0;
                        }

                        if (caracters == null)
                        {
                            int arrayIndex = content.IndexOf("]", i);
                            int separeteIndex = content.IndexOf(",", i);

                            if (separeteIndex > 0 && separeteIndex < arrayIndex)
                                caracters = new Span<char>(new char[(separeteIndex - i) - 1]);
                            else
                                caracters = new Span<char>(new char[arrayIndex - i]);
                        }

                        break;

                    case ',':
                    case ']':
                        if (caracters != null)
                        {
                            list.Add(caracters.ToString());
                            caracters = null;
                            count = 0;
                        }

                        break;

                    default:

                        if (caracters == null)
                        {
                            int arrayIndex = content.IndexOf("]", i);
                            int separeteIndex = content.IndexOf(",", i);

                            if (separeteIndex > 0 && separeteIndex < arrayIndex)
                                caracters = new Span<char>(new char[separeteIndex - i]);
                            else
                                caracters = new Span<char>(new char[arrayIndex - i]);
                        }

                        caracters[count] = lethers[i];
                        count++;
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
