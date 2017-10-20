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
        public static readonly string letra = "[a-z|A-Z]";
        public static readonly string digito = "^[0-9]$";
        public static readonly string OpLog = "[&|\u007c\u007c|&&|!]";
        public static readonly string OpRel = "[<|>|=|>=|<=|==|!=]";
        public static readonly string OpUni = "^[\u002B]{2}$|^[--]{2}$";
        public static readonly string esp = "[\u0020]";
        public static readonly string CarEsp = "[\u005d|\u0040|\u0028|\u0029|{|}|#|\u003f|\u005b]";
        public static readonly string OpMat = @"^(\-|\u002b|\u002a|%|/)$";
        public static readonly string Num = "^("+digito + "+)$";
        public static readonly string palRes = "si|mientras|para|leer|imp|log";
        public static readonly string TipDat = "ent|cad|dec";
        public static readonly string dec = digito + "*[.]" + Num;
        public static readonly string cad = "^\"$[" + digito + "|" + letra + "|" + OpLog + "|" + CarEsp + "|" + esp + "]*^\"$";
        public static readonly string vari = "^(" + letra + "|_)[" + letra + "|" + digito + "|_]*$";
        public static readonly string operacion = "(" + Num + "|" + dec + ")" + OpMat + "(" + Num + "|" + dec + "|" + ")";
        public static readonly string consNum = "[[" + TipDat + "]=[" + Num + "|" + dec + "];]";

        Regex numero = new Regex(Num);
        Regex log = new Regex(OpLog);
        Regex rel = new Regex(OpRel);
        Regex uni = new Regex(OpUni);
        Regex car = new Regex(CarEsp);
        Regex cadena = new Regex(cad);
        Regex palabras = new Regex(palRes);
        Regex dat = new Regex(TipDat);
        Regex decim = new Regex(dec);
        Regex varib = new Regex(vari);
        Regex opera = new Regex(operacion);
        Regex readonlyNum = new Regex(consNum);


        Regex pru = new Regex(OpMat);

        public string ConvertirToken(string token)
        {

            if (palabras.IsMatch(token))
            {
                return "palabra reservada";
            }

            if (readonlyNum.IsMatch(token))
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

            if (cadena.IsMatch(token))
            {
                return "cadena";
            }
            return "No asginado";
        }
    }
}

