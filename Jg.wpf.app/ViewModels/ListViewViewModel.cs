using System.Collections.Generic;
using Jg.wpf.core.Notify;

namespace Jg.wpf.app.ViewModels
{
    public class ListViewViewModel : ViewModelBase
    {
        public List<Student> Students { get; set; }
        
        public ListViewViewModel()
        {
            Students = new List<Student>();

            Students.Add(new Student("Mike", 30, "Street 1."));
            Students.Add(new Student("Jack", 32, "Street 12."));
            Students.Add(new Student("Rose", 28, "Street 13."));
        }
    }

    public class Student
    {
        public string Name { get;  }
        public int Age { get; }
        public string Address { get; }

        public Student(string name, int age, string address)
        {
            Name = name;
            Age = age;
            Address = address;
        }
    }
}
