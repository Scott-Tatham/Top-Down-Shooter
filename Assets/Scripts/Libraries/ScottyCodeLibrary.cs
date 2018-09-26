using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScottyCode
{
    namespace Extensions
    {
        public static class StringExtentions
        {
            #region Public Methods

            // Creates a pretty print of the string, useful for Json.
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

    namespace Helpers
    {
        namespace JsonHelpers
        {
            // A generic class type that contains a list for using Unity's Json Utilities with arrays/lists.
            [Serializable]
            public class JsonArrayContainer<T>
            {
                [SerializeField]
                List<T> contained = new List<T>();

                public List<T> GetContained() { return contained; }

                public void SetContained(List<T> contents) { contained = contents; }
            }
        }
    }
}
