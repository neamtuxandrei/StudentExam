export interface PresentationSchedule {
    id: string
    startDate: Date;
    endDate: Date;
    breakStart: Date;
    breakDuration: number;
    studentPresentationDuration: number;
    presentationScheduleEntries: PresentationScheduleEntry[]
}

export interface PresentationScheduleEntry {
    id: string
    date: string
    student?: PresentationScheduleStudent
}

export interface PresentationScheduleStudent {
    id: string
    name: string
    diplomaProjectName: string
    coordinatorName: string
}