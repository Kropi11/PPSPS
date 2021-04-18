using System.Collections;
using System.Collections.Generic;
using PPSPS.Models;

namespace PPSPS.ViewModel
{
    public class MultipleData
    {
        public IEnumerable<PPSPSAssignment> assignments { get; set; }
        public IEnumerable<PPSPSFile> files { get; set; }
    }
}