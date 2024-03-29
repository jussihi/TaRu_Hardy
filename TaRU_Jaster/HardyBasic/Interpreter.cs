﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/* TaRu Logger */
using static TaRU_Jaster.Logger;


namespace TaRU_Jaster.HardyBasic
{
    public class Interpreter
    {
        public bool HasPrint { get; set; } = true;
        public bool HasInput { get; set; } = false;

        public bool ShouldExit { get; set; } = false; // do we need to exit?

        private Lexer lex;
        private Token prevToken; // token before last one
        private Token lastToken; // last seen token

        private Dictionary<string, Value> vars; // all variables are stored here
        private Dictionary<string, Marker> labels; // already seen labels 
        private Dictionary<string, Marker> loops; // for loops

        public delegate Func<Interpreter, List<Value>, Task<Value>> AsyncBasicFunction();
        private Dictionary<string, Func<Interpreter, List<Value>, Task<Value>>> funcs;

        private int ifcounter; // counter used for matching "if" with "else"

        private Marker lineMarker; // current line marker

        

        

        private void __printLine(string w_input)
        {
            LOG(w_input, OUT);
        }

        public Interpreter(string input, HardyExecutor w_executor, List<int> w_targets)
        {
            // sanity check input
            if(input == null || input.Length == 0)
            {
                throw new ArgumentException("The script can't be empty");
            }

            // Initialize lexer and dictionaries
            this.lex = new Lexer(input);
            this.vars = new Dictionary<string, Value>();
            this.labels = new Dictionary<string, Marker>();
            this.loops = new Dictionary<string, Marker>();
            this.funcs = new Dictionary<string, Func<Interpreter, List<Value>, Task<Value>>>();
            this.ifcounter = 0;

            // Initialize the HardyBuiltIns and map its functions
            HardyBuiltIns._HardyExecutor = w_executor;
            HardyBuiltIns._targets = w_targets;
            HardyBuiltIns.InstallAll(this);
        }

        public Value GetVar(string name)
        {
            if (!vars.ContainsKey(name))
                throw new Exception("Variable with name " + name + " does not exist.");
            return vars[name];
        }

        public void SetVar(string name, Value val)
        {
            if (!vars.ContainsKey(name)) vars.Add(name, val);
            else vars[name] = val;
        }

        public string GetLine()
        {
            return lex.GetLine(lineMarker);
        }

        public void AddFunction(string name, Func<Interpreter, List<Value>, Task<Value>> function)
        {
            if (!funcs.ContainsKey(name)) funcs.Add(name, function);
            else funcs[name] = function;
        }

        void Error(string text)
        {
            throw new Exception(text + " at line " + lineMarker.Line + ": " + GetLine());
        }

        void Match(Token tok)
        {
            // check if current token is what we expect it to be
            if (lastToken != tok)
                Error("Expect " + tok.ToString() + " got " + lastToken.ToString());
        }

        public async Task Exec()
        {
            ShouldExit = false;
            LOG("Script started!", OUT);
            GetNextToken();
            while (!ShouldExit)
            {
                // Go through all lines and give the program a little break
                // to prevent deadloops
                // edit: no Task.Delay; its overhead is huge (~8ms)
                await Line();
            }
            LOG("Script ended!", OUT);
        }

        Token GetNextToken()
        {
            prevToken = lastToken;
            lastToken = lex.GetToken();

            if (lastToken == Token.EOF && prevToken == Token.EOF)
                Error("Unexpected end of file");

            return lastToken;
        }

        async Task Line()
        {
            // skip empty new lines
            while (lastToken == Token.NewLine) GetNextToken();

            if (lastToken == Token.EOF)
            {
                ShouldExit = true;
                return;
            }

            lineMarker = lex.TokenMarker; // save current line marker
            await Statment(); // evaluate statment

            if (lastToken != Token.NewLine && lastToken != Token.EOF)
                Error("Expect new line got " + lastToken.ToString());
        }

