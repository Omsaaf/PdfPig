﻿namespace UglyToad.Pdf.Tests.Fonts.Parser
{
    using Pdf.Fonts.Parser;
    using Xunit;

    public class CMapParserTests
    {
        private const string GoogleDocToUnicodeCmap = @"/CIDInit /ProcSet findresource begin
12 dict begin
begincmap
/CIDSystemInfo
<<  /Registry (Adobe)
/Ordering (UCS)
/Supplement 0
>> def
/CMapName /Adobe-Identity-UCS def
/CMapType 2 def
1 begincodespacerange
<0000> <FFFF>
endcodespacerange
6 beginbfchar
<0003> <0020>
<0011> <002E>
<0024> <0041>
<0037> <0054>
<0044> <0061>
<005B> <0078>
endbfchar
4 beginbfrange
<0046> <0049> <0063>
<004B> <004C> <0068>
<004F> <0052> <006C>
<0055> <0058> <0072>
endbfrange
endcmap
CMapName currentdict /CMap defineresource pop
end
end";

        private readonly CMapParser cMapParser = new CMapParser(); 

        [Fact]
        public void CanParseCidSystemInfoAndOtherInformation()
        {
            var input = StringBytesTestConverter.Convert(GoogleDocToUnicodeCmap, false);

            var cmap = cMapParser.Parse(input.Bytes, false);

            Assert.Equal("Adobe", cmap.Info.Registry);
            Assert.Equal("UCS", cmap.Info.Ordering);
            Assert.Equal(0, cmap.Info.Supplement);

            Assert.Equal("Adobe-Identity-UCS", cmap.Name);
            Assert.Equal(2, cmap.Type);
        }

        [Fact]
        public void CanParseCodespaceRange()
        {
            var input = StringBytesTestConverter.Convert(GoogleDocToUnicodeCmap, false);

            var cmap = cMapParser.Parse(input.Bytes, false);

            Assert.Equal(1, cmap.CodespaceRanges.Count);

            Assert.Equal(0, cmap.CodespaceRanges[0].StartInt);
            Assert.Equal(65535, cmap.CodespaceRanges[0].EndInt);
            Assert.Equal(2, cmap.CodespaceRanges[0].CodeLength);
        }

        [Fact]
        public void CanParseBaseFontCharacters()
        {
            var input = StringBytesTestConverter.Convert(GoogleDocToUnicodeCmap, false);

            var cmap = cMapParser.Parse(input.Bytes, false);

            Assert.True(cmap.BaseFontCharacterMap.Count >= 6);

            Assert.Equal(" ", cmap.BaseFontCharacterMap[3]);
            Assert.Equal(".", cmap.BaseFontCharacterMap[17]);
            Assert.Equal("A", cmap.BaseFontCharacterMap[36]);
            Assert.Equal("T", cmap.BaseFontCharacterMap[55]);
            Assert.Equal("a", cmap.BaseFontCharacterMap[68]);
            Assert.Equal("x", cmap.BaseFontCharacterMap[91]);
        }
    }
}