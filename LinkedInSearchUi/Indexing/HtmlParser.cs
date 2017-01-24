using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using LinkedInSearchUi.DataTypes;

namespace LinkedInSearchUi.Indexing
{
    public class HtmlParser
    {
        public Person GeneratePersonFromHtmlDocument(HtmlDocument document)
        {
            //Retrieve the person's name from the document
            var personName = GetPersonsName(document);

            //Retrieve the persons prior experience from the document
            var experiences = GetPersonsExperience(document);

            return new Person() { Name = personName, Experience = experiences };
        }

        private string GetPersonsName(HtmlDocument document)
        {
            //Retrieve the person's name from the document
            var firstOrDefault = document.DocumentNode.SelectNodes("//span[@class='full-name']").FirstOrDefault();
            if (firstOrDefault != null)
                return firstOrDefault.InnerText;
            return null;
        }

        private List<Experience> GetPersonsExperience(HtmlDocument document)
        {
            List<Experience> experiences = new List<Experience>();
            //Retrieve the persons prior experience from the document
            var experienceNode = document.DocumentNode.SelectNodes("//div[@id='profile-experience'] //div[@class='content vcalendar']");
            if (experienceNode == null) return new List<Experience>();
            foreach (var node in experienceNode)
            {
                var experience = new Experience();
                experience.Role = node.SelectSingleNode(".  //div[@class='postitle'] //span[@class='title']").InnerText;
                experience.Organisation = node.SelectSingleNode(".//div[@class='postitle'] //span[@class='org summary']").InnerText;
                experiences.Add(experience);
                experience.Duration = node.SelectSingleNode(".//span[@class='duration']").InnerText;
            }
            return experiences;
        }
    }
}