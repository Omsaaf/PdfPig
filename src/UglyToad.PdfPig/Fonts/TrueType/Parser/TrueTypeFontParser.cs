﻿namespace UglyToad.PdfPig.Fonts.TrueType.Parser
{
    using System;
    using System.Collections.Generic;
    using Tables;
    using Util.JetBrains.Annotations;

    internal class TrueTypeFontParser
    {
        public TrueTypeFontProgram Parse(TrueTypeDataBytes data)
        {
            var version = (decimal)data.Read32Fixed();
            int numberOfTables = data.ReadUnsignedShort();

            // Read these data points to move to the correct data location.
            // ReSharper disable UnusedVariable
            int searchRange = data.ReadUnsignedShort();
            int entrySelector = data.ReadUnsignedShort();
            int rangeShift = data.ReadUnsignedShort();
            // ReSharper restore UnusedVariable

            var tables = new Dictionary<string, TrueTypeHeaderTable>();

            for (var i = 0; i < numberOfTables; i++)
            {
                var table = ReadTable(data);

                if (table.HasValue)
                {
                    tables[table.Value.Tag] = table.Value;
                }
            }

            var result = ParseTables(version, tables, data);

            return result;
        }

        [CanBeNull]
        private static TrueTypeHeaderTable? ReadTable(TrueTypeDataBytes data)
        {
            var tag = data.ReadTag();
            var checksum = data.ReadUnsignedInt();
            var offset = data.ReadUnsignedInt();
            var length = data.ReadUnsignedInt();

            // skip tables with zero length (except glyf)
            if (length == 0 && !string.Equals(tag, TrueTypeHeaderTable.Glyf))
            {
                return null;
            }

            return new TrueTypeHeaderTable(tag, checksum, offset, length);
        }

        private static TrueTypeFontProgram ParseTables(decimal version, IReadOnlyDictionary<string, TrueTypeHeaderTable> tables, TrueTypeDataBytes data)
        {
            var isPostScript = tables.ContainsKey(TrueTypeHeaderTable.Cff);

            var builder = new TableRegister.Builder();

            if (!tables.TryGetValue(TrueTypeHeaderTable.Head, out var table))
            {
                throw new InvalidOperationException($"The {TrueTypeHeaderTable.Head} table is required.");
            }

            // head
            builder.HeaderTable = HeaderTable.Load(data, table);

            if (!tables.TryGetValue(TrueTypeHeaderTable.Hhea, out var hHead))
            {
                throw new InvalidOperationException("The horizontal header table is required.");
            }

            // hhea
            builder.HorizontalHeaderTable = HorizontalHeaderTable.Load(data, hHead);

            if (!tables.TryGetValue(TrueTypeHeaderTable.Maxp, out var maxHeaderTable))
            {
                throw new InvalidOperationException("The maximum profile table is required.");
            }

            // maxp
            builder.MaximumProfileTable = BasicMaximumProfileTable.Load(data, maxHeaderTable);

            // post
            if (tables.TryGetValue(TrueTypeHeaderTable.Post, out var postscriptHeaderTable))
            {
                builder.PostScriptTable = PostScriptTable.Load(data, postscriptHeaderTable, builder.MaximumProfileTable);
            }
            
            if (!isPostScript)
            {
                if (!tables.TryGetValue(TrueTypeHeaderTable.Loca, out var indexToLocationHeaderTable))
                {
                    throw new InvalidOperationException("The location to index table is required for non-PostScript fonts.");
                }

                // loca
                builder.IndexToLocationTable =
                    IndexToLocationTable.Load(data, indexToLocationHeaderTable, builder);

                if (!tables.TryGetValue(TrueTypeHeaderTable.Glyf, out var glyphHeaderTable))
                {
                    throw new InvalidOperationException("The glyph table is required for non-PostScript fonts.");
                }

                // glyf
                builder.GlyphDataTable = GlyphDataTable.Load(data, glyphHeaderTable, builder);

                OptionallyParseTables(tables, data, builder);
            }

            return new TrueTypeFontProgram(version, tables, builder.Build());
        }

        private static void OptionallyParseTables(IReadOnlyDictionary<string, TrueTypeHeaderTable> tables, TrueTypeDataBytes data, TableRegister.Builder tableRegister)
        {
            // cmap
            if (tables.TryGetValue(TrueTypeHeaderTable.Cmap, out var cmap))
            {
                tableRegister.CMapTable = CMapTable.Load(data, cmap, tableRegister);
            }

            // hmtx
            if (tables.TryGetValue(TrueTypeHeaderTable.Hmtx, out var hmtxHeaderTable))
            {
                tableRegister.HorizontalMetricsTable = HorizontalMetricsTable.Load(data, hmtxHeaderTable, tableRegister);
            }

            // name
            if (tables.TryGetValue(TrueTypeHeaderTable.Name, out var nameHeaderTable))
            {
                // TODO: Not important
            }

            // os2

            // kern
            if (tables.TryGetValue(TrueTypeHeaderTable.Kern, out var kernHeaderTable))
            {
                tableRegister.KerningTable = KerningTable.Load(data, kernHeaderTable);
            }
        }
    }
}

