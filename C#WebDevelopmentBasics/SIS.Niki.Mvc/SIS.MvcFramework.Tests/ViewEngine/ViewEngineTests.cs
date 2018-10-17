namespace SIS.MvcFramework.Tests.ViewEngine
{
    using System.IO;
    using System.Collections.Generic;
    using SIS.MvcFramework.ViewEngine.Contracts;
    using Xunit;

    public class ViewEngineTests
    {
        [Theory]
        [InlineData("ViewWithNoCode")]
        [InlineData("IfForAndForeach")]
        [InlineData("WorkWithViewModel")]
        public void RunTestViews(string testViewName)
        {
            var viewCode = File.ReadAllText($"TestViews/{testViewName}.html");
            var expectedResult = File.ReadAllText($"TestViews/{testViewName}.Result.html");
            IViewEngine viewEngine = new MvcFramework.ViewEngine.ViewEngine();
            var model = new TestModel
            {
                String = "Username",
                List = new List<string> { "Item1", "item2", "test", "123", "" },
            };

            var actualResult = viewEngine.GetHtml(testViewName, viewCode, model);
            Assert.Equal(expectedResult, actualResult);
        }

        public class TestModel
        {
            public string String { get; set; }

            public IEnumerable<string> List { get; set; }
        }
    }
}
