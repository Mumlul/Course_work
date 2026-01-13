using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace Appi_Stand.Models.Services;

public interface IFileDialogService
{
    Task<string> ShowOpenFileDialogAsync(string filter = null);
}

public class FileDialogService : IFileDialogService
{
    public async Task<string> ShowOpenFileDialogAsync(string filter = null)
    {
        var dialog = new OpenFileDialog();
        if (!string.IsNullOrEmpty(filter))
        {
            dialog.Filters.Add(new FileDialogFilter { Name = "Files", Extensions = { filter } });
        }
        
        var files = await dialog.ShowAsync(new Window());
        return files?.FirstOrDefault();
    }
}