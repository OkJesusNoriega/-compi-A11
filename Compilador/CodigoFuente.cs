using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    public class CodigoFuente
    {
        private readonly List<string> _lineas;

        public CodigoFuente(string textoFuente)
        {
            if (textoFuente == null)
                textoFuente = string.Empty;

            // Normalizar saltos de línea
            textoFuente = textoFuente.Replace("\r\n", "\n")
                                     .Replace("\r", "\n");

            _lineas = textoFuente.Split('\n').ToList();
        }

        public int TotalLineas => _lineas.Count;

        public string ObtenerLinea(int numeroLinea)
        {
            if (numeroLinea < 1 || numeroLinea > _lineas.Count)
                throw new ArgumentOutOfRangeException(nameof(numeroLinea));

            return _lineas[numeroLinea - 1];
        }

        public IEnumerable<string> ObtenerLineas()
        {
            return _lineas;
        }
    }
}
