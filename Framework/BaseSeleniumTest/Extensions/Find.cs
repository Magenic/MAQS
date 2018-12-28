//--------------------------------------------------
// <copyright file="Find.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>This is the FindElements class</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace Magenic.Maqs.BaseSeleniumTest.Extensions
{
    /// <summary>
    /// General Element functions for finding and returning Web Elements
    /// </summary>
    public class Find
    {
        /// <summary>
        /// The search context item
        /// </summary>
        private readonly ISearchContext searchItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="Find"/> class.
        /// </summary>
        /// <param name="searchItem">The search context item</param>
        internal Find(ISearchContext searchItem)
        {
            this.searchItem = searchItem;
        }

        /// <summary>
        /// General Find Element
        /// </summary>
        /// <param name="by">Css Selector </param>
        /// <param name="assert">optional assert parameter - throws an assert exception if no element is found</param>
        /// <returns>Returns A Web Element</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="FindElement" lang="C#" />
        /// </example>
        public IWebElement Element(By by, bool assert = true)
        {
            // returns the 1st element in the collection if it is not null or empty
            ICollection<IWebElement> elementList = this.ElemList(by, assert);

            if (!elementList.Any())
            {
                return null;
            }

            IWebElement element = elementList.ElementAt(0);
            return element;
        }

        /// <summary>
        /// Find a specified Web Element by text
        /// </summary>
        /// <param name="by">Css Selector</param>
        /// <param name="text">Text to search the Web Element Collection</param>
        /// <param name="assert">optional assert parameter</param>
        /// <returns>Returns a Web Element</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="FindElementWithText" lang="C#" />
        /// </example>
        public IWebElement ElementWithText(By by, string text, bool assert = true)
        {
            // loop through elementList collection to find text match -- returns if found, else null
            ICollection<IWebElement> elementList = this.ElemList(by, assert);

            if (!elementList.Any())
            {
                return null;
            }

            for (int i = 0; i < elementList.Count(); i++)
            {
                if (elementList.ElementAt(i).Text.Equals(text))
                {
                    IWebElement element = elementList.ElementAt(i);
                    return element;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the index of the specified Web Element
        /// </summary>
        /// <param name="by">Css Selector</param>
        /// <param name="text">Text to search the Web Element Collection</param>
        /// <param name="assert">optional assert parameter</param>
        /// <returns>Returns the index of a Web Element Collection</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="FindIndexFromText" lang="C#" />
        /// </example>
        public int IndexOfElementWithText(By by, string text, bool assert = true)
        {
            // return -1 if index not found..  assert a fail if true
            ICollection<IWebElement> elementList = this.ElemList(by, assert);

            if (!elementList.Any())
            {
                return -1;
            }

            for (int i = 0; i < elementList.Count(); i++)
            {
                if (elementList.ElementAt(i).Text.Equals(text))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Find the Index of the Specified Web Element Collection
        /// </summary>
        /// <param name="list">ICollection of Web Elements</param>
        /// <param name="text">Text to search the Web Element Collection</param>
        /// <param name="assert">optional assert parameter</param>
        /// <returns>Returns the index of the Web Element in the inputted Web Element Collection </returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="FindIndexWithText" lang="C#" />
        /// </example>
        public int IndexOfElementWithText(ICollection<IWebElement> list, string text, bool assert = true)
        {
            // if list size was null or empty and assert was true
            if (!list.Any() && assert == true)
            {
                throw new NotFoundException(StringProcessor.SafeFormatter("Empty or null Element Collection passed in {0}", list?.ToString() ?? "NULL list"));
            }

            // if assert was true and list size > 0
            for (int i = 0; i < list.Count(); i++)
            {
                if (list.ElementAt(i).Text.Equals(text))
                {
                    return i;
                }
            }

            // if assert is  == false and no match was found
            if (assert == false)
            {
                return -1;
            }

            // if assert is == true and no match was found
            throw new NotFoundException(StringProcessor.SafeFormatter("Text did not match any element in collection {0}", list.ToString()));
        }

        /// <summary>
        /// Function to get the Web Collection 
        /// </summary>
        /// <param name="by">Css Selector</param>
        /// <param name="assert">optional assert parameter</param>
        /// <returns> Returns a Web Element Collection</returns>
        private ICollection<IWebElement> ElemList(By by, bool assert = true)
        {
            ICollection<IWebElement> elems = this.searchItem.FindElements(by);

            if (elems.Count > 0 || assert == false)
            {
                return elems;
            }

            throw new NotFoundException(StringProcessor.SafeFormatter("No result found for By {0}", by.ToString()));
        }
    }
}
