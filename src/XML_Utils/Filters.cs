namespace XML_Utils;

public struct Filters
{
    public string Name { get; set; }

    public string Subject { get; set; }

    public string Faculty { get; set; }

    public string Chair { get; set; }

    public string Date { get; set; }

    public string Audience { get; set; }

    public string Group { get; set; }

    public Filters()
    {
        Name = "";
        Subject = "";
        Faculty = "";
        Chair = "";
        Audience = "";
        Date = "";
        Group = "";
    }

    public readonly bool ValidateClass(Class cl)
    {
        var name = cl.Person.Name.ToString().ToLower().Contains(Name.ToLower());
        var date = cl.Date.ToString().ToLower().Contains(Date.ToLower());
        var faculty = cl.Person.Faculty.ToLower().Contains(Faculty.ToLower());
        var chair = cl.Person.Chair.ToLower().Contains(Chair.ToLower());
        var audience = cl.Audience.ToLower().Contains(Audience.ToLower());
        var subject = cl.Subject.ToLower().Contains(Subject.ToLower());
        var group = ValidateGroup(cl.Students);

        return name && date && faculty && chair && audience && subject && group;
    }

    private readonly bool ValidateGroup(IList<Student> students)
    {
        foreach (var st in students)
        {
            if (st.Group.ToLower().Contains(Group.ToLower()))
            {
                return true;
            }
        }

        return false;
    }
}
