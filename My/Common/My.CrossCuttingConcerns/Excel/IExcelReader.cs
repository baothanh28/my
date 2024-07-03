using System.IO;

namespace My.CrossCuttingConcerns.Excel;

public interface IExcelReader<T>
{
    T Read(Stream stream);
}
