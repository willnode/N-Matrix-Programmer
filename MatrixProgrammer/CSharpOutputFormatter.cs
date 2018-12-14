using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixProgrammer
{
    class CSharpOutputFormatter : OutputFormatter
    {
        public string MatrixElement(int matrixSize, int row, int column)
        {
            return string.Format("m{0}{1}", row, column);
        }

        public string SourceMatrixElement(int matrixSize, int row, int column)
        {
            return "m." + MatrixElement(matrixSize, row, column);
        }

        public string Determinant(string content)
        {
            return string.Format("var det = {0};\ndet = 1 / det;\n", content);
        }

        public string Result(int matrixSize, string content)
        {
            return string.Format("Matrix{0}x{0} inv = new Matrix{0}x{0}() {{\n{1}}};", matrixSize, content);
        }

        public string InverseMember(string matrixElement, string sign, string content)
        {
            return string.Format("   {0} = det * {1}({2}),", matrixElement, sign, content);
        }

        public bool IsCacheMemberUppercase()
        {
            return true;
        }

        public string CacheMember(string name, string content)
        {
            return string.Format("var {0} = {1};", name, content);
        }

        public string CacheContent(string a, string b, string c, string d)
        {
            return string.Format("{0} * {1} - {2} * {3}", a, b, c, d);
        }
    }
}
