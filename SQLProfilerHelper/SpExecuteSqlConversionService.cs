using System;
using System.Collections.Generic;
using System.Text;

namespace SQLProfilerHelper
{
    public class SpExecuteSqlConversionService
    {
        public string SPExecuteSQLInput { get; set; }
        public string SQLOutput { get; set; }
        private const int BUFFER_SIZE = 80000;

        public enum State
        {
            BeginningAndSqlBody,
            VariablesDecls,
            VariablesAssign,
            VariableValue
        }

        public void Convert()
        {
            var indentCount = 0;
            var buffer = new StringBuilder(BUFFER_SIZE);

            void AppendIndent()
            {
                for(int i = 0; i < 2 * indentCount; ++i)
                {
                    buffer.Append(' ');
                }
            }

            void AppendNewLine()
            {
                buffer.Append('\r');
                buffer.Append('\n');
            }

            char currentChar = '\0', previousChar = '\0';
            var variableNameBuffer = new StringBuilder();
            var variableValueBuffer = new StringBuilder();
            var isInsideVariableName = false;
            var isInsideStringValue = false;
            var parserState = State.BeginningAndSqlBody;
            var variablesValuesDictionary = new Dictionary<string, string>();
            var quoteCounterInVariableDecls = 0;

            for (int i = 0; i < SPExecuteSQLInput.Length; i++)
            {
                previousChar = currentChar;
                currentChar = SPExecuteSQLInput[i];

                switch (parserState)
                {
                    case State.BeginningAndSqlBody:

                        if (i == 20)
                        {
                            if (buffer.ToString().ToLowerInvariant() != "exec sp_executesql n")
                            {
                                throw new InvalidInputSpExecuteSqlTextException();
                            }
                            buffer.Remove(0, 20);
                            continue;
                        }

                        if (currentChar == '\'')
                        {
                            if (
                                SPExecuteSQLInput[i + 1] == currentChar
                                && SPExecuteSQLInput[i + 2] == currentChar
                                && SPExecuteSQLInput[i + 3] == currentChar
                            )
                            {
                                buffer.Append('\'');
                                buffer.Append('\'');
                                i += 3;
                                continue;
                            }
                            if (SPExecuteSQLInput[i + 1] == currentChar)
                            {
                                // COALESCE([T].[c], ''0001-01-01T00:00:00.000'') AS [Coalesce] case
                                buffer.Append('\'');
                                i += 1;
                                continue;
                            }
                            parserState = State.VariablesDecls;
                        }
                        else
                        {
                            if (currentChar == '(')
                            {
                                if (previousChar != '(')
                                {
                                    AppendNewLine();
                                    AppendIndent();
                                }
                                indentCount++;
                                buffer.Append(currentChar);
                                AppendNewLine();
                                AppendIndent();
                            }
                            else if (currentChar == '\n')
                            {
                                buffer.Append(currentChar);
                                AppendIndent();
                            }
                            else if (currentChar == ')')
                            {
                                indentCount--;
                                AppendNewLine();
                                AppendIndent();
                                buffer.Append(currentChar);
                            }
                            else
                            {
                                buffer.Append(currentChar);
                            }
                        }

                        break;

                    case State.VariablesDecls:

                        if (currentChar == '\'')
                        {
                            quoteCounterInVariableDecls++;
                            if (quoteCounterInVariableDecls == 2)
                            {
                                parserState = State.VariablesAssign;
                            }
                        }

                        break;

                    case State.VariablesAssign:

                        if (currentChar == '@' && !isInsideVariableName)
                        {
                            isInsideVariableName = true;
                            variableNameBuffer.Append(currentChar);
                        }
                        else if (isInsideVariableName && (char.IsLetterOrDigit(currentChar) || currentChar == '_'))
                        {
                            variableNameBuffer.Append(currentChar);
                        }
                        else if (currentChar == '=' && isInsideVariableName)
                        {
                            parserState = State.VariableValue;
                        }

                        break;

                    case State.VariableValue:

                        if (variableValueBuffer.Length == 0 && currentChar == ' ')
                        {
                            continue;
                        }

                        if (currentChar == '\'')
                        {
                            isInsideStringValue = !isInsideStringValue;
                        }

                        if (
                            (
                                currentChar != ','
                                && !(previousChar != '\r' && currentChar == '\r')
                                && !(previousChar == '\r' && currentChar == '\n')
                                && currentChar != ' '
                            ) ||
                            (currentChar == ' ' && isInsideStringValue)
                        )
                        {
                            variableValueBuffer.Append(currentChar);
                            if (i == SPExecuteSQLInput.Length - 1)
                            {
                                variablesValuesDictionary[variableNameBuffer.ToString()] = variableValueBuffer.ToString();
                            }
                        }
                        else if (currentChar == ',')
                        {
                            var variableValue = variableValueBuffer.ToString();
                            if (variableValue.Contains(" 00:00:00'"))
                            {
                                variableValue = variableValue.Replace(" 00:00:00", string.Empty);
                            }
                            variablesValuesDictionary[variableNameBuffer.ToString()] = variableValue;
                            variableValueBuffer.Clear();
                            variableNameBuffer.Clear();
                            parserState = State.VariablesAssign;
                            isInsideVariableName = false;
                        }
                        else if (currentChar == '\r' && i == SPExecuteSQLInput.Length - 2)
                        {
                            variablesValuesDictionary[variableNameBuffer.ToString()] = variableValueBuffer.ToString();
                            break;
                        }

                        break;
                }

            }

            SQLOutput = buffer.ToString();

            foreach (var k in variablesValuesDictionary.Keys)
            {
                SQLOutput = SQLOutput.Replace(k, variablesValuesDictionary[k]);
            }

        }

    }

    //////////////////////////////////////////////////////////////////////////////

    public class SpExecuteSqlConversionException : Exception
    {
        public SpExecuteSqlConversionException(string message) : base(message)
        {
        }
    }

    public class InvalidInputSpExecuteSqlTextException : SpExecuteSqlConversionException
    {
        public InvalidInputSpExecuteSqlTextException() : base("Input text is not valid 'EXEC sp_executesq' SQL statement")
        {
        }
    }

}
