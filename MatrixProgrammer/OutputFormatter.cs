using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixProgrammer
{
    interface OutputFormatter
    {
        string MatrixElement(int matrixSize, int row, int column);

        string SourceMatrixElement(int matrixSize, int row, int column);

        string Determinant(string content);

        string Result(int matrixSize, string content);

        string InverseMember(string matrixElement, string sign, string content);

        bool IsCacheMemberUppercase();

        string CacheMember(string name, string content);

        string CacheContent(string a, string b, string c, string d);
    }
}
