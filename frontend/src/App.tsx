import { BrowserRouter, Routes, Route, Navigate } from "react-router";
import Layout from "@/components/Layout";
import HomePage from "@/pages/HomePage";
import LoginPage from "@/pages/LoginPage";
import DashboardPage from "@/pages/DashboardPage";
import PaymentPage from "@/pages/PaymentPage";
import { SignedIn, SignedOut } from "@clerk/clerk-react";
import AdminLayout from "./components/AdminLayout";
import ManageBookingsPage from "./pages/admin/ManageBookingsPage";
import CreateMentorPage from "./pages/admin/CreateMentorPage";
import CreateSubjectPage from "./pages/admin/CreateSubjectPage";
import AdminOverviewPage from "./pages/admin/AdminOverviewPage";
import AdminSubjectsPage from "./pages/admin/AdminSubjectsPage";
import SubjectsPage from "./pages/SubjectsPage";
import SubjectDetailPage from "./pages/SubjectDetailPage";

function App() {
  return (
    <BrowserRouter>
      <Layout>
        <Routes>
          
          <Route path="/" element={<HomePage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route
            path="/dashboard"
            element={
              <>
                <SignedIn>
                  <DashboardPage />
                </SignedIn>
                <SignedOut>
                  <LoginPage />
                </SignedOut>
              </>
            }
          />
          <Route
            path="/payment/:sessionId"
            element={
              <>
                <SignedIn>
                  <PaymentPage />
                </SignedIn>
                <SignedOut>
                  <LoginPage />
                </SignedOut>
              </>
            }
          />
          

          <Route path="/subjects" element={<SubjectsPage />} />
          <Route path="/subjects/:id" element={<SubjectDetailPage />} />


          <Route
            path="/admin"
            element={
              <>
                <SignedIn>
                  <AdminLayout />
                </SignedIn>
                <SignedOut>
                  <Navigate to="/login" replace />
                </SignedOut>
              </>
            }
          >

          
            <Route index element={<AdminOverviewPage />} />
            <Route path="subjects/create" element={<CreateSubjectPage />} />
            <Route path="mentors/create" element={<CreateMentorPage />} />
            <Route path="bookings" element={<ManageBookingsPage />} />
            <Route path="subjects" element={<AdminSubjectsPage />} />
          </Route>

          <Route path="*" element={<LoginPage />} />
        </Routes>

      </Layout>
    </BrowserRouter>
  );
}

export default App;
