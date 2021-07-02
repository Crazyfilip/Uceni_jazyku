using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uceni_jazyku.Common
{
    /// <summary>
    /// Interface providing Id field.
    /// Used for types used in AbstractRepository
    /// </summary>
    public interface IId
    {
        string Id { get; init; }
    }
}
