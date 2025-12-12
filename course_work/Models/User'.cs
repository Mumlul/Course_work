namespace course_work.Models;

public class User_
{
    int age;
    string name;
    string email;
    string password;
    
    
    public string Get_Message()
    {
        return $"Hello, {name}!";
    }
    
}