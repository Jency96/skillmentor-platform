// Modified to match with backend SubjectResponseDTO
export interface Subject {
  id: number;
  subjectName: string;
  description: string;
  courseImageUrl: string;
}

// Lightweight subject shape used inside mentor responses / previews
export interface MentorSubject {
  id: number;
  subjectName: string;
  description?: string;
  courseImageUrl?: string;
}

// Modified to match with backend MentorResponseDTO (from GET /api/v1/mentors)
export interface Mentor {
  id: number;
  mentorId: string;
  firstName: string;
  lastName: string;
  email?: string;
  phoneNumber?: string;
  title?: string;
  profession?: string;
  company?: string;
  experienceYears?: number;
  bio?: string;
  profileImageUrl?: string;
  positiveReviews: number;
  totalEnrollments: number;
  isCertified: boolean;
  startYear?: number;
  subjects: MentorSubject[];
}

// Modified to support student + admin session views
export interface Enrollment {
  id: number;
  studentName?: string;
  mentorName: string;
  mentorProfileImageUrl?: string;
  subjectName: string;
  sessionAt: string;
  durationMinutes?: number;
  sessionStatus?: "pending" | "confirmed" | "completed" | "cancelled";
  paymentStatus:
    | "pending"
    | "accepted"
    | "confirmed"
    | "completed"
    | "cancelled";
  meetingLink?: string | null;
}

export interface User {
  id: string;
  name: string;
  email: string;
}

export interface PaginatedResponse<T> {
  content: T[];
  totalElements: number;
  totalPages: number;
  size?: number;
  number?: number;
}