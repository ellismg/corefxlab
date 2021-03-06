﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Formatting;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace System.Text.Formatting.Tests
{
    public class PerfSmokeTests
    {
        const int numbersToWrite = 10000;
        static Stopwatch timer = new Stopwatch();

        const int itterationsInvariant = 300;
        const int itterationsCulture = 200;

        public void PrintTime([CallerMemberName] string memberName = "")
        {
            Console.WriteLine(String.Format("{0} : Elapsed {1}ms", memberName, timer.ElapsedMilliseconds));
        }

        [Fact]
        private void InvariantFormatIntDec()
        {
            timer.Restart();
            for (int itteration = 0; itteration < itterationsInvariant; itteration++)
            {
                StringFormatter sb = new StringFormatter(numbersToWrite);
                for (int i = 0; i < numbersToWrite; i++)
                {
                    sb.Append(((int)(i % 10)));
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        [Fact]
        private void InvariantFormatIntDecClr()
        {
            timer.Restart();
            for (int itteration = 0; itteration < itterationsInvariant; itteration++)
            {
                StringBuilder sb = new StringBuilder(numbersToWrite);
                for (int i = 0; i < numbersToWrite; i++)
                {
                    sb.Append(((int)(i % 10)));
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        [Fact]
        private void InvariantFormatIntHex()
        {
            timer.Restart();
            for (int itteration = 0; itteration < itterationsInvariant; itteration++)
            {
                StringFormatter sb = new StringFormatter(numbersToWrite);
                for (int i = 0; i < numbersToWrite; i++)
                {
                    sb.Append(((int)(i % 10)), Format.Parsed.HexUppercase);
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        [Fact]
        private void InvariantFormatIntHexClr()
        {
            timer.Restart();
            for (int itteration = 0; itteration < itterationsInvariant; itteration++)
            {
                StringBuilder sb = new StringBuilder(numbersToWrite);
                for (int i = 0; i < numbersToWrite; i++)
                {
                    sb.Append(((int)(i % 10)).ToString("X"));
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        [Fact]
        private void InvariantFormatStruct()
        {
            timer.Restart();
            for (int itteration = 0; itteration < itterationsInvariant; itteration++)
            {
                StringFormatter sb = new StringFormatter(numbersToWrite * 2);
                for (int i = 0; i < numbersToWrite; i++)
                {
                    sb.Append(new Age(i % 10));
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite * 2)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        [Fact]
        private void InvariantFormatStructClr()
        {
            timer.Restart();
            for (int itteration = 0; itteration < itterationsInvariant; itteration++)
            {
                StringBuilder sb = new StringBuilder(numbersToWrite * 2);
                for (int i = 0; i < numbersToWrite; i++)
                {
                    sb.Append(new Age(i % 10));
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite * 2)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        [Fact]
        private void CustomCultureFormat()
        {
            StringFormatter sb = new StringFormatter(numbersToWrite * 3);
            sb.FormattingData = CreateCustomCulture();

            timer.Restart();
            for (int itteration = 0; itteration < itterationsCulture; itteration++)
            {
                sb.Clear();
                for (int i = 0; i < numbersToWrite; i++)
                {
                    var next = (i % 128) + 101;
                    sb.Append(next);
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite * 3)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        [Fact]
        private void CustomCultureFormatClr()
        {
            StringBuilder sb = new StringBuilder(numbersToWrite * 3);
            var culture = new CultureInfo("th");

            timer.Restart();
            for (int itteration = 0; itteration < itterationsCulture; itteration++)
            {
                sb.Clear();
                for (int i = 0; i < numbersToWrite; i++)
                {
                    sb.Append(((i % 128) + 100).ToString(culture));
                }
                var text = sb.ToString();
                if (text.Length != numbersToWrite * 3)
                {
                    throw new Exception("test failed");
                }
            }
            PrintTime();
        }

        static FormattingData CreateCustomCulture()
        {
            var utf16digitsAndSymbols = new byte[13][];
            for (ushort digit = 0; digit < 10; digit++)
            {
                char digitChar = (char)(digit + 'A');
                var digitString = new string(digitChar, 1);
                utf16digitsAndSymbols[digit] = GetBytesUtf16(digitString);
            }
            utf16digitsAndSymbols[(ushort)FormattingData.Symbol.DecimalSeparator] = GetBytesUtf16(".");
            utf16digitsAndSymbols[(ushort)FormattingData.Symbol.GroupSeparator] = GetBytesUtf16(",");
            utf16digitsAndSymbols[(ushort)FormattingData.Symbol.MinusSign] = GetBytesUtf16("_?");
            return new FormattingData(utf16digitsAndSymbols, FormattingData.Encoding.Utf16);
        }
        static byte[] GetBytesUtf16(string text)
        {
            return System.Text.Encoding.Unicode.GetBytes(text);
        }
    }
}

