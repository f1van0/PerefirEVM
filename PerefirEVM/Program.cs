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
            //double number = 513.361;
            

            List<char> notationAlphabetFrom = new List<char>();// { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };//, '10', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            List<char> notationAlphabetTo = new List<char>();
            List<int> notations = new List<int>() { 2, 10, 16 };

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
            Console.WriteLine("\nВ какую систему счисления нужно перевести число?");
            switch (inputValue)
            {
                case 1:
                    {
                        Console.WriteLine("[1]: Десятичная");
                        Console.WriteLine("[2]: Шестнадцатиричная");
                        fromNotation = 2;
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("[1]: Двоичная");
                        Console.WriteLine("[2]: Шестнадцатиричная");
                        fromNotation = 10;
                        break;
                    }
                default:
                    {
                        Console.WriteLine("[1]: Двоичная");
                        Console.WriteLine("[2]: Десятичная");
                        fromNotation = 16;
                        break;
                    }
            }

            notations.Remove(fromNotation);

            for (int i = 0; i < fromNotation; i++)
            {
                if (i <= 9)
                    notationAlphabetFrom.Add((char)(48 + i));
                else if (i >= 10)
                    notationAlphabetFrom.Add((char)(55 + i));
            }

            //string numInNotation = GetNumberWithNotationFromDecimal(2, notationAlphabet, 10.1f, 4);
            //Console.WriteLine(numInNotation);
            //Console.WriteLine(GetDecimalNumber(2, notationAlphabet, numInNotation));

            //Ввод системы счисления, в которую будет переведено число 
            int toNotation = 0;
            inputStringValue = Console.ReadLine();
            isNumber = int.TryParse(inputStringValue, out inputValue);
            while (isNumber != true)
            {
                Console.WriteLine("Введите значение корректно");
                inputStringValue = Console.ReadLine();
                isNumber = int.TryParse(inputStringValue, out inputValue);
            }

            if (inputValue == 1)
                toNotation = notations[0];
            else
                toNotation = notations[1];

            for (int i = 0; i < toNotation; i++)
            {
                if (i <= 9)
                    notationAlphabetTo.Add((char)(48 + i));
                else if (i >= 10)
                    notationAlphabetTo.Add((char)(55 + i));
            }

            //Ввод точности значения
            Console.WriteLine("\nВведите (точность) количество дробный цифр после запятой");

            int accuracy = -1;

            inputStringValue = Console.ReadLine();
            isNumber = int.TryParse(inputStringValue, out accuracy);
            while (isNumber != true || accuracy < 0)
            {
                Console.WriteLine("Введите значение корректно");
                inputStringValue = Console.ReadLine();
                isNumber = int.TryParse(inputStringValue, out accuracy);
            }

            //Ввод десятичной дроби, которую программа будет переводить в другую систему счисления
            Console.WriteLine("\nВведите десятичную дробь");
            string numberInNotation;
            bool isCorrect = false;
            inputStringValue = Console.ReadLine();

            while (isCorrect != true)
            {
                isCorrect = true;
                bool wasDot = false;
                bool isFractionPartEmpty = true;
                for (int iter = 0; iter < inputStringValue.Length; iter++)
                {
                    if (inputStringValue[iter] == ',')
                    {
                        if (wasDot)
                        {
                            isCorrect = false;
                        }
                        else
                        {
                            wasDot = true;
                        }
                    }
                    else if (notationAlphabetFrom.IndexOf(inputStringValue[iter]) < 0)
                    {
                        isCorrect = false;
                    }
                    else if (notationAlphabetFrom.IndexOf(inputStringValue[iter]) >= 0 && wasDot)
                    {
                        isFractionPartEmpty = false;
                    }
                }

                if (!wasDot || isFractionPartEmpty)
                {
                    isCorrect = false;
                }

                if (!isCorrect)
                {
                    Console.WriteLine("Введите значение корректно");
                    inputStringValue = Console.ReadLine();
                }
            }
            numberInNotation = inputStringValue;        

            //Перевод значения в десятичную систему счисления, если оно не в десятичном виде
            double decimalNumber = 0;
            if (fromNotation != 10)
            {
                int power = 0;
                int dotIndex = 0;
                int digit = 0;
                int integerPart = 0;
                double fractionPart = 0f;
                while (numberInNotation[dotIndex] != ',')
                {
                    dotIndex++;
                }

                power = 0;
                for (int iter = dotIndex - 1; iter >= 0; iter--)
                {
                    digit = notationAlphabetFrom.IndexOf(numberInNotation[iter]);
                    integerPart += digit * (int)Math.Pow(fromNotation, power);

                    power++;
                }

                power = 1;
                for (int iter = dotIndex + 1; iter < numberInNotation.Length; iter++)
                {
                    digit = notationAlphabetFrom.IndexOf(numberInNotation[iter]);
                    fractionPart += digit / Math.Pow(fromNotation, power);
                    power++;
                }

                decimalNumber = integerPart + fractionPart;
            }
            else
            {
                double.TryParse(numberInNotation, out decimalNumber);
            }

            //Перевод значения из десятичной системы счисления в toNotation систему счисления, если, конечно, toNotation не равно 10
            string resultNumber = "";
            if (toNotation != 10)
            {
                int digit = 0;
                int integerPart = (int)Math.Truncate(decimalNumber);
                double doublePart = decimalNumber - integerPart;
                while (integerPart > 0)
                {
                    digit = integerPart % toNotation;

                    resultNumber = notationAlphabetTo[digit] + resultNumber;
                    integerPart /= toNotation;
                }

                resultNumber += ',';
                int i = 0;
                while (doublePart != 0 && i < accuracy)
                {
                    doublePart = doublePart * toNotation;
                    digit = (int)Math.Truncate(doublePart);

                    doublePart = doublePart - (int)Math.Truncate(doublePart);

                    resultNumber += notationAlphabetTo[digit];
                    i++;
                }
            }
            else
            {
                resultNumber = decimalNumber.ToString();
            }

            Console.WriteLine("Результат перевода: " + resultNumber);

            Console.WriteLine("Работа программы завершена");
            Console.ReadLine();
        }
    }
}
