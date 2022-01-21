namespace FileSystemProvider;

public interface IFileProvider
{
    bool Exists(string filename);
    Stream Read(string filename);
    void Write(string filename, Stream stream);
}
