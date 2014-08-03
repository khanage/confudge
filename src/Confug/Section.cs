using System.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace Confudge
{
    public class Section : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var xdoc = XDocument.Load(new XmlNodeReader(section));
            return xdoc;
        }
    }
}
