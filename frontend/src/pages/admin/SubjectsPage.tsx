import { useEffect, useState } from "react";
import { useAuth } from "@clerk/clerk-react";
import { getSubjects } from "@/lib/api";
import type { Subject } from "@/types";
import { useToast } from "@/components/hooks/use-toast";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

export default function SubjectsPage() {
  const { getToken } = useAuth();
  const { toast } = useToast();
  const [subjects, setSubjects] = useState<Subject[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function loadSubjects() {
      try {
        const token = await getToken({ template: "skill-mentor" });
        if (!token) throw new Error("Not authenticated");

        const data = await getSubjects(token);
        setSubjects(data);
      } catch (error) {
        toast({
          title: "Failed to load subjects",
          description:
            error instanceof Error ? error.message : "Something went wrong.",
          variant: "destructive",
        });
      } finally {
        setLoading(false);
      }
    }

    loadSubjects();
  }, [getToken, toast]);

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">Subjects</h1>
        <p className="text-muted-foreground mt-1">
          View all subjects available in the platform.
        </p>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Subject List</CardTitle>
        </CardHeader>
        <CardContent>
          {loading ? (
            <div className="py-8 text-center text-muted-foreground">
              Loading subjects...
            </div>
          ) : subjects.length === 0 ? (
            <div className="py-8 text-center text-muted-foreground">
              No subjects found.
            </div>
          ) : (
            <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
              {subjects.map((subject) => (
                <div
                  key={subject.id}
                  className="overflow-hidden rounded-xl border bg-background"
                >
                  {subject.courseImageUrl ? (
                    <img
                      src={subject.courseImageUrl}
                      alt={subject.subjectName}
                      className="h-40 w-full object-cover"
                    />
                  ) : (
                    <div className="flex h-40 items-center justify-center bg-muted text-sm text-muted-foreground">
                      No image
                    </div>
                  )}

                  <div className="space-y-2 p-4">
                    <h2 className="text-lg font-semibold">{subject.subjectName}</h2>
                    <p className="text-sm text-muted-foreground line-clamp-3">
                      {subject.description}
                    </p>
                  </div>
                </div>
              ))}
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}