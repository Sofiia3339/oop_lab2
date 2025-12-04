using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Xml;

namespace XML_Utils;

public class SAXParser : Parser
{
    public SAXParser()
    {
        Classes = new List<Class>();
    }

    public override bool Load(Stream inputStream, XmlReaderSettings settings)
    {
        Classes.Clear();
        try
        {
            var reader = XmlReader.Create(inputStream, settings);
            while (reader.Read())
            {
                if (!(reader.NodeType == XmlNodeType.Element && reader.Name == "Person"))
                {
                    continue;
                }

                var person = new Person();
                SkipToText(reader);
                person.Name.FirstName = reader.Value;
                SkipToText(reader);
                person.Name.MiddleName = reader.Value;
                SkipToText(reader);
                person.Name.LastName = reader.Value;
                SkipToText(reader);
                person.Faculty = reader.Value;
                SkipToText(reader);
                person.Chair = reader.Value;

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Classes")
                    {
                        break;
                    }

                    if (reader.NodeType != XmlNodeType.Element)
                    {
                        continue;
                    }

                    var cl = new Class
                    {
                        Person = person,
                    };
                    SkipToText(reader);
                    cl.Subject = reader.Value;
                    SkipToText(reader);
                    cl.Date.Day = reader.Value;
                    SkipToText(reader);
                    cl.Date.Time = reader.Value;
                    SkipToText(reader);
                    cl.Audience = reader.Value;

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Students")
                        {
                            break;
                        }

                        if (!(reader.NodeType == XmlNodeType.Element && reader.Name == "Student"))
                        {
                            continue;
                        }

                        var student = new Student();
                        SkipToText(reader);
                        student.Name.FirstName = reader.Value;
                        SkipToText(reader);
                        student.Name.MiddleName = reader.Value;
                        SkipToText(reader);
                        student.Name.LastName = reader.Value;
                        SkipToText(reader);
                        student.Group = reader.Value;

                        cl.Students.Add(student);
                    }

                    Classes.Add(cl);
                }

            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static void SkipToText(XmlReader reader)
    {
        do
        {
            if (!reader.Read())
            {
                throw new Exception();
            }
        } while (reader.NodeType != XmlNodeType.Text);
    }
}
