using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace XML_Utils;

public interface IParser
{
    IList<Class> Classes { get; }

    public bool Load(Stream inputStream, XmlReaderSettings settings);
    public IList<Class> Find(Filters filters);
}
