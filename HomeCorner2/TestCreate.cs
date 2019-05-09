using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace HomeCorner2
{
    public class TestCreate
    {
        public void Test() {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("localhost:51977/Houses");
            Thread.Sleep(5000);
            driver.Quit();
        }       
    }
}