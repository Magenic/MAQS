﻿using System;
using Magenic.Maqs.BaseAppiumTest;
using OpenQA.Selenium;
namespace Models.Appium
{
    /// <summary>
    /// Page object for AndroidHomePageModel inheriting from the AHomePageModel
    /// </summary>
    public class AndroidHomePageModel : AHomePageModel
    {
       /// <summary>
        /// The greeting message element 'By' finder
        /// </summary>
        protected override LazyMobileElement GreetingMessage
        {
            get { return new LazyMobileElement(this.TestObject, By.XPath("//android.widget.TextView[@resource-id='com.magenic.appiumtesting.maqsregistrydemo:id/welcomeLabel']"), "Welcome Label"); }
        }

        /// <summary>
        /// The time description element 'By' finder
        /// </summary>
        protected override LazyMobileElement TimeDisc
        {
            get { return new LazyMobileElement(this.TestObject, By.XPath("//android.widget.TextView[@resource-id='com.magenic.appiumtesting.maqsregistrydemo:id/timeDesc']"), "Timer Label"); }
        }

        /// <summary>
        /// The time element 'By' finder
        /// </summary>
        protected override LazyMobileElement Time
        {
            get { return new LazyMobileElement(this.TestObject, By.XPath("//android.widget.TextView[@resource-id='com.magenic.appiumtesting.maqsregistrydemo:id/time']"), "Timer"); }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidHomePageModel" /> class.
        /// </summary>
        /// <param name="testObject">The appium test object</param>
        public AndroidHomePageModel(AppiumTestObject testObject)
        {
            this.TestObject = testObject;
        }
    }
}