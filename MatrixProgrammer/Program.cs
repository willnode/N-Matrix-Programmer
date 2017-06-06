using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixProgrammer
{
	class Program
	{
		static int N = 3;
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

		//
		// String formats which you're free to change
		//
        const string FormatPostDeterm = "var det ={0};\ndet = 1 / det;\n";
		const string FormatPostInvers = "return new Matrix{1}x{1}() {{\n{0}}};";
		const string FormatMemberInvers = "   {0}= det * {1} ({2}),";

		static public void Main(string[] args)
		{
			CheckArguments(args);
			var S = new StringBuilder();
			var S2 = new StringBuilder();
			WriteDeterminant(N, S);
			S.Length -= 1;
			WriteInverse(N, S2);
			Console.WriteLine(string.Format(FormatPostDeterm, S, N));
			Console.WriteLine(string.Format(FormatPostInvers, S2, N));
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
					WriteDeterminant(N - 1, Y.Where(n => n != y).ToArray(), X.Where(n => n != x).ToArray(), S2, false);
					S.AppendLine(string.Format(FormatMemberInvers, STR2(x, y), plus, S2));
					S2.Clear();
				}
			}
		}
	}
}