using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathGame
{
    class Mather
    {
        private bool result;
        public Signs sign { get; set; }
        public double num1 { get; set; }
        public double num2 { get; set; }
        public double num3 { get; set; }

        public Mather(double num1, double num2, double num3)
        {
            this.num1 = num1;
            this.num2 = num2;
            this.num3 = num3;
        }

        public bool GetResult(Signs signs)
        {
            switch (signs)
            {
                case Signs.plus:
                    return result = num1 + num2 == num3;
                case Signs.minus:
                    return result = num1 - num2 == num3;
                case Signs.divide:
                    return result = num1 / num2 == num3;
                case Signs.muliply:
                    return result = num1 * num2 == num3;
                default:
                    break;
            }
            return result = false;
        }

        public string GetResultTranslation()
        {
            return result ? "Верно" : "Неверно";
        }

        public static string GetSignTranslation(Signs sign)
        {
            switch (sign)
            {
                case Signs.plus:
                    return "+";
                case Signs.minus:
                    return "-";
                case Signs.divide:
                    return ":";
                case Signs.muliply:
                    return "*";
                default:
                    break;
            }
            return null;
        }
    }
    public enum Signs
    {
        plus,
        minus,
        divide,
        muliply
    }
}