        async Task Statment()
        {
            Token keyword = lastToken;
            GetNextToken();
            switch (keyword)
            {
                case Token.Print: await Print(); break;
                case Token.Input: Input(); break;
                case Token.Goto: await Goto(); break;
                case Token.If: await If(); break;
                case Token.Else: Else(); break;
                case Token.EndIf: break;
                case Token.For: await For(); break;
                case Token.Next: Next(); break;
                case Token.Let: await Let(); break;
                case Token.End: End(); break;
                case Token.Assert: await Assert(); break;
                case Token.Identifier:
                    if (lastToken == Token.Equal) await Let();
                    else if (lastToken == Token.Colon) Label();
                    else goto default;
                    break;
                case Token.EOF:
                    ShouldExit = true;
                    break;
                default:
                    // case if HardyBuiltins is used
                    if (funcs.ContainsKey(lex.Identifier))
                    {
                        string name = lex.Identifier;
                        List<Value> args = new List<Value>();
                        Match(Token.LParen);

                    start:
                        if (GetNextToken() != Token.RParen)
                        {
                            args.Add(await Expr());
                            if (lastToken == Token.Comma)
                                goto start;
                        }

                        await funcs[name](this, args);
                        GetNextToken();
                        break;
                    }
                    else
                    {
                        Error("Undeclared variable " + lex.Identifier);
                    }
                    Error("Expected keyword!");
                    break;
            }
            if (lastToken == Token.Colon)
            {
                // we can execute more statments in single line if we use ";"
                GetNextToken();
                await Statment();
            }
        }

        async Task Print()
        {
            if (!HasPrint)
                Error("Print command not allowed");

            __printLine((await Expr()).ToString());
        }

        void Input()
        {
            if (!HasInput)
                Error("Input command not allowed");

            while (true)
            {
                Match(Token.Identifier);

                if (!vars.ContainsKey(lex.Identifier)) vars.Add(lex.Identifier, new Value());

                string input = Console.ReadLine();
                double d;
                // try to parse as double, if failed read value as string
                if (double.TryParse(input, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out d))
                    vars[lex.Identifier] = new Value(d);
                else
                    vars[lex.Identifier] = new Value(input);

                GetNextToken();
                if (lastToken != Token.Comma) break;
                GetNextToken();
            }
        }

        async Task Goto()
        {
            Match(Token.Identifier);
            string name = lex.Identifier;

            if (!labels.ContainsKey(name))
            {
                // if we didn't encaunter required label yet, start to search for it
                while (true)
                {
                    if (GetNextToken() == Token.Colon && prevToken == Token.Identifier)
                    {
                        if (!labels.ContainsKey(lex.Identifier))
                            labels.Add(lex.Identifier, lex.TokenMarker);
                        if (lex.Identifier == name)
                            break;
                    }
                    if (lastToken == Token.EOF)
                    {
                        Error("Cannot find label named " + name);
                    }
                }
            }
            lex.GoTo(labels[name]);
            lastToken = Token.NewLine;
        }

        async Task If()
        {
            // check if argument is equal to 0
            bool result = ((await Expr()).BinOp(new Value(0), Token.Equal).Real == 1);

            Match(Token.Then);
            GetNextToken();

            if (result)
            {
                // in case "if" evaulate to zero skip to matching else or endif
                int i = ifcounter;
                while (true)
                {
                    if (lastToken == Token.If)
                    {
                        i++;
                    }
                    else if (lastToken == Token.Else)
                    {
                        if (i == ifcounter)
                        {
                            GetNextToken();
                            return;
                        }
                    }
                    else if (lastToken == Token.EndIf)
                    {
                        if (i == ifcounter)
                        {
                            GetNextToken();
                            return;
                        }
                        i--;
                    }
                    GetNextToken();
                }
            }
        }

        void Else()
        {
            // skip to matching endif
            int i = ifcounter;
            while (true)
            {
                if (lastToken == Token.If)
                {
                    i++;
                }
                else if (lastToken == Token.EndIf)
                {
                    if (i == ifcounter)
                    {
                        GetNextToken();
                        return;
                    }
                    i--;
                }
                GetNextToken();
            }
        }

        void Label()
        {
            string name = lex.Identifier;
            if (!labels.ContainsKey(name)) labels.Add(name, lex.TokenMarker);

            GetNextToken();
            Match(Token.NewLine);
        }

        void End()
        {
            ShouldExit = true;
        }

        async Task Let()
        {
            if (lastToken != Token.Equal)
            {
                Match(Token.Identifier);
                GetNextToken();
                Match(Token.Equal);
            }

            string id = lex.Identifier;

            GetNextToken();

            SetVar(id, await Expr());
        }

