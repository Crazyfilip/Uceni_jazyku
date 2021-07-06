using LanguageLearning.Cycle;
using LanguageLearning.Language;
using LanguageLearning.Language.Impl;
using LanguageLearning.Language.Topic;
using LanguageLearning.User;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml;

namespace User_interface
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            PrepareApp();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            UserService userAccountService = new UserService();
            if (userAccountService.IsAnyoneLogged())
            {
                string username = userAccountService.GetLoggedUser();
                LanguageCourse languageCourse = LanguageCourseService.GetInstance().GetActiveLanguageCourse(username);
                CycleService cycleService = CycleService.GetInstance();
                cycleService.SetActiveCourse(username, languageCourse);
                Application.Run(new WelcomePage(username));
            }
            else
            {
                Application.Run(new LoginPage());
            }
        }

        // TODO better solution
        private static void PrepareApp()
        {
            Directory.CreateDirectory("./users");
            Directory.CreateDirectory("./user-models");
            Directory.CreateDirectory("./cycles/service");
            Directory.CreateDirectory("./courses/service");
            Directory.CreateDirectory("./planners");
            PrepateTemplateCourses();
        }

        private static void PrepateTemplateCourses()
        {
            if (!File.Exists("./courses/service/database.xml"))
            {
                LanguageTopic topic1 = new LanguageTopic() { TopicId = "a-id", Description = "A", Lessons = new List<LanguageProgramItem> { new LanguageProgramItem("A1"), new LanguageProgramItem("A2") } };
                LanguageTopic topic2 = new LanguageTopic() { TopicId = "b-id", Description = "B", Lessons = new List<LanguageProgramItem> { new LanguageProgramItem("B1"), new LanguageProgramItem("B2") } };
                TemplateLanguageCourse template = new TemplateLanguageCourse(new List<LanguageTopic>() { topic1, topic2 }) { Id = "template-default" };
                List<LanguageCourse> languageCourses = new List<LanguageCourse>() { template };

                var serializer = new DataContractSerializer(typeof(List<LanguageCourse>));
                using XmlWriter writer = XmlWriter.Create("./courses/service/database.xml");
                serializer.WriteObject(writer, languageCourses);
            }
        }
    }
}
