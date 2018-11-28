﻿namespace UglyToad.PdfPig.Content
{
    using Geometry;

    /// <summary>
    /// A glyph or combination of glyphs (characters) drawn by a PDF content stream.
    /// </summary>
    public class Letter
    {
        /// <summary>
        /// The text for this letter or unicode character.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// The placement position of the character in PDF space.
        /// </summary>
        public PdfPoint Location { get; }

        /// <summary>
        /// The width occupied by the character within the PDF content.
        /// </summary>
        public decimal Width { get; }

        /// <summary>
        /// Position of the bounding box for the glyph, this is the box surrounding the visible glyph as it appears on the page.
        /// For example letters with descenders, p, j, etc., will have a box extending below the <see cref="Location"/> they are placed at.
        /// The width of the glyph may also be more or less than the <see cref="Width"/> allocated for the character in the PDF content.
        /// </summary>
        public PdfRectangle GlyphRectangle { get; }

        /// <summary>
        /// Size as defined in the PDF file. This is not equivalent to font size in points but is relative to other font sizes on the page.
        /// </summary>
        public decimal FontSize { get; }

        /// <summary>
        /// The name of the font.
        /// </summary>
        public string FontName { get; }

        /// <summary>
        /// The size of the font in points. This is not ready for public consumption as the calculation is incorrect.
        /// </summary>
        internal decimal PointSize { get; }

        /// <summary>
        /// Create a new letter to represent some text drawn by the Tj operator.
        /// </summary>
        internal Letter(string value, PdfRectangle glyphRectangle, PdfPoint location, decimal width, decimal fontSize, string fontName, decimal pointSize)
        {
            Value = value;
            GlyphRectangle = glyphRectangle;
            FontSize = fontSize;
            FontName = fontName;
            PointSize = pointSize;
            Location = location;
            Width = width;
        }

        /// <summary>
        /// Produces a string representation of the letter and its position.
        /// </summary>
        public override string ToString()
        {
            return $"{Value} {Location} {FontName} {PointSize}";
        }
    }
}
