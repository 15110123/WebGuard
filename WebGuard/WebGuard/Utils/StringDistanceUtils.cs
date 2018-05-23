using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using static System.Threading.Tasks.Task;
using DiffMatchPatch;

namespace WebGuard.Utils
{
    public static class StringDistanceUtils
    {
        //private static int Min3(int a, int b, int c)
        //    => a < b ? (a < c ? a : c) : (b < c ? b : c);

        public static async Task<int> Distance(string s1, string s2)
        {
            //var column = new int[s1.Length + 1];

            //for (var y = 1; y <= s1.Length; ++y)
            //    column[y] = y;

            //for (var x = 1; x <= s2.Length; ++x)
            //{
            //    Console.WriteLine(x);
            //    //await Delay(1);
            //    column[0] = x;

            //    for (int y = 1, lastdiag = x - 1; y <= s1.Length; ++y)
            //    {
            //        await Delay(1);
            //        var olddiag = column[y];
            //        column[y] = Min3(column[y] + 1, column[y - 1] + 1, lastdiag + (s1[y - 1] == s2[x - 1] ? 0 : 1));
            //        lastdiag = olddiag;
            //    }
            //}

            //return column[s1.Length];

            if (s1.Length != s2.Length)
            {
                //throw new Exception("Strings must be equal length");
                while (s1.Length < s2.Length)
                {
                    await Delay(1);
                    s1 += " ";
                }

                while (s1.Length > s2.Length)
                {
                    await Delay(1);
                    s2 += " ";
                }
            }

            var distance =
                s1.ToCharArray()
                    .Zip(s2.ToCharArray(), (c1, c2) => new { c1, c2 })
                    .Count(m => m.c1 != m.c2);

            return distance;
        }

        public static async Task<double> DistanceEqualPercents(string s1, string s2, bool isHtmlCode)
        {
            var newS1 = ClearString(s1, isHtmlCode);
            var newS2 = ClearString(s2, isHtmlCode);

            return (double)await Distance(newS1, newS2) / Math.Max(newS1.Length, newS2.Length) * 100;
        }

        public static async Task<int> WordDistance(string s1, string s2)
        {
            s1 = s1.Replace(" ", "\n");
            s2 = s2.Replace(" ", "\n");
            var dmp = DiffMatchPatchModule.Default;
            var a = dmp.DiffLinesToChars(s1, s2);
            var lineText1 = a.Item1;
            var lineText2 = a.Item2;
            var lineArray = a.Item3;
            var diffs = dmp.DiffMain(lineText1, lineText2, false);
            dmp.DiffCharsToLines(diffs, lineArray);
            //diffs.ForEach(Console.WriteLine);
            //diffs.Where(x => !Equals(x.Operation, Operation.Equal))
            //    .ToList()
            //    .ForEach(x => Console.WriteLine(x.Text));
            //Console.WriteLine(diffs.Where(x => !Equals(x.Operation, Operation.Equal))
                //.Sum(x => x.Text.Split('\n').Length));
            return diffs.Where(x => !Equals(x.Operation, Operation.Equal))
                .Sum(x => x.Text.Split('\n').Length);
        }

        public static async Task<double> WordDistanceEqualPercents(string s1, string s2, bool isHtmlCode)
        {
            var newS1 = ClearString(s1, isHtmlCode);
            var newS2 = ClearString(s2, isHtmlCode);

            Console.WriteLine("newS1");
            Console.WriteLine(newS1);

            Console.WriteLine("newS2");
            Console.WriteLine(newS2);

            return (double)await WordDistance(newS1, newS2) / (newS1.Replace(" ", "\n").Split('\n').Length + newS2.Replace(" ", "\n").Split('\n').Length) * 100;
        }

        private static string ClearString(string str, bool isHtmlCode)
        {
            string newStr;
            if (!isHtmlCode)
            {
                dynamic obj = JsonConvert.DeserializeObject(str);
                foreach (var ele in obj.categories)
                {
                    ele.score = Math.Round((double)ele.score.Value, 1);
                }

                foreach (var ele in obj.tags)
                {
                    ele.confidence = Math.Round((double)ele.confidence.Value, 1);
                }

                // ReSharper disable once AssignNullToNotNullAttribute
                obj.description.tags = JArray.FromObject((obj.description.tags
                        .ToObject<List<string>>() as List<string>)
                    .OrderBy(x => x[0])
                    .ToArray());

                foreach (var ele in obj.description.captions)
                {
                    ele.confidence = Math.Round((double)ele.confidence.Value, 1);
                }

                obj.requestId = "";
                obj.metadata = null;
                obj.color.accentColor = null;

                newStr = JsonConvert.SerializeObject(obj);
            }
            else
            {
                newStr = str;
            }

            var targetArr = new[] { '{', '}', '"', ':', ',', '[', ']' };
            var strBuilder = new StringBuilder(newStr);
            for (var i = 0; i < strBuilder.Length; i++)
            {
                if (targetArr.All(x => x != strBuilder[i])) continue;
                strBuilder.Remove(i, 1);
                i--;
            }
            return strBuilder.ToString();
        }
    }
}
