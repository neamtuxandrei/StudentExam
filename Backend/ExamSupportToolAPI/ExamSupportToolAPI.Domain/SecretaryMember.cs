namespace ExamSupportToolAPI.Domain
{
    public class SecretaryMember : User
    {
        private List<ExaminationSession> _examinationSessions = new List<ExaminationSession>();
        public IReadOnlyCollection<ExaminationSession> ExaminationSessions => _examinationSessions;
        private SecretaryMember() { }

        public static SecretaryMember Create(string name, string email)
        {
            SecretaryMember secretary = new SecretaryMember();
            secretary.Id = Guid.NewGuid();
            secretary.SetName(name);
            secretary.SetEmail(email);

            return secretary;
        }
        public void AddExaminationSession(ExaminationSession examinationSession)
        {
            _examinationSessions.Add(examinationSession);
        }
        
        
        public PresentationSchedule GeneratePresentationSchedule(Guid examinationSessionId, DateTime? startDate, DateTime? endDate, DateTime? breakStart, int? breakDuration, int? studentPresentationDuration)
        {
            var examinationSession = ExaminationSessions
                                    .Where(es => es.Id == examinationSessionId)
                                    .First();
            var students = examinationSession.Students.OrderBy(s => s.Name).ToList();

            //set default values if params are null
            startDate ??= examinationSession.StartDate.Date.AddHours(9); // 09:00 AM
            endDate ??= examinationSession.EndDate.Date.AddHours(14);    // 02:00 PM
            breakStart ??= examinationSession.StartDate.Date.AddHours(11);
            breakDuration ??= 40;
            studentPresentationDuration ??= 20;
            var hasBreak = false;


            var presentationSchedule = PresentationSchedule.Create(startDate.Value, endDate.Value, breakStart.Value, breakDuration.Value, studentPresentationDuration.Value);
            var currentDate = startDate;

            for (int i = 0; i < students.Count; ++i)
            {
                if (currentDate.Value.Hour >= breakStart.Value.Hour && currentDate.Value.Minute >= breakStart.Value.Minute && !hasBreak)
                {
                    var breakEntry = PresentationScheduleEntry.Create(currentDate.Value);
                    breakEntry.SetStudent(null);
                    presentationSchedule.AddPresentationScheduleEntry(breakEntry);
                    currentDate = currentDate.Value.AddMinutes(breakDuration.Value);
                    hasBreak = true;
                }

                var entry = PresentationScheduleEntry.Create(currentDate.Value);
                entry.SetStudent(students[i]);
                students[i].SetCurrentPresentationScheduleId(presentationSchedule.Id);
                presentationSchedule.AddPresentationScheduleEntry(entry);

                if (currentDate.Value.Hour > 0 && currentDate.Value.Hour % endDate.Value.Hour == 0)
                {
                    currentDate = currentDate.Value.AddDays(1);
                    currentDate = currentDate.Value.Date.AddHours(startDate.Value.Hour);
                    hasBreak = false ;
                }
                else
                {
                    currentDate = currentDate.Value.AddMinutes(studentPresentationDuration.Value);
                }
            }

            return presentationSchedule;
        }
    }
}
