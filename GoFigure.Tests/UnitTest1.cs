using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using GoFigure.App.Utils;
using GoFigure.App.Model.Settings;

namespace GoFigure.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var calculator = new Calculator();
            var gameSettings = new GameSettings();
            var svc = new SolutionGenerator(
                calculator,
                new SolutionComputer(calculator, gameSettings),
                gameSettings
            );

            var result = svc.Generate(1);

            Console.WriteLine("Wa");
        }
    }
}
