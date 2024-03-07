import { PresentationScheduleStudent } from "./presentation-schedule-student"

export interface PresentationScheduleEntry {
    id: string
    date: string
    student: PresentationScheduleStudent
}