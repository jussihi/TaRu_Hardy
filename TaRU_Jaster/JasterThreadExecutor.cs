using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Text.RegularExpressions;

using System.Threading;

namespace TaRU_Jaster
{
    class Global
    {
        public static Form1 g_form1;
    }

    class JasterThreadExecutor
    {
        private SerialPort _serialPort;

        public JasterThreadExecutor()
        {
            _serialPort = new SerialPort();
            Global.g_form1.log_msg("Jaster Executor initialized!");
        }


        /* 
         * SERIAL FUNCTIONS
         */
        public string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        public bool ConnectSerial(string w_portName)
        {
            try
            {
                _serialPort.PortName  = w_portName;
                _serialPort.BaudRate  = 4800;
                _serialPort.Parity    = Parity.None;
                _serialPort.DataBits  = 8;
                _serialPort.StopBits  = StopBits.One;
                _serialPort.Handshake = Handshake.None;
                _serialPort.Open();
                return true;
            }
            catch (Exception ex)
            {
                Global.g_form1.log_msg("ERROR opening port " + _serialPort.PortName + "! error message: " + ex.Message);
                return false;
            }
        }

        public bool IsSerialOpen()
        {
            return _serialPort.IsOpen;
        }

        /*
         * METAPROGRAMMER PARSER
         */

        Dictionary<string, int> labels;
        Dictionary<string, int> variables;

