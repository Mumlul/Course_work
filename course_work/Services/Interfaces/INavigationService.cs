using course_work.ViewModels;

namespace course_work.Models;

public interface INavigationService
{
    void Navigate<T>() where T : ViewModelBase;
}