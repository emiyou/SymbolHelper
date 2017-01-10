using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymbolAnalysis
{
    //public class Gate
    public enum SymbolResult
    {
        NameNull,
        AddNull,
        AddError,
        AddOut,

    }

    public class Symbol
    {
        public string Name { get;  }
        public string Address { get; }
        public string Type { get; set; }
        public string Comment { get;  }

        public Symbol(string Name, string Address, string Type, string Comment)
        {
            this.Name = Name;
            this.Address = Address;
            this.Type = Type;
            this.Comment = Comment;
        }

        public bool isValidate()
        {
            //if(string.IsNullOrEmpty(Name))
            return false;
        }
    }
}
