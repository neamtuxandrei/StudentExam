import { StudentPresentationStatus } from "../student-presentation-status";
import { ExaminationTicketForCommittee } from "./examination-ticket-for-committee";
import { StudentForCommittee } from "./student-for-committee";


export interface StudentPresentationForCommittee {
    id: string;
    isAbsent: boolean;
    startingTime?: Date;
    status: StudentPresentationStatus;
    student: StudentForCommittee | null;
    examinationTicket: ExaminationTicketForCommittee | null;
    studentPresentationDuration: number;
    currentCommitteeId: string;
}
