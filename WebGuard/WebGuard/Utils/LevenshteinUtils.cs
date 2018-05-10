namespace WebGuard.Utils
{
    public static class LevenshteinUtils
    {
        private static int Min3(int a, int b, int c)
            => a < b ? (a < c ? a : c) : (b < c ? b : c);

        public static int LevenshteinDistance(string s1, string s2)
        {
            var column = new int[s1.Length + 1];

            for (var y = 1; y <= s1.Length; ++y)
                column[y] = y;

            for (var x = 1; x <= s2.Length; ++x)
            {
                column[0] = x;

                for (int y = 1, lastdiag = x - 1; y <= s1.Length; ++y)
                {
                    var olddiag = column[y];
                    column[y] = Min3(column[y] + 1, column[y - 1] + 1, lastdiag + (s1[y - 1] == s2[x - 1] ? 0 : 1));
                    lastdiag = olddiag;
                }
            }

            return column[s1.Length];
        }

        public static double LevenshteinPercents(string s1, string s2)
            =>  (double)LevenshteinDistance(s1, s2) / s1.Length * 100;
    }
}
