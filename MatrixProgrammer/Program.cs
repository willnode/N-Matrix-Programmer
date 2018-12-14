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
		static int MatrixSize = 4;
        /// <summary>
        /// Optimization Level. Adjusted automatically
        /// </summary>
        static int O = 1;

        static StringBuilder STROB = new StringBuilder();
        static string STRO(int[] t, int[] u)
        {
            STROB.Clear();
            STROB.Append(((char)((formatter.IsCacheMemberUppercase() ? '?' : '_') + t.Length)).ToString());
            STROB.Append(string.Join("", t));
            STROB.Append(string.Join("", u));
            return STROB.ToString();
        }

        //
        // String formats which you're free to change
        //
        static OutputFormatter formatter;

        static public void Main(string[] args)
		{
			CheckArguments(args);

            var S = new StringBuilder();
			var S2 = new StringBuilder();
			WriteDeterminant(MatrixSize, S);
			WriteInverse(MatrixSize, S2);

            if (O >= 2)
                WriteCachedCodes();
			Console.WriteLine(formatter.Determinant(S.ToString()));
			Console.WriteLine(formatter.Result(MatrixSize, S2.ToString()));
        }

		static void CheckArguments (string[] args)
		{
            if (args.Length == 4)
            {
                for (int i = 0; i < args.Length; i += 2)
                {
                    switch (args[i])
                    {
                        case "-n":
                          
                            if (!int.TryParse(args[i + 1], out MatrixSize))
                            {
                                Console.WriteLine("Wrong matrix size");
                                Environment.Exit(0);
                            }
                            break;
                        case "-f":
                            switch (args[i + 1])
                            {
                                case "cpp":
                                    formatter = new CPPOutputFormatter();
                                    break;
                                case "cs":
                                    formatter = new CSharpOutputFormatter();
                                    break;
                                default:
                                    Console.WriteLine("Invalid format");
                                    Environment.Exit(0);
                                    break;
                            }
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Following arguments can be used:");
                Console.WriteLine("\t-n [int] ... size of the matrix");
                Console.WriteLine("\t-f [cpp, cs] ... code formatting");
                Console.WriteLine("\t\t cpp ... C++ code style");
                Console.WriteLine("\t\t cs ... C# code style");
                Environment.Exit(0);
            }

            Console.WriteLine("// " + (formatter is CPPOutputFormatter ? "C++" : "C#")+ " code for compute invertion " + MatrixSize + " x " + MatrixSize + " matrix by willnode and ThomasCZ");

            if (MatrixSize > 3)
                O = MatrixSize - 2;
            else
                O = 1;
		}

		static void WriteDeterminant (int N, StringBuilder S)
		{
			var X = Enumerable.Range(0, N).ToArray();
			var Y = Enumerable.Range(0, N).ToArray();
			WriteDeterminant(N, X, Y, S, true);
		}

		static void WriteDeterminant (int N, int[] X, int[] Y, StringBuilder S, bool NS)
		{
            if (O >= N && O >= 2)
            {
                S.Append(WriteOptimizedNDet(N, X, Y));
                return;
            }

            bool plus = true;
			for (int i = 0; i < N; i++)
			{
				// Sign (and necessary stylings)
				if (i > 0)
				{
					if (NS)
						S.Append("\n\t");
					S.Append(plus ? " + " : " - ");
				}

				// Write this member
                int x = X[i];
				int y = Y[0];
				S.Append(formatter.SourceMatrixElement(MatrixSize, y, x));
				plus = !plus;

				// Take recursive call at minor matrix
                if (N > 1)
				{
					if (ShouldAddBracket(N))
						S.Append(" * (");
					else
						S.Append(" * ");
					WriteDeterminant(N - 1, X.Where(n => n != x).ToArray(), Y.Where(n => n != y).ToArray(), S, false);
					if (ShouldAddBracket(N))
						S.Append(")");
				}
			}
		}

        static bool ShouldAddBracket (int N)
        {
             return N > O + 1;
        }

       
        static string WriteOptimizedNDet(int N, int[] X, int[] Y)
        {
            var S = new StringBuilder();
            var C = GetCacheSets(N);
            var M = STRO(X, Y);
            if (!C.ContainsKey(M))
            {
                if (N > 2)
                {
                    bool plus = true;
                    for (int i = 0; i < N; i++)
                    {
                        // Sign (and necessary stylings)
                        if (i > 0)
                            S.Append(plus ? " + " : " - ");

                        // Write this member
                        int x = X[i];
                        int y = Y[0];
                        S.Append(formatter.SourceMatrixElement(MatrixSize, y, x));
                        plus = !plus;

                        // Take recursive call at minor matrix
                        S.Append(" * ");
                        S.Append(WriteOptimizedNDet(N - 1, X.Where(n => n != x).ToArray(), Y.Where(n => n != y).ToArray()));
                    }
                } else
                    S.AppendFormat(formatter.CacheContent(formatter.SourceMatrixElement(MatrixSize, Y[0], X[0]), formatter.SourceMatrixElement(MatrixSize, Y[1], X[1]), formatter.SourceMatrixElement(MatrixSize, Y[0], X[1]), formatter.SourceMatrixElement(MatrixSize, Y[1], X[0])));

                C[M] = formatter.CacheMember(M, S.ToString());
                S.Clear();
            }
            return M;
        }

      

        static void WriteCachedCodes ()
        {
            foreach (var I in CachedNSets)
            {
                foreach (var J in I)
                {
                    Console.WriteLine(J.Value);
                }
                Console.WriteLine();
            }
        }

        static List<Dictionary<string, string>> CachedNSets = new List<Dictionary<string, string>>();

        static Dictionary<string, string> GetCacheSets (int N)
        {
            while (CachedNSets.Count < N + 1)
            {
                CachedNSets.Add(new Dictionary<string, string>());
            }
            return CachedNSets[N];
        }

        static void WriteInverse(int N, StringBuilder S)
        {
            var S2 = new StringBuilder();

            var X = Enumerable.Range(0, N).ToArray();
            var Y = Enumerable.Range(0, N).ToArray();

            for (int y = 0; y < N; y++)
            {
                for (int x = 0; x < N; x++) 
                {
                    var plus = (x + y) % 2 == 1 ? "-" : "";
                    // X and y flipped here for traverse matrix
                    WriteDeterminant(N - 1, Y.Where(n => n != y).ToArray(), X.Where(n => n != x).ToArray(), S2, false);
                    S.AppendLine(formatter.InverseMember(formatter.MatrixElement(MatrixSize, y, x), plus, S2.ToString()));
                    S2.Clear();
                }
            }
        }
    }
}