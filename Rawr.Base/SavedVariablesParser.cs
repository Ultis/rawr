using System.Collections.Generic;
using System;
using System.IO;
namespace Rawr
{
    /**
     * SavedVariablesParser
     * @author Charinna
     * This class is a utility function that should (I hope) act as a general parser for
     * Saved Variable files.  Essentially it can handle Lua scripts that have variable name = value pairs.
     * Since it makes no assumptions about the data in the files or its layout, the result is
     * a list of dictionary items for tables, and native types for strings, booleans and numbers.
     */

    /**
     * This class parses a saved variable file and outputs a list of variable = value keys.
     * Note that it does not support the following:
     * 1. Values that are expressions other than numeric constants, strings, booleans or nil values.
     * 2. String values that are defined using long format (long brackets)
     * 3. Multi-line comments
     * 4. Whatever else it doesn't support :)
     * 
     * There's no technical reason not to support these items, I just did not get around to it.
     */
    public class SavedVariablesParser
    {
        public enum LuaTokenTypes
        {
            LUA_STRING = 1,
            LUA_INTEGER,
            LUA_DOUBLE,
            LUA_KEYWORD,
            LUA_BOOLEAN,
            LUA_NIL,
            LUA_FIELD,
            LUA_TABLE,
        };

        abstract class LuaToken
        {
            LuaTokenTypes m_type;

            public LuaToken(LuaTokenTypes type)
            {
                m_type = type;
            }

            public LuaTokenTypes getTokenType()
            {
                return m_type;
            }

            public abstract object getValue();

            public virtual object getInterpretedValue()
            {
                return getValue();
            }
        };

        class LuaStringToken : LuaToken
        {
            string m_sValue;

            public LuaStringToken(string sValue)
                : base(LuaTokenTypes.LUA_STRING)
            {
                m_sValue = sValue;
            }

            public override object getValue()
            {
                return m_sValue;
            }

            public string getString()
            {
                return m_sValue;
            }
        };

        class LuaNilToken : LuaToken
        {
            public LuaNilToken()
                : base(LuaTokenTypes.LUA_NIL)
            {
            }

            public override object getValue()
            {
                return null;
            }
        };

        class LuaBooleanToken : LuaToken
        {
            bool m_bValue;

            public LuaBooleanToken(bool bValue)
                : base(LuaTokenTypes.LUA_BOOLEAN)
            {
                m_bValue = bValue;
            }

            public override object getValue()
            {
                return m_bValue;
            }

            public bool getBoolean()
            {
                return m_bValue;
            }
        };

        class LuaKeywordToken : LuaToken
        {
            string m_sValue;

            public LuaKeywordToken(string sValue)
                : base(LuaTokenTypes.LUA_KEYWORD)
            {
                m_sValue = sValue;
            }

            public override object getValue()
            {
                return m_sValue;
            }

            public string getKeyword()
            {
                return m_sValue;
            }

            public override object getInterpretedValue()
            {
                return reinterpretAsValue().getValue();
            }

            public LuaToken reinterpretAsValue()
            {
                if (m_sValue.Equals("true"))
                {
                    return new LuaBooleanToken(true);
                }
                else if (m_sValue.Equals("false"))
                {
                    return new LuaBooleanToken(false);
                }
                else if (m_sValue.Equals("nil"))
                {
                    return new LuaNilToken();
                }
                else
                {
                    throw new InvalidDataException("Don't know how to interpret " + m_sValue + " as a value.");
                }
            }
        };

        class LuaIntegerToken : LuaToken
        {
            // Integers are stored as longs because of an edge
            // case.
            // It turns out that you can have the value 2147483648
            // as an integer output in a saved variable file, but
            // that's out of the range of legitimate C# int32 values.
            long m_lValue;

            public LuaIntegerToken(long lValue)
                : base(LuaTokenTypes.LUA_INTEGER)
            {
                m_lValue = lValue;
            }

            public override object getValue()
            {
                return m_lValue;
            }

            public long getInteger()
            {
                return m_lValue;
            }
        };

        class LuaDoubleToken : LuaToken
        {
            double m_dValue;

