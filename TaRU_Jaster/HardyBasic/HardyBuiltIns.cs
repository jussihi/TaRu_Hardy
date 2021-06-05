using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaRU_Jaster.HardyBasic
{
    class HardyBuiltIns
    {
        public static HardyExecutor _HardyExecutor;
        public static List<int> _targets;

        public static void InstallAll(Interpreter interpreter)
        {
            interpreter.AddFunction("str", Str);
            interpreter.AddFunction("num", Num);
            interpreter.AddFunction("abs", Abs);
            interpreter.AddFunction("min", Min);
            interpreter.AddFunction("max", Max);
            interpreter.AddFunction("not", Not);
            interpreter.AddFunction("rnd", Rnd);
            interpreter.AddFunction("sleep", ExecutorSleep);
            interpreter.AddFunction("reset", Reset);
            interpreter.AddFunction("up", Up);
            interpreter.AddFunction("down", Down);
        }

        public static async Task<Value> Str(Interpreter interpreter, List<Value> args)
        {
            if (args.Count < 1)
                throw new ArgumentException();

            return args[0].Convert(ValueType.String);
        }

        public static async Task<Value> Num(Interpreter interpreter, List<Value> args)
        {
            if (args.Count < 1)
                throw new ArgumentException();

            return args[0].Convert(ValueType.Real);
        }

        public static async Task<Value> Abs(Interpreter interpreter, List<Value> args)
        {
            if (args.Count < 1)
                throw new ArgumentException();

            return new Value(Math.Abs(args[0].Real));
        }

        public static async Task<Value> Min(Interpreter interpreter, List<Value> args)
        {
            if (args.Count < 2)
                throw new ArgumentException();

            return new Value(Math.Min(args[0].Real, args[1].Real));
        }

        public static async Task<Value> Max(Interpreter interpreter, List<Value> args)
        {
            if (args.Count < 1)
                throw new ArgumentException();

            return new Value(Math.Max(args[0].Real, args[1].Real));
        }

        public static async Task<Value> Not(Interpreter interpreter, List<Value> args)
        {
            if (args.Count < 1)
                throw new ArgumentException();

            return new Value(args[0].Real == 0 ? 1 : 0);
        }

        public static async Task<Value> Rnd(Interpreter interpreter, List<Value> args)
        {
            if (args.Count != 2)
                throw new ArgumentException("RND expects exactly two integer values!");


            Random rd = new Random();
            int rand_num;
            try
            {
                rand_num = rd.Next(Convert.ToInt32(args[0].Real), Convert.ToInt32(args[1].Real));
            }
            catch(Exception ex)
            {
                throw new ArgumentException("RND expects integer values! Error: " + ex.Message);
            }
            
            return new Value(rand_num);
        }

        private static List<int> ParseTargets(List<Value> args)
        {
            List<int> ret = new List<int>();

            if (args.Count == 0)
            {
                ret.AddRange(Enumerable.Range(1, 30));
                return ret;
            }

            foreach (Value target in args)
            {
                ret.Add(Convert.ToInt32(target.Real));
            }
            return ret;
        }

        public static async Task<Value> ExecutorSleep(Interpreter interpreter, List<Value> args)
        {
            if (args.Count != 1)
                throw new ArgumentException("SLEEP expects exactly one integer value!");


            await Task.Delay(Convert.ToInt32(args[0].Real));

            return Value.Zero;
        }

        public static async Task<Value> Reset(Interpreter interpreter, List<Value> args)
        {
            List<int> targets = ParseTargets(args);

            await _HardyExecutor.OneShotTargetsSimpleExecute(targets, HardyExecutor.OneShotCommand.Reset);
            return Value.Zero;
        }

        public static async Task<Value> Up(Interpreter interpreter, List<Value> args)
        {
            List<int> targets = ParseTargets(args);

            await _HardyExecutor.OneShotTargetsSimpleExecute(targets, HardyExecutor.OneShotCommand.Up);
            return Value.Zero;
        }

        public static async Task<Value> Down(Interpreter interpreter, List<Value> args)
        {
            List<int> targets = ParseTargets(args);

            await _HardyExecutor.OneShotTargetsSimpleExecute(targets, HardyExecutor.OneShotCommand.Down);
            return Value.Zero;
        }
    }
}
