﻿namespace UglyToad.PdfPig.Tests.Tokenization
{
    using PdfPig.Tokenization;
    using PdfPig.Tokens;
    using Xunit;

    public class HexTokenizerTests
    {
        private readonly HexTokenizer tokenizer = new HexTokenizer();

        [Theory]
        [InlineData(">not hex")]
        [InlineData("\\<not hex")]
        [InlineData("not hex")]
        [InlineData("AE1094 still not hex")]
        public void CannotTokenizeInvalidBytes(string s)
        {
            var input = StringBytesTestConverter.Convert(s);

            var result = tokenizer.TryTokenize(input.First, input.Bytes, out var token);

            Assert.False(result);
            Assert.Null(token);
        }

        [Theory]
        [InlineData("<00>", "")]
        [InlineData("<A1>", "¡")]
        public void TokenizesHexStringsCorrectly(string s, string expected)
        {
            var input = StringBytesTestConverter.Convert(s);

            var result = tokenizer.TryTokenize(input.First, input.Bytes, out var token);

            Assert.True(result);

            Assert.Equal(expected, AssertHexToken(token).Data);
        }

        private static HexToken AssertHexToken(IToken token)
        {
            Assert.NotNull(token);

            var hexToken = Assert.IsType<HexToken>(token);

            return hexToken;
        }
    }
}
