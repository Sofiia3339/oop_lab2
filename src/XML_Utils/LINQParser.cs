using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace XML_Utils;

public class LINQParser : Parser
{
    public LINQParser()
    {
        Classes = new List<Class>();
    }

    public override bool Load(Stream inputStream, XmlReaderSettings settings)
    {
        XDocument document;
        using var reader = XmlReader.Create(inputStream, settings);
        try
        {
            Classes.Clear();
            document = XDocument.Load(reader);
            if (document == null)
            {
                return true;
            }
            var result = from person in document.Descendants("Person")
                         from cl in person.Descendants("Class")
                         select
            new Class
            {
                Person = new Person
                {
                    Name = new FullName
                    {
                        FirstName = person.Element("Name")?.Element("FirstName")?.Value ?? "",
                        LastName = person.Element("Name")?.Element("LastName")?.Value ?? "",
                    },
                    Faculty = person.Element("Faculty")?.Value ?? "",
                    Chair = person.Element("Chair")?.Value ?? "",
                },
                Date = new ClassDate
                {
                    Day = cl.Element("Date")?.Element("Day")?.Value ?? "",
                    Time = cl.Element("Date")?.Element("Time")?.Value ?? "",
                },
                Audience = cl.Element("Audience")?.Value ?? "",
                Students = (from st in cl.Descendants("Student")
                            select new Student
                            {
                                Name = new FullName
                                {
                                    FirstName = st.Element("FirstName")?.Value ?? "",
                                    MiddleName = st.Element("MiddleName")?.Value ?? "",
                                    LastName = st.Element("LastName")?.Value ?? "",
                                },
                                Group = st.Element("Group")?.Value ?? "",
                            }).ToList(),
            };
            foreach (var cl in result)
            {
                Classes.Add(cl);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
}
