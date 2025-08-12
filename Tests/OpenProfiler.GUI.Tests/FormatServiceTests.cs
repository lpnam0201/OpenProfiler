using Ninject;
using NUnit.Framework;
using OpenProfiler.GUI.DI;
using OpenProfiler.GUI.Model;
using OpenProfiler.GUI.Service;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace OpenProfiler.GUI.Tests
{
    [TestFixture]
    public class FormatServiceTests
    {
        private IFormatService _formatService;

        [SetUp]
        public void Setup()
        {
            var kernel = new StandardKernel();
            kernel.Load<OpenProfilerGUIModule>();
            _formatService = kernel.Get<IFormatService>();
        }

        [Test]
        public void ApplyParameters__Test()
        {
            var sqlText = "select * from customer where name = @p1 and age = @p10 and birthday = @p2";
            var paramList = new List<SqlParameter>
            {
                new SqlParameter
                {
                    Name = "@p1",
                    Value = "'bar'"
                },
                new SqlParameter
                {
                    Name = "@p2",
                    Value = "2025-01-02"
                },
                new SqlParameter
                {
                    Name = "@p10",
                    Value = "1"
                }
            };
            var args = new object[]
            {
                sqlText,
                paramList
            };
            var applyParameterResult = _formatService.GetType()
                .GetMethod("ApplyParameters", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(_formatService, args) as string;
        }
    }
}