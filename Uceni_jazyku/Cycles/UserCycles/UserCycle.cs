using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Uceni_jazyku.Cycles
{
    /// <summary>
    /// Abstract class for user cycles.
    /// User cycle reflects user's activity in learning and application
    /// Has role in internal process of adapting to user's progress
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

        /// <summary>
        /// Clean up method used in lifecycle steps
        /// </summary>
        public void DeleteCycleFile()
        {
            DeleteCycleFile(path);
        }
    }
}
