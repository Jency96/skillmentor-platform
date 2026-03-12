import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

export default function AdminOverviewPage() {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Admin Dashboard</h1>
        <p className="text-muted-foreground mt-1">
          Manage mentors, subjects, and bookings from one place.
        </p>
      </div>
      <div className="grid gap-4 md:grid-cols-3">
        <Card>
          <CardHeader>
            <CardTitle>Subjects</CardTitle>
          </CardHeader>
          <CardContent className="text-sm text-muted-foreground">
            Create and assign learning subjects to mentors.
          </CardContent>
        </Card>
        <Card>
          <CardHeader>
            <CardTitle>Mentors</CardTitle>
          </CardHeader>
          <CardContent className="text-sm text-muted-foreground">
            Add mentor profiles and maintain tutor records.
          </CardContent>
        </Card>
        <Card>
          <CardHeader>
            <CardTitle>Bookings</CardTitle>
          </CardHeader>
          <CardContent className="text-sm text-muted-foreground">
            Confirm payments, add meeting links, and close sessions.
          </CardContent>
        </Card>
      </div>
    </div>
  );
}