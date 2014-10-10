using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerpetuumSoft.Knockout.Tests
{
    [TestClass]
    public class KnockoutUtilitiesTest
    {
        [TestMethod]
        public void ConvertDataTest01()
        {
            TestModel model = new TestModel();
            bool doesNotCauseStackOverflow = false;

            Knockout.KnockoutUtilities.ConvertData(model);
            
            doesNotCauseStackOverflow = true;
            Assert.IsTrue(doesNotCauseStackOverflow);
        }
    }
}
