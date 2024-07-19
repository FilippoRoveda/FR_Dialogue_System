using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace CSVPlugin
{
    public class CSVReader
    {
        private enum ParsingMode
        {
            // default(treat as null)
            None,
            // processing a character which is out of quotes 
            OutQuote,
            // processing a character which is in quotes
            InQuote
        }

        /// <summary>
        /// Parses the CSV string.
        /// </summary>
        /// <returns>a two-dimensional array. first index indicates the row.  second index indicates the column.</returns>
        /// <param name="src">raw CSV contents as string</param>
        public List<List<string>> ParseCSV(string src)
        {
            var rows = new List<List<string>>();
            var cols = new List<string>();

#pragma warning disable XS0001 // Find APIs marked as TODO in Mono
            var temp = new StringBuilder();
#pragma warning restore XS0001 // Find APIs marked as TODO in Mono

            ParsingMode mode = ParsingMode.OutQuote;
            bool requireTrimLineHead = false;
            var isBlank = new Regex(@"\s");

            int len = src.Length;

            for (int i = 0; i < len; ++i)
            {
                char c = src[i];

                // remove whilespace at beginning of line
                if (requireTrimLineHead)
                {
                    if (isBlank.IsMatch(c.ToString()))
                    {
                        continue;
                    }
                    requireTrimLineHead = false;
                }

                // finalize when c is the last character
                if ((i + 1) == len)
                {
                    // final char
                    switch (mode)
                    {
                        case ParsingMode.InQuote:
                            if (c == '"')
                            {
                                // ignore
                            }
                            else
                            {
                                // if close quote is missing
                                temp.Append(c);
                            }
                            cols.Add(temp.ToString());
                            rows.Add(cols);
                            return rows;

                        case ParsingMode.OutQuote:
                            if (c == ',')
                            {
                                // if the final character is comma, add an empty cell
                                // next col
                                cols.Add(temp.ToString());
                                cols.Add(string.Empty);
                                rows.Add(cols);
                                return rows;
                            }
                            if (cols.Count == 0)
                            {
                                // if the final line is empty, ignore it. 
                                if (string.Empty.Equals(c.ToString().Trim()))
                                {
                                    return rows;
                                }
                            }
                            temp.Append(c);
                            cols.Add(temp.ToString());
                            rows.Add(cols);
                            return rows;
                    }
                }

                // the next character
                char n = src[i + 1];

                switch (mode)
                {
                    case ParsingMode.OutQuote:
                        // out quote
                        if (c == '"')
                        {
                            // to in-quote
                            mode = ParsingMode.InQuote;
                            continue;

                        }
                        else if (c == ',')
                        {
                            // next cell
                            cols.Add(temp.ToString());
                            temp.Remove(0, temp.Length);

                        }
                        else if (c == '\r' && n == '\n')
                        {
                            // new line(CR+LF)
                            cols.Add(temp.ToString());
                            rows.Add(cols);
                            cols = new List<string>();
                            temp.Remove(0, temp.Length);
                            ++i; // skip next code
                            requireTrimLineHead = true;

                        }
                        else if (c == '\n' || c == '\r')
                        {
                            // new line
                            cols.Add(temp.ToString());
                            rows.Add(cols);
                            cols = new List<string>();
                            temp.Remove(0, temp.Length);
                            requireTrimLineHead = true;

                        }
                        else
                        {
                            //  get one char
                            temp.Append(c);
                        }
                        break;

                    case ParsingMode.InQuote:
                        // in quote
                        if (c == '"' && n != '"')
                        {
                            // to out-quote
                            mode = ParsingMode.OutQuote;

                        }
                        else if (c == '"' && n == '"')
                        {
                            // get "
                            temp.Append('"');
                            ++i;

                        }
                        else
                        {
                            // get one char
                            temp.Append(c);
                        }
                        break;
                }
            }
            return rows;
        }

    }
}