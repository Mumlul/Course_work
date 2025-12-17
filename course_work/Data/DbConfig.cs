using System;
using Microsoft.EntityFrameworkCore;

namespace course_work.Data;

public static class DbConfig
{
    // Строка подключения к базе данных
    public static string ConnectionString =>
        "server=185.247.17.245;" +
        "port=3306;" +
        "database=default_db;" +
        "user=gen_user;" +
        "password=&u3q,,T50oQKzX;" +
        "charset=utf8mb3;" +
        "SslMode=Preferred";

    // Версия MySQL
    public static MySqlServerVersion ServerVersion =>
        new MySqlServerVersion(new Version(8, 4, 4));
}