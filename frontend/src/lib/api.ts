import type { Enrollment, Mentor, Subject } from "@/types";
import type { PaginatedResponse } from "../types";

const API_BASE_URL =
  import.meta.env.VITE_API_BASE_URL || "http://localhost:8081";

async function fetchWithAuth(
  endpoint: string,
  token: string,
  options: RequestInit = {},
): Promise<Response> {
  const res = await fetch(`${API_BASE_URL}${endpoint}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
      ...options.headers,
    },
  });

  if (!res.ok) {
    const error = await res.json().catch(() => ({ message: "Request failed" }));
    throw new Error(error.message || `HTTP ${res.status}`);
  }

  return res;
}

// Public route without auth
export async function getPublicMentors(
  page = 0,
  size = 10,
): Promise<{ content: Mentor[]; totalElements: number; totalPages: number }> {
  const res = await fetch(
    `${API_BASE_URL}/api/v1/mentors?page=${page}&size=${size}`,
  );
  if (!res.ok) throw new Error("Failed to fetch mentors");
  return res.json();
}

// Enrollments
export async function enrollInSession(
  token: string,
  data: {
    mentorId: number;
    subjectId: number;
    sessionAt: string;
    durationMinutes?: number;
  },
): Promise<Enrollment> {
  const res = await fetchWithAuth("/api/v1/sessions/enroll", token, {
    method: "POST",
    body: JSON.stringify(data),
  });
  return res.json();
}

export async function getMyEnrollments(token: string): Promise<Enrollment[]> {
  const res = await fetchWithAuth("/api/v1/sessions/my-sessions", token);
  return res.json();
}

// ---------- Admin APIs ----------

export async function getMentors(
  token?: string,
  page = 0,
  size = 100,
): Promise<PaginatedResponse<Mentor>> {
  const endpoint = `/api/v1/mentors?page=${page}&size=${size}`;

  if (!token) {
    const res = await fetch(`${API_BASE_URL}${endpoint}`);
    if (!res.ok) throw new Error("Failed to fetch mentors");
    return res.json();
  }

  const res = await fetchWithAuth(endpoint, token);
  return res.json();
}

export async function getSubjects(token: string): Promise<Subject[]> {
  const res = await fetchWithAuth("/api/v1/subjects", token);
  return res.json();
}

export async function createSubject(
  token: string,
  data: {
    subjectName: string;
    description: string;
    courseImageUrl: string;
    mentorId: number;
  },
): Promise<Subject> {
  const res = await fetchWithAuth("/api/v1/subjects", token, {
    method: "POST",
    body: JSON.stringify(data),
  });
  return res.json();
}

export async function createMentor(
  token: string,
  data: {
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber?: string;
    title?: string;
    profession?: string;
    company?: string;
    experienceYears?: number;
    bio?: string;
    profileImageUrl?: string;
    isCertified: boolean;
    startYear?: number;
  },

): Promise<Mentor> {
  const res = await fetchWithAuth("/api/v1/mentors", token, {
    method: "POST",
    body: JSON.stringify(data),
  });
  return res.json();
}

export async function getAllBookings(
  token: string,
  params?: {
    page?: number;
    size?: number;
    search?: string;
    status?: string;
    startDate?: string;
    endDate?: string;
    sort?: string;
  },

): Promise<PaginatedResponse<Enrollment>> {
  const query = new URLSearchParams();
  if (params?.page !== undefined) query.set("page", String(params.page));
  if (params?.size !== undefined) query.set("size", String(params.size));
  if (params?.search) query.set("search", params.search);
  if (params?.status && params.status !== "all") query.set("status", params.status);
  if (params?.startDate) query.set("startDate", params.startDate);
  if (params?.endDate) query.set("endDate", params.endDate);
  if (params?.sort) query.set("sort", params.sort);

  const res = await fetchWithAuth(`/api/v1/admin/sessions?${query.toString()}`, token);
  return res.json();
}


export async function confirmBookingPayment(
  token: string,
  sessionId: number,
): Promise<Enrollment> {
  const res = await fetchWithAuth(`/api/v1/admin/sessions/${sessionId}/confirm-payment`, token, {
    method: "PATCH",
  });
  return res.json();
}

export async function markBookingComplete(
  token: string,
  sessionId: number,
): Promise<Enrollment> {
  const res = await fetchWithAuth(`/api/v1/admin/sessions/${sessionId}/complete`, token, {
    method: "PATCH",
  });
  return res.json();
}


export async function addMeetingLink(
  token: string,
  sessionId: number,
  meetingLink: string,
): Promise<Enrollment> {
  const res = await fetchWithAuth(`/api/v1/admin/sessions/${sessionId}/meeting-link`, token, {
    method: "PATCH",
    body: JSON.stringify({ meetingLink }),
  });
  return res.json();
}


