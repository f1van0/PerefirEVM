using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List
{
    public static class ListExtentions
    {
        public static List<(char, int, string)> SortToLower(this List<(char c, int n, string code)> _alphabet)
        {

            if (_alphabet.Count <= 1)
            {
                return _alphabet;
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
                int count1Group;
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
                    _alphabet[i] = (_alphabet[i].c, _alphabet[i].n, _alphabet[i].code + '1');
                    alphabet1.Add(_alphabet[i]);
                }

                for (int i = count1Group; i < _alphabet.Count; i++)
                {
                    _alphabet[i] = (_alphabet[i].c, _alphabet[i].n, _alphabet[i].code + '0');
                    alphabet2.Add(_alphabet[i]);
                }
                alphabet1 = SortToLower(alphabet1);
                alphabet2 = SortToLower(alphabet2);

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
    }
}
