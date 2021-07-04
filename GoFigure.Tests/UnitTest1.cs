using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoFigure.App.Service;
using System;

namespace GoFigure.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var svc = new SolutionService();

            var result = svc.Generate();


            Console.WriteLine("Wa");
        }
    }
}
