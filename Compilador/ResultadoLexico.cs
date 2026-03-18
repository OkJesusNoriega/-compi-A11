using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    public class ResultadoLexico // Cambiado de internal a public
    {
        public List<Token> Tokens { get; } = new List<Token>();
        // Cambiamos el nombre de 'strings' a 'Avisos'
        public List<string> Avisos { get; } = new List<string>();

        public void AgregarAviso(string mensaje)
        {
            Avisos.Add(mensaje); // Ahora el nombre coincide
        }
    }
}
