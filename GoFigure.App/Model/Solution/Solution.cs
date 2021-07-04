using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFigure.App.Model.Solution
{
    class Solution
    {
        public int Target { get; set; }

        public IEnumerable<ISolutionSlotValue> CpuSolution { get; set; }

        public IEnumerable<int> AvailableNumbers => 
            CpuSolution.Where(s => s is NumberSlotValue)
            .Cast<NumberSlotValue>()
            .Select(n => n.Value);
    }
}