            public LuaDoubleToken(double dValue)
                : base(LuaTokenTypes.LUA_DOUBLE)
            {
                m_dValue = dValue;
            }

            public override object getValue()
            {
                return m_dValue;
            }

            public double getDouble()
            {
                return m_dValue;
            }
        };

        class LuaFieldToken : LuaToken
        {
            KeyValuePair<IComparable, object> m_keyValue;

            public LuaFieldToken(LuaToken key, object value)
                : base(LuaTokenTypes.LUA_FIELD)
            {
                switch (key.getTokenType())
                {
                    case LuaTokenTypes.LUA_INTEGER:
                    case LuaTokenTypes.LUA_KEYWORD:
                    case LuaTokenTypes.LUA_STRING:
                        m_keyValue = new KeyValuePair<IComparable, object>((IComparable)key.getValue(), value);
                        break;
                    default:
                        throw new InvalidDataException("Don't know how to hash by " + key.getTokenType() + " token type.");
                }
            }

            public override object getValue()
            {
                return m_keyValue;
            }

            public IComparable getKeyValueKey()
            {
                return m_keyValue.Key;
            }

            public object getKeyValueValue()
            {
                return m_keyValue.Value;
            }
        };

        class LuaTableToken : LuaToken
        {
            Dictionary<IComparable, object> m_values;

            public LuaTableToken(Dictionary<IComparable, object> values)
                : base(LuaTokenTypes.LUA_TABLE)
            {
                m_values = values;
            }

            public override object getValue()
            {
                return m_values;
            }

            public Dictionary<IComparable, object> getTable()
            {
                return m_values;
            }
        };

        /**
         * This internal helper class treats a SavedVariableFile as a stream.
         * It has read and peek functions, and allows ignoring white spaces & comments
         * or processing them.
         */
        class SavedVariableFileStream
        {
            string m_sFullFile;
            int m_iCurrentOffset;
            int m_iLastOffset;

            public SavedVariableFileStream(string sFileName)
            {
                // Allow the file to be read/written by other processes.
                // Even though having the file written to while it's being read is likely to
                // yield an invalid Saved Variables file, it will at least allow the
                // game to function properly.
                FileStream fileStream = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader streamReader = new StreamReader(fileStream);
                m_sFullFile = streamReader.ReadToEnd();
                fileStream.Close();
                m_iCurrentOffset = 0;
                m_iLastOffset = m_sFullFile.Length;
            }

            /**
             * Helper function for stream.getNextCharacter/stream.peekNextCharacter to read the next character
             * off the stream, if there is one.
             * Throws EndOfStreamException if there is no next character.
             */
            private char advanceCharacter()
            {
                if (m_iCurrentOffset + 1 == m_iLastOffset)
                    throw new EndOfStreamException();
                return m_sFullFile[m_iCurrentOffset++];
            }

            /**
             * Returns the next character in the stream. Advances the read pointer.
             * bUnparsed: When true, comments, white spaces, and the like are not interpreted.
             * Throws EndOfStreamException if there is no next character.
             */
            public char getNextCharacter(bool bUnparsed)
            {
                bool bValidCharacter = bUnparsed;
                char cNextCharacter = advanceCharacter();

                while (!bValidCharacter)
                {
                    if (cNextCharacter == ' ' ||
                        cNextCharacter == '\r' ||
                        cNextCharacter == '\n' ||
                        cNextCharacter == '\t')
                    {
                        // Ignore white spaces
                        cNextCharacter = advanceCharacter();
                    }
                    else if (cNextCharacter == '-' && m_iCurrentOffset + 1 < m_iLastOffset &&
                        m_sFullFile[m_iCurrentOffset] == '-')
                    {
                        // Ignore comments till end of line
                        for (cNextCharacter = advanceCharacter();
                            (cNextCharacter != '\r') && (cNextCharacter != '\n');
                            cNextCharacter = advanceCharacter())
                        {
                        }

                        cNextCharacter = advanceCharacter();
                    }
                    else
                    {
                        bValidCharacter = true;
                    }
                }

                return cNextCharacter;
            }

