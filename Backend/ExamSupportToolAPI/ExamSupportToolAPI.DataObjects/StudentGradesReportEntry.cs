using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSupportToolAPI.DataObjects
{
    public class StudentGradesReportEntry
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal TheoryGrade { get; set; } = 0;
        public decimal ProjectGrade { get; set; } = 0;  
        public bool Absent { get; set; } = false;

    }
}
