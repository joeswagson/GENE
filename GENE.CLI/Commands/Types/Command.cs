using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GENE.Flow.Typescript;
using GENE.Flow.Typescript.Members.Data;

namespace GENE.CLI.Commands.Types
{
    public abstract class Command
    {
        private const string BadOp = "An argument acquisition was attempted without an exeuction context.";
        private const string BadArg = "Failed to construct argument at index";

        public abstract string Identifier { get; }
        public abstract Usage Help();

        private string[]? _context;
        internal int ExecuteInternal(string[] args)
        {
            _context = args;
            var result = Execute(args);
            if (result is Exception ex)
            {
                Logger.Error(ex.Message);
                return ex.HResult;
            }
            _context = null;
            return (int) result;
        }

        public abstract object Execute(string[] args);

        private void CheckContext()
        {
            if (_context == null)
                throw new InvalidOperationException(BadOp);
        }

        private readonly Dictionary<Type, Func<string, object>> _simpleTypes = new()
        {
            { typeof(bool), s => bool.Parse(s) },
            { typeof(byte), s => byte.Parse(s) },
            { typeof(sbyte), s => sbyte.Parse(s) },
            { typeof(char), s => char.Parse(s) },
            { typeof(decimal), s => decimal.Parse(s) },
            { typeof(double), s => double.Parse(s) },
            { typeof(float), s => float.Parse(s) },
            { typeof(int), s => int.Parse(s) },
            { typeof(uint), s => uint.Parse(s) },
            { typeof(nint), s => nint.Parse(s) },
            { typeof(nuint), s => nuint.Parse(s) },
            { typeof(long), s => long.Parse(s) },
            { typeof(ulong), s => ulong.Parse(s) },
            { typeof(short), s => short.Parse(s) },
            { typeof(ushort), s => ushort.Parse(s) },
            { typeof(string), s => s },
        };

        protected T Argument<T>(int index, T? defaultValue = default, Func<string, T>? processor = null)
        {
            CheckContext();

            if(index >= _context!.Length)
                goto DEFAULT;
            
            var rawArg = _context![index];
            if (_simpleTypes.TryGetValue(typeof(T), out var func))
                return (T) func(rawArg);

            if (processor != null)
                return processor(rawArg);
            
            DEFAULT:
            return defaultValue ?? throw new ArgumentNullException(nameof(defaultValue));
        }
        protected T Required<T>(int index, Func<string, T>? processor = null)
        {
            CheckContext();

            if(index >= _context!.Length)
                goto DEFAULT;
            
            var rawArg = _context![index];
            if (_simpleTypes.TryGetValue(typeof(T), out var func))
                return (T) func(rawArg);

            if (processor != null)
                return processor(rawArg);
            
            DEFAULT:
            throw new ArgumentNullException(BadArg);
        }
    }
}