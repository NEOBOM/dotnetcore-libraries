using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Library.Framework.JsonUtil;
using System;

namespace Library.Framework.Console
{
    [MemoryDiagnoser]
    public class Program
    {
        private readonly string _contentTest = "[[123154,4564564,654654564]]";

        static void Main(string[] args)
        {
            //string str = "[[123154,4564564,654654564]]";

            //for (int i = 0; i < 10000; i++)
            //{
            //    //var result = ArrayConvert.GenerateArray(str);

            //    var result = ArrayConvert.GenerateArray2(str);

            //    foreach (var item in result)
            //    {
            //        System.Console.WriteLine($"Resultado {i} - valor {item}");
            //    }
            //}

            //System.Console.ReadLine();

            var summarry = BenchmarkRunner.Run<Program>();
        }

        [Benchmark]
        public void GenerateArray() => ArrayConvert.GenerateArray(_contentTest);

        [Benchmark]
        public void GenerateArray2() => ArrayConvert.GenerateArray2(_contentTest);

        [Benchmark]
        public void GenerateArray3() => ArrayConvert.GenerateArray3(_contentTest);

        [Benchmark]
        public void GenerateArray4() => ArrayConvert.GenerateArray4 (_contentTest);
    }
}