        private bool ValidateExpression(string w_line, int w_lineno)
        {
            string[] tokens = w_line.Split(' ');
            tokens = tokens.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            // parse LABEL 
            if (tokens[0] == "LABEL")
            {
                if (tokens.Length != 2)
                {
                    Global.g_form1.log_msg("ERROR LABEL must have a name! line :" + w_lineno);
                }

                // Check that the LABEL is valid
                if (!tokens[1].All(Char.IsLetter))
                {
                    Global.g_form1.log_msg("ERROR LABEL must have a character-only name! line :" + w_lineno);
                }

                // Check if the label already is in variables
                if (variables.ContainsKey(tokens[1]))
                {
                    Global.g_form1.log_msg("ERROR LABEL must not have a same name as a variable! line :" + w_lineno);
                }

                // everything ok, add label to dict
                try { labels.Add(tokens[1], w_lineno); }
                catch (Exception ex)
                {
                    Global.g_form1.log_msg("ERROR LABEL already used! line :" + w_lineno);
                }
            }

            // parse VAR
            else if (tokens[0] == "VAR")
            {
                if (tokens.Length != 4)
                {
                    Global.g_form1.log_msg("ERROR VARIABLE not properly initialized! line :" + w_lineno);
                }

                // Check that the VARIABLE name is valid
                if (!tokens[1].All(Char.IsLetter))
                {
                    Global.g_form1.log_msg("ERROR VARIABLE name must only contain characters! line :" + w_lineno);
                }

                // check that this token is not in the labels list
                if (labels.ContainsKey(tokens[1]))
                {
                    Global.g_form1.log_msg("ERROR VARIABLE name must not have a same name as a label! line :" + w_lineno);
                }

                // Check that the VARIABLE is being assigned
                if (!tokens[2].Equals("="))
                {
                    Global.g_form1.log_msg("ERROR VARIABLE must be initialized with a =! line :" + w_lineno);
                }

                // Check if the assignee is another variable
                if (tokens[3].All(Char.IsLetter))
                {
                    // everything ok, add label to dict
                    try { variables.Add(tokens[1], variables[tokens[3]]); }
                    catch (Exception ex)
                    {
                        Global.g_form1.log_msg("ERROR VARIABLE name already in use or invalid other var name! line :" + w_lineno);
                    }
                }

                // Check if the assignee is another variable
                if (tokens[3].All(Char.IsDigit))
                {
                    // everything ok, add label to dict
                    try { variables.Add(tokens[1], Int32.Parse(tokens[3])); }
                    catch (Exception ex)
                    {
                        Global.g_form1.log_msg("ERROR VARIABLE name already in use! line :" + w_lineno);
                    }
                }

            }

            // parse IF
            else if (tokens[0] == "IF")
            {
                if (tokens.Length <= 4)
                {
                    Global.g_form1.log_msg("ERROR IF expression must have at least 5 tokens! line :" + w_lineno);
                }

                // Check that the first is valid
                if (tokens[1].All(Char.IsLetter))
                {
                    if(!variables.ContainsKey(tokens[1]))
                    {
                        Global.g_form1.log_msg("ERROR IF expression 1st variable not valid! line :" + w_lineno);
                    }
                }
                else
                {
                    if(!tokens[1].All(Char.IsDigit))
                    {
                        Global.g_form1.log_msg("ERROR IF expression 1st number not valid! line :" + w_lineno);
                    }
                }

                // Check that the last compared is valid
                string[] valids = { "==", "<", ">", "<=", ">=" };
                if(!valids.Contains(tokens[2]))
                {
                    Global.g_form1.log_msg("ERROR IF expression comparator unknown! line :" + w_lineno);
                }

                // Check that the second is valid
                if (tokens[3].All(Char.IsLetter))
                {
                    if (!variables.ContainsKey(tokens[3]))
                    {
                        Global.g_form1.log_msg("ERROR IF expression 2nd variable not valid! line :" + w_lineno);
                    }
                }
                else
                {
                    if(!tokens[3].All(Char.IsDigit))
                    {
                        Global.g_form1.log_msg("ERROR IF expression 2nd number not valid! line :" + w_lineno);
                    }
                }

                string toEvaluate = "";
                for(int i = 4; i < tokens.Length; i++)
                {
                    toEvaluate += tokens[i] + " ";
                }
                // recursively call evaluation for the if expression
                if (!ValidateExpression(toEvaluate, w_lineno))
                {
                    Global.g_form1.log_msg("ERROR IF expression failed to evaluate! line :" + w_lineno);
                }

            }

            // parse SLEEP
            else if (tokens[0] == "SLEEP")
            {
                if (tokens.Length != 2)
                {
                    Global.g_form1.log_msg("ERROR SLEEP duration must be set! line :" + w_lineno);
                }

                // Check that the VARIABLE name is valid
                if (!tokens[1].All(Char.IsDigit))
                {
                    Global.g_form1.log_msg("ERROR SLEEP duration must be a number! line :" + w_lineno);
                }
            }

            // parse GOTO
            else if (tokens[0] == "GOTO")
            {
                if (tokens.Length != 2)
                {
                    Global.g_form1.log_msg("ERROR GOTO must point to a label! " + tokens.Length + " line :" + w_lineno);
                }

                // Check that the label name is valid
                if (!labels.ContainsKey(tokens[1]))
                {
                    Global.g_form1.log_msg("ERROR GOTO pointee not found from labels! line :" + w_lineno);
                }
            }

            // parse PLUS
            else if (tokens[0] == "PLUS")
            {
                if (tokens.Length != 3)
                {
                    Global.g_form1.log_msg("ERROR PLUS must have 2 arguments! line :" + w_lineno);
                }

                // Check that the VARIABLE name is valid
                if (!variables.ContainsKey(tokens[1]))
                {
                    Global.g_form1.log_msg("ERROR PLUS variable not initialized! line :" + w_lineno);
                }

                // Check that the number is valid
                if (!(tokens[2].All(Char.IsDigit)))
                {
                    Global.g_form1.log_msg("ERROR PLUS 2nd argument must be a number! line :" + w_lineno);
                }
            }

            // parse SUB
            else if (tokens[0] == "SUB")
            {
                if (tokens.Length != 3)
                {
                    Global.g_form1.log_msg("ERROR SUB must have 2 arguments! line :" + w_lineno);
                }

                // Check that the VARIABLE name is valid
                if (!variables.ContainsKey(tokens[1]))
                {
                    Global.g_form1.log_msg("ERROR SUB variable not initialized! line :" + w_lineno);
                }

                // Check that the number is valid
                if (!(tokens[2].All(Char.IsDigit)))
                {
                    Global.g_form1.log_msg("ERROR SUB 2nd argument must be a number! line :" + w_lineno);
                }
            }

            // parse UP
            else if (tokens[0] == "UP")
            {
                if (tokens.Length > 2)
                {
                    Global.g_form1.log_msg("ERROR UP only takes 0 or 1 argument! line :" + w_lineno);
                }

            }

            // parse DOWN
            else if (tokens[0] == "DOWN")
            {
                if (tokens.Length > 2)
                {
                    Global.g_form1.log_msg("ERROR DOWN only takes 0 or 1 argument! line :" + w_lineno);
                }

            }

            // parse RESET
            else if (tokens[0] == "RESET")
            {
                if (tokens.Length > 2)
                {
                    Global.g_form1.log_msg("ERROR RESET only takes 0 or 1 argument! line :" + w_lineno);
                }

            }

            else
            {
                Global.g_form1.log_msg("ERROR Unknown command " + tokens[0] + "! line :" + w_lineno);
                return false;
            }


            return true;
        }

