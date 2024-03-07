using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSupportToolAPI.DataObjects
{
    public class StudentGradesReport
    {
        public string ReportName { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; } = DateTime.Now;
        public List<StudentGradesReportEntry> ReportEntries { get; set; } = new();
    }
}
