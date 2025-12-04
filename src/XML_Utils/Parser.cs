using System.Xml;
using System.Collections.Generic;
using System.Linq;


namespace XML_Utils;

public abstract class Parser : IParser
{
    public IList<Class> Classes { get; protected set; } = new List<Class>();

    public IList<Class> Find(Filters filters)
    {
        if (Classes == null) return new List<Class>();

        return (from cl in Classes
                where filters.ValidateClass(cl)
                select cl).ToList();
    }

    public abstract bool Load(Stream inputstream, XmlReaderSettings settings);
}