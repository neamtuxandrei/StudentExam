using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSupportToolAPI.DataObjects
{
    public class StudentGrade
    {
        public Guid Id { get; set; }        
        public string Name { get; set; } = string.Empty;       
        public string DiplomaProjectName { get; set; } = string.Empty;
        public string CoordinatorName { get; set; } = string.Empty;
        public int TheoryGrade { get; set; } = 0;
        public int ProjectGrade { get; set; } = 0;
        public decimal TheoryAverage{ get;set; } = 0;    
        public decimal ProjectAverage { get; set; } = 0;
    }
}
