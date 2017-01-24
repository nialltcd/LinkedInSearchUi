using System.Collections.Generic;
using System.Data;
using LinkedInSearchUi.DataTypes;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace LinkedInSearchUi.Indexing
{
    public class LuceneService
    {
        // Note there are many different types of Analyzer that may be used with Lucene, the exact one you use
        // will depend on your requirements
        private Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
        private Directory luceneIndexDirectory;
        private IndexWriter writer;
        private string indexPath = @"c:\temp\LuceneIndex";

        public LuceneService(List<Person> people)
        {
            luceneIndexDirectory = BuildIndex(people);
        }

        private Directory BuildIndex(IEnumerable<Person> people )
        {
            var directory = new RAMDirectory();

            using (Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30))
            using (var writer = new IndexWriter(directory, analyzer, new IndexWriter.MaxFieldLength(1000)))
            { // the writer and analyzer will popuplate the directory with documents

                foreach (Person person in people)
                {
                    var document = new Document();

                    document.Add(new Field("Name", person.Name, Field.Store.YES, Field.Index.ANALYZED));
                    string all = person.Name;
                    foreach (var experience in person.Experiences)
                    {
                        document.Add(new Field("Experience", experience, Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("Organisation", experience.Organisation, Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("Role", experience.Role, Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("Duration", experience.Duration, Field.Store.YES, Field.Index.NOT_ANALYZED));
                        all += " "+experience.Organisation +" "+ experience.Role+" ";
                    }
                    document.Add(new Field("All", all, Field.Store.YES, Field.Index.ANALYZED));
                    writer.AddDocument(document);
                }

                writer.Optimize();
                writer.Flush(true, true, true);
            }
            return directory;
        }

        public List<Person> SearchIndex(string textSearch)
        {
            List<Person> searchResults = new List<Person>();
            using (var reader = IndexReader.Open(luceneIndexDirectory, true))
            using (var searcher = new IndexSearcher(reader))
            {
                using (Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30))
                {
                    var queryParser = new QueryParser(Version.LUCENE_30, "All", analyzer);
                    var query = queryParser.Parse(textSearch);
                    //queryParser.AllowLeadingWildcard = true;

                    //var query = queryParser.Parse(textSearch);

                    var collector = TopScoreDocCollector.Create(1000, true);

                    searcher.Search(query, collector);

                    var matches = collector.TopDocs().ScoreDocs;

                    foreach (var item in matches)
                    {

                        var id = item.Doc;
                        var doc = searcher.Doc(id);

                        foreach (IFieldable field in doc.GetFields())
                        {

                            var person = new Person();
                            if (field.Name == "Name")
                            {
                                person.Name = doc.GetField("Name").StringValue;
                            }
                            else if (field.Name == "Role")
                            {
                                person.Experiences.Add.MyList.Add(field.StringValue);
                            }
                        }

                        var person = new Person();

                        person.Name = doc.GetField("Name").StringValue;
                        foreach(var x,y in doc.GetFields("Role"))
                        {

                        }
                        person.Experiences.Role = doc.GetField("Role").StringValue;

                        searchResults.Add(person);
                    }
                }
            }
            return searchResults;
        }
    }
}