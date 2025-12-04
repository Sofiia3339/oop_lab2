using System.Text;

namespace XML_Utils;

public class ClassDate
{
    public string Day { get; set; }

    public string Time { get; set; }

    public ClassDate()
    {
        Day = "";
        Time = "";
    }

    public override string ToString() => new StringBuilder()
        .Append(Day)
        .Append(", ")
        .Append(Time)
        .ToString();
}