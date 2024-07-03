namespace My.CrossCuttingConcerns.Csv;

public interface ICsvReader<T>
{
    IEnumerable<T> Read(Stream stream);
}
