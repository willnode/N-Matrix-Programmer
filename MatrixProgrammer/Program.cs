using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixProgrammer
{
    class Program
    {
        /// <summary>
        /// Number of matrix rows X colums
        /// </summary>
        static int N = 3;

        /// <summary>
        /// Optimization Level. Adjusted automatically
        /// </summary>
        static int O = 1;

        /// <summary>
        /// The name for members to be processed
        /// </summary>
        static string STR(int row, int column)
        {
            return string.Format(" m.m{0}{1} ", row, column);
        }

        /// <summary>
        /// The name for members that will receive the final data
        /// </summary>
        static string STR2(int row, int column)
        {
            return string.Format("m{0}{1} ", row, column);
        }

        static string STRO(Tuple<int, int> t)
        {
            return string.Format(" P{0}{1} ", t.Item1, t.Item2);
        }

        static string STRO(Tuple<int, int, int> t)
        {
            return string.Format(" Q{0}{1}{2} ", t.Item1, t.Item2, t.Item3);
        }

        static StringBuilder STROB = new StringBuilder();

        static string STRO(int[] t, int[] u)
        {
            STROB.Clear();
            STROB.Append(((char) ('?' + t.Length)).ToString());
            STROB.Append(string.Join("", t));
            STROB.Append(string.Join("", u));
            return " " + STROB.ToString() + " ";
        }

        //
        // String formats which you're free to change
        //
        const string FormatPostDeterm = "var det ={0};\ndet = 1 / det;\n";
        const string FormatPostInvers = "return new Matrix{1}x{1}() {{\n{0}}};";
        const string FormatMemberInvers = "   {0}= det * {1} ({2}),";

        const string FormatMemberCached = "var{0}={1};";

        const string FormatCachedN2 = "{0}*{1}-{2}*{3}";

        //const string FormatCachedN3 = "var{0}={1}*{2}-{3}*{4}+{5}*{6};";
        //const string FormatCachedN4 = "var{0}={1}*{2}-{3}*{4}+{5}*{6}-{7}*{8};";

        static public void Main(string[] args)
        {
            CheckArguments(args);

            var S = new StringBuilder();
            var S2 = new StringBuilder();
            WriteDeterminant(N, S);
            WriteInverse(N, S2);

            if (O >= 2)
                WriteCachedCodes();
            Console.WriteLine(string.Format(FormatPostDeterm, S, N));
            Console.WriteLine(string.Format(FormatPostInvers, S2, N));
        }

        static void CheckArguments(string[] args)
        {
            if (args.Length > 0) {
                int parsed;
                if (int.TryParse(args[0], out parsed)) {
                    N = parsed;
                }
            }

            {
                if (N >= 4)
                    O = N - 2;
                else
                    O = 1;
            }

            if (args.Length > 1) {
                int parsed;
                if (int.TryParse(args[1], out parsed)) {
                    O = parsed;
                }
            }
        }

        static void WriteDeterminant(int N, StringBuilder S)
        {
            var X = Enumerable.Range(0, N).ToArray();
            var Y = Enumerable.Range(0, N).ToArray();
            WriteDeterminant(N, X, Y, S, true);
        }

        static void WriteDeterminant(int N, int[] X, int[] Y, StringBuilder S, bool NS)
        {
            if (O >= N && O >= 2) {
                S.Append(WriteOptimizedNDet(N, X, Y));
                return;
            }

            bool plus = true;
            for (int i = 0; i < N; i++) {
                // Sign (and necessary stylings)
                if (i > 0) {
                    if (NS)
                        S.Append("\n\t");
                    S.Append(plus ? "+" : "-");
                }

                // Write this member
                int x = X[i];
                int y = Y[0];
                S.Append(STR(y, x));
                plus = !plus;

                // Take recursive call at minor matrix
                if (N > 1) {
                    if (ShouldAddBracket(N))
                        S.Append("* (");
                    else
                        S.Append("*");
                    WriteDeterminant(N - 1, X.Where(n => n != x).ToArray(), Y.Where(n => n != y).ToArray(), S, false);
                    if (ShouldAddBracket(N))
                        S.Append(") ");
                }
            }
        }

        static bool ShouldAddBracket(int N)
        {
            return N > O + 1;
        }


        static string WriteOptimizedNDet(int N, int[] X, int[] Y)
        {
            var S = new StringBuilder();
            var C = GetCacheSets(N);
            var M = STRO(X, Y);
            if (!C.ContainsKey(M)) {
                if (N > 2) {
                    bool plus = true;
                    for (int i = 0; i < N; i++) {
                        // Sign (and necessary stylings)
                        if (i > 0)
                            S.Append(plus ? "+" : "-");

                        // Write this member
                        int x = X[i];
                        int y = Y[0];
                        S.Append(STR(y, x));
                        plus = !plus;

                        // Take recursive call at minor matrix
                        S.Append("*");
                        S.Append(WriteOptimizedNDet(N - 1, X.Where(n => n != x).ToArray(),
                                                    Y.Where(n => n != y).ToArray()));
                    }
                } else
                    S.AppendFormat(FormatCachedN2, STR(Y[0], X[0]), STR(Y[1], X[1]), STR(Y[0], X[1]), STR(Y[1], X[0]));

                C[M] = string.Format(FormatMemberCached, M, S);
                S.Clear();
            }

            return M;
        }


        static void WriteCachedCodes()
        {
            foreach (var I in CachedNSets) {
                foreach (var J in I) {
                    Console.WriteLine(J.Value);
                }

                Console.WriteLine();
            }
        }

        static List<Dictionary<string, string>> CachedNSets = new List<Dictionary<string, string>>();

        static Dictionary<string, string> GetCacheSets(int N)
        {
            while (CachedNSets.Count < N + 1) {
                CachedNSets.Add(new Dictionary<string, string>());
            }

            return CachedNSets[N];
        }

        static void WriteInverse(int N, StringBuilder S)
        {
            var S2 = new StringBuilder();

            var X = Enumerable.Range(0, N).ToArray();
            var Y = Enumerable.Range(0, N).ToArray();

            for (int y = 0; y < N; y++) {
                for (int x = 0; x < N; x++) {
                    var plus = (x + y) % 2 == 1 ? "-" : " ";

                    // X and y flipped here for traverse matrix
                    WriteDeterminant(N - 1, Y.Where(n => n != y).ToArray(), X.Where(n => n != x).ToArray(), S2, false);
                    S.AppendLine(string.Format(FormatMemberInvers, STR2(y, x), plus, S2));
                    S2.Clear();
                }
            }
        }
    }
}
