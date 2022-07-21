using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MathGame
{
    public class WordsGame
    {
        public Color Colorize()
        {
            Random rnd = new Random();
            int a = rnd.Next(1, 5);
            switch (a)
            {
                case 1:
                    return ColorerBlue();
                case 2:
                    return COlorerYellow();
                case 3:
                    return ColorerGreen();
                case 4:
                    return  ColorerRed();
                default:
                    return ColorerBlue();
            }
            
        }
        public string Wordize()
        {
            Random rnd = new Random();
            int a = rnd.Next(1, 5);
            switch (a)
            {
                case 1:
                    return "Красный";
                case 2:
                    return "Синий";
                case 3:
                    return "Жёлтый";
                case 4:
                    return "Зеленый";
                default:
                    return "Желтый";
            }
        }
        private Color ColorerBlue()
        {
            return Colors.Blue;
        }
        private Color ColorerRed()
        {
            return Colors.Red;
        }
        private Color COlorerYellow()
        {
            return Colors.Yellow;
        }
        private Color ColorerGreen()
        {
            return Colors.Green;

        }
    }
}
