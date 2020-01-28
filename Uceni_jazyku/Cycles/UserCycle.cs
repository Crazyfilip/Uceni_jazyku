using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Abstract class for user cycles
    /// </summary>
    public abstract class UserCycle : AbstractCycle
    {
        public override void Update()
        {
            throw new NotImplementedException();
        }

        protected virtual void DeleteCycleFile(string filepath)
        {
            File.Delete(filepath);
        }

        public void DeleteCycleFile()
        {
            DeleteCycleFile(path);
        }
    }
}
