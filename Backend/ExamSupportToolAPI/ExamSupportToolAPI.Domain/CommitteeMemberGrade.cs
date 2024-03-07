using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExamSupportToolAPI.Domain
{
    public class CommitteeMemberGrade: BaseEntity
    {     
        public Guid CommitteeMemberId { get; private set; }
        public int TheoryGrade { get; private set; }
        public int ProjectGrade { get; private set; }
        private CommitteeMemberGrade() { }
        public static CommitteeMemberGrade Create(Guid memberId, int theoryGrade, int projectGrade) 
        {
            return new CommitteeMemberGrade
            {
                CommitteeMemberId = memberId,
                TheoryGrade = theoryGrade,
                ProjectGrade = projectGrade
            };
        }
        public void UpdateGrades(int theoryGrade, int projectGrade) 
        {
            if (theoryGrade > 0 && theoryGrade <= 10)
                TheoryGrade = theoryGrade;
            if (projectGrade > 0 && projectGrade <= 10)
                ProjectGrade = projectGrade;
        }
    }
}
