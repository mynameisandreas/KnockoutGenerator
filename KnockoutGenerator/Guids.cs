// Guids.cs
// MUST match guids.h
using System;

namespace AndreasGustafsson.KnockoutGenerator
{
    static class GuidList
    {
        public const string guidKnockoutGeneratorPkgString = "59098c95-a57d-45a2-93b8-43172913df80";
        public const string guidKnockoutGeneratorCmdSetString = "7a36f6e6-41ba-486e-bea9-8bd6ccd3832c";
        public const string guidKnockoutGeneratorCopyToClipBoardCodeWindowCmdString = "40abf7d3-4c94-4a44-a124-294510e17549";
        public const string guidKnockoutGeneratorCodeWindowCmdSetString = "1cd2d0d0-2bb6-4de0-bc16-755dbc86a099";

        public static readonly Guid guidKnockoutGeneratorCodeWindowCmdSet = new Guid(guidKnockoutGeneratorCodeWindowCmdSetString);
        public static readonly Guid guidKnockoutGeneratorCopyToClipBoardCodeWindowCmdSet = new Guid(guidKnockoutGeneratorCopyToClipBoardCodeWindowCmdString);
        public static readonly Guid guidKnockoutGeneratorCmdSet = new Guid(guidKnockoutGeneratorCmdSetString);


    };
}