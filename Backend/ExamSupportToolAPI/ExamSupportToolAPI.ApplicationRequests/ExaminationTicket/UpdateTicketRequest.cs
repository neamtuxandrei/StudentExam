using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSupportToolAPI.ApplicationRequests.ExaminationTicket
{
    public class UpdateTicketRequest
    {
        public string Question1 { get; set; } = string.Empty;
        public string Question2 { get; set; } = string.Empty;
        public string Question3 { get; set;} = string.Empty;
        public string Answer1 { get; set; } = string.Empty;
        public string Answer2 { get; set;} = string.Empty;
        public string Answer3 { get; set;} = string.Empty;
    }
}
