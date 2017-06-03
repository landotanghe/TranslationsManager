using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Partsunlimited.UITests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Remote;
    using OpenQA.Selenium.PhantomJS;
    using System;
    using OpenQA.Selenium.Support.UI;
    using TranslatorStore;

    [TestClass]
    public class ChucksClass1
    {
        

        [TestMethod]
        [TestCategory("Selenium")]
        [Priority(1)]
        [Owner("Chrome")]

        public void TireSearch_Any()
        {
            using (var translator = new GoogleTranslateApi())
            {
                translator.SetInput("hello");
                translator.UpdateTranslation();
                Assert.AreEqual("hallo", translator.LatestResult.ToLower());
                translator.Clear();
                translator.UpdateTranslation();
                Assert.AreEqual("", translator.LatestResult.ToLower());
            }
        }
    }
}
