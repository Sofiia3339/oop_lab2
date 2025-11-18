namespace XML_Utils;

public class Student
{
    public FullName Name { get; set; }

    public string Group { get; set; }

    public Student()
    {
        Name = new();
        Group = "";
    }
}