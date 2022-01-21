namespace DataProvider.FileProvider;

public class FileSystemProvider : IFileSystemProvider
{
    public bool Exists(string filename)
    {
        return File.Exists(filename);
    }

    public Stream Read(string filename)
    {
        return !Exists(filename)
            ? throw new FileNotFoundException()
            : File.OpenRead(filename);
    }

    public void Write(string filename, Stream stream)
    {
        var buffer = new byte[stream.Length];
        _ = stream.Read(buffer, 0, (int)stream.Length);
        using var writer = new StreamWriter(filename, false);
        writer.WriteLine(System.Text.Encoding.UTF8.GetString(buffer));
    }
}
