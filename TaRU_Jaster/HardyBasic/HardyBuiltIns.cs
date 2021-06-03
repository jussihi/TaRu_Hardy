using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaRU_Jaster.HardyBasic
{
    class HardyBuiltIns
    {
        public static void InstallAll(Interpreter interpreter)
        {
            interpreter.AddFunction("str", Str);
            interpreter.AddFunction("num", Num);
            interpreter.AddFunction("abs", Abs);
            interpreter.AddFunction("min", Min);
            interpreter.AddFunction("max", Max);
            interpreter.AddFunction("not", Not);
            interpreter.AddFunction("rnd", Rnd);
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

        
    }
}
