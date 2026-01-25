using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using course_work.Services.Interfaces;

namespace course_work.Services.Service;

public sealed class ImageLoaderService : IImageLoaderService
{
    private static readonly HttpClient _http = new();
    private readonly Dictionary<string, Bitmap> _cache = new();

    public async Task<Bitmap?> LoadAsync(string url, CancellationToken ct = default)
    {
        if (_cache.TryGetValue(url, out var cached))
            return cached;

        var bytes = await _http.GetByteArrayAsync(url, ct);

        await using var ms = new MemoryStream(bytes);
        var bitmap = new Bitmap(ms);

        _cache[url] = bitmap;
        return bitmap;
    }
}