﻿namespace UglyToad.PdfPig.Tests.Fonts.Type1
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using PdfPig.Fonts.Type1.Parser;
    using PdfPig.IO;
    using Xunit;

    public class Type1FontParserTests
    {
        private readonly Type1FontParser parser = new Type1FontParser(new Type1EncryptedPortionParser());

        [Fact]
        public void CanReadHexEncryptedPortion()
        {
            var bytes = GetFileBytes("AdobeUtopia.pfa");

            parser.Parse(new ByteArrayInputBytes(bytes), 0, 0);
        }

        [Fact]
        public void CanReadBinaryEncryptedPortionOfFullPfb()
        {
            // TODO: support reading in these pfb files
            var bytes = GetFileBytes("Raleway-Black.pfb");

            parser.Parse(new ByteArrayInputBytes(bytes), 0, 0);
        }

        [Fact]
        public void CanReadCharStrings()
        {
            var bytes = GetFileBytes("CMBX10.pfa");

            parser.Parse(new ByteArrayInputBytes(bytes), 0, 0);
        }

        [Fact]
        public void CanReadEncryptedPortion()
        {
            var bytes = GetFileBytes("CMCSC10");

            parser.Parse(new ByteArrayInputBytes(bytes), 0, 0);
        }

        [Fact]
        public void CanReadAsciiPart()
        {
            var bytes = GetFileBytes("CMBX12");

            parser.Parse(new ByteArrayInputBytes(bytes), 0, 0);
        }

        [Fact]
        public void OutputCmbx10Svgs()
        {
            var bytes = GetFileBytes("CMBX10");

            var result = parser.Parse(new ByteArrayInputBytes(bytes), 0, 0);

            var builder = new StringBuilder("<!DOCTYPE html><html><head></head><body>");
            foreach (var charString in result.CharStrings.CharStrings)
            {
                var path = result.CharStrings.Generate(charString.Key);
                builder.AppendLine(path.ToFullSvg());
            }

            builder.Append("</body></html>");

            File.WriteAllText("cmbx10.html", builder.ToString());
        }

        [Fact]
        public void CanReadFontWithCommentsInOtherSubrs()
        {
            var bytes = GetFileBytes("CMR10");

            parser.Parse(new ByteArrayInputBytes(bytes), 0, 0);
        }

        private static byte[] GetFileBytes(string name)
        {
            var documentFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Fonts", "Type1"));
            var files = Directory.GetFiles(documentFolder);

            var file = files.FirstOrDefault(x => x.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);

            if (file == null)
            {
                throw new InvalidOperationException($"Could not find test file {name} in folder {documentFolder}.");
            }

            return File.ReadAllBytes(file);
        }
    }
}
