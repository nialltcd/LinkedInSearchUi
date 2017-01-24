using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using HtmlAgilityPack;
using LinkedInSearchUi.DataTypes;
using LinkedInSearchUi.Indexing;
using System;

namespace LinkedInSearchUi.Model
{
    public class Model
    {
        private HtmlParser _parser;
        private LuceneService _luceneService;
        public Model()
        {
            _parser = new HtmlParser();
            
        }

        public List<Person> ParseRawHtmlFilesFromDirectory()
        {
            List<Person> people = new List<Person>();
            var document = new HtmlDocument();
            foreach (var file in Directory.EnumerateFiles(@"C:\Users\Niall\Documents\Visual Studio 2015\Projects\LinkedInSearchUi\LinkedIn Dataset\SmallLinkedInDataSet", "*.html"))
            {
                try
                {
                    document.Load(file);
                    people.Add(_parser.GeneratePersonFromHtmlDocument(document));
                }
                catch (Exception e) { }
            }
            _luceneService = new LuceneService(people);
            return people;
        }

        public List<Person> LuceneSearch(string textSearch)
        {
            return _luceneService.SearchIndex(textSearch);
        }
    }
}