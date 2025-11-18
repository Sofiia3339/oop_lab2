using System.Xml;
using System.Collections.Generic;
using System.Linq;


namespace XML_Utils;

public abstract class Parser : IParser
{
    protected IList<Class> Classes;
    public IList<Class> Find(Filters filters) => (from cl in Classes where filters.ValidateClass(cl) select cl).ToList();

    public abstract bool Load(Stream inputstream, XmlReaderSettings settings);
}