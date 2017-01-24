using LinkedInSearchUi.DataTypes;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace LinkedInSearchUi.Indexing
{
    public class CustomXmlWriter
    {
         public void WriteToFile(List<Person> people)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Person>));
            TextWriter tw = new StreamWriter(@"U:\5th Year\Thesis\LinkedIn\XML\data.xml");
            xs.Serialize(tw, people);
        }
    }
}