        async Task For()
        {
            Match(Token.Identifier);
            string var = lex.Identifier;

            GetNextToken();
            Match(Token.Equal);

            GetNextToken();
            Value v = await Expr();

            // save for loop marker
            if (loops.ContainsKey(var))
            {
                loops[var] = lineMarker;
            }
            else
            {
                SetVar(var, v);
                loops.Add(var, lineMarker);
            }

            Match(Token.To);

            GetNextToken();
            v = await Expr();

            if (vars[var].BinOp(v, Token.More).Real == 1)
            {
                while (true)
                {
                    while (!(GetNextToken() == Token.Identifier && prevToken == Token.Next)) ;
                    if (lex.Identifier == var)
                    {
                        loops.Remove(var);
                        GetNextToken();
                        Match(Token.NewLine);
                        break;
                    }
                }
            }
        }

        void Next()
        {
            // jump to begining of the "for" loop
            Match(Token.Identifier);
            string var = lex.Identifier;
            vars[var] = vars[var].BinOp(new Value(1), Token.Plus);
            lex.GoTo(new Marker(loops[var].Pointer - 1, loops[var].Line, loops[var].Column - 1));
            lastToken = Token.NewLine;
        }

        async Task Assert()
        {
            bool result = ((await Expr()).BinOp(new Value(0), Token.Equal).Real == 1);

            if (result)
            {
                Error("Assertion fault"); // if out assert evaluate to false, throw error with souce code line
            }
        }



        async Task<Value> Expr(int min = 0)
        {
            // originally we were using shunting-yard algorithm, but now we parse it recursively 
            Dictionary<Token, int> precedens = new Dictionary<Token, int>()
            {
                { Token.Or, 0 }, { Token.And, 0 },
                { Token.Equal, 1 }, { Token.NotEqual, 1 },
                { Token.Less, 1 }, { Token.More, 1 },
                { Token.LessEqual, 1 },  { Token.MoreEqual, 1 },
                { Token.Plus, 2 }, { Token.Minus, 2 },
                { Token.Asterisk, 3 }, {Token.Slash, 3 },
                { Token.Caret, 4 }
            };

            Value lhs = await Primary();

            while (true)
            {
                if (lastToken < Token.Plus || lastToken > Token.And || precedens[lastToken] < min)
                    break;

                Token op = lastToken;
                int prec = precedens[lastToken]; // Operator Precedence
                int assoc = 0; // 0 left, 1 right; Operator associativity
                int nextmin = assoc == 0 ? prec : prec + 1;
                GetNextToken();
                Value rhs = await Expr(nextmin);
                lhs = lhs.BinOp(rhs, op);
            }

            return lhs;
        }

        async Task<Value> Primary()
        {
            Value prim = Value.Zero;

            if (lastToken == Token.Value)
            {
                // number | string
                prim = lex.Value;
                GetNextToken();
            }
            else if (lastToken == Token.Identifier)
            {
                // ident | ident '(' args ')'
                if (vars.ContainsKey(lex.Identifier))
                {
                    prim = vars[lex.Identifier];
                }
                else if (funcs.ContainsKey(lex.Identifier))
                {
                    string name = lex.Identifier;
                    List<Value> args = new List<Value>();
                    GetNextToken();
                    Match(Token.LParen);

                start:
                    if (GetNextToken() != Token.RParen)
                    {
                        args.Add(await Expr());
                        if (lastToken == Token.Comma)
                            goto start;
                    }

                    prim = await funcs[name](null, args);
                }
                else
                {
                    Error("Undeclared variable " + lex.Identifier);
                }
                GetNextToken();
            }
            else if (lastToken == Token.LParen)
            {
                // '(' expr ')'
                GetNextToken();
                prim = await Expr();
                Match(Token.RParen);
                GetNextToken();
            }
            else if (lastToken == Token.Plus || lastToken == Token.Minus || lastToken == Token.Not)
            {
                // unary operator
                // '-' | '+' primary
                Token op = lastToken;
                GetNextToken();
                Value help = await Primary();
                prim = help.UnaryOp(op);
            }
            else
            {
                Error("Unexpexted token in primary!");
            }

            return prim;
        }
    }
}
