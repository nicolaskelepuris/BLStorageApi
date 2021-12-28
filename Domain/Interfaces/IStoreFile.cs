using System.IO;
using System.Threading.Tasks;

namespace Domain.Interfaces;
public interface IStoreFile
{
    Task<string> UploadAsync(Stream data, string fileName);
}
