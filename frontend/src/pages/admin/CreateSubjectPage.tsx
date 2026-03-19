import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useAuth } from "@clerk/clerk-react";
import { createSubject, getMentors } from "@/lib/api";
import type { Mentor } from "@/types";
import { useToast } from "@/components/hooks/use-toast";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";

import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select";

import {
    Form,
    FormControl,
    FormDescription,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/form";

const subjectSchema = z.object({
    subjectName: z.string().min(2, "Subject name is required"),
    description: z.string().min(10, "Description must be at least 10 characters"),
    courseImageUrl: z.string().url("Enter a valid image URL"),
    mentorId: z.string().min(1, "Please select a mentor"),
});

type SubjectFormValues = z.infer<typeof subjectSchema>;

export default function CreateSubjectPage() {
    const { getToken } = useAuth();
    const { toast } = useToast();
    const navigate = useNavigate();
    const [mentors, setMentors] = useState<Mentor[]>([]);
    const [loadingMentors, setLoadingMentors] = useState(true);
    const [submitting, setSubmitting] = useState(false);

    const form = useForm<SubjectFormValues>({
        resolver: zodResolver(subjectSchema),
        defaultValues: {
            subjectName: "",
            description: "",
            courseImageUrl: "",
            mentorId: "",
        },
    });

    useEffect(() => {
        async function loadMentors() {
            try {
                const token = await getToken({ template: "skill-mentor" });
                const data = await getMentors(token ?? undefined);
                setMentors(data.content);
            } catch (error) {
                toast({
                    title: "Failed to load mentors",
                    description: "Please refresh and try again.",
                    variant: "destructive",
                });
            } finally {
                setLoadingMentors(false);
            }
        }

        loadMentors();
    }, [getToken, toast]);

    

    const onSubmit = async (values: SubjectFormValues) => {
        setSubmitting(true);
        try {
            const token = await getToken({ template: "skill-mentor" });
            if (!token) throw new Error("Not authenticated");

            const newSubject = await createSubject(token, {
                subjectName: values.subjectName,
                description: values.description,
                courseImageUrl: values.courseImageUrl,
                mentorId: Number(values.mentorId),
            });

            toast({
                title: "Subject created",
                description: `${newSubject.subjectName} has been added successfully.`,
            });

            navigate("/admin/subjects");
        } catch (error) {
            toast({
                title: "Could not create subject",
                description:
                    error instanceof Error ? error.message : "Something went wrong.",
                variant: "destructive",
            });
        } finally {
            setSubmitting(false);
        }
    };

    return (
    <Card className="max-w-3xl">
      <CardHeader>
        <CardTitle>Create Subject</CardTitle>
      </CardHeader>
      <CardContent>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
            <FormField
              control={form.control}
              name="subjectName"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Subject Name</FormLabel>
                  <FormControl>
                    <Input placeholder="AWS Developer Associate" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
             <FormField
              control={form.control}
              name="description"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Description</FormLabel>
                  <FormControl>
                    <Textarea
                      placeholder="Describe what this subject covers..."
                      className="min-h-30"
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="courseImageUrl"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Course Image URL</FormLabel>
                  <FormControl>
                    <Input placeholder="https://example.com/course-image.jpg" {...field} />
                  </FormControl>
                  <FormDescription>
                    Use a hosted image URL for the course card.
                  </FormDescription>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="mentorId"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Assign Mentor</FormLabel>
                  <Select onValueChange={field.onChange} defaultValue={field.value}>
                    <FormControl>
                      <SelectTrigger>
                        <SelectValue placeholder={loadingMentors ? "Loading mentors..." : "Select a mentor"} />
                      </SelectTrigger>
                    </FormControl>
                    <SelectContent>
                      {mentors.map((mentor) => (
                        <SelectItem key={mentor.id} value={String(mentor.id)}>
                          {mentor.firstName} {mentor.lastName}
                        </SelectItem>
                      ))}
                    </SelectContent>
                    </Select>
                  <FormMessage />
                </FormItem>
              )}
            />

             <div className="flex justify-end gap-3">
              <Button type="button" variant="outline" onClick={() => navigate("/admin/subjects")}>
                Cancel
              </Button>
              <Button type="submit" disabled={submitting}>
                {submitting ? "Creating..." : "Create Subject"}
              </Button>
            </div>
          </form>
        </Form>
      </CardContent>
    </Card>
  );
}
                

