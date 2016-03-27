using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrentTasksTrayIconNotifier
{
    public interface IProgressBarSource
    {
        Decimal Progress { get; }
        Decimal Max { get; }
        string Name { get; }
    }
}
