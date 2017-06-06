using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixProgrammer
{
    class Program
    {
        static int N = 4;
        static bool DiscardBracketAtN1 = true;

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

        const string FormatPostDeterm = "var det ={0};\ndet = 1 / det;\n";
        const string FormatPostInvers = "return new Matrix4x4() {{\n{0}}};";
        const string FormatMemberInvers = "   {0}= det * {1} ({2});";

        static public void Main(string[] args)
        {
            CheckArguments(args);
            var S = new StringBuilder();
            var S2 = new StringBuilder();
            WriteDeterminant(N, S);
            S.Length -= 1;
            WriteInverse(N, S2);
            Console.WriteLine(string.Format(FormatPostDeterm, S));
            Console.WriteLine(string.Format(FormatPostInvers, S2));
        }

        static void CheckArguments (string[] args)
        {
            if (args.Length > 0)
            {
                int parsed;
                if (int.TryParse(args[0], out parsed))
                {
                    N = parsed;
                }
            }
            if (args.Length > 1)
            {
                bool parsed;
                if (bool.TryParse(args[1], out parsed))
                {
                    DiscardBracketAtN1 = parsed;
                }
            }
        }

        static void WriteDeterminant (int N, StringBuilder S)
        {
            var X = Enumerable.Range(0, N).ToArray();
            var Y = Enumerable.Range(0, N).ToArray();
            WriteDeterminant(N, X, Y, S, true);
        }

        static void WriteDeterminant (int N, int[] X, int[] Y, StringBuilder S, bool NS)
        {
            bool plus = true;
            for (int i = 0; i < N; i++)
            {
                // Sign (and necessary stylings)
                if (i > 0)
                {
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
                if (N > 1)
                {
                    if (N == 2 && DiscardBracketAtN1)
                        S.Append("*");
                    else
                        S.Append("* (");
                    WriteDeterminant(N - 1, X.Where(n => n != x).ToArray(), Y.Where(n => n != y).ToArray(), S, false);
                    if (N != 2 || !DiscardBracketAtN1)
                        S.Append(") ");
                }
            }
        }
        

        static void WriteInverse (int N, StringBuilder S)
        {
            var S2 = new StringBuilder();

            var X = Enumerable.Range(0, N).ToArray();
            var Y = Enumerable.Range(0, N).ToArray();

            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < N; y++)
                {
                    var plus = (x + y) % 2 == 1 ? "-" : " ";
                    WriteDeterminant(N - 1, Y.Where(n => n != x).ToArray(), X.Where(n => n != y).ToArray(), S2, false);
                    S.AppendLine(string.Format(FormatMemberInvers, STR2(x, y), plus, S2));
                    S2.Clear();
                }
            }
        }
        
        /*
        static void WriteHead ()
        {
            StringBuilder s = new StringBuilder();
            s.Append("var det =");
            for (int j = 0; j < N; j++)
            {
                // The plus part
                if (j > 0)
                    s.Append("+");
                for (int k = 0; k < N; k++)
                {
                    var mult = k != (N - 1) ? "*" : "";
                    s.Append(STR(k, (k + j) % N) + mult);
                }
                //s.Append("\n\t");
            }

            for (int j = 0; j < N; j++)
            {
                // The minus part
                if (j == 0)
                    s.Append("\n        ");
                s.Append("-");
                for (int k = 0; k < N; k++)
                {
                    var mult = k != (N - 1) ? "*" : "";
                    s.Append(STR(k, ((N - k) + j) % N) + mult);
                }
                //s.Append("\n\t");
            }
            s.Length -= 1;
            s.AppendLine(";");
            s.AppendLine("det = 1 / det;");
            Console.WriteLine(s.ToString());
        }

        static void WriteBody()
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < N * N; i++)
            {
                int c = i % N, r = i / N;
                s.Append(STR2(r, c) + "= det * (");

                for (int j = 1; j < N; j++)
                {
                    // The plus part
                    if (j > 1)
                        s.Append("+");
                    for (int k = 1; k < N; k++)
                    {
                        var mult = k != (N - 1) ? "*" : "";
                        s.Append(STR(LOOP(k, r), LOOP(k + (j - 1), c)) + mult);
                    }
                    //s.Append("\n\t");
                }

                for (int j = 1; j < N; j++)
                {
                    // The minus part
                    if (j == 1)
                        s.Append("\n             ");
                    s.Append("-");
                    for (int k = 1; k < N; k++)
                    {
                        var mult = k != (N - 1) ? "*" : "";
                        s.Append(STR(LOOP(k, r), LOOP((N - k) + (j - 1), c)) + mult);
                    }
                    //s.Append("\n\t");
                }
                //s.Length -= 1;
                s.Append("),");
                Console.WriteLine(s.ToString());
                s.Clear();
            }
        }


        */
    }
}
