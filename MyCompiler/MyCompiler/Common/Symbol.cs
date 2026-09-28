using System;
using System.Data.Common;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using MyCompiler.Common;

namespace MyCompiler.Common
{
    public class Symbol
    {
        public string Name { get; set; }

        public TokenType DataType { get; set; }

        public bool IsInitialized { get; set; }

        public int ScopeLevel { get; set; }

        /*Value

        Address

        Size

        Scope

        ParameterCount

        ReturnType*/

        public Symbol(string name, TokenType dataType)
        {
            Name = name;
            DataType = dataType;
            IsInitialized = false;
            ScopeLevel = 0;
        }
    }

    public class SymbolTable
    {
        private Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();

        public bool Add(Symbol symbol)
        {
            if (symbols.ContainsKey(symbol.Name))
                return false;

            symbols.Add(symbol.Name, symbol);

            return true;
        }
        public bool Exists(string name)
        {
            return symbols.ContainsKey(name);
        }
        public Symbol Get(string name)
        {
            if (symbols.ContainsKey(name))
                return symbols[name];

            return null;
        }
        public void Remove(string name)
        {
            symbols.Remove(name);
        }
        public IEnumerable<Symbol> GetAll()
        {
            return symbols.Values;
        }
    }

}
