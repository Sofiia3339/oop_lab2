using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace XML_Utils;

public interface IParser
{
    public bool Load(Stream inputStream, XmlReaderSettings settings);
    public IList<Class> Find(Filters filters);
}