        public int EvaluateExpression(string w_line, int w_lineno)
        {
            string[] tokens = w_line.Split(' ');
            tokens = tokens.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            int returnLineNo = w_lineno + 1;

            if (tokens[0] == "IF")
            {
                int first;
                int second;

                // Check that the first is valid
                if (tokens[1].All(Char.IsLetter))
                {
                    first = variables[tokens[1]];
                }
                else
                {
                    first = Int32.Parse(tokens[1]);
                }

                // Check that the last compared is valid
                string[] valids = { "==", "<", ">", "<=", ">=" };

                // Check that the second is valid
                if (tokens[3].All(Char.IsLetter))
                {
                    second = variables[tokens[3]];
                }
                else
                {
                    second = Int32.Parse(tokens[3]);
                }

                bool validated = false;
                if(tokens[2].Equals("=="))
                {
                    if(first == second)
                    {
                        validated = true;
                    }
                }
                else if (tokens[2].Equals("<"))
                {
                    if (first < second)
                    {
                        validated = true;
                    }
                }
                else if (tokens[2].Equals(">"))
                {
                    if (first > second)
                    {
                        validated = true;
                    }
                }
                else if (tokens[2].Equals(">="))
                {
                    if (first >= second)
                    {
                        validated = true;
                    }
                }
                else if (tokens[2].Equals("<="))
                {
                    if (first <= second)
                    {
                        validated = true;
                    }
                }

                string toEvaluate = "";
                for (int i = 4; i < tokens.Length; i++)
                {
                    toEvaluate += tokens[i] + " ";
                }
                // recursively call evaluation for the if expression
                if (validated)
                {
                    returnLineNo = EvaluateExpression(toEvaluate, w_lineno);
                }

            }

            else if (tokens[0] == "SLEEP")
            {
                Global.g_form1.log_msg("SLEEP");
                Thread.Sleep(Int32.Parse(tokens[1]));
            }

            else if (tokens[0] == "GOTO")
            {
                return labels[tokens[1]];
            }

            else if (tokens[0] == "PLUS")
            {
                variables[tokens[1]] += Int32.Parse(tokens[2]);
            }

            else if (tokens[0] == "SUB")
            {
                variables[tokens[1]] -= Int32.Parse(tokens[2]);
            }

            else if (tokens[0] == "UP")
            {
                CommandAllJastersUp();
            }

            else if (tokens[0] == "DOWN")
            {
                CommandAllJastersDown();

            }

            else if (tokens[0] == "RESET")
            {
                CommandAllJastersReset();
            }

            else if(tokens[0] == "LABEL" || tokens[0] == "VAR")
            {
                // passthrough
            }

            else
            {
                Global.g_form1.log_msg("ERROR Unknown command " + tokens[0] + "! line :" + w_lineno);
            }

            return returnLineNo;
        }


        public bool RunCommands(string w_program)
        {
            string[] lines = Regex.Split(w_program, Environment.NewLine);
            lines = lines.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            int currentExecutionLine = 0;

            labels = new Dictionary<string, int>();
            variables = new Dictionary<string, int>();

            for(int i = 0; i < lines.Length; i++)
            {
                if(!ValidateExpression(lines[i], i))
                {
                    // fail
                    return false;
                }
            }

            while (currentExecutionLine >= 0 && currentExecutionLine < lines.Length)
            {
                Global.g_form1.log_msg("EXECUTING line " + currentExecutionLine.ToString());
                currentExecutionLine = EvaluateExpression(lines[currentExecutionLine], currentExecutionLine);
            }

            Global.g_form1.log_msg("DONE EXECUTING");

            return true;
        }


        /*
         * JASTER COMMAND FUNCTIONS
         */
        public bool CommandAllJastersReset()
        {

            try
            {
                if (_serialPort.IsOpen)
                {
                    // reset Jasters
                    byte[] jastersResetBuffer = { 0x91 };
                    _serialPort.Write(jastersResetBuffer, 0, 1);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Global.g_form1.log_msg("ERROR while reseting all Jaster on COM port " +
                    _serialPort.PortName + "! error message: " + ex.Message);
                return false;
            }
        }

        public bool CommandAllJastersUp()
        {
            Global.g_form1.log_msg("UP all Jasters ...");
            try
            {
                if (_serialPort.IsOpen)
                {
                    // initialize UP byte buffer
                    byte[] jastersUpBuffer = { 0x80 };

                    // send command on serial
                    _serialPort.Write(jastersUpBuffer, 0, 1);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Global.g_form1.log_msg("ERROR while commanding all Jaster UP on COM port " +
                    _serialPort.PortName + "! error message: " + ex.Message);
                return false;
            }
        }

        public bool CommandAllJastersDown()
        {
            Global.g_form1.log_msg("DOWN all Jasters ...");
            try
            {
                if (_serialPort.IsOpen)
                {
                    // initialize UP byte buffer
                    byte[] jastersDownBuffer = { 0x81 };

                    // send command on serial
                    _serialPort.Write(jastersDownBuffer, 0, 1);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Global.g_form1.log_msg("ERROR while commanding all Jaster DOWN on COM port " +
                    _serialPort.PortName + "! error message: " + ex.Message);
                return false;
            }
        }

    }


}
