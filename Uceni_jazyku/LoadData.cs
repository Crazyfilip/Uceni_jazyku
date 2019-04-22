using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

namespace Uceni_jazyku
{
    class LoadData
    {
        private XmlDocument CreateModel(string name, out string filename)
        {
            XDocument xDoc = new XDocument(
            new XDeclaration("1.0", "UTF-16", null),
            new XElement("language",
                new XElement("iso_code",null),
                new XElement("language_name",
                    new XElement("name", name)
                    ),
                new XElement("words",null)
            ));
            filename = Environment.CurrentDirectory + "\\Model_jazyka\\" + name + ".xml";
            xDoc.Save(filename);
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            return doc;
        }

        private XmlDocument FindModel(IEnumerable<string> files, string which, out string file)
        {
            XmlDocument doc = new XmlDocument();
            foreach (var filename in files)
            {
                doc.Load(filename);
                XmlNode root = doc.DocumentElement;
                XmlNodeList nodes = root.SelectNodes("/language/language_name/name");
                for (int i = 0; i < nodes.Count; i++)
			    {
			        XmlNode node = nodes.Item(i);
                    if (node.InnerText == which)
                    {
                        file = filename;
                        return doc;
                    }
			    }
            }
            return CreateModel(which,out file);
            // throw new Exception("Language model for " + which + "not found");
        }

        private XmlNode CreateValue(XmlDocument model, string val)
        {
            XmlNode value = model.CreateElement("value");
            value.InnerText = val;
            return value;
        }

        private pair<string> getParsedCategory(string cat)
        {
            pair<string> pom;
            pom.first = cat.Substring(0, cat.IndexOf('('));
            pom.second = cat.Substring(cat.IndexOf('(') + 1, cat.IndexOf(')') - cat.IndexOf('(') - 1);
            return pom;
        }

        private XmlNode CreateCategories(XmlDocument model, string cat)
        {
            XmlNode categories = model.CreateElement("categories");
            XmlElement category = model.CreateElement("category");
            pair<string> categ = getParsedCategory(cat);
            category.SetAttribute("id", categ.first);
            category.InnerText = categ.second;
            categories.AppendChild(category);
            return categories;
        }

        private XmlNode CreateTranslation(XmlDocument model, string word, string lang)
        {
            XmlNode trans = model.CreateElement("translation");
            XmlElement target_language = model.CreateElement("target_language");
            target_language.SetAttribute("name", lang);
            target_language.InnerText = word;
            trans.AppendChild(target_language);
            return trans;
        }

        private void Insert(XmlDocument model, List<pair<string>> words, string lang, string cat, Comparison<string> comparer)
        {

            XmlNode root = model.DocumentElement;

            XmlNode wordsNode = root.SelectNodes("/language/words").Item(0);
            int pom = 0;
            pair<string> kategorie = getParsedCategory(cat);
            foreach (var word in words)
            {
                XmlNode value = CreateValue(model, word.first);
                XmlNode categories = CreateCategories(model, cat);
                XmlNode translation = CreateTranslation(model, word.second, lang);
                XmlNode newNode = model.CreateElement("word");
                newNode.AppendChild(value);
                newNode.AppendChild(categories);
                newNode.AppendChild(translation);
                wordsNode.AppendChild(newNode);  
            }
        }

        struct pair<T>
        {
            public T first;
            public T second;
            
        }

        public void LoadDataToDatabase(string filename)
        {
            String currentDir = Environment.CurrentDirectory;
            //Console.WriteLine(currentDir);
            var files = Directory.EnumerateFiles(currentDir+"\\Model_jazyka","*.xml");
            XmlDocument model1, model2;
            StreamReader reader = new StreamReader(filename);
            string line;
            string first_language, second_language;
            string category; //zatím jen jedna kategorie
            line = reader.ReadLine();
            String[] pom = line.Substring(1, line.Length - 2).Split('=');
            String[] langs = pom[1].Split('|');
            first_language = langs[0];
            string filename_model1;
            string filename_model2;
            model1 = FindModel(files, first_language,out filename_model1);
            second_language = langs[1];
            model2 = FindModel(files, second_language, out filename_model2);
            line = reader.ReadLine();
            pom = line.Substring(1, line.Length - 2).Split('=');
            category = pom[1];
            List<pair<string>> slovicka = new List<pair<string>>();
            List<pair<string>> slovicka2 = new List<pair<string>>();
            while ((line = reader.ReadLine()) != null)
            {
                pom = line.Split('-');
                pair<string> p,q;
                p.first = pom[0];
                p.second = pom[1];
                q.first = pom[1];
                q.second = pom[0];
                slovicka.Add(p);
                slovicka2.Add(q);
            }
            Comparison<pair<string>> comp = (x, y) => 
            { 
                if (x.first != y.first)
                    return String.Compare(x.first, y.first, false, new CultureInfo("de-DE"));
                else
                    return String.Compare(x.second, y.second, false, new CultureInfo("cs-CZ"));
            };
            slovicka.Sort(comp);
            Insert(model1, slovicka, second_language, category, (x, y) => { return String.Compare(x, y, false, new CultureInfo("de-DE")); });
            model1.Save(filename_model1);

            Comparison<pair<string>> comp2 = (x, y) =>
            {
                if (x.first != y.first)
                    return String.Compare(x.first, y.first, false, new CultureInfo("cs-CZ"));
                else
                    return String.Compare(x.second, y.second, false, new CultureInfo("de-DE"));
            };
            slovicka2.Sort(comp2);
            Insert(model2, slovicka2, first_language, category, (x, y) => { return String.Compare(x, y, false, new CultureInfo("cs-CZ")); });
            model2.Save(filename_model2);
        }
    }
}
