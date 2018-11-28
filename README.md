<image src="https://raw.githubusercontent.com/UglyToad/Pdf/master/documentation/pdfpig.png" width="128px" height="128px"/>

# PdfPig #


[![Build status](https://ci.appveyor.com/api/projects/status/ni7et2j2ml60pdi3?svg=true)](https://ci.appveyor.com/project/EliotJones/pdf)
[![codecov](https://codecov.io/gh/UglyToad/PdfPig/branch/master/graph/badge.svg)](https://codecov.io/gh/UglyToad/PdfPig)

This project allows users to read text content from PDF files.

This project aims to port [PDFBox](https://github.com/apache/pdfbox) to C#.

## Get Started ##

The simplest usage at this stage is to open a document, reading the words from every page:

    using (PdfDocument document = PdfDocument.Open(@"C:\Documents\document.pdf"))
    {
        for (var i = 0; i < document.NumberOfPages; i++)
        {
            // This starts at 1 rather than 0.
            var page = document.GetPage(i + 1);

            foreach (var word in page.GetWords())
            {
                Console.WriteLine(word.Text);
            }
        }
    }

## Installation ##

The package is available via the releases tab or from Nuget:

https://www.nuget.org/packages/PdfPig/

Or from the package manager console:

    > Install-Package PdfPig

While the version is below 1.0.0 minor versions will change the public API without warning (SemVer will not be followed until 1.0.0 is reached).

## API Changes ##

+ 0.0.3 - Changes to position data for ```Letter```. Letter has a Location, Width and GlyphRectangle property. Consult the [Wiki](https://github.com/UglyToad/PdfPig/wiki/Letters) for details of the new API. Adds ```PdfDocument.Structure``` property allowing access to raw data.

## Usage ##

The ```PdfDocument``` class provides access to the contents of a document loaded either from file or passed in as bytes. To open from a file use the ```PdfDocument.Open``` static method:

    using UglyToad.PdfPig;
    using UglyToad.PdfPig.Content;

    using (PdfDocument document = PdfDocument.Open(@"C:\my-file.pdf"))
    {
        int pageCount = document.NumberOfPages;

        Page page = document.GetPage(1);

        decimal widthInPoints = page.Width;
        decimal heightInPoints = page.Height;

        string text = page.Text;
    }
    
```PdfDocument``` should only be used in a ```using``` statement since it implements ```IDisposable``` (unless the consumer disposes of it elsewhere).

Since this is alpha software the consumer should wrap all access in a ```try catch``` block since it is extremely likely to throw exceptions. As a fallback you can try running PDFBox using [IKVM](https://www.ikvm.net/) or using [PDFsharp](http://www.pdfsharp.net) or by a native library wrapper using [docnet](https://github.com/GowenGit/docnet).

The document contains the version of the PDF specification it complies with, accessed by ```document.Version```:

    decimal version = document.Version;

### Document Information ###

The ```PdfDocument``` provides access to the document metadata as ```DocumentInformation``` defined in the PDF file. These tend not to be provided therefore most of these entries will be ```null```:

    PdfDocument document = PdfDocument.Open(fileName);

    // The name of the program used to convert this document to PDF.
    string producer = document.Information.Producer;

    // The title given to the document
    string title = document.Information.Title;
    // etc...

### Document Structure ###

New in 0.0.3 the document now has a Structure member:

    UglyToad.PdfPig.Structure structure = document.Structure;

This provides access to tokenized PDF document content:

    Catalog catalog = structure.Catalog;
    DictionaryToken pagesDictionary = catalog.PagesDictionary;

The pages dictionary is the root of the pages tree within a PDF document. The structure also exposes a ```GetObject(IndirectReference reference)```  method which allows random access to any object in the PDF as long as its identifier number is known. This is an identifier of the form ```69 0 R``` where 69 is the object number and 0 is the generation.

### Page ###
    
The ```Page``` contains the page width and height in points as well as mapping to the ```PageSize``` enum:

    PageSize size = Page.Size;
    
    bool isA4 = size == PageSize.A4;

```Page``` provides access to the text of the page:

    string text = page.Text;

There is a new (0.0.3) method which provides access to the words. This uses basic heuristics and is not reliable or well-tested:

    IEnumerable<Word> words = page.GetWords();

There is also an early access (0.0.3) API for retrieving the raw bytes of PDF image objects per page:

    IEnumerable<XObjectImage> images = page.ExperimentalAccess.GetRawImages();

This API will be changed in future releases.

### Letter ###

Due to the way a PDF is structured internally the page text may not be a readable representation of the text as it appears in the document. Since PDF is a presentation format, text can be drawn in any order, not necessarily reading order. This means spaces may be missing or words may be in unexpected positions in the text.

To help users resolve actual text order on the page, the ```Page``` file provides access to a list of the letters:


    IReadOnlyList<Letter> letters = page.Letters;

These letters contain:

+ The text of the letter: ```letter.Value```.
+ The location of the lower left of the letter: ```letter.Location```.
+ The width of the letter: ```letter.Width```.
+ The font size in unscaled relative text units (these sizes are internal to the PDF and do not correspond to sizes in pixels, points or other units): ```letter.FontSize```.
+ The name of the font used to render the letter if available: ```letter.FontName```.
+ A rectangle which is the smallest rectangle that completely contains the visible region of the letter/glyph: ```letter.GlyphRectangle```.

Letter position is measured in PDF coordinates where the origin is the lower left corner of the page. Therefore a higher Y value means closer to the top of the page.

At this stage letter position is experimental and **will change in future versions**! Do not rely on letter positions remaining constant between different versions of this package.

## Issues ##

At this stage the software is in Alpha. In order to proceed to Beta and production we need to see a wide variety of document types.

Please do file an issue if you encounter a bug.

However in order for us to assist you, you **must** provide the file which causes your issue. Please host this in a publically available place.

Issues on unplanned features are off topic for now and will probably be closed with a comment explaining roughly the importance on the road map.

## Status ##

*Why is class or property X internal?* With the exception of ```letter.Location``` and ```XObjectImage``` internal properties and classes are not stable enough for the end user yet. If you want to access them feel free to use reflection but be aware they may change or disappear between versions.

The initial version of this package aims only to support reading text content from unencrypted PDF files. Due to the legal and dependency consequences of decrypting, handling encrypted documents is not in scope.

An encrypted document will throw a ```NotSupportedException```.

We plan to eventually support writing PDFs as well as reading images, form objects and graphics from the PDF however these are future enhancements which do not feature in the first version.

Additionally most testing has taken place with Latin character sets. Due to the more complex way the PDF specification handles CJK (Chinese, Japanese and Korean) character sets these will probably not be handled correctly for now.

Please raise an issue (or preferably a pull request) if you're trying to read these documents however we may not get to it for a while depending on the volume of bugs.

## Credit ##

This project wouldn't be possible without the work done by the [PDFBox](https://pdfbox.apache.org/) team and the Apache Foundation. Any bugs in the code are entirely my fault.
