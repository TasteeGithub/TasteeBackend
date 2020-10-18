
using Microsoft.AspNetCore.Http;

namespace Tastee.Application.Interfaces
{
    public interface IFileService
    {
        string[] Upload(IFormFileCollection files);
    }
}
