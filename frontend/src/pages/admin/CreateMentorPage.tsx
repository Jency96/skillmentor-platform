import { useMemo, useState } from "react";
import { useAuth } from "@clerk/clerk-react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";

import { createMentor } from "@/lib/api";
import type { Mentor } from "@/types";

import { useToast } from "@/components/hooks/use-toast";
import { MentorCard } from "@/components/MentorCard";

import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Checkbox } from "@/components/ui/checkbox";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";

const currentYear = new Date().getFullYear();

const mentorSchema = z.object({
  firstName: z.string().min(1, "First name is required"),
  lastName: z.string().min(1, "Last name is required"),
  email: z.string().email("Enter a valid email address"),
  phoneNumber: z.string().optional(),
  title: z.string().optional(),
  profession: z.string().optional(),
  company: z.string().optional(),
  experienceYears: z.coerce.number().min(0, "Must be 0 or more").optional(),
  bio: z
    .string()
    .min(20, "Bio should be at least 20 characters")
    .optional()
    .or(z.literal("")),
  profileImageUrl: z
    .string()
    .url("Enter a valid image URL")
    .optional()
    .or(z.literal("")),
  isCertified: z.boolean(),
  startYear: z.coerce
    .number()
    .min(2000, "Year must be 2000 or later")
    .max(currentYear, `Year cannot be later than ${currentYear}`)
    .optional(),
});

type MentorFormValues = z.infer<typeof mentorSchema>;

