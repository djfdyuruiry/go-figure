using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using GoFigure.App.Utils;

namespace GoFigure.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var svc = new SolutionGenerator(new Calculator());

            var result = svc.Generate(1);

            Console.WriteLine("Wa");
        }
    }
}
