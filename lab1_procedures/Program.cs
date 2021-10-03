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
            Menu();
        }


        public static int GetFromNotationOnInput()
        {
            Console.WriteLine("Из какой системы счисления нужно перевести число?\n" +
                "[1]: Двоичная\n" +
                "[2]: Десятичная\n" +
                "[3]: Шестнадцатиричная");

            int _inputValue = ReadInteger();

            switch (_inputValue)
            {
                case 1:
                    {
                        return 2;
                    }
                case 2:
                    {
                        return 10;
                    }
                default:
                    {
                        return 16;
                    }
            }
        }

        public static int GetToNotationOnInput(int _fromNotation, List<int> _notations)
        {
            Console.WriteLine("\nВ какую систему счисления нужно перевести число?");
            switch (_fromNotation)
            {
                case 1:
                    {
                        Console.WriteLine("[1]: Десятичная");
                        Console.WriteLine("[2]: Шестнадцатиричная");
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("[1]: Двоичная");
                        Console.WriteLine("[2]: Шестнадцатиричная");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("[1]: Двоичная");
                        Console.WriteLine("[2]: Десятичная");
                        break;
                    }
            }

            int _inputValue = ReadInteger();
            _notations.Remove(_fromNotation);
            if (_inputValue == 1)
            {
                return _notations[0];
            }
            else
            {
                return _notations[1];
            }
        }

        public static string NumberInNotationTransition(string _numberInNotation, int _fromNotation, int _toNotation, int _accuracy)
        {
            double decimalNumber;
            string resultNumber;
            if (_fromNotation != 10)
            {
                decimalNumber = GetDecimalNumber(_fromNotation, _numberInNotation);
                decimalNumber = Math.Round(decimalNumber, _accuracy);
            }
            else
            {
                double.TryParse(_numberInNotation, out decimalNumber);
            }

            if (_toNotation != 10)
            {
                resultNumber = GetNumberWithNotationFromDecimal(_toNotation, decimalNumber, _accuracy);
            }
            else
            {
                resultNumber = decimalNumber.ToString();
            }

            return resultNumber;
        }

        public static void Menu()
        {
            List<int> notations = new List<int>() { 2, 10, 16 };
            int fromNotation;
            int toNotation;
            int accuracy;

            fromNotation = GetFromNotationOnInput();
            toNotation = GetToNotationOnInput(fromNotation, notations);

            Console.WriteLine("\nВведите (точность) количество дробный цифр после запятой");
            accuracy = ReadInteger();

            Console.WriteLine("\nВведите десятичную дробь");
            string numberInNotation = ReadNumberInNotation(fromNotation);

            string resultNumber = NumberInNotationTransition(numberInNotation, fromNotation, toNotation, accuracy);

            Console.WriteLine("Результат перевода: " + resultNumber);

            Console.WriteLine("Работа программы завершена");
            Console.ReadLine();
        }

        public static string ReadNumberInNotation(int notation)
        {
            bool isCorrect = false;
            string inputStringValue = Console.ReadLine();
            List<char> notationAlphabet = GetNotationAlphabet(notation);

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
                    else if (notationAlphabet.IndexOf(inputStringValue[iter]) < 0)
                    {
                        isCorrect = false;
                    }
                    else if (notationAlphabet.IndexOf(inputStringValue[iter]) >= 0 && wasDot)
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
            return inputStringValue;
        }

        public static int ReadInteger()
        {
            int inputValue;
            string inputStringValue = Console.ReadLine();
            bool isNumber = int.TryParse(inputStringValue, out inputValue);
            while (isNumber != true)
            {
                Console.WriteLine("Введите значение корректно");
                inputStringValue = Console.ReadLine();
                isNumber = int.TryParse(inputStringValue, out inputValue);
            }

            return inputValue;
        }

        public static List<char> GetNotationAlphabet(int notation)
        {
            List<char> notationAlphabet = new List<char>();
            for (int i = 0; i < notation; i++)
            {
                if (i <= 9)
                    notationAlphabet.Add((char)(48 + i));
                else if (i >= 10)
                    notationAlphabet.Add((char)(55 + i));
            }

            return notationAlphabet;
        }

        public static double GetDecimalNumber(int notation, string number)
        {
            List<char> notationAlphabet = GetNotationAlphabet(notation);
            int power = 0;
            int dotIndex = 0;
            int digit = 0;
            int integerPart = 0;
            double fractionPart = 0f;
            while (number[dotIndex] != ',')
            {
                dotIndex++;
            }

            power = 0;
            for (int iter = dotIndex - 1; iter >= 0; iter--)
            {
                digit = notationAlphabet.IndexOf(number[iter]);
                integerPart += digit * (int)Math.Pow(notation, power);

                power++;
            }

            power = 1;
            for (int iter = dotIndex + 1; iter < number.Length; iter++)
            {
                digit = notationAlphabet.IndexOf(number[iter]);
                fractionPart += digit / Math.Pow(notation, power);
                power++;
            }

            return integerPart + fractionPart;
        }

        public static string GetNumberWithNotationFromDecimal(int notation, double decimalNumber, int accuracy)
        {
            List<char> notationAlphabet = GetNotationAlphabet(notation);
            string resultNumber = "";
            int digit = 0;
            int integerPart = (int)Math.Truncate(decimalNumber);
            double doublePart = decimalNumber - integerPart;
            while (integerPart > 0)
            {
                digit = integerPart % notation;

                resultNumber = notationAlphabet[digit] + resultNumber;
                integerPart /= notation;
            }

            resultNumber += ',';
            int i = 0;
            while (doublePart != 0 && i < accuracy)
            {
                doublePart = doublePart * notation;
                digit = (int)Math.Truncate(doublePart);

                doublePart = doublePart - (int)Math.Truncate(doublePart);

                resultNumber += notationAlphabet[digit];
                i++;
            }

            return resultNumber;
        }

    }
}
