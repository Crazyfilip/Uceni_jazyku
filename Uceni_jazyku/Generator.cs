﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Uceni_jazyku
{
    class Generator : IGenerator
    {
        public StreamReader Generate(IStudentModel ms, ILanguageModel lm, IGenData data)
        {
            return data.GetDataFile();
        }
    }
}
