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
                numPyC = 0;
                
                error.Add(DefVar(i + 1,lineas[i]));
            }

            if (carEsp.Count > 0)
                error.Add(new ErrorTableModel { Line = 1, Error = "Caracteres especiales no balanceados" });
            if (hayComillas)
                error.Add(new ErrorTableModel { Line = 1, Error = "Caracteres ' \" ' sin cierre" });


            return error;
        }

        public ErrorTableModel DefVar(int line,string token)
        {
            string error="",valor;

            ExpresionesReg expresion = new ExpresionesReg();
            
            valor= expresion.ConvertirToken(token);

            if (valor == "tipo de dato")
                error = "";
            else
                error = "erro en declaracion devariable";
            return new ErrorTableModel { Line = line, Error=error };
        }

    }
}
