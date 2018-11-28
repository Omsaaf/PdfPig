﻿namespace UglyToad.PdfPig.Tests.Integration
{
    using Content;
    using Xunit;

    public class SwedishTouringCarChampionshipTests
    {
        private static string GetFilename()
        {
            return IntegrationHelpers.GetDocumentPath("2006_Swedish_Touring_Car_Championship.pdf");
        }

        [Fact]
        public void HasCorrectNumberOfPages()
        {
            using (var document = PdfDocument.Open(GetFilename()))
            {
                Assert.Equal(4, document.NumberOfPages);
            }
        }

        [Fact]
        public void HasCorrectVersion()
        {
            using (var document = PdfDocument.Open(GetFilename()))
            {
                Assert.Equal(1.4m, document.Version);
            }
        }

        [Fact]
        public void GetsFirstPageContent()
        {
            using (var document = PdfDocument.Open(GetFilename()))
            {
                var page = document.GetPage(1);

                Assert.Contains("A privateers championship named Caran Cup was created for drivers using cars constructed in 2003 or earlier", page.Text);

                Assert.Equal(PageSize.A4, page.Size);
            }
        }

        [Fact]
        public void GetsSwedishCharacters()
        {
            using (var document = PdfDocument.Open(GetFilename()))
            {
                var page = document.GetPage(2);

                Assert.Contains("Vålerbanen", page.Text);

                page = document.GetPage(3);

                Assert.Contains("Söderberg", page.Text);
            }
        }
    }
}
