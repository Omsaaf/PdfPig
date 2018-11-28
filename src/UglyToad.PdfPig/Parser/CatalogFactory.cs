﻿namespace UglyToad.PdfPig.Parser
{
    using System;
    using Content;
    using Exceptions;
    using Parts;
    using Tokenization.Scanner;
    using Tokens;

    internal class CatalogFactory
    {
        public Catalog Create(IPdfTokenScanner scanner, DictionaryToken dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (dictionary.TryGet(NameToken.Type, out var type) && !ReferenceEquals(type, NameToken.Catalog))
            {
                throw new PdfDocumentFormatException($"The type of the catalog dictionary was not Catalog: {dictionary}.");
            }

            if (!dictionary.TryGet(NameToken.Pages, out var value))
            {
                throw new PdfDocumentFormatException($"No pages entry was found in the catalog dictionary: {dictionary}.");
            }

            var pages = DirectObjectFinder.Get<DictionaryToken>(value, scanner);
            
            return new Catalog(dictionary, pages);
        }
    }
}
