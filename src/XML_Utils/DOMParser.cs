using System.Xml;
using System.Xml.Schema;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace XML_Utils;

public class DOMParser : Parser
{
    public DOMParser()
    {
        Classes = new List<Class>();
    }

    public override bool Load(Stream inputStream, XmlReaderSettings settings)
    {
        Classes.Clear();
        var document = new XmlDocument();
        using var reader = XmlReader.Create(inputStream, settings);
        try
        {
            document.Load(reader);
            if (document == null || document.DocumentElement == null)
            {
                return true;
            }
            foreach (XmlNode person in document.DocumentElement.ChildNodes)
            {
                foreach (XmlNode item in person?.SelectNodes("./Classes/Class"))
                {
                    var personName = new FullName
                    {
                        FirstName = person?.SelectSingleNode("./Name/FirstName")?.InnerText ?? "",
                        MiddleName = person?.SelectSingleNode("./Name/MiddleName")?.InnerText ?? "",
                        LastName = person?.SelectSingleNode("./Name/LastName")?.InnerText ?? "",
                    };
                    List<Student> students = new();

                    foreach (XmlNode stNode in item?.SelectNodes("./Students/Student"))
                    {
                        students.Add(new Student()
                        {
                            Name = new FullName
                            {
                                FirstName = stNode?.SelectSingleNode("./FirstName")?.InnerText ?? "",
                                MiddleName = stNode?.SelectSingleNode("./MiddleName")?.InnerText ?? "",
                                LastName = stNode?.SelectSingleNode("./LastName")?.InnerText ?? "",
                            },
                            Group = stNode?.SelectSingleNode("./Group")?.InnerText ?? "",
                        });
                    }

                    var cl = new Class
                    {
                        Person = new Person
                        {
                            Name = personName,
                            Chair = person?.SelectSingleNode("./Chair")?.InnerText ?? "",
                            Faculty = person?.SelectSingleNode("./Faculty")?.InnerText ?? "",
                        },
                        Date = new ClassDate
                        {
                            Day = item?.SelectSingleNode("./Date/Day")?.InnerText ?? "",
                            Time = item?.SelectSingleNode("./Date/Time")?.InnerText ?? "",
                        },
                        Audience = item?.SelectSingleNode("./Audience")?.InnerText ?? "",
                        Subject = item?.SelectSingleNode("./Subject")?.InnerText ?? "",
                        Students = students,
                    };

                    Classes.Add(cl);
                }
            }
        }
        catch
        {
            return false;
        }

        return true;
    }
}
