using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador
{
    public class AnalizadorLexico
    {
        private readonly MatrizTransicion _matriz;
        private readonly PalabrasReservadas _palabrasReservadas;

        public AnalizadorLexico()
        {
            _matriz = new MatrizTransicion();
            _palabrasReservadas = new PalabrasReservadas();
        }

        public ResultadoLexico Analizar(CodigoFuente fuente)
        {
            var resultado = new ResultadoLexico();
            resultado.AgregarAviso("LÉXICO INICIADO");

            try
            {
                for (int numLinea = 1; numLinea <= fuente.TotalLineas; numLinea++)
                {
                    ProcesarLinea(fuente.ObtenerLinea(numLinea), numLinea, resultado);
                }

                resultado.AgregarAviso("LÉXICO FINALIZADO EXITOSAMENTE");
            }
            catch (Exception ex)
            {
                resultado.AgregarAviso("ERROR GRAVE EN LÉXICO: " + ex.Message);
            }

            return resultado;
        }

        private void ProcesarLinea(string lineaOriginal, int numLinea, ResultadoLexico resultado)
        {
            int estado = 0;
            var lexema = new StringBuilder();
            string linea = (lineaOriginal ?? string.Empty) + " "; // Centinela

            for (int i = 0; i < linea.Length; i++)
            {
                char c = linea[i];
                int columna = _matriz.ObtenerColumna(c);
                int valorMatriz = _matriz.SiguienteEstado(estado, columna);
               
                 /* Se implemento un corte inmediato del análisis cuando se detecta // ,
                   ya que los comentarios de una línea no generan tokens y deben ignorarse 
                   completamente.*/

                // --- DETECCÍÓN DIRECTA DE COMENTARIO ---
                if (c == '/' && i + 1 < linea.Length && linea[ i + 1] == '/')
                {
                    string comentario = linea.Substring(i);
                    resultado.AgregarAviso($"Comentario detectado en linea {numLinea}: {comentario}");
                    // Al detectar // ignoramos el resto de la línea
                    break;
                }    

                    // --- ESTADO DE ERROR ---
                    if (valorMatriz >= 500)
                {
                    resultado.AgregarAviso($"ERROR LÉXICO: Símbolo '{c}' no válido en línea {numLinea}");
                    estado = 0;
                    lexema.Clear();
                    continue;
                }

                // --- TRANSICIÓN ENTRE ESTADOS ---
                if (valorMatriz < 100 && valorMatriz > 0)
                {
                    estado = valorMatriz;
                    lexema.Append(c);
                }
                else if (valorMatriz == 0) // Espacios o resets
                {
                    estado = 0;
                    lexema.Clear();
                }

                // --- ESTADOS DE ACEPTACIÓN (TOKENS) ---
                else if (valorMatriz >= 100)
                {
                    string lex = lexema.ToString();
                    int tokenID = 0;

                    switch (valorMatriz)
                    {
                        case 100: // Fin de ID o Palabra Reservada
                            tokenID = _palabrasReservadas.ObtenerToken(lex);
                            i--; // Retrocedemos para procesar el símbolo que cerró el ID
                            break;

                        case 200: // Fin de Número Entero
                            tokenID = 200;
                            i--;
                            break;

                        case 300: // Símbolo Directo (Ej: +, ;, :)
                            lex = c.ToString();
                            tokenID = _palabrasReservadas.ObtenerToken(lex);
                            // No hace falta i-- porque el símbolo YA es el token
                            break;

                        case 305: // Operador compuesto
                            tokenID = _palabrasReservadas.ObtenerToken(lex);
                            break;

                        case 400: // Fin de Número Real
                            tokenID = 400;
                            i--;
                            break;
                    }

                    if (tokenID > 0)
                    {
                        resultado.Tokens.Add(new Token(tokenID.ToString(), lex, numLinea));
                        Console.WriteLine($"[Línea {numLinea}] TOKEN: {tokenID} | LEXEMA: {lex}");
                    }

                    lexema.Clear();
                    estado = 0;
                }
            }
        }

    }
}
