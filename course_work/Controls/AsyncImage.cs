using System;
using Avalonia;
using Avalonia.Controls;
using course_work.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace course_work.Controls;

public static class AsyncImage
{
    public static readonly AttachedProperty<string?> SourceUrlProperty =
        AvaloniaProperty.RegisterAttached<Image, string?>(
            "SourceUrl",
            typeof(AsyncImage));

    public static void SetSourceUrl(Image image, string? value)
        => image.SetValue(SourceUrlProperty, value);

    public static string? GetSourceUrl(Image image)
        => image.GetValue(SourceUrlProperty);

    static AsyncImage()
    {
        SourceUrlProperty.Changed.Subscribe(OnSourceUrlChanged);
    }

    private static async void OnSourceUrlChanged(
        AvaloniaPropertyChangedEventArgs<string?> e)
    {
        if (e.Sender is not Image image)
            return;

        if (string.IsNullOrWhiteSpace(e.NewValue.Value))
            return;

        var loader = App.Services.GetRequiredService<IImageLoaderService>();

        var bitmap = await loader.LoadAsync(e.NewValue.Value);

        image.Source = bitmap;
    }
}