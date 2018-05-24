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
        private static int Min3(int a, int b, int c)
            => a < b ? (a < c ? a : c) : (b < c ? b : c);

        public static async Task<int> Distance(string s1, string s2)
        {
            var column = new int[s1.Length + 1];

            for (var y = 1; y <= s1.Length; ++y)
                column[y] = y;

            for (var x = 1; x <= s2.Length; ++x)
            {
                Console.WriteLine(x);
                //await Delay(1);
                column[0] = x;

                for (int y = 1, lastdiag = x - 1; y <= s1.Length; ++y)
                {
                    await Delay(1);
                    var olddiag = column[y];
                    column[y] = Min3(column[y] + 1, column[y - 1] + 1, lastdiag + (s1[y - 1] == s2[x - 1] ? 0 : 1));
                    lastdiag = olddiag;
                }
            }

            return column[s1.Length];
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
            //var lineArray = a.Item3;
            var diffs = dmp.DiffMain(lineText1, lineText2);
            //dmp.DiffCharsToLines(diffs, lineArray);
            //diffs.Where(x => !Equals(x.Operation, Operation.Equal))
            //    .ToList()
            //    .ForEach(x => Console.WriteLine(x.Text));
            //diffs.ForEach(Console.WriteLine);
            return diffs.Where(x => !Equals(x.Operation, Operation.Equal))
                .Sum(x => x.Text.Length);
        }

        public static async Task<double> WordDistanceEqualPercents(string s1, string s2, bool isHtmlCode)
        {
            var newS1 = ClearString(s1, isHtmlCode);
            var newS2 = ClearString(s2, isHtmlCode);

            //Console.WriteLine("newS1");
            //Console.WriteLine(newS1);

            //Console.WriteLine("newS2");
            //Console.WriteLine(newS2);

            return (double)await WordDistance(newS1, newS2) / (newS1.Replace(" ", "\n").Split('\n').Length + newS2.Replace(" ", "\n").Split('\n').Length) * 100;
        }

        private static string ClearString(string str, bool isHtmlCode)
        {
            string newStr;
            if (!isHtmlCode)
            {
                dynamic obj = JsonConvert.DeserializeObject(str);

                try
                {
                    foreach (var ele in obj.categories)
                    {
                        ele.score = Math.Round((double)ele.score.Value, 1);
                    }
                }
                catch
                {
                    //ignore
                }

                try
                {
                    foreach (var ele in obj.tags)
                    {
                        ele.confidence = Math.Round((double)ele.confidence.Value, 1);
                    }

                    // ReSharper disable once AssignNullToNotNullAttribute
                    obj.description.tags = JArray.FromObject((obj.description.tags
                            .ToObject<List<string>>() as List<string>)
                        .OrderBy(x => x[0])
                        .ToArray());
                }
                catch
                {
                    //ignore
                }


                try
                {
                    foreach (var ele in obj.description.captions)
                    {
                        ele.confidence = Math.Round((double)ele.confidence.Value, 1);
                    }

                    obj.requestId = "";
                    obj.metadata = null;
                    obj.color.accentColor = null;
                }
                catch
                {
                    //ignore
                }

                newStr = JsonConvert.SerializeObject(obj);
            }
            else
            {
                newStr = str;
            }

            var targetArr = new[] { '{', '}', '"', ':', '[', ']', ',' };
            var strBuilder = new StringBuilder(newStr);
            for (var i = 0; i < strBuilder.Length; i++)
            {
                if (targetArr.All(x => x != strBuilder[i])) continue;
                strBuilder.Remove(i, 1);
                i--;
            }
            return strBuilder.ToString()
                             .Replace(" ", "\n");
        }
    }
}