            /**
             * Returns the next character in the stream, null if there is none.
             * Does not advance the read pointer.
             * bUnparsed: When true, comments, white spaces, and the like are not interpreted.
             */
            public char? peekNextCharacter(bool bUnparsed)
            {
                try
                {
                    int iSavedPosition = m_iCurrentOffset;
                    char cNextCharacter = getNextCharacter(bUnparsed);
                    m_iCurrentOffset = iSavedPosition;
                    return cNextCharacter;
                }
                catch (EndOfStreamException)
                {
                    return null;
                }
            }

            public string getErrorContext()
            {
                return "At offset " + m_iCurrentOffset + " (" + m_sFullFile.Substring(Math.Max(m_iCurrentOffset - 16, 0), 32) + ")";
            }
        }

        private static LuaToken getNextToken(SavedVariableFileStream stream)
        {
            // Note:
            // The general rule on the true/false parameter of getNextCharacter/peekNextCharacter.
            // That boolean indicates whether the function should parse out comments & white-spaces
            // from the next possible character.  While it's useful to ignore those characters
            // when talking about separators between keywords, those characters should not
            // be ignored when the next keyword is being processed.  Imagine the following
            // example:
            // var1 = 0xc0
            // ffee = 72
            // In this case, if we were processing the numer '0xc0' and asked for the next
            // character ignoring white spaces, 'ffee' would be appended letting var1 = 0xc0ffee
            // when in actuality ffee is the name of the next keyword / variable.
            char cCharacter = stream.getNextCharacter(false);

            if ((cCharacter >= '0' && cCharacter <= '9') || (cCharacter == '-' || cCharacter == '+'))
            {
                char? cNextCharacter = stream.peekNextCharacter(true);
                // Numeric token
                if (cCharacter == '0' && cNextCharacter != null && Char.ToUpper((char)cNextCharacter) == 'X')
                {
                    // Hexadecimal character
                    int iValue = 0;
                    // Consume 'X'
                    stream.getNextCharacter(true);

                    while (true)
                    {
                        cNextCharacter = stream.peekNextCharacter(true);

                        if (cNextCharacter != null && Char.ToUpper((char)cNextCharacter) >= 'A' && Char.ToUpper((char)cNextCharacter) <= 'F')
                        {
                            iValue = iValue * 16 + (10 + (char)cNextCharacter - 'A');
                        }
                        else if (cNextCharacter >= '0' && (char)cNextCharacter <= '9')
                        {
                            iValue = iValue * 16 + (char)cNextCharacter - '0';
                        }
                        else
                        {
                            break;
                        }

                        // Consume the character we just read in
                        stream.getNextCharacter(true);
                    }
                    return new LuaIntegerToken(iValue);
                }
                else
                {
                    // Numeric token -- scan for the whole string then choose the number format
                    String sValue = null;
                    bool bIsInteger = true;

                    sValue += cCharacter;

                    while (true)
                    {
                        cNextCharacter = stream.peekNextCharacter(true);

                        if (cNextCharacter == 'e' || cNextCharacter == 'E' || cNextCharacter == '.' || cNextCharacter == '-')
                        {
                            bIsInteger = false;
                        }
                        else if (cNextCharacter >= '0' && cNextCharacter <= '9')
                        {
                        }
                        else
                        {
                            break;
                        }

                        sValue += cNextCharacter;
                        // Consume the character we just read in
                        stream.getNextCharacter(true);
                    }

                    if (bIsInteger)
                    {
                        return new LuaIntegerToken(Int64.Parse(sValue));
                    }
                    else
                    {
                        return new LuaDoubleToken(Double.Parse(sValue));
                    }
                }
            }
            else if (cCharacter == '"' || cCharacter == '\'')
            {
                string sValue = "";

                while (true)
                {
                    char cNextCharacter = stream.getNextCharacter(true);

                    // String terminates when it ends with the same character
                    // it began with (i.e. ' is needed to close ', and " for ")
                    if (cNextCharacter == cCharacter)
                    {
                        break;
                    }
                    else if (cNextCharacter == '\\')
                    {
                        sValue += cNextCharacter;
                        // Escape the next character...  Read it in whatever it is.
                        cNextCharacter = stream.getNextCharacter(true);
                    }
                    sValue += cNextCharacter;
                }

                return new LuaStringToken(sValue);
            }
            else if (cCharacter == '[')
            {
                LuaToken key = getNextToken(stream);

                if (stream.getNextCharacter(false) != ']')
                {
                    throw new InvalidDataException("Expected a ] after field key");
                }
                else if (stream.getNextCharacter(false) != '=')
                {
                    throw new InvalidDataException("Expected a value after field key");
                }
                else
                {
                    LuaToken nextToken = getNextToken(stream);

                    return new LuaFieldToken(key, nextToken.getInterpretedValue());
                }
            }
            else if (cCharacter == '{')
            {
                Dictionary<IComparable, object> items = new Dictionary<IComparable, object>();
                int iAnonymousKey = 1;

                while (true)
                {
                    char? cNextCharacter = stream.peekNextCharacter(false);

                    if (cNextCharacter == '}')
                    {
                        // Consume the close brackets
                        stream.getNextCharacter(false);
                        break;
                    }
                    else if (cNextCharacter == ',' || cNextCharacter == ';')
                    {
                        stream.getNextCharacter(false);
                    }
                    else
                    {
                        LuaToken nextToken = getNextToken(stream);

                        // Case 1: [key] = value pair
                        if (nextToken.getTokenType() == LuaTokenTypes.LUA_FIELD)
                        {
                            items.Add(((LuaFieldToken)nextToken).getKeyValueKey(), ((LuaFieldToken)nextToken).getKeyValueValue());
                        }
                        else if (stream.peekNextCharacter(false) == '=')
                        // Case 2: key = value pair
                        {
                            // Consume the '='
                            stream.getNextCharacter(false);
                            switch (nextToken.getTokenType())
                            {
                                case LuaTokenTypes.LUA_INTEGER:
                                case LuaTokenTypes.LUA_KEYWORD:
                                case LuaTokenTypes.LUA_STRING:
                                    items.Add((IComparable)nextToken.getValue(), getNextToken(stream).getInterpretedValue());
                                    break;

                                default:
                                    throw new InvalidDataException("Don't know how to hash by " + nextToken.getTokenType() + " token type.");
                            }
                        }
                        else
                        // Case 3: value
                        {
                            items.Add(iAnonymousKey++, nextToken.getInterpretedValue());
                        }
                    }
                }

                return new LuaTableToken(items);
            }
            else if ((cCharacter >= 'a' && cCharacter <= 'z') || (cCharacter >= 'A' && cCharacter <= 'Z') || cCharacter == '_')
            {
                string sKeyword = null;

                sKeyword += cCharacter;

                while (true)
                {
                    char? cNextCharacter = stream.peekNextCharacter(true);

                    if ((cNextCharacter >= 'a' && cNextCharacter <= 'z') || (cNextCharacter >= 'A' && cNextCharacter <= 'Z') || (cNextCharacter >= '0' && cNextCharacter <= '9') || cNextCharacter == '_')
                    {
                        sKeyword += stream.getNextCharacter(true);
                    }
                    else
                    {
                        return new LuaKeywordToken(sKeyword);
                    }
                }
            }

            throw new InvalidDataException("Can't parse string " + stream.getErrorContext());
        }


        public static Dictionary<string, object> parse(string sFileName)
        {
            Dictionary<string, object> savedVariables = new Dictionary<string, object>();

            // Note -- file errors are deliberately allowed to bubble to the caller.
            // Note 2: This function reads in the entire file.  There are undoubtedly better ways to do this.
            SavedVariableFileStream stream = new SavedVariableFileStream(sFileName);

            try
            {
                while (true)
                {
                    LuaToken variable = getNextToken(stream);

                    if (stream.getNextCharacter(false) != '=')
                    {
                        throw new InvalidDataException("Expected 'variable_name = value' pairs in saved variables file.");
                    }

                    savedVariables.Add(((LuaKeywordToken)variable).getKeyword(), getNextToken(stream).getInterpretedValue());
                }
            }
            catch (EndOfStreamException)
            {
            }

            stream = null;

            return savedVariables;
        }
    }
}