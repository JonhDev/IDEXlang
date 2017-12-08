using IDEXlan.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IDEXlan.Analizer
{
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
        public const string isEnt = "^(\u005Cn)*(((cons )*)(ent) ((([a-z|A-Z]|_)+|(([a-z|A-Z]|_)[0-9]*)) = (\u002D*)([0-9]+)(,){0,1})+);$";
        public const string isDec = "^(\u005Cn)*(((cons )*)(dec) ((([a-z|A-Z]|_)+|(([a-z|A-Z]|_)[0-9]*)) = (\u002D*)([0-9]+).([0-9]+)(,){0,1})+);$";
        public const string isCad = "^(\u005Cn)*(((cons )*)(cad) ((([a-z|A-Z]|_)+|(([a-z|A-Z]|_)[0-9]*)) = \u0022(([0-9]|[a-z|A-Z]|_|.)*)\u0022(,){0,1})+);$";

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

        public string LineComp(string line)
        {
            if (DefVar.IsMatch(line))
            {
                return "definicion de variable";
            }

            if (DefCons.IsMatch(line))
            {
                return "definicion de constante";
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

