using System;
using Avalonia;
using Avalonia.Controls;
using course_work.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace course_work.Services.Service;

public static class ImageExtensions
{
    // Регистрируем AttachedProperty
    public static readonly AttachedProperty<string?> SourceUrlProperty =
        AvaloniaProperty.RegisterAttached<Image, string?>(
            "SourceUrl",
            typeof(ImageExtensions));

    public static string? GetSourceUrl(Image control) => control.GetValue(SourceUrlProperty);
    public static void SetSourceUrl(Image control, string? value) => control.SetValue(SourceUrlProperty, value);

    static ImageExtensions()
    {
        SourceUrlProperty.Changed.Subscribe(async e =>
        {
            if (e.Sender is not Image image)
                return;

            var url = e.NewValue.Value ?? string.Empty;
            if (string.IsNullOrWhiteSpace(url))
            {
                image.Source = null;
                return;
            }

            try
            {
                var loader = App.Services.GetRequiredService<IImageLoaderService>();
                var bitmap = await loader.LoadAsync(url);

                image.Source = bitmap;
            }
            catch (Exception ex)
            {
                image.Source = null;
                Console.WriteLine($"Failed to load image '{url}': {ex.Message}");
            }
        });
    }
}