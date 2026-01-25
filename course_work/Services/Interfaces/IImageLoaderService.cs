using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace course_work.Services.Interfaces;

public interface IImageLoaderService
{
    Task<Bitmap?> LoadAsync(string url, CancellationToken ct = default);
}