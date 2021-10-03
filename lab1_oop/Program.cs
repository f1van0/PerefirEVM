using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1_oop
{
    static class MenuManager
    {
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

        public static int GetAccuracyOnInput()
        {
            Console.WriteLine("\nВведите (точность) количество дробный цифр после запятой");
            return ReadInteger();
        }

        public static string GetNumberInNotationOnInput(int notation, List<char> notationAlphabet)
        {
            Console.WriteLine("\nВведите десятичную дробь");
            return ReadNumberInNotation(notation, notationAlphabet);
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

        public static string ReadNumberInNotation(int notation, List<char> notationAlphabet)
        {
            bool isCorrect = false;
            string inputStringValue = Console.ReadLine();

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

        public static void PrintResult(string resultNumber)
        {
            Console.WriteLine("Результат перевода: " + resultNumber);

            Console.WriteLine("Работа программы завершена");
            Console.ReadLine();
        }
    }

    class NotationTranslator
    {
        private string number;
        private double decimalNumber;
        public int fromNotation { get; private set; }
        public int toNotation { get; private set; }
        public int accuracy { get; private set; }
        private List<char> fromNotationAlphabet;
        private List<char> toNotationAlphabet;

        public NotationTranslator()
        {
            number = "";
            fromNotation = 0;
            toNotation = 0;
            fromNotationAlphabet = new List<char>();
            toNotationAlphabet = new List<char>();
        }

        public NotationTranslator(int _fromNotation, int _toNotation, int _accuracy)
        {
            fromNotation = _fromNotation;
            toNotation = _toNotation;
            accuracy = _accuracy;
            fromNotationAlphabet = GetNotationAlphabet(fromNotation);
            toNotationAlphabet = GetNotationAlphabet(toNotation);
            number = MenuManager.GetNumberInNotationOnInput(fromNotation, fromNotationAlphabet);
        }

        public string Translate()
        {
            if (fromNotation != 10)
            {
                decimalNumber = GetDecimalNumber();
            }
            else
            {
                double.TryParse(number, out decimalNumber);
            }

            if (toNotation != 10)
            {
                return GetNumberWithNotationFromDecimal();
            }
            else
            {
                return decimalNumber.ToString();
            }
        }

        public List<char> GetNotationAlphabet(int _notation)
        {
            List<char> _notationAlphabet = new List<char>();
            for (int i = 0; i < _notation; i++)
            {
                if (i <= 9)
                    _notationAlphabet.Add((char)(48 + i));
                else if (i >= 10)
                    _notationAlphabet.Add((char)(55 + i));
            }

            return _notationAlphabet;
        }

        public double GetDecimalNumber()
        {
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
                digit = fromNotationAlphabet.IndexOf(number[iter]);
                integerPart += digit * (int)Math.Pow(fromNotation, power);

                power++;
            }

            power = 1;
            for (int iter = dotIndex + 1; iter < number.Length; iter++)
            {
                digit = fromNotationAlphabet.IndexOf(number[iter]);
                fractionPart += digit / Math.Pow(fromNotation, power);
                power++;
            }

            return integerPart + fractionPart;
        }

        public string GetNumberWithNotationFromDecimal()
        {
            string resultNumber = "";
            int digit = 0;
            int integerPart = (int)Math.Truncate(decimalNumber);
            double doublePart = decimalNumber - integerPart;
            while (integerPart > 0)
            {
                digit = integerPart % toNotation;

                resultNumber = toNotationAlphabet[digit] + resultNumber;
                integerPart /= toNotation;
            }

            resultNumber += ',';
            int i = 0;
            while (doublePart != 0 && i < accuracy)
            {
                doublePart = doublePart * toNotation;
                digit = (int)Math.Truncate(doublePart);

                doublePart = doublePart - (int)Math.Truncate(doublePart);

                resultNumber += toNotationAlphabet[digit];
                i++;
            }

            return resultNumber;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<int> notations = new List<int>() { 2, 10, 16 };
            int fromNotation = MenuManager.GetFromNotationOnInput();
            int toNotation = MenuManager.GetToNotationOnInput(fromNotation, notations);
            int accuracy = MenuManager.GetAccuracyOnInput();
            NotationTranslator notationTranslator = new NotationTranslator(fromNotation, toNotation, accuracy);
            MenuManager.PrintResult(notationTranslator.Translate());
        }
    }
}