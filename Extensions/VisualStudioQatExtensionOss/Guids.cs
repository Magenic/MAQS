// Guids.cs
// MUST match guids.h
using System;

namespace Magenic.MaqsFramework
{
    static class GuidList
    {
        public const string guidQATSeleniumPkgString = "7f8a17ec-cc72-413f-ae85-bbbbb77f2d1f";
        public const string guidQATSeleniumCmdSetString = "f5cc9cd1-eeee-eeee-ba33-fa438db102a4";

        public static readonly Guid guidQATSeleniumCmdSet = new Guid(guidQATSeleniumCmdSetString);
    };
}