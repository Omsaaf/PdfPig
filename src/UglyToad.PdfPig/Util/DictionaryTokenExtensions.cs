﻿namespace UglyToad.PdfPig.Util
{
    using System;
    using Exceptions;
    using JetBrains.Annotations;
    using Tokens;

    internal static class DictionaryTokenExtensions
    {
        public static int GetInt(this DictionaryToken token, NameToken name)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (!token.TryGet(name, out var keyedToken) || !(keyedToken is NumericToken numeric))
            {
                throw new PdfDocumentFormatException($"The dictionary did not contain a number with the key {name}. Dictionary way: {token}.");
            }

            return numeric.Int;
        }

        public static int GetIntOrDefault(this DictionaryToken token, NameToken name, int defaultValue = 0)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (!token.TryGet(name, out var keyedToken) || !(keyedToken is NumericToken numeric))
            {
                return defaultValue;
            }

            return numeric.Int;
        }

        public static long? GetLongOrDefault(this DictionaryToken token, NameToken name)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (!token.TryGet(name, out var keyedToken) || !(keyedToken is NumericToken numeric))
            {
                return null;
            }

            return numeric.Long;
        }

        [CanBeNull]
        public static NameToken GetNameOrDefault(this DictionaryToken token, NameToken name)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (!token.TryGet(name, out var nameToken) || !(nameToken is NameToken result))
            {
                return null;
            }

            return result;
        }
    }

    internal static class ArrayTokenExtensions
    {
        public static NumericToken GetNumeric(this ArrayToken array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0 || index >= array.Data.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (array.Data[index] is NumericToken numeric)
            {
                return numeric;
            }

            throw new PdfDocumentFormatException();
        }
    }
}
