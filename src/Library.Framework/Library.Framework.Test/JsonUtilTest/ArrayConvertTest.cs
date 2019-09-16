using Library.Framework.JsonUtil;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Library.Framework.Test.JsonUtilTest
{
    public class ArrayConvertTest
    {
        [Fact]
        public void Convert_Json_With_Simple_Array_To_Array()
        {
            string str = "[[1,2]]";

            var result = ArrayConvert.GenerateArray(str);

            Assert.NotNull(result);
            Assert.True(result.Count > 0) ;
        }

        [Fact]
        public void Convert_Json_With_Simple_Array_To_Array2()
        {
            string str = "[[18,2]]";

            var result = ArrayConvert.GenerateArray2(str);

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void Convert_Json_With_Simple_Array_To_Array3()
        {
            string str = "[[18,2]]";

            var result = ArrayConvert.GenerateArray3(str);

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact]
        public void Convert_Json_With_JsonHelper()
        {
            string str = "[[18,2]]";

            var result = ArrayConvert.GenerateArray4(str);

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }
    }

}
