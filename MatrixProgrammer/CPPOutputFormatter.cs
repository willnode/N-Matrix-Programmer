using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixProgrammer
{
    class CPPOutputFormatter : OutputFormatter
    {
        public string MatrixElement(int matrixSize, int row, int column)
        {
            return string.Format("mat[{0}]", row * matrixSize + column);
        }

        public string SourceMatrixElement(int matrixSize, int row, int column)
        {
            return "mat." + MatrixElement(matrixSize, row, column);
        }

        public string Determinant(string content)
        {
            return string.Format("float det = {0};\ndet = 1 / det;\n", content);
        }

        public string Result(int matrixSize, string content)
        {
            return string.Format("Mat{0} inv = Mat{0}();\n{1}", matrixSize, content);
        }

        public string InverseMember(string matrixElement, string sign, string content)
        {
            return string.Format("inv.{0} = det * {1}({2});", matrixElement, sign, content);
        }

        public bool IsCacheMemberUppercase()
        {
            return false;
        }

        public string CacheMember(string name, string content)
        {
            return string.Format("float {0} = {1};", name, content);
        }

        public string CacheContent(string a, string b, string c, string d)
        {
            return string.Format("{0} * {1} - {2} * {3}", a, b, c, d);
        }
    }
}
