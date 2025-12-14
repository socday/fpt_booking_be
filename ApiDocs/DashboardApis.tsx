import React from 'react';
import EndpointTable, { ApiEndpoint } from './components/EndpointTable';

const endpoints: ApiEndpoint[] = [
  {
    method: 'GET',
    path: '/api/dashboard/stats',
    description: 'Aggregate booking, facility, and notification statistics for dashboard tiles.',
    auth: 'Roles: Admin, Manager'
  }
];

const DashboardApis: React.FC = () => (
  <EndpointTable title="DashboardController" endpoints={endpoints} />
);

export default DashboardApis;
