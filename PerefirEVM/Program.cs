using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerefirEVM
{
    class Program
    {
        static void Main(string[] args)
        {
            double number = 513.361;
            string numInNotation = GetNumberWithNotationFromDecimal(16, number, 4);
            Console.WriteLine(numInNotation);
            Console.WriteLine(GetDecimalNumber(16, numInNotation));

            int inputValue;
            Console.WriteLine("Из какой системы счисления нужно перевести число?\n" +
                "[1]: Двоичная\n" +
                "[2]: Десятичная\n" +
                "[3]: Шестнадцатиричная");

            string inputStringValue = Console.ReadLine();
            bool isNumber = int.TryParse(inputStringValue, out inputValue);
            while (isNumber != true)
            {
                Console.WriteLine("Введите значение корректно");
                inputStringValue = Console.ReadLine();
                isNumber = int.TryParse(inputStringValue, out inputValue);
            }

            int fromNotation = 0;
            Console.WriteLine("В какую систему счисления нужно перевести число?\n");
            switch (inputValue)
            {
                case 1:
                    {
                        Console.WriteLine("[1]: Десятичная\n");
                        Console.WriteLine("[2]: Шестнадцатиричная\n");
                        fromNotation = 2;
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("[1]: Двоичная\n");
                        Console.WriteLine("[2]: Шестнадцатиричная\n");
                        fromNotation = 10;
                        break;
                    }
                default:
                    {
                        Console.WriteLine("[1]: Двоичная\n");
                        Console.WriteLine("[2]: Десятичная\n");
                        fromNotation = 16;
                        break;
                    }
            }

            inputStringValue = Console.ReadLine();
            isNumber = int.TryParse(inputStringValue, out inputValue);
            while (isNumber != true)
            {
                Console.WriteLine("Введите значение корректно");
                inputStringValue = Console.ReadLine();
                isNumber = int.TryParse(inputStringValue, out inputValue);
            }

            Console.WriteLine("Введите (точность) количество дробный цифр после запятой");

            int accuracy = -1;

            inputStringValue = Console.ReadLine();
            isNumber = int.TryParse(inputStringValue, out accuracy);
            while (isNumber != true && accuracy < 0)
            {
                Console.WriteLine("Введите значение корректно");
                inputStringValue = Console.ReadLine();
                isNumber = int.TryParse(inputStringValue, out accuracy);
            }

            string numberInNotation;
            bool isCorrect = false;
            inputStringValue = Console.ReadLine();

            while (isCorrect != true)
            {
                for (int i = 0; i < inputStringValue)
                {

                }
            }


            int toNotation = 0;
            switch (inputValue)
            {
                case 1:
                    {
                        switch (fromNotation)
                        {
                            
                        }
                        break;
                    }
            }

            //while
            //int number
        }
        
        /*
        public string GetNumberInNotation(int notation, string decimalNumber)
        {
            int number = Convert.ToInt32(decimalNumber);
            int receivedNumber = 0;
            int maxPower = 0;
            string numberInNotation = "";
            while (number < Math.Pow(notation, maxPower))
            {
                maxPower++;
            }

            while (number != receivedNumber)
            {

            }
        }
        */
        public static double GetDecimalNumber(int notation, string number)
        {
            int power = 0;
            int dotIndex = 0;
            int digit = 0;
            int integerPart = 0;
            double fractionPart = 0f;
            while(number[dotIndex] != '.' && number[dotIndex] != ',')
            {
                dotIndex++;
            }

            power = 0;
            for (int iter = dotIndex - 1; iter >= 0; iter--)
            {
                digit = number[iter] - 'A';
                if (digit < 0)
                    digit = number[iter] - '0';

                integerPart += digit * (int)Math.Pow(notation, power);

                power++;
            }

            power = 1;
            for (int iter = dotIndex + 1; iter < number.Length; iter++)
            {
                digit = number[iter] - 'A';
                if (digit < 0)
                    digit = number[iter] - '0';

                fractionPart += digit / Math.Pow(notation, power);
                power++;
            }

            return integerPart + fractionPart;
        }

        public static string GetNumberWithNotationFromDecimal(int notation, double decimalNumber, int accuracy)
        {
            string resultNumber = "";
            int digit = 0;
            char symbol = ' ';
            int integerPart = (int)Math.Truncate(decimalNumber);
            double doublePart = decimalNumber - integerPart;
            while (integerPart > 0)
            {
                digit = integerPart % notation;
                if (digit > 10)
                {
                    digit = digit + 6;
                }

                symbol = (char)(digit + 48);

                resultNumber = symbol + resultNumber;
                integerPart /= notation;
            }

            resultNumber += ',';
            int i = 0;
            while (doublePart != 0 && i < accuracy)
            {
                doublePart = doublePart * notation;
                digit = (int)Math.Truncate(doublePart);
                if (digit >= 10)
                {
                    digit = digit + 7;
                }

                symbol = (char)(digit + 48);
                doublePart = doublePart - (int)Math.Truncate(doublePart);

                resultNumber += symbol;
                i++;
            }

            return resultNumber;
        }
        
    }
}
