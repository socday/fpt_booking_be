import React from 'react';
import AuthApis from './AuthApis';
import BookingsApis from './BookingsApis';
import CampusesApis from './CampusesApis';
import DashboardApis from './DashboardApis';
import FacilitiesApis from './FacilitiesApis';
import FacilityAssetApis from './FacilityAssetApis';
import FacilityTypesApis from './FacilityTypesApis';
import NotificationsApis from './NotificationsApis';
import ReportsApis from './ReportsApis';
import SecurityTaskApis from './SecurityTaskApis';
import SlotsApis from './SlotsApis';
import UsersApis from './UsersApis';

const ApiDocs: React.FC = () => (
  <main style={{ padding: 32, fontFamily: '"Space Grotesk", "Segoe UI", system-ui, sans-serif', backgroundColor: '#f1f5f9' }}>
    <header style={{ marginBottom: 32 }}>
      <p style={{ color: '#64748b', letterSpacing: 1 }}>FPT Facility Booking</p>
      <h1 style={{ fontSize: 36, fontWeight: 700, color: '#0f172a', margin: '8px 0' }}>REST API Catalogue</h1>
      <p style={{ color: '#475569', maxWidth: 640 }}>
        Each section below mirrors a backend controller and lists the routes currently available in the ASP.NET Core project.
        Plug these lightweight components into any React admin surface to keep the documentation close to the code.
      </p>
    </header>
    <AuthApis />
    <BookingsApis />
    <CampusesApis />
    <DashboardApis />
    <FacilitiesApis />
    <FacilityAssetApis />
    <FacilityTypesApis />
    <NotificationsApis />
    <ReportsApis />
    <SecurityTaskApis />
    <SlotsApis />
    <UsersApis />
  </main>
);

export default ApiDocs;
