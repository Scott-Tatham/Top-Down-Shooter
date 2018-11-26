/// <summary>
/// A collection of useful code pieces.
/// </summary>
namespace LazyTitan
{
    /// <summary>
    /// Extensions.
    /// </summary>
    namespace Extensions
    {
        using UnityEngine;

        /// <summary>
        /// Extensions that work with Rich Text.
        /// </summary>
        public static class RichTextExtentions
        {
            /// <summary>
            /// Bolds a string.
            /// </summary>
            /// <param name="val"> The input string. </param>
            /// <returns> The string with bold applied. </returns>
            public static string Bold(this string val)
            {
                return "<b>" + val + "</b>";
            }

            /// <summary>
            /// Italics a string.
            /// </summary>
            /// <param name="val"> The input string. </param>
            /// <returns> The string with italics applied. </returns>
            public static string Italics(this string val)
            {
                return "<i>" + val + "</i>";
            }

            /// <summary>
            /// Change the size of string font to the specified size.
            /// </summary>
            /// <param name="val"> The input string. </param>
            /// <param name="size"> The resized string font. </param>
            /// <returns></returns>
            public static string Size(this string val, int size)
            {
                return "<size=" + size + ">" + val + "</size>";
            }

            /// <summary>
            /// Colour a string with the specified colour.
            /// </summary>
            /// <param name="val"> The input string. </param>
            /// <param name="color"> The specified colour. </param>
            /// <returns> The colourised string. </returns>
            public static string Colour(this string val, Color colour)
            {
                return "<color=#" + ColorUtility.ToHtmlStringRGBA(colour) + ">" + val + "</color>";
            }

            /// <summary>
            /// Colour a string with the specified colour.
            /// </summary>
            /// <param name="val"> The input string. </param>
            /// <param name="hexColour"> The specified colour. </param>
            /// <returns> The colourised string. </returns>
            public static string Colour(this string val, string hexColour)
            {
                return "<color=" + hexColour + ">" + val + "</color>";
            }
        }

        /// <summary>
        /// Extensions that manipulate strings.
        /// </summary>
        public static class StringExtentions
        {

            #region Public Methods

            /// <summary>
            /// Creates a pretty print of the string, useful for JSON.
            /// </summary>
            /// <param name="val"> The input string. </param>
            /// <returns> The neatened string. </returns>
            public static string Neaten(this string val)
            {
                int indent = 0;
                string newVal = "";

                for (int i = 0; i < val.Length; i++)
                {
                    if (val[i] == '{' || val[i] == '[')
                    {
                        if (i != 0)
                        {
                            if (val[i - 1] != '{' && val[i - 1] != '[')
                            {
                                newVal = newVal + "\n" + Indent(indent);
                            }
                        }

                        indent++;
                        newVal = newVal + val[i] + "\n" + Indent(indent);
                    }

                    else if (val[i] == '}' || val[i] == ']')
                    {
                        indent--;
                        newVal = newVal + "\n" + Indent(indent) + val[i];
                    }

                    else if (val[i] == ',')
                    {
                        newVal = newVal + val[i] + "\n" + Indent(indent);
                    }

                    else if (val[i] == ':' && val[i + 1] != '{' && val[i + 1] != '[')
                    {
                        newVal = newVal + val[i] + " ";
                    }

                    else
                    {
                        newVal = newVal + val[i];
                    }
                }

                return newVal;
            }

            /// <summary>
            /// Returns a number in word form.
            /// </summary>
            /// <param name="val"> Number to convert. </param>
            /// <returns> Number word. </returns>
            public static string NumberNames(this int val)
            {
                switch (val)
                {
                    case 0:

                        return "Zero";

                    case 1:

                        return "One";

                    case 2:

                        return "Two";

                    case 3:

                        return "Three";

                    case 4:

                        return "Four";

                    case 5:

                        return "Five";

                    case 6:

                        return "Six";

                    case 7:

                        return "Seven";

                    case 8:

                        return "Eight";

                    case 9:

                        return "Nine";

                    default:
                        Debug.LogWarning("Out Of Range");

                        return "Out of Range";
                }
            }

            #endregion

            #region Private Methods

            /// <summary>
            /// Simple indenting function.
            /// </summary>
            /// <param name="indent"> How far to indent. </param>
            /// <returns> The indented string. </returns>
            static string Indent(int indent)
            {
                string indentString = "";

                for (int i = 0; i < indent; i++)
                {
                    indentString = indentString + "\t";
                }

                return indentString;
            }

            #endregion
        }
    }
}