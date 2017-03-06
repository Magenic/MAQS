//--------------------------------------------------
// <copyright file="XBullet.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base Selenium test class</summary>
//--------------------------------------------------
using System;

namespace Magenic.MaqsFramework.Utilities.Extensions
{
    /// <summary>
    /// Extension methods to wrap Try Catch statements
    /// </summary>
    public static class XBullet
    {
        /// <summary>
        /// Allows you to return an injected type
        /// </summary>
        /// <typeparam name="T">the type to return</typeparam>
        /// <param name="code">call to code, to return the type when ready</param>
        /// <param name="error">Already handled Exception, return the type</param>
        /// <returns>The type requested</returns>
        public static T Proof<T>(Func<string, T> code, Func<Exception, T> error) where T : new()
        {
            var result = new T();
            Proof(() => { result = code("Code"); }, exception => { result = error(exception); });
            return result;
        }

        /// <summary>
        /// Wraps a try catch statement construct
        /// </summary>
        /// <param name="code">Callback to code</param>
        /// <param name="error">Callback if error happens (already handled)</param>
        public static void Proof(Action code, Action<Exception> error)
        {
            try
            {
                code();
            }
            catch (Exception x)
            {
                error(x);
            }
        }
    }
}