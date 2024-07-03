using System.IO;

namespace My.CrossCuttingConcerns.Excel;

public interface IExcelWriter<T>
{
    void Write(T data, Stream stream);
}
