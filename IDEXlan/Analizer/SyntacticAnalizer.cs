﻿using IDEXlan.Helpers;
using IDEXlan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDEXlan.Analizer
{
    public class SyntacticAnalizer
    {
        public string Code { get; set; }
        public SyntacticAnalizer(string code)
        {
            Code = code;
        }

        public List<ErrorTableModel> Analize()
        {
            List<ErrorTableModel> error = new List<ErrorTableModel>();
            Stack<char> carEsp = new Stack<char>();
            bool hayComillas = false;
            string linea="";
            

            string[] lineas = Code.Split('\r');
            int numPyC = 0;
            for (int i = 0; i < lineas.Length; i++)
            {
                
                foreach (char c in lineas[i])
                {
                    if (c == ';')
                    {
                        numPyC++;
                    }
                    if (c == '(' || c == '{' || c == '[' || c == '"')
                    {
                        if (c == '"')
                        {
                            if (!hayComillas)
                            {
                                carEsp.Push(c);
                                hayComillas = true;
                            }
                            else
                            {
                                carEsp.Pop();
                                hayComillas = false;
                            }
                        }
                        else if (!hayComillas) carEsp.Push(c);
                    }
                    else if (c == ')' || c == '}' || c == ']')
                    {
                        if (!hayComillas && carEsp.Count > 0)
                        {
                            if (carEsp.Count == 0)
                                error.Add(new ErrorTableModel { Line = i + 1, Error = $"Error: se esperaba apertura de {c} " });
                            if (carEsp.Peek() == '(' && c == ')')
                                carEsp.Pop();
                            else if (carEsp.Peek() == '{' && c == '}')
                                carEsp.Pop();
                            else if (carEsp.Peek() == '[' && c == ']')
                                carEsp.Pop();
                            else
                            {
                                error.Add(new ErrorTableModel { Line = i + 1, Error = $"Error: caracter {c} no balanceado" });
                                carEsp.Pop();
                            }
                        }
                    }
                }



                if (numPyC > 1)
                    error.Add(new ErrorTableModel { Line = i + 1, Error = "No puede haber mas de un ';' en una linea" });
                else
                    if (!(lineas[i][lineas[i].Length - 1] == ';'))
                    error.Add(new ErrorTableModel { Line = i + 1, Error = "Error: Se esperaba ';'" });
                else
                {
                    linea += lineas[i];
                    if(DefVar(i + 1, linea, Code)!=null)
                        error.Add(DefVar(i + 1, linea, Code));
                }
                linea = "";
                numPyC = 0;
            }

            if (carEsp.Count > 0)
                error.Add(new ErrorTableModel { Line = 1, Error = "Caracteres especiales no balanceados" });
            if (hayComillas)
                error.Add(new ErrorTableModel { Line = 1, Error = "Caracteres ' \" ' sin cierre" });


            return error;
        }

        public ErrorTableModel AsignacionVars(VarTypes tipo, string line)
        {
            ExpresionesReg reg = new ExpresionesReg();
            //bool ans = reg.CorrectaDeclaracion(linea, tipo);
            return null;
        }

        public ErrorTableModel DefVar(int nLine,string sLine, string allCode)
        {
            string error="",valor;

            ExpresionesReg expresion = new ExpresionesReg();
            
            valor= expresion.LineComp(sLine, nLine, allCode);

            if (valor == "definicion de variable")
                return null;
            else if (valor == "definicion de constante")
                return null;
            else if (valor == "declaracion entero" || valor == "declaracion decimal" || valor == "declaracion cadena")
                return null;
            else if (valor == "definicion con variable")
                return null;
            else if (valor.Contains("no existe la variable"))
                return new ErrorTableModel { Line = nLine, Error = $"Error en declaracion de variable, no existe: {valor.Split('|')[1]}" };
            else if (valor == "Error tipo dato entero")
                return new ErrorTableModel { Line = nLine, Error = "Error en declaracion de variable de tipo entera" };
            else if (valor == "Error tipo dato decimal")
                return new ErrorTableModel { Line = nLine, Error = "Error en declaracion de variable de tipo decimal" };
            else if (valor == "Error tipo dato cadena")
                return new ErrorTableModel { Line = nLine, Error = "Error en declaracion de variable de tipo cadena" };
            error = "error en declaracion de variables";
            return new ErrorTableModel { Line = nLine, Error=error };
        }

    }
}
