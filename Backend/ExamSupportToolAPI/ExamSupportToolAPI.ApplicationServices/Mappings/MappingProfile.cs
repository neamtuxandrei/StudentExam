using AutoMapper;
using ExamSupportToolAPI.DataObjects;
using ExamSupportToolAPI.Domain;

namespace ExamSupportToolAPI.ApplicationServices.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // for committee
            CreateMap<CommitteeMember, CommitteeMemberData>();
            CreateMap<Student, StudentInPresentationScheduleForCommittee>();
            CreateMap<PresentationSchedule, PresentationScheduleForCommittee>();
            CreateMap<PresentationScheduleEntry, PresentationScheduleEntryForCommittee>();
            CreateMap<ExaminationSession, ExaminationSessionListForCommittee>()
                .ForMember(dest => dest.NumberOfStudents, opt => opt.MapFrom(source => source.Students.Count))
                .ForMember(dest => dest.NumberOfCommitteeMembers, opt => opt.MapFrom(source => source.CommitteeMembers.Count));
            CreateMap<StudentPresentation, StudentPresentationForCommittee>();
            CreateMap<ExaminationTicket, ExaminationTicketForCommittee>();
            CreateMap<Student, StudentForCommittee>();

            //for secretary
            CreateMap<Student, StudentForSecretary>();
            CreateMap<Student, StudentForSecretaryDropdown>();
            CreateMap<Student, StudentInPresentationScheduleForSecretary>();
            CreateMap<ExaminationSession, ExaminationSessionForSecretary>();
            CreateMap<ExaminationSession, ExaminationSessionPresentationForSecretary>();
            CreateMap<StudentPresentation, StudentPresentationForSecretary>();
            CreateMap<PresentationSchedule, PresentationScheduleForSecretary>();
            CreateMap<PresentationScheduleEntry, PresentationScheduleEntryForSecretary>();
            CreateMap<ExaminationTicket, ExaminationTicketForSecretary>();
            CreateMap<ExaminationSession, ExaminationSessionListForSecretary>()
                .ForMember(dest => dest.NumberOfStudents, opt => opt.MapFrom(source => source.Students.Count))
                .ForMember(dest => dest.NumberOfCommitteeMembers, opt => opt.MapFrom(source => source.CommitteeMembers.Count))
                .ForMember(dest => dest.NumberOfTickets, opt => opt.MapFrom(source => source.ExaminationTickets.Count));
            CreateMap<SecretaryMember, SecretaryMemberData>();


            // for student
            CreateMap<ExaminationSession, ExaminationSessionForStudent>()
                .ForMember(dest => dest.StudentPresentation, opt => opt.MapFrom((source, dest, destMember, context) =>
                {
                    var studentId = context.Items["studentId"] as Guid?; // Retrieve the studentId value from the context.

                    return source.StudentPresentations.FirstOrDefault(pse => pse.StudentId == studentId.Value);
                }));


            //CreateMap<PresentationScheduleForSecretary, PresentationSchedule>();
            //.ForMember(s => s.Id, o => o.Ignore())
            //.ConstructUsing(dto => PresentationSchedule.Create(dto.StartDate, dto.EndDate, dto.BreakStart, dto.BreakDuration, dto.StudentPresentationDuration, dto.PresentationScheduleEntries))

            CreateMap<PresentationSchedule, PresentationScheduleForStudent>();
            CreateMap<PresentationScheduleEntry, PresentationScheduleEntryForStudent>();
            CreateMap<StudentPresentation, StudentPresentationForSecretary>();
            CreateMap<StudentPresentation, StudentPresentationForStudent>();
            CreateMap<ExaminationTicket, ExaminationTicketForStudent>();
        }
    }
}
