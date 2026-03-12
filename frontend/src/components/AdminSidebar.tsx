import { LayoutDashboard, BookOpen, Users, CalendarCheck2, PlusCircle } from "lucide-react";
import { NavLink } from "react-router";
import { cn } from "@/lib/utils";

const items = [
    { to: "/admin", label: "Overview", icon: LayoutDashboard },
    { to: "/admin/subjects", label: "Subjects", icon: BookOpen },
    { to: "/admin/subjects/create", label: "Create Subject", icon: PlusCircle  },
    { to: "/admin/mentors/create", label: "Create Mentor", icon: Users },
    { to: "/admin/bookings", label: "Manage Bookings", icon: CalendarCheck2 },
];

export function AdminSidebar() {
    return (
        <aside className="w-full md:w-64 shrink-0 border-r bg-card">
            <div className="p-4 border-b">
                <h2 className="text-lg font-semibold">Admin Panel</h2>
                <p className="text-sm text-muted-foreground">SkillMentor operations</p>
            </div>
            <nav className="p-3 space-y-1">
                {items.map((item) => {
                    const Icon = item.icon;
                    return (
                        <NavLink
                            key={item.to}
                            to={item.to}
                             end={item.to === "/admin" || item.to === "/admin/subjects" || item.to === "/admin/mentors" || item.to === "/admin/bookings"}
                            className={({ isActive }) =>
                                cn(
                                    "flex items-center gap-3 rounded-lg px-3 py-2 text-sm transition-colors hover:bg-muted",
                                    isActive && "bg-primary text-primary-foreground hover:bg-primary/90",
                                )
                            }
                        >
                            <Icon className="h-4 w-4" />
                            <span>{item.label}</span>
                        </NavLink>
                    );
                })}
            </nav>
        </aside>
    );
}

