using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataExporter
{
    public interface IExporter
    {
        Task Export();
    }
}
