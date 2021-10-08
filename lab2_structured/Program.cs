using List;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_structured
{
    class Program
    {
        static void Main(string[] args)
        {
            List<(char c, int n, string code)> alphabet = new List<(char c, int n, string code)>();
            while (true)
            {
                Console.WriteLine("[1]: Закодировать текст\n[2]: Расшифровать текст\n[0]: Выйти из программы");
                int inputValue;
                string inputStringValue = Console.ReadLine();
                bool isNumber = int.TryParse(inputStringValue, out inputValue);
                while (isNumber != true)
                {
                    Console.WriteLine("Введите значение корректно");
                    inputStringValue = Console.ReadLine();
                    isNumber = int.TryParse(inputStringValue, out inputValue);
                }
                int choice = inputValue;

                if (choice == 1)
                {
                    char currentChar;
                    Console.WriteLine("Введите название файла, в котором хранится текст, который нужно закодировать");
                    string filePath = Console.ReadLine();
                    string text;
                    try
                    {
                        StreamReader fileStream = new StreamReader(filePath + ".txt");
                        text = fileStream.ReadToEnd();
                        fileStream.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Файл с таким именем не найден");
                        text = "";
                    }
                    if (text != "")
                    {
                        List<char> charsAlphabet = new List<char>();
                        List<int> charsCount = new List<int>();

                        //Получение всех символов текста и числа их повторений
                        for (int i = 0; i < text.Length; i++)
                        {
                            currentChar = text[i];
                            if (currentChar != '\0' && currentChar != '\n' && currentChar != '\r')
                            {
                                if (charsAlphabet.Contains(currentChar))
                                {
                                    charsCount[charsAlphabet.IndexOf(currentChar)]++;
                                }
                                else
                                {
                                    charsAlphabet.Add(currentChar);
                                    charsCount.Add(1);
                                }
                            }
                        }

                        //Сортировка алфавита по убыванию частоты встречаемости в тексте
                        int iter = 0;
                        int iterations = charsCount.Count;
                        int max, indexMax;
                        while (iter < iterations)
                        {
                            max = -1;
                            indexMax = -1;

                            for (int i = iter; i < iterations; i++)
                            {
                                if (charsCount[i] > max)
                                {
                                    max = charsCount[i];
                                    indexMax = i;
                                }
                            }

                            int exchangeCount = charsCount[iter];
                            charsCount[iter] = max;
                            charsCount[indexMax] = exchangeCount;

                            char exchangeChar = charsAlphabet[iter];
                            charsAlphabet[iter] = charsAlphabet[indexMax];
                            charsAlphabet[indexMax] = exchangeChar;

                            iter++;
                        }

                        alphabet = new List<(char c, int n, string code)>();

                        for (int i = 0; i < charsAlphabet.Count; i++)
                        {
                            alphabet.Add((charsAlphabet[i], charsCount[i], ""));
                        }

                        if (alphabet.Count <= 1)
                        {
                            
                        }
                        else if (alphabet.Count == 2)
                        {
                            alphabet[0] = (alphabet[0].c, alphabet[0].n, alphabet[0].code + '1');
                            alphabet[1] = (alphabet[1].c, alphabet[1].n, alphabet[1].code + '0');
                        }
                        else
                        {
                            List<(char c, int n, string code)> alphabet1 = new List<(char c, int n, string code)>();
                            List<(char c, int n, string code)> alphabet2 = new List<(char c, int n, string code)>();
                            int count1Group;
                            if (alphabet.Count > 0)
                            {
                                //Суммарная вероятность первой группы изначально равна 1 элементу
                                double pAmount1Group = alphabet[0].n;
                                double pAmount2Group = 0;
                                double dAmount = 0;
                                int countOfItemsInFirstGroup = 1;

                                //Цикл считает суммарное значение вероятности второй группы
                                for (int i = 1; i < alphabet.Count; i++)
                                {
                                    pAmount2Group += alphabet[i].n;
                                }

                                dAmount = Math.Abs(pAmount1Group - pAmount2Group);

                                //Пока суммарная вероятность в первой группе меньше суммарной вероятности во второй группе
                                while (pAmount1Group < pAmount2Group)
                                {
                                    //Расчитывает дельта двух сумм под 
                                    dAmount = Math.Abs(pAmount1Group - pAmount2Group);

                                    //Значение вероятности из второй группы переходит в первую
                                    pAmount1Group += alphabet[countOfItemsInFirstGroup].n;
                                    pAmount2Group -= alphabet[countOfItemsInFirstGroup].n;

                                    //Количество элементов (символов) в первой группе увеличивается на 1
                                    countOfItemsInFirstGroup++;
                                }

                                //После последней итерации dAmount имеет значение, полученное при прошлой итерации и теперь его можно заменить на новое, если новое будет меньше
                                //В данной ситуации значение можно не заменять, а просто сделать вывод о том, какое количество элементов должно быть в первой группе
                                if (dAmount >= Math.Abs(pAmount1Group - pAmount2Group))
                                {
                                    count1Group = countOfItemsInFirstGroup;
                                }
                                else
                                {
                                    count1Group = countOfItemsInFirstGroup - 1;
                                }
                            }
                            else
                            {
                                count1Group = -1;
                            }

                            for (int i = 0; i < count1Group; i++)
                            {
                                alphabet[i] = (alphabet[i].c, alphabet[i].n, alphabet[i].code + '1');
                                alphabet1.Add(alphabet[i]);
                            }

                            for (int i = count1Group; i < alphabet.Count; i++)
                            {
                                alphabet[i] = (alphabet[i].c, alphabet[i].n, alphabet[i].code + '0');
                                alphabet2.Add(alphabet[i]);
                            }

                            for (int i = 0; i < count1Group; i++)
                            {
                                alphabet[i] = alphabet1[i];
                            }

                            for (int i = 0; i < alphabet.Count - count1Group; i++)
                            {
                                alphabet[i + count1Group] = alphabet2[i];
                            }
                        }
                        alphabet = alphabet.SortToLower();

                        StreamWriter fileStream = new StreamWriter("alphabet.txt");
                        string line;
                        foreach (var element in alphabet)
                        {
                            line = element.Item1.ToString() + " " + element.Item3.ToString();
                            fileStream.WriteLine(line);
                        }
                        fileStream.Close();

                        string codedText = "";
                        for (int i = 0; i < text.Length; i++)
                        {
                            currentChar = text[i];
                            if (alphabet.Exists(elem => elem.c == currentChar)) //Если в алфавите есть данный символ, то в закодированный текст дописывается код принадлежащий данному символу
                            {
                                codedText += alphabet.Find(stat => stat.c == currentChar).code;
                            }
                            else
                            {
                                codedText += currentChar;
                            }
                        }

                        //Результат записывается в файл
                        fileStream = new StreamWriter(filePath + "_coded.txt");
                        fileStream.Write(codedText);
                        fileStream.Close();
                    }
                }
                else if (choice == 2)
                {
                    string text;
                    Console.WriteLine("Введите название файла, в котором хранится текст, который нужно раскодировать");
                    string filePath = Console.ReadLine();
                    if (alphabet == null)
                    {
                        string line;
                        alphabet = new List<(char, int, string)>();
                        char c = ' ';
                        string code = "";
                        StreamReader fileStream = new StreamReader("alphabet.txt");
                        while (!fileStream.EndOfStream)
                        {
                            code = "";
                            line = fileStream.ReadLine();
                            if (line.Length <= 2)
                            {
                                Console.WriteLine("Файл с алфавитом имеет ошибки");
                            }
                            else
                            {
                                c = line[0];
                                if (line[1] != ' ')
                                {
                                    Console.WriteLine("Файл с алфавитом имеет ошибки");
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
                    }

                    try
                    {
                        StreamReader fileStream = new StreamReader(filePath + ".txt");
                        text = fileStream.ReadToEnd();
                        fileStream.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Файл с таким именем не найден");
                        text = "";
                    }
                    if (text != "")
                    {
                        string decodedText = "";
                        //Считанный код символа
                        string currentCharCode = "";
                        for (int i = 0; i < text.Length; i++)
                        {
                            //Если в текущий символ текста является цифрой двоичного кода, то эта цифра дописывается в конец кода символа
                            if (text[i] == '0' || text[i] == '1')
                            {
                                currentCharCode += text[i];

                                //Если код символа совпадает с каким-либо кодом символа из заданного списка _alphabet, то его можно декодировать
                                if (alphabet.Exists(elem => elem.code == currentCharCode))
                                {
                                    //Символ в декодированном виде дописывается в декодированный текст
                                    char decodedChar = alphabet.Find(elem => elem.code == currentCharCode).c;
                                    decodedText += decodedChar;
                                    //А код текущего символа стирается
                                    currentCharCode = "";
                                }
                            }
                            else //Иначе в тексте есть ошибка
                            {
                                decodedText += text[i];
                            }
                        }

                        //Результат записывается в файл
                        StreamWriter fileStream = new StreamWriter(filePath + "_decoded.txt");
                        fileStream.Write(decodedText);
                        fileStream.Close();
                    }
                }
                else
                {
                    break;
                }

                Console.WriteLine();
            }
        }
    }
}

