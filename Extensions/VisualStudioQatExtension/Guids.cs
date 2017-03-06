// Guids.cs
// MUST match guids.h
using System;

namespace Magenic.MaqsFramework
{
    static class GuidList
    {
        public const string guidQATSeleniumPkgString = "7f8a17ec-cc72-413f-ae85-b3e1077f2d1f";
        public const string guidQATSeleniumCmdSetString = "f5cc9cd1-e986-4ee8-ba33-fa438db102a4";

        public static readonly Guid guidQATSeleniumCmdSet = new Guid(guidQATSeleniumCmdSetString);
    };
}