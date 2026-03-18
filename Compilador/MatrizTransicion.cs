using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    /* La matriz fue extendida manteniendo la arquitectura del AFD original,
     agregando nuevos estados para operadores compuestos y comentarios
    sin romper la lógica previa. */

    public class MatrizTransicion
    {
        // Columnas:
        // 0 = Letra o '_'
        // 1 = Dígito
        // 2 = Punto decimal '.'
        // 3 = Operadores = < > /
        // 4 = Símbolos especiales (+ - * ; : { } ( ) ,)
        // 5 = Espacio en blanco

        // Estados:
        // 0 = Inicio
        // 1 = Identificador
        // 2 = Número entero
        // 3 = Operador simple detectado
        // 4 = Punto detectado (posible real)
        // 5 = Número real
        // 6 = Posible operador compuesto (== >= <=)
        // 7 = Posible comentario (después de /)
        // 8 = Comentario de una línea

        private readonly int[,] _matriz =
         {
            //     L     D    .      OP    S       ESP
            /*0*/{ 1,    2,   500,   3,    300,    0   },   // Inicio
            /*1*/{ 1,    1,   100,   100,  100,    100 },   // Identificador
            /*2*/{ 200,  2,   4,     200,  200,    200 },   // Entero
            /*3*/{ 300,  300, 300,   6,    300,    300 },   // Operador simple
            /*4*/{ 500,  5,   500,   500,  500,    500 },   // Punto detectado
            /*5*/{ 400,  5,   400,   400,  400,    400 },   // Real
            /*6*/{ 300,  300, 300,   305,  300,    300 },   // Operador compuesto
            /*7*/{ 300,  300, 300,   8,    300,    300 },   // Posible comentario
            /*8*/{ 8,    8,   8,     8,    8,      8   }     // Comentario línea
         };


        public int ObtenerColumna(char c)
        {
            if (char.IsLetter(c) || c == '_') return 0;
            if (char.IsDigit(c)) return 1;
            if (c == '.') return 2;
            if ("=<>/".Contains(c)) return 3;
            if ("+-*;:{}(),\'".Contains(c)) return 4;
            if (char.IsWhiteSpace(c)) return 5;

            return -1; // Error léxico
        }

        public int SiguienteEstado(int estado, int columna)
        {
            // Si la columna es inválida, forzamos error lexico
            if (columna == -1)
                return 500;

            if (estado < 0 || estado >= _matriz.GetLength(0))
                return 500;
            if (columna < 0 || columna >= _matriz.GetLength(1))
                return 500;

            return _matriz[estado, columna];
        }


    }
}
