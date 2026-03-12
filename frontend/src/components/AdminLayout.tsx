import { Navigate, Outlet } from "react-router";
import { useUser } from "@clerk/clerk-react";
import { AdminSidebar } from "@/components/AdminSidebar";

export default function AdminLayout() {
    const { isLoaded, user } = useUser();

    if (!isLoaded) {
        return (
            <div className="container py-10">
                <div className="text-center text-lg">Loading admin area...</div>
            </div>
        );
    }
    const isAdmin =
    Array.isArray(user?.publicMetadata?.roles) &&
    user.publicMetadata.roles.includes("ADMIN");

  if (!isAdmin) {
    return <Navigate to="/dashboard" replace />;
  }

    return (
        <div className="container py-8">
            <div className="flex flex-col md:flex-row rounded-2xl border overflow-hidden bg-background min-h-[calc(100vh-12rem)]">
                <AdminSidebar />
                <div className="flex-1 p-6 md:p-8 bg-muted/20">
                    <Outlet />
                </div>
            </div>
        </div>
    );
}