namespace ExamSupportToolAPI.Domain
{
    public class ExaminationTicket: BaseEntity
    {
        public Guid ExaminationSessionId { get; set; }
        public int TicketNo { get; set; }
        public bool IsActive { get; set; }
        public string Question1 { get; set; } = string.Empty;
        public string Question2 { get; set; } = string.Empty;
        public string Question3 { get; set; } = string.Empty;
        public string Answer1 { get; set; } = string.Empty;
        public string Answer2 { get; set; } = string.Empty;
        public string Answer3 { get; set; } = string.Empty;
        

        private ExaminationTicket() { }

        public static ExaminationTicket Create(int ticketNo,bool isActive, string question1, string question2, string question3,
            string answer1, string answer2, string answer3)
        {
            ExaminationTicket ticket = new ExaminationTicket();
            ticket.TicketNo = ticketNo;
            ticket.IsActive = isActive;
            ticket.Question1 = question1;
            ticket.Question2 = question2;
            ticket.Question3 = question3;
            ticket.Answer1 = answer1;
            ticket.Answer2 = answer2;
            ticket.Answer3 = answer3;
            ticket.Id = Guid.NewGuid();
            return ticket;
        }

        public void SetActiveStatus(bool isActive)
        {
            IsActive = isActive;
        }

        public void AssignTicketToSession(Guid sessionId)
        {
            ExaminationSessionId = sessionId;
        }
       
    }
}
