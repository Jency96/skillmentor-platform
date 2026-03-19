import { useEffect, useState } from "react";
import { useParams } from "react-router";
import { useAuth } from "@clerk/clerk-react";
import { getSubjects } from "@/lib/api";
import type { Subject } from "@/types";
import { useToast } from "@/components/hooks/use-toast";
import { Card, CardContent } from "@/components/ui/card";

export default function SubjectDetailPage() {
    const { id } = useParams();
    const { getToken } = useAuth();
    const { toast } = useToast();

    const [subject, setSubject] = useState<Subject | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function loadSubject() {
            try {
                const token = await getToken({ template: "skill-mentor" });
                if (!token) throw new Error("Not authenticated");

                const subjects = await getSubjects(token);
                const found = subjects.find((item) => item.id === Number(id)) ?? null;
                setSubject(found);
            } catch (error) {
                toast({
                    title: "Failed to load subject",
                    description:
                        error instanceof Error ? error.message : "Something went wrong.",
                    variant: "destructive",
                });
            } finally {
                setLoading(false);
            }
        }

        loadSubject();
    }, [getToken, id, toast]);

    if (loading) {
        return (
            <div className="container py-10">
                <div className="text-center text-muted-foreground">Loading subject...</div>
            </div>
        );
    }

    if (!subject) {
        return (
            <div className="container py-10">
                <div className="text-center text-muted-foreground">Subject not found.</div>
            </div>
        )

    }

    return (
        <div className="container py-10">
            <Card className="overflow-hidden">
                {subject.courseImageUrl ? (
                    <img>
                        src={subject.courseImageUrl}
                        alt={subject.subjectName}
                        className="h-72 w-full object-cover"
                    </img>

                ) : (
                    <div className="flex h-72 items-center justify-center bg-muted text-muted-foreground">
                        No image
                    </div>
                )}
                
                <CardContent>
                    <h1 className="text-3xl font-bold tracking-tight mb-4">
                        {subject.subjectName}
                    </h1>
                    <p className="text-muted-foreground">{subject.description}</p>
                </CardContent>
            </Card>
        </div>
    )

}