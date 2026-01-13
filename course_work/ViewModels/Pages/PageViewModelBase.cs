using Amazon.S3;
using Amazon.S3.Model;
using Appi_Stand.Models.Services;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace course_work.ViewModels.Pages;

public partial class PageViewModelBase:ViewModelBase
{
    public string? Title { get; set; }
    public string? Image { get; set; }
    
    [ObservableProperty]
    public bool _textVisible  = true;
    public virtual Task OnNavigatedTo() => Task.CompletedTask;

    public static async Task SendMessageAsync(
        string fromEmail,
        string password,
        string toEmail,
        string subject,
        string body)
    {
        try
        {
            var message = new MailMessage(fromEmail, toEmail, subject, body)
            {
                IsBodyHtml = true 
            };

            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };

            await smtp.SendMailAsync(message);
            Console.WriteLine("Письмо успешно отправлено!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отправке: {ex.Message}");
        }
    }

    public string ChoseeImage()
    {
        string choice="";
        
        return choice;
    }



    public static async Task<string?> ChooseFile()
    {
        var file = new FileDialogService();

        var filePath = await file.ShowOpenFileDialogAsync("jpg");
        if (string.IsNullOrEmpty(filePath))
            return "";
        Console.WriteLine(filePath);
        return filePath;
    }




    //how use 
    /*SendMessageAsync(
            fromEmail: "ploskih44@gmail.com",
            password: "qhyz ocrc yvfi lxbr",
            toEmail: "azarenko2000lipa@gmail.com",
            subject: "Тестовое сообщение",
            body: "Привет! Это тестовое письмо."
        );*/

}