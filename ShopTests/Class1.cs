using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NSubstitute;
using NUnit.Framework;
using Shop.Controllers;
using Shop.Logger;

namespace ShopTests
{
    [TestFixture]
    public class AdminTests
    {
        private ILogger logger;
        private IContainer container;

        [SetUp]
        public void SetUp()
        {
            ContainerBuilder builder = new ContainerBuilder();
            logger = Substitute.For<ILogger>();
            builder.Register<ILogger>(ctx => logger);
            builder.RegisterType<AdminController>();
            container = builder.Build();
        }

        [Test]
        public void Test1()
        {
            var c = container.Resolve<AdminController>();
            c.Administrator();
            logger.DidNotReceive().Debug(Arg.Any<string>());

        }
    }
}
