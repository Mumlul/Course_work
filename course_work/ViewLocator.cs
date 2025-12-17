using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using course_work.ViewModels;

namespace course_work;

/// <summary>
/// Given a view model, returns the corresponding view if possible.
/// </summary>
[RequiresUnreferencedCode(
    "Default implementation of ViewLocator involves reflection which may be trimmed away.",
    Url = "https://docs.avaloniaui.net/docs/concepts/view-locator")]
public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param == null) return null;

        var viewModelType = param.GetType();
        var viewTypeName = viewModelType.FullName!.Replace("ViewModel", "View");

        // Ищем в том же assembly
        var viewType = viewModelType.Assembly.GetType(viewTypeName);
        if (viewType == null)
        {
            Console.WriteLine($"View not found for {viewTypeName}");
            return new TextBlock { Text = "View not found: " + viewTypeName };
        }

        var control = (Control)Activator.CreateInstance(viewType)!;
        control.DataContext = param;
        Console.WriteLine($"View built for {viewModelType.Name}");
        return control;
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}