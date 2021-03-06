﻿using IDEXlan.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IDEXlan.Analizer
{
    //Clase que contine las expresiones regulares que ayudan en el analisis lexico y sintactico del compilador
   public  class ExpresionesReg
    {
        public const string letra = "^[a-z|A-Z]$";
        public const string digito = "^[0-9]$";
        public const string OpLog = "^(&|\u007c\u007c|&&|!)$";
        public const string OpRel = "[<|>|=|>=|<=|==|!=]";
        public const string OpUni = "[\u002B]{2}$|[--]{2}$";
        public const string esp = "[\u0020]";
        public const string CarEsp = "[\u005d|\u0040|\u0028|\u0029|{|}|#|\u003f|\u005b]";
        public const string OpMat = "^[-|\u002b|\u002a|%|/]$";
        public const string Num = "^[0-9]+$";
        public const string palRes = "si|mientras|para|leer|imp|log";
        public const string TipDat = "ent|cad|dec";
        public const string dec = "^[0-9]*[.][0-9]+?$";
        public const string cad = "^(\"(" + digito + "|" + letra + "|" + OpLog + "|" + CarEsp + "|" + esp + ")+^\")$";
        public const string vari = "^([a-z|A-Z]|_)+|(([a-z|A-Z]|_)[0-9])$";
        public const string operacion = "(" + Num + "|" + dec + ")" + OpMat + "(" + Num + "|" + dec + "|" + ")";
        public const string consNum = "[[" + TipDat + "]=[" + Num + "|" + dec + "];]";
        public const string defVar = "^((\u005Cn)*(ent|cad|dec) ((([a-z|A-Z]|_)+|(([a-z|A-Z]|_)[0-9]*))(,){0,1})+);$";
        public const string defCons = "^(\u005Cn)*(cons (ent|cad|dec) ((([a-z|A-Z]|_)+|(([a-z|A-Z]|_)[0-9]*)) = (\"[a-z|A-Z]*\"|[0-9]*)(,){0,1})+);$";
        public const string isEnt = "^(\u005Cn)*(((cons )*)(ent) ((([a-z|A-Z]|_)+(([a-z|A-Z]|_|[0-9])*)) = (\u002D*)([0-9]+)(,){0,1})+);$";
        public const string isDec = "^(\u005Cn)*(((cons )*)(dec) ((([a-z|A-Z]|_)+(([a-z|A-Z]|_|[0-9])*)) = (\u002D*)([0-9]+).([0-9]+)(,){0,1})+);$";
        public const string isCad = "^(\u005Cn)*(((cons )*)(cad) ((([a-z|A-Z]|_)+(([a-z|A-Z]|_|[0-9])*)) = \u0022(([0-9]|[a-z|A-Z]|_|.)*)\u0022(,){0,1})+);$";
        public const string asigVar = "^(\u005Cn)*(((cons )*)(cad|ent|dec) ((([a-z|A-Z]|_)+(([a-z|A-Z]|_|[0-9])*)) = (([a-z|A-Z|_]+)([a-z|A-Z|_|[0-9])*)(,){0,1})+);$";

        //Aqui comienza el analisis por medio de Regex para el analisis lexico
        //TODO Mejora: Buscar mejores maneras de programar esto 
        Regex numero = new Regex(Num);
        Regex log = new Regex(OpLog);
        Regex rel = new Regex(OpRel);
        Regex uni = new Regex(OpUni);
        Regex car = new Regex(CarEsp);
        Regex palabras = new Regex(palRes);
        Regex dat = new Regex(TipDat);
        Regex decim = new Regex(dec);
        Regex varib = new Regex(vari);
        Regex opera = new Regex(operacion);
        Regex constNum = new Regex(consNum);

        Regex DefVar = new Regex(defVar);
        Regex DefCons = new Regex(defCons);


        Regex pru = new Regex(OpMat);

        //Analizador lexico
        public string ConvertirToken(string token)
        {
            if (palabras.IsMatch(token))
            {
                return "palabra reservada";
            }

            if (constNum.IsMatch(token))
            {
                return "asignacion numero";
            }

            if (dat.IsMatch(token))
            {
                return "tipo de dato";
            }

            if (uni.IsMatch(token))
            {
                return "operador unitario";
            }

            if (opera.IsMatch(token))
            {
                return "operacion matematica";
            }

            if (pru.IsMatch(token))
            {
                return "operador matematico";
            }

            if (decim.IsMatch(token))
            {
                return "numero decimal";
            }

            if (numero.IsMatch(token))
            {
                return "numero";
            }

            if (rel.IsMatch(token))
            {
                return "operador relacional";
            }

            if (log.IsMatch(token))
            {
                return "operador logico";
            }

            if (car.IsMatch(token))
            {
                return "caracter especial";
            }

            if (varib.IsMatch(token))
            {
                return "variable";
            }

            if (token[0]=='"' && token[token.Length-1]=='"')
            {
                return "cadena";
            }
            return "No asginado";
        }

        //Termina TODO Mejora

        //Este metodo valida la correcta declaracion de los tres tipos de datos (cad, ent, dec) de tipo: "ent var1 = 10; ent var2 = var1; cad var3 = "cadena"; "
        //A traves de expresiones regulares identifica si la sintaxis de decalracion es correcta
        public string CorrectaDeclaracion(string line, VarTypes tipo)
        {
            Regex r;
            switch(tipo)
            {
                case VarTypes.Ent:
                    r = new Regex(isEnt);
                    return r.IsMatch(line) ? "declaracion entero" : "Error tipo dato entero";
                case VarTypes.Dec:
                    r = new Regex(isDec);
                    return r.IsMatch(line) ? "declaracion decimal" : "Error tipo dato decimal";
                case VarTypes.Cad:
                    r = new Regex(isCad);
                    return r.IsMatch(line) ? "declaracion cadena" : "Error tipo dato cadena";
                default:
                    throw new Exception("Default case");
            }
        }

        //Metodo que valida la definicion de cadenas con expresiones regulares de tipo "ent var1, var2, var3, var4;
        public string LineComp(string line, int nLine, string allCode)
        {
            if (DefVar.IsMatch(line))
            {
                return "definicion de variable";
            }

            if (DefCons.IsMatch(line))
            {
                return "definicion de constante";
            }

            if(line.Contains("ent")||line.Contains("cad")||line.Contains("dec") && line.Contains("="))
            {
                Regex r = new Regex(asigVar);
                if (r.IsMatch(line))
                {
                    var splitLine = line.Split('=');
                    string varToSearch = splitLine[1].Substring(1, splitLine[1].Length - 2);
                    var spl = allCode.Split('\r');
                    int n = 0;
                    for(int i = 0; i < nLine; i++)
                    {
                        var temp = spl[i].Split(' ');
                        foreach (var item in temp)
                        {
                            if (item == varToSearch || item == (varToSearch +";"))
                                n++;
                        }
                    }
                    if(n>1)
                        return "definicion con variable";
                    else
                        return $"no existe la variable|{varToSearch}";
                }

            }

            if(line.Contains("ent") && line.Contains("="))
            {
                return CorrectaDeclaracion(line, VarTypes.Ent);
            }

            if (line.Contains("dec") && line.Contains("="))
            {
                return CorrectaDeclaracion(line, VarTypes.Dec);
            }

            if (line.Contains("cad") && line.Contains("="))
            {
                return CorrectaDeclaracion(line, VarTypes.Cad);
            }

            return "Error";

        }
    }
}

