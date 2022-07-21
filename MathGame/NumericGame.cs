using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathGame
{
    class NumericGame
    {
        public int CurrentNumber { get; set; }
        private int NextNumber { get; set; }
        private int[] array = new int[25];


        public NumericGame()
        {
            for (int i = 0; i < 25; i++)
            {
                array[i] = i + 1;
            }
            ShuffleElements();
        }

        private void ChangeNumbers()
        {
            CurrentNumber++;
        }

        public void SetNumber(int NextValue)
        {
            this.NextNumber = NextValue;
        }

        public bool IsNumbersSequent()
        {
            if (CurrentNumber + 1 == NextNumber)
            {
                ChangeNumbers();
                return true;
            }

            return false;
        }


        private void ShuffleElements()
        {
            Random random = new Random();
            for (int i = 25 - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                var temp = array[j];
                array[j] = array[i];
                array[i] = temp;
            }
        }

        public int[] GetNumbers()
        {
            return array;
        }
    }

}
