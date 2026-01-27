using Amazon.S3;
using Amazon.S3.Model;
using Appi_Stand.Models.Services;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using course_work.Models.Classes;


namespace course_work.ViewModels.Pages;

public partial class PageViewModelBase:ViewModelBase
{
    public string? Title { get; set; }
    public string? ImageBlock { get; set; }
    
    
    [ObservableProperty]
    public bool _textVisible  = false;
    public virtual Task OnNavigatedTo() => Task.CompletedTask;
    public virtual Task OnNavigatedFrom() => Task.CompletedTask;

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

    public static async Task<Bitmap> ConvertImageToByteArray(string url)
    {
        if (string.IsNullOrEmpty(url)) return null;

        try
        {
            using var http = new HttpClient();
            var bytes = await http.GetByteArrayAsync(url);
            using var ms = new MemoryStream(bytes);
            
            return new Bitmap(ms);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Преобразовать фото");
            return null;
        }
    }



    public static async Task<string?> ChooseFile()
    {
        var file = new FileDialogService();

        var filePath = await file.ShowOpenFileDialogAsync("jpg");
        if (string.IsNullOrEmpty(filePath))
            return "";
        return filePath;
    }

    public static int GenerateSecretCode()
    {
        Random random = new Random();
        return random.Next(100000, 999999);
    }

    public async Task<string> SendSecretCode(string email)
    {
        var code = GenerateSecretCode();
        SendMessageAsync(
            fromEmail: "ploskih44@gmail.com",
            password: "qhyz ocrc yvfi lxbr",
            toEmail: email,
            subject: "Секретный код",
            body: $"Здравствуйте,вот ваш секретный код: {code}. Никому не сообщайте его"
        );
        return code.ToString();
    }
    
    public async Task SendMail(string email,string text)
    {
        var code = GenerateSecretCode();
        SendMessageAsync(
            fromEmail: "ploskih44@gmail.com",
            password: "qhyz ocrc yvfi lxbr",
            toEmail: email,
            subject: "Сообщение от администрации",
            body: text
        ); 
    }

    private static string pas = "2H4NLFXQSWUC8A31U1PB";

    private static string pas2 = "EYBr2GBUGTtSdS7fTM8XgBXwSEUDROFMK1wpCwcF";
    public static  async Task<string> UploadImage(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;
        
         var config = new AmazonS3Config
         {
            ServiceURL = "https://s3.twcstorage.ru",
            ForcePathStyle = true
         };

        using var client = new AmazonS3Client(pas, pas2, config);

        var putRequest = new PutObjectRequest
        {
            BucketName = "6a3814f9-ce7403ca-f211-439b-8e9f-f85196600672",
            Key = $"{Path.GetFileName(name)}",
            FilePath = $"{name}",
            ContentType = "image/jpeg"
        };
        var response = await client.PutObjectAsync(putRequest);
        return $"https://6a3814f9-ce7403ca-f211-439b-8e9f-f85196600672.s3.twcstorage.ru/{Path.GetFileName(name)}";
    }
    
    
    //Скачивание и создание word
    public static async Task<string> DownloadWordToTempAsync(string url)
    {
        using var http = new HttpClient();
        var bytes = await http.GetByteArrayAsync(url);
        var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".docx");
        await File.WriteAllBytesAsync(tempFile, bytes);
        return tempFile;
    }
    
    //ZAGRUZKA WORD
    public static async Task<string> UploadWordToTempAsync(string path)
    {
        var config = new AmazonS3Config
        {
            ServiceURL = "https://s3.twcstorage.ru",
            ForcePathStyle = true
        };

        using var client = new AmazonS3Client("2H4NLFXQSWUC8A31U1PB", "EYBr2GBUGTtSdS7fTM8XgBXwSEUDROFMK1wpCwcF", config);

        var putRequest = new PutObjectRequest
        {
            BucketName = "6a3814f9-ce7403ca-f211-439b-8e9f-f85196600672",
            Key = $"{Path.GetFileName(path)}",
            FilePath = $"{path}"
        };
        var response = await client.PutObjectAsync(putRequest);
        return $"https://6a3814f9-ce7403ca-f211-439b-8e9f-f85196600672.s3.twcstorage.ru/{Path.GetFileName(path)}";
    }
    
}