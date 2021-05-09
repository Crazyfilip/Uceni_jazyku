﻿using System;
using System.Windows.Forms;
using Uceni_jazyku.Cycles;
using System.IO;
using log4net;
using System.Reflection;
using log4net.Config;
using Uceni_jazyku.Language.Impl;
using Uceni_jazyku.Language;
using System.Collections.Generic;
using Uceni_jazyku.Cycles.Program;
using System.Runtime.Serialization;
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
            UserCycle userCycle = CycleService.GetInstance().GetActiveCycle();
            if (userCycle != null)
            {
                Application.Run(new WelcomePage(userCycle));
            }
            else
            {
                Application.Run(new LoginPage(new UserCycle()));
            }
        }

        // TODO better solution
        private static void PrepareApp()
        {
            Directory.CreateDirectory("./users");
            Directory.CreateDirectory("./cycles/service");
            Directory.CreateDirectory("./courses/service");
            PrepateTemplateCourses();
        }

        private static void PrepateTemplateCourses()
        {
            if (File.Exists("./courses/service/database.xml"))
            {
                LanguageTopic topic1 = new LanguageTopic() { TopicId = "a-id", Description = "A", Lessons = new List<LanguageProgramItem> { new LanguageProgramItem("A1"), new LanguageProgramItem("A2") } };
                LanguageTopic topic2 = new LanguageTopic() { TopicId = "b-id", Description = "B", Lessons = new List<LanguageProgramItem> { new LanguageProgramItem("B1"), new LanguageProgramItem("B2") } };
                TemplateLanguageCourse template = new TemplateLanguageCourse(new List<LanguageTopic>() { topic1, topic2 }) { CourseId = "template-default" };
                List<LanguageCourse> languageCourses = new List<LanguageCourse>() { template };

                var serializer = new DataContractSerializer(typeof(List<LanguageCourse>));
                using XmlWriter writer = XmlWriter.Create("./courses/service/database.xml");
                serializer.WriteObject(writer, languageCourses);
            }
        }
    }
}