export default function CreateMentorPage() {
  const { getToken } = useAuth();
  const { toast } = useToast();
  const [submitting, setSubmitting] = useState(false);
  const [createdMentor, setCreatedMentor] = useState<Mentor | null>(null);

  const form = useForm<MentorFormValues>({
    resolver: zodResolver(mentorSchema),
    defaultValues: {
      firstName: "",
      lastName: "",
      email: "",
      phoneNumber: "",
      title: "",
      profession: "",
      company: "",
      experienceYears: undefined,
      bio: "",
      profileImageUrl: "",
      isCertified: false,
      startYear: currentYear,
    },
  });

  const watchedValues = form.watch();

  const previewMentor = useMemo<Mentor>(() => {
    return {
      id: 0,
      mentorId: "preview",
      firstName: watchedValues.firstName || "First",
      lastName: watchedValues.lastName || "Last",
      email: watchedValues.email,
      phoneNumber: watchedValues.phoneNumber,
      title: watchedValues.title,
      profession: watchedValues.profession,
      company: watchedValues.company,
      experienceYears: watchedValues.experienceYears,
      bio: watchedValues.bio,
      profileImageUrl: watchedValues.profileImageUrl,
      isCertified: watchedValues.isCertified,
      startYear: watchedValues.startYear,
      positiveReviews: 98,
      totalEnrollments: 0,
      subjects: [
        {
          id: 0,
          subjectName:
            watchedValues.profession ||
            watchedValues.title ||
            "Mentorship Program",
          description: watchedValues.bio || "Mentor subject preview",
          courseImageUrl: watchedValues.profileImageUrl || "",
        },
      ],
    };
  }, [watchedValues]);

  const onSubmit = async (values: MentorFormValues) => {
    setSubmitting(true);

    try {
      const token = await getToken({ template: "skill-mentor" });
      if (!token) throw new Error("Not authenticated");

      const mentor = await createMentor(token, {
        firstName: values.firstName,
        lastName: values.lastName,
        email: values.email,
        phoneNumber: values.phoneNumber || undefined,
        title: values.title || undefined,
        profession: values.profession || undefined,
        company: values.company || undefined,
        experienceYears: values.experienceYears,
        bio: values.bio || undefined,
        profileImageUrl: values.profileImageUrl || undefined,
        isCertified: values.isCertified,
        startYear: values.startYear,
      });

      setCreatedMentor(mentor);

      form.reset({
        firstName: "",
        lastName: "",
        email: "",
        phoneNumber: "",
        title: "",
        profession: "",
        company: "",
        experienceYears: 0,
        bio: "",
        profileImageUrl: "",
        isCertified: false,
        startYear: currentYear,
      });

      toast({
        title: "Mentor created",
        description: `${mentor.firstName} ${mentor.lastName} was created successfully.`,
      });
    } catch (error) {
      toast({
        title: "Could not create mentor",
        description:
          error instanceof Error ? error.message : "Something went wrong.",
        variant: "destructive",
      });
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="grid gap-6 xl:grid-cols-[1.25fr_0.75fr]">
      <Card>
        <CardHeader>
          <CardTitle>Create Mentor</CardTitle>
        </CardHeader>

        <CardContent>
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
              <div className="grid gap-4 md:grid-cols-2">
                <FormField
                  control={form.control}
                  name="firstName"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>First Name</FormLabel>
                      <FormControl>
                        <Input placeholder="John" {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name="lastName"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Last Name</FormLabel>
                      <FormControl>
                        <Input placeholder="Doe" {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>

              <div className="grid gap-4 md:grid-cols-2">
                <FormField
                  control={form.control}
                  name="email"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Email</FormLabel>
                      <FormControl>
                        <Input
                          type="email"
                          placeholder="john@example.com"
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name="phoneNumber"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Phone Number</FormLabel>
                      <FormControl>
                        <Input placeholder="+94 77 123 4567" {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>

              <div className="grid gap-4 md:grid-cols-2">
                <FormField
                  control={form.control}
                  name="title"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Title</FormLabel>
                      <FormControl>
                        <Input
                          placeholder="Senior Software Engineer"
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name="profession"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Profession</FormLabel>
                      <FormControl>
                        <Input placeholder="Cloud Engineer" {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>

              <div className="grid gap-4 md:grid-cols-3">
                <FormField
                  control={form.control}
                  name="company"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Company</FormLabel>
                      <FormControl>
                        <Input placeholder="Amazon" {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name="experienceYears"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Experience Years</FormLabel>
                      <FormControl>
                        <Input type="number" min={0} {...field} />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name="startYear"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Start Year</FormLabel>
                      <FormControl>
                        <Input
                          type="number"
                          min={2000}
                          max={currentYear}
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>

              <FormField
                control={form.control}
                name="profileImageUrl"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Profile Image URL</FormLabel>
                    <FormControl>
                      <Input
                        placeholder="https://example.com/profile.jpg"
                        {...field}
                      />
                    </FormControl>
                    <FormDescription>
                      Use an image URL for now. You can add real upload support
                      later.
                    </FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="bio"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Bio</FormLabel>
                    <FormControl>
                      <Textarea
                        className="min-h-35"
                        placeholder="Write a short professional bio..."
                        {...field}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="isCertified"
                render={({ field }) => (
                  <FormItem className="flex flex-row items-start gap-3 rounded-lg border p-4">
                    <FormControl>
                      <Checkbox
                        checked={field.value}
                        onCheckedChange={(checked) =>
                          field.onChange(Boolean(checked))
                        }
                      />
                    </FormControl>

                    <div className="space-y-1">
                      <FormLabel>Certified Mentor</FormLabel>
                      <FormDescription>
                        Mark this mentor as certified on the platform.
                      </FormDescription>
                    </div>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <div className="flex justify-end">
                <Button type="submit" disabled={submitting}>
                  {submitting ? "Creating..." : "Create Mentor"}
                </Button>
              </div>
            </form>
          </Form>

          {createdMentor && (
            <div className="mt-6 rounded-lg border bg-muted/30 p-4 text-sm">
              <p className="font-medium">Mentor created successfully.</p>
              <p className="mt-1 text-muted-foreground">
                Profile: {createdMentor.firstName} {createdMentor.lastName}
              </p>
            </div>
          )}
        </CardContent>
      </Card>

      <div className="space-y-2.5">
        <div>
          <h2 className="text-lg font-semibold">Live Preview</h2>
          <p className="text-sm text-muted-foreground">
            This shows roughly how the mentor card looks before submission.
          </p>
        </div>

        <MentorCard mentor={previewMentor} />
      </div>
    </div>
  );
}