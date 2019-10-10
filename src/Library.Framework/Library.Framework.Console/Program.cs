using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Library.Framework.Console.Entity;
using Library.Framework.JsonUtil;
using System;
using System.Diagnostics;

namespace Library.Framework.Console
{
    //[MemoryDiagnoser]
    public class Program
    {
        private static readonly string _contentTest = "[[123154,4564564,654654564], [456485,8565445,222222], [333333,666666,444444]]";

        static void Main(string[] args)
        {
            //RunBenchMark();

            //var summarry = BenchmarkRunner.Run<Program>();
        }

        //[Benchmark]
        //public void ConvertCustomList() => ArrayConvert.ConvertCustomList<ObjectTest>(_contentTest);

        //[Benchmark]
        //public void DeserializeObject() => ArrayConvert.DeserializeObject(_contentTest);

        public static void RunBenchMark()
        {
            var sw = new Stopwatch();
            var before2 = GC.CollectionCount(2);
            var before1 = GC.CollectionCount(1);
            var before0 = GC.CollectionCount(0);

            sw.Start();
            for (int i = 0; i < 1_000_000; i++)
            {
                var result = ArrayConvert.ConvertCustomList<ObjectTest>(_contentTest);
                //var result = ArrayConvert.DeserializeObject(_contentTest);

                if (result == null || result.Count == 0)
                    throw new Exception("Error!");
            }
            sw.Stop();

            System.Console.WriteLine($"Tempo total: {sw.ElapsedMilliseconds}ms");
            System.Console.WriteLine($"GC Gen #2  : {GC.CollectionCount(2) - before2}");
            System.Console.WriteLine($"GC Gen #1  : {GC.CollectionCount(1) - before1}");
            System.Console.WriteLine($"GC Gen #0  : {GC.CollectionCount(0) - before0}");
            System.Console.WriteLine("Done!");
        }
    }
}
