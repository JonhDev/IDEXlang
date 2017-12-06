using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotacionPolaca.Lib
{
    public class NotPolaca
    {
        Queue<char> salida = new Queue<char>();
        Stack<char> pila = new Stack<char>();

        public string ObtenerNotacion(string cadena)
        {
            for (int i = 0; i < cadena.Length; i++)
            {
                if (cadena[i] != '+' && cadena[i] != '-' && cadena[i] != '*' && cadena[i] != '/' && cadena[i] != '(' && cadena[i] != ')')
                    salida.Enqueue(cadena[i]);
                else if (pila.Count == 0 && (cadena[i] == '+' || cadena[i] == '-' || cadena[i] == '*' || cadena[i] == '/'))
                    pila.Push(cadena[i]);
                else if (pila.Count > 0 && (pila.Peek() == '('))
                    pila.Push(cadena[i]);
                else if (pila.Count > 0 && ((pila.Peek() == '+' || pila.Peek() == '-') && (cadena[i] == '+' || cadena[i] == '-')))
                {
                    salida.Enqueue(pila.Pop());
                    pila.Push(cadena[i]);
                }
                else if (pila.Count > 0 && ((pila.Peek() == '*' || pila.Peek() == '/') && (cadena[i] == '*' || cadena[i] == '/' || cadena[i] == '+' || cadena[i] == '-')))
                {
                    salida.Enqueue(pila.Pop());
                    if(pila.Count > 0)
                    {
                        if ((pila.Peek() == '+' || pila.Peek() == '-') && (cadena[i] == '+' || cadena[i] == '-'))
                        {
                            salida.Enqueue(pila.Pop());
                            pila.Push(cadena[i]);
                        }
                        else
                        {
                            pila.Push(cadena[i]);
                        }
                    }
                    else
                        pila.Push(cadena[i]);

                }
                else if (cadena[i] == '(')
                    pila.Push(cadena[i]);
                else if (cadena[i] == ')')
                {
                    while (pila.Peek() != '(')
                        salida.Enqueue(pila.Pop());
                    pila.Pop();
                }    
                else
                {
                    pila.Push(cadena[i]);
                }
            }
            int cuenta = pila.Count;
            for (int i = 0; i < cuenta; i++)
                salida.Enqueue(pila.Pop());

            StringBuilder cadenaFinal = new StringBuilder();
            foreach (var item in salida)
                cadenaFinal.Append(item);
            return cadenaFinal.ToString();
        }

        public double ObtenerResultado(string exprNotPolaca)
        {
            Stack<string> pila = new Stack<string>();
            foreach (var item in exprNotPolaca)
            {
                if (item == '+' || item == '-' || item == '*' || item == '/')
                {
                    double num2 = double.Parse(pila.Pop() + "");
                    double num1 = double.Parse(pila.Pop() + "");
                    if (item == '+')
                        pila.Push((num1 + num2) + "");
                    if (item == '-')
                        pila.Push((num1 - num2) + "");
                    if (item == '*')
                        pila.Push((num1 * num2) + "");
                    if (item == '/')
                        pila.Push((num1 / num2) + "");
                }
                else
                    pila.Push(item + "");
            }
            return double.Parse(pila.Pop());
        }

    }
}
