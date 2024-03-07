export interface ExaminationTicket{
    id: string;
    ticketNo: number;
    isActive?: boolean;
    question1: string;
    question2: string;
    question3: string;
    answer1?: string;
    answer2?: string;  
    answer3?: string;
}