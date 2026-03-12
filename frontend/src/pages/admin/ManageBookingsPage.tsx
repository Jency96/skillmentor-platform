import { useEffect, useMemo, useState } from "react";
import { useAuth } from "@clerk/clerk-react";
import {
  addMeetingLink,
  confirmBookingPayment,
  getAllBookings,
  markBookingComplete,
} from "@/lib/api";

import type { Enrollment } from "@/types";
import { useToast } from "@/components/hooks/use-toast";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { StatusPill } from "@/components/StatusPill";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";

const PAGE_SIZE = 10;

type SortKey = "sessionAt" | "studentName" | "mentorName" | "subjectName";

export default function ManageBookingsPage() {
  const { getToken } = useAuth();
  const { toast } = useToast();
  const [bookings, setBookings] = useState<Enrollment[]>([]);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(0);
  const [totalPages, setTotalPages] = useState(1);
  const [search, setSearch] = useState("");
  const [status, setStatus] = useState("all");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [sortBy, setSortBy] = useState<SortKey>("sessionAt");
  const [meetingDialogOpen, setMeetingDialogOpen] = useState(false);
  const [selectedBookingId, setSelectedBookingId] = useState<number | null>(
    null,
  );
  const [meetingLink, setMeetingLink] = useState("");
  const [busyId, setBusyId] = useState<number | null>(null);

  const sort = useMemo(() => `${sortBy},desc`, [sortBy]);

  const loadBookings = async () => {
    setLoading(true);
    try {
      const token = await getToken({ template: "skill-mentor" });
      if (!token) throw new Error("Not authenticated");

      const data = await getAllBookings(token, {
        page,
        size: PAGE_SIZE,
        search,
        status,
        startDate: startDate || undefined,
        endDate: endDate || undefined,
        sort,
      });

      setBookings(data.content);
      setTotalPages(Math.max(data.totalPages || 1, 1));
    } catch (error) {
      toast({
        title: "Failed to load bookings",
        description:
          error instanceof Error ? error.message : "Please try again.",
        variant: "destructive",
      });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadBookings();
  }, [page, status, sort]);

  const handleSearch = async () => {
    setLoading(true);

    try {
      const token = await getToken({ template: "skill-mentor" });
      if (!token) throw new Error("Not authenticated");

      const data = await getAllBookings(token, {
        page: 0,
        size: PAGE_SIZE,
        search,
        status,
        startDate: startDate || undefined,
        endDate: endDate || undefined,
        sort,
      });

      setBookings(data.content);
      setTotalPages(Math.max(data.totalPages || 1, 1));
      setPage(0);
    } catch (error) {
      toast({
        title: "Failed to load bookings",
        description:
          error instanceof Error ? error.message : "Please try again.",
        variant: "destructive",
      });
    } finally {
      setLoading(false);
    }
  };

  const handleConfirmPayment = async (sessionId: number) => {
    setBusyId(sessionId);
    try {
      const token = await getToken({ template: "skill-mentor" });
      if (!token) throw new Error("Not authenticated");

      await confirmBookingPayment(token, sessionId);

      toast({
        title: "Payment confirmed",
        description: "Booking payment updated successfully.",
      });

      await loadBookings();
    } catch (error) {
      toast({
        title: "Action failed",
        description:
          error instanceof Error ? error.message : "Please try again.",
        variant: "destructive",
      });
    } finally {
      setBusyId(null);
    }
  };

  const handleMarkComplete = async (sessionId: number) => {
    setBusyId(sessionId);
    try {
      const token = await getToken({ template: "skill-mentor" });
      if (!token) throw new Error("Not authenticated");

      await markBookingComplete(token, sessionId);

      toast({
        title: "Session completed",
        description: "Booking marked as completed.",
      });

      await loadBookings();
    } catch (error) {
      toast({
        title: "Action failed",
        description:
          error instanceof Error ? error.message : "Please try again.",
        variant: "destructive",
      });
    } finally {
      setBusyId(null);
    }
  };

  const openMeetingDialog = (sessionId: number) => {
    setSelectedBookingId(sessionId);
    setMeetingLink("");
    setMeetingDialogOpen(true);
  };

  const handleSaveMeetingLink = async () => {

    const normalizedMeetingLink = meetingLink.trim();

    if (!selectedBookingId || !meetingLink.trim()) {
      toast({
        title: "Meeting link required",
        description: "Please enter a valid meeting link.",
        variant: "destructive",
      });
      return;
    }

    // NEW validation
    if (!/^https?:\/\/.+/.test(normalizedMeetingLink)) {
      toast({
        title: "Invalid meeting link",
        description: "Please enter a valid URL (e.g. https://meet.google.com/...)",
        variant: "destructive",
      });
      return;
    }

    setBusyId(selectedBookingId);

    try {
      const token = await getToken({ template: "skill-mentor" });
      if (!token) throw new Error("Not authenticated");

      await addMeetingLink(token, selectedBookingId, meetingLink);

      toast({
        title: "Meeting link added",
        description: "The meeting link was saved successfully.",
      });

      setMeetingDialogOpen(false);
      setSelectedBookingId(null);
      setMeetingLink("");

      await loadBookings();
    } catch (error) {
      toast({
        title: "Could not save meeting link",
        description:
          error instanceof Error ? error.message : "Please try again.",
        variant: "destructive",
      });
    } finally {
      setBusyId(null);
    }
  };

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Manage Bookings</h1>
        <p className="text-muted-foreground mt-1">
          Review sessions, confirm payments, add meeting links, and mark
          sessions as completed.
        </p>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Filters</CardTitle>
        </CardHeader>
        <CardContent className="grid gap-4 md:grid-cols-2 xl:grid-cols-5">
          <div className="space-y-2 xl:col-span-2">
            <Label htmlFor="search">Search</Label>
            <Input
              id="search"
              placeholder="Search by student or mentor"
              value={search}
              onChange={(e) => setSearch(e.target.value)}
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="status">Status</Label>
            <select
              id="status"
              value={status}
              onChange={(e) => setStatus(e.target.value)}
              className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            >
              <option value="all">All</option>
              <option value="pending">Pending</option>
              <option value="confirmed">Confirmed</option>
              <option value="completed">Completed</option>
              <option value="cancelled">Cancelled</option>
            </select>
          </div>

          <div className="space-y-2">
            <Label htmlFor="startDate">Start Date</Label>
            <Input
              id="startDate"
              type="date"
              value={startDate}
              onChange={(e) => setStartDate(e.target.value)}
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="endDate">End Date</Label>
            <Input
              id="endDate"
              type="date"
              value={endDate}
              onChange={(e) => setEndDate(e.target.value)}
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="sortBy">Sort By</Label>
            <select
              id="sortBy"
              value={sortBy}
              onChange={(e) => setSortBy(e.target.value as SortKey)}
              className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
            >
              <option value="sessionAt">Session Date</option>
              <option value="studentName">Student Name</option>
              <option value="mentorName">Mentor Name</option>
              <option value="subjectName">Subject Name</option>
            </select>
          </div>

          <div className="xl:col-span-5 flex justify-end">
            <Button onClick={handleSearch}>Apply Filters</Button>
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>All Sessions</CardTitle>
        </CardHeader>
        <CardContent className="overflow-x-auto">
          {loading ? (
            <div className="py-10 text-center text-muted-foreground">
              Loading bookings...
            </div>
          ) : bookings.length === 0 ? (
            <div className="py-10 text-center text-muted-foreground">
              No bookings found.
            </div>
          ) : (
            <table className="w-full min-w-275 text-sm">
              <thead>
                <tr className="border-b text-left">
                  <th className="py-3 pr-4">Session ID</th>
                  <th className="py-3 pr-4">Student</th>
                  <th className="py-3 pr-4">Mentor</th>
                  <th className="py-3 pr-4">Subject</th>
                  <th className="py-3 pr-4">Date / Time</th>
                  <th className="py-3 pr-4">Duration</th>
                  <th className="py-3 pr-4">Payment</th>
                  <th className="py-3 pr-4">Session</th>
                  <th className="py-3 pr-4">Actions</th>
                </tr>
              </thead>
              <tbody>
                {bookings.map((booking) => (
                  <tr key={booking.id} className="border-b align-top">
                    <td className="py-4 pr-4 font-medium">#{booking.id}</td>
                    <td className="py-4 pr-4">
                      {booking.studentName ?? "—"}
                    </td>
                    <td className="py-4 pr-4">{booking.mentorName}</td>
                    <td className="py-4 pr-4">{booking.subjectName}</td>
                    <td className="py-4 pr-4">
                      {new Date(booking.sessionAt).toLocaleString()}
                    </td>
                    <td className="py-4 pr-4">
                      {booking.durationMinutes ?? 60} min
                    </td>
                    <td className="py-4 pr-4">
                      <StatusPill
                        status={
                          booking.paymentStatus === "confirmed"
                            ? "accepted"
                            : booking.paymentStatus === "accepted"
                              ? "accepted"
                              : booking.paymentStatus === "completed"
                                ? "completed"
                                : booking.paymentStatus === "cancelled"
                                  ? "cancelled"
                                  : "pending"
                        }
                      />
                    </td>
                    <td className="py-4 pr-4">
                      <span className="inline-flex rounded-full bg-muted px-3 py-1 text-xs font-medium capitalize">
                        {booking.sessionStatus ?? "pending"}
                      </span>
                    </td>
                    <td className="py-4 pr-4">
                      <div className="flex flex-wrap gap-2">
                        <Button
                          size="sm"
                          variant="outline"
                          disabled={busyId === booking.id || booking.paymentStatus !== "pending"}
                          onClick={() => handleConfirmPayment(booking.id)}
                        >
                          Confirm Payment
                        </Button>

                        <Button
                          size="sm"
                          variant="outline"
                          disabled={busyId === booking.id || booking.sessionStatus !== "confirmed"}
                          onClick={() => handleMarkComplete(booking.id)}
                        >
                          Mark Complete
                        </Button>

                        <Button
                          size="sm"
                          onClick={() => openMeetingDialog(booking.id)}
                          disabled={busyId === booking.id}
                        >
                          Add Meeting Link
                        </Button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}

          <div className="mt-6 flex items-center justify-between">
            <p className="text-sm text-muted-foreground">
              Page {page + 1} of {totalPages}
            </p>

            <div className="flex gap-2">
              <Button
                variant="outline"
                disabled={page === 0}
                onClick={() => setPage((p) => p - 1)}
              >
                Previous
              </Button>

              <Button
                variant="outline"
                disabled={page + 1 >= totalPages}
                onClick={() => setPage((p) => p + 1)}
              >
                Next
              </Button>
            </div>
          </div>
        </CardContent>
      </Card>

      <Dialog open={meetingDialogOpen} onOpenChange={setMeetingDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Add Meeting Link</DialogTitle>
            <DialogDescription>
              Save the meeting URL for this mentorship session.
            </DialogDescription>
          </DialogHeader>

          <div className="space-y-2">
            <Label htmlFor="meetingLink">Meeting URL</Label>
            <Input
              id="meetingLink"
              placeholder="https://meet.google.com/..."
              value={meetingLink}
              onChange={(e) => setMeetingLink(e.target.value)}
            />
          </div>

          <DialogFooter>
            <Button
              variant="outline"
              onClick={() => setMeetingDialogOpen(false)}
            >
              Cancel
            </Button>
            <Button onClick={handleSaveMeetingLink}>Save Link</Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
}