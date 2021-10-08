using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_OOP
{
    class Program
    {
        struct CharStats
        {
            public char c;
            public int n;
            public string code;

            public CharStats(char _c, int _n)
            {
                c = _c;
                n = _n;
                code = "";
            }

            public void inc()
            {
                n++;
            }
        }

        struct AlphabetManager
        {
            public List<CharStats> alphabet;

            public int Find(char _char)
            {
                for (int i = 0; i < alphabet.Count; i++)
                {
                    if (alphabet[i].c == _char)
                        return i;
                }
                return -1;
            }

            public int Find(int _n)
            {
                for (int i = 0; i < alphabet.Count; i++)
                {
                    if (alphabet[i].n == _n)
                        return i;
                }
                return -1;
            }

            public int Find(string _code)
            {
                for (int i = 0; i < alphabet.Count; i++)
                {
                    if (alphabet[i].code == _code)
                        return i;
                }
                return -1;
            }

            //Подсчёт вероятностей по матрице _aplhabetMatrix и _summaryPairs
            public AlphabetManager(string _text)
            {
                alphabet = new List<CharStats>();

                //Получение всех символов текста и числа их повторений
                for (int i = 0; i < _text.Length; i++)
                {
                    char currentChar = _text[i];
                    if (currentChar != '\0' && currentChar != '\n' && currentChar != '\r')
                    {
                        int index = Find(currentChar);
                        if (index != -1)
                        {
                            alphabet[index].inc();
                        }
                        else
                        {
                            alphabet.Add(new CharStats(currentChar, 1));
                        }
                    }
                }

                //Сортировка алфавита по убыванию частоты встречаемости в тексте
                int iter = 0;
                int iterations = alphabet.Count;
                CharStats maxCS;
                int indexMax;
                while (iter < iterations)
                {
                    maxCS = new CharStats(' ', -1);
                    indexMax = -1;

                    for (int i = iter; i < iterations; i++)
                    {
                        if (alphabet[i].n > maxCS.n)
                        {
                            maxCS = alphabet[i];
                            indexMax = i;
                        }
                    }

                    CharStats exchangeCS = alphabet[iter];
                    alphabet[iter] = maxCS;
                    alphabet[indexMax] = exchangeCS;

                    iter++;
                }

                List<(char c, int n, string code)> alphabetCodes = new List<(char c, int n, string code)>();

                for (int i = 0; i < alphabet.Count; i++)
                {
                    alphabetCodes.Add((alphabet[i].c, alphabet[i].n, ""));
                }

                alphabetCodes = MethodShannonFanoCoding(alphabetCodes);

                WriteAlphabet("alphabet.txt", alphabetCodes);
            }

            public static List<(char, int, string)> MethodShannonFanoCoding(List<(char c, int n, string code)> _alphabet)
            {

                if (_alphabet.Count <= 1)
                {

                }
                else if (_alphabet.Count == 2)
                {
                    _alphabet[0] = (_alphabet[0].c, _alphabet[0].n, _alphabet[0].code + '1');
                    _alphabet[1] = (_alphabet[1].c, _alphabet[1].n, _alphabet[1].code + '0');
                }
                else
                {
                    List<(char c, int n, string code)> alphabet1 = new List<(char c, int n, string code)>();
                    List<(char c, int n, string code)> alphabet2 = new List<(char c, int n, string code)>();
                    int count1Group = DivideAlphabetIn2Groups(_alphabet);

                    for (int i = 0; i < count1Group; i++)
                    {
                        _alphabet[i] = (_alphabet[i].c, _alphabet[i].n, _alphabet[i].code + '1');
                        alphabet1.Add(_alphabet[i]);
                    }

                    for (int i = count1Group; i < _alphabet.Count; i++)
                    {
                        _alphabet[i] = (_alphabet[i].c, _alphabet[i].n, _alphabet[i].code + '0');
                        alphabet2.Add(_alphabet[i]);
                    }

                    alphabet1 = MethodShannonFanoCoding(alphabet1);
                    alphabet2 = MethodShannonFanoCoding(alphabet2);

                    for (int i = 0; i < count1Group; i++)
                    {
                        _alphabet[i] = alphabet1[i];
                    }

                    for (int i = 0; i < _alphabet.Count - count1Group; i++)
                    {
                        _alphabet[i + count1Group] = alphabet2[i];
                    }
                }

                return _alphabet;
            }

            public static int DivideAlphabetIn2Groups(List<(char c, int n, string code)> _alphabet)
            {
                if (_alphabet.Count > 0)
                {
                    //Суммарная вероятность первой группы изначально равна 1 элементу
                    double pAmount1Group = _alphabet[0].n;
                    double pAmount2Group = 0;
                    double dAmount = 0;
                    int countOfItemsInFirstGroup = 1;

                    //Цикл считает суммарное значение вероятности второй группы
                    for (int i = 1; i < _alphabet.Count; i++)
                    {
                        pAmount2Group += _alphabet[i].n;
                    }

                    dAmount = Math.Abs(pAmount1Group - pAmount2Group);

                    //Пока суммарная вероятность в первой группе меньше суммарной вероятности во второй группе
                    while (pAmount1Group < pAmount2Group)
                    {
                        //Расчитывает дельта двух сумм под 
                        dAmount = Math.Abs(pAmount1Group - pAmount2Group);

                        //Значение вероятности из второй группы переходит в первую
                        pAmount1Group += _alphabet[countOfItemsInFirstGroup].n;
                        pAmount2Group -= _alphabet[countOfItemsInFirstGroup].n;

                        //Количество элементов (символов) в первой группе увеличивается на 1
                        countOfItemsInFirstGroup++;
                    }

                    //После последней итерации dAmount имеет значение, полученное при прошлой итерации и теперь его можно заменить на новое, если новое будет меньше
                    //В данной ситуации значение можно не заменять, а просто сделать вывод о том, какое количество элементов должно быть в первой группе
                    if (dAmount >= Math.Abs(pAmount1Group - pAmount2Group))
                    {
                        return countOfItemsInFirstGroup;
                    }
                    else
                    {
                        return countOfItemsInFirstGroup - 1;
                    }
                }
                else
                {
                    return -1;
                }
            }
        }

        public class MenuManager
        {
            public static string ReadTextFromFile(string _path)
            {
                try
                {
                    string text;
                    StreamReader fileStream = new StreamReader(_path);
                    text = fileStream.ReadToEnd();
                    fileStream.Close();
                    return text;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Файл с таким именем не найден");
                    return "";
                }

            }

            public static void WriteTextToFile(string _path, string _text)
            {
                StreamWriter fileStream = new StreamWriter(_path);
                fileStream.Write(_text);
                fileStream.Close();
            }

            public static List<(char, int, string)> ReadAlphabet(string _path)
            {
                string line;
                List<(char, int, string)> alphabet = new List<(char, int, string)>();
                char c = ' ';
                string code = "";
                StreamReader fileStream = new StreamReader(_path);
                while (!fileStream.EndOfStream)
                {
                    code = "";
                    line = fileStream.ReadLine();
                    if (line.Length <= 2)
                    {
                        Console.WriteLine("Файл с алфавитом имеет ошибки");
                        return null;
                    }
                    else
                    {
                        c = line[0];
                        if (line[1] != ' ')
                        {
                            Console.WriteLine("Файл с алфавитом имеет ошибки");
                            return null;
                        }
                        else
                        {
                            for (int i = 2; i < line.Length; i++)
                            {
                                code += line[i];
                            }

                            alphabet.Add((c, 0, code));
                        }
                    }

                }

                fileStream.Close();
                return alphabet;
            }

            public static void WriteAlphabet(string _path, List<(char c, int n, string code)> _alphabet)
            {
                StreamWriter fileStream = new StreamWriter(_path);
                string line;
                foreach (var element in _alphabet)
                {
                    line = element.c.ToString() + " " + element.code.ToString();
                    fileStream.WriteLine(line);
                }
                fileStream.Close();
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

            static void Menu()
            {
                List<(char, int, string)> alphabet = new List<(char, int, string)>();
                while (true)
                {
                    Console.WriteLine("[1]: Закодировать текст\n[2]: Расшифровать текст\n[0]: Выйти из программы");
                    int choice = ReadInteger();

                    if (choice == 1)
                    {
                        Console.WriteLine("Введите название файла, в котором хранится текст, который нужно закодировать");
                        string filePath = Console.ReadLine();
                        string text = ReadTextFromFile(filePath + ".txt");
                        if (text != "")
                        {
                            alphabet = GetAlphabet(text);
                            CodeText(text, filePath + "_coded.txt", GetAlphabet(text));
                        }
                    }
                    else if (choice == 2)
                    {
                        Console.WriteLine("Введите название файла, в котором хранится текст, который нужно раскодировать");
                        string filePath = Console.ReadLine();
                        if (alphabet == null)
                            alphabet = ReadAlphabet("alphabet.txt");
                        string text = ReadTextFromFile(filePath + ".txt");
                        if (text != "")
                            DecodeText(text, filePath + "_decoded.txt", alphabet);
                    }
                    else
                    {
                        break;
                    }

                    Console.WriteLine();
                }
            }
        }

        public static void CodeText(string _text, string _outputFile, List<(char c, int n, string code)> _alphabet)
        {
            string codedText = "";
            char currentChar;
            for (int i = 0; i < _text.Length; i++)
            {
                currentChar = _text[i];
                if (_alphabet.Exists(elem => elem.c == currentChar)) //Если в алфавите есть данный символ, то в закодированный текст дописывается код принадлежащий данному символу
                {
                    codedText += _alphabet.Find(stat => stat.c == currentChar).code;
                }
                else
                {
                    codedText += currentChar;
                }
            }

            //Результат записывается в файл
            WriteTextToFile(_outputFile, codedText);
        }

        public static void DecodeText(string _text, string _outputFile, List<(char c, int n, string code)> _alphabet)
        {
            string decodedText = "";
            //Считанный код символа
            string currentCharCode = "";
            for (int i = 0; i < _text.Length; i++)
            {
                //Если в текущий символ текста является цифрой двоичного кода, то эта цифра дописывается в конец кода символа
                if (_text[i] == '0' || _text[i] == '1')
                {
                    currentCharCode += _text[i];

                    //Если код символа совпадает с каким-либо кодом символа из заданного списка _alphabet, то его можно декодировать
                    if (_alphabet.Exists(elem => elem.code == currentCharCode))
                    {
                        //Символ в декодированном виде дописывается в декодированный текст
                        char decodedChar = _alphabet.Find(elem => elem.code == currentCharCode).c;
                        decodedText += decodedChar;
                        //А код текущего символа стирается
                        currentCharCode = "";
                    }
                }
                else //Иначе в тексте есть ошибка
                {
                    decodedText += _text[i];
                }
            }

            //Результат записывается в файл
            WriteTextToFile(_outputFile, decodedText);
        }

        static void Main(string[] args)
        {
            Menu();
        }
    }
}
