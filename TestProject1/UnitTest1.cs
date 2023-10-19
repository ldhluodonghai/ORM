using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using System;
using System.Runtime.CompilerServices;

namespace TestProject1
{
    public class Tests
    {
        private readonly IMemoryCache memory;

        public Tests(IMemoryCache memory)
        {
            this.memory = memory;
        }

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test1()
        {

            memory.Set("a",  "b");

            long v = memory.Get<long>("a");
            Console.WriteLine(v);
        }
    }
}