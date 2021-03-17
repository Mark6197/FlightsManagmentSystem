using BL;
using BL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BL_Tests
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly log4net.ILog my_logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [TestMethod]
        public void TestMethod1()
        {
            my_logger.Error("unit test log");
            IAnonymousUserFacade anonymousUserFacade = new AnonymousUserFacade();
            anonymousUserFacade.GetFlightsByDepatrureDate(DateTime.Now);
        }
    }
}
