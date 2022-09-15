using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab02
{
    public class ArithmeticCoding
    {
        public double Coding(string texto, Dictionary<char, Letra> dictionario)
        {
            double I = 0, S = 0, AI = 0, AS = 1;
            for (int i = 0; i < texto.Length; i++)
            {
                double NI = dictionario[texto[i]].inferior;
                double NS = dictionario[texto[i]].superior;
                I = AI + (AS - AI) * NI;
                S = AI + (AS - AI) * NS;
                AI = I;
                AS = S;
            }
            return I;
        }

        public string Decode(double code, Dictionary<char, Letra> dictionario)
        {
            string resul = "";            
            while (resul.Length < 13)
            {
                double inf, sup;
                foreach (var item in dictionario)
                {
                    inf = item.Value.inferior;
                    sup = item.Value.superior;
                    if (code >= inf && code < sup)
                    {
                        resul += item.Key;                        
                        code = (code - inf) / (sup - inf);
                    }
                }                
            }
            return resul;
        }
    }
}
