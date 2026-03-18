using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    public class Token // Cambiado de internal a public
    {
        public string Tipo { get; set; }
        public string Lexema { get; set; }
        public int Linea { get; set; }

        public Token(string tipo, string lexema, int linea)
        {
            Tipo = tipo;
            Lexema = lexema;
            Linea = linea;
        }

        public override string ToString()
        {
            return $"[{Linea}] {Tipo} -> {Lexema}";
        }
    }
}
