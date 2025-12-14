import React from 'react';
import EndpointTable, { ApiEndpoint } from './components/EndpointTable';

const endpoints: ApiEndpoint[] = [
  {
    method: 'POST',
    path: '/api/reports',
    description: 'Submit an incident/issue report tied to the current user.',
    auth: 'Any authenticated user',
    body: 'ReportCreateRequest'
  },
  {
    method: 'GET',
    path: '/api/reports',
    description: 'Return reports filtered by caller role (students see theirs, admins see all).',
    auth: 'Any authenticated user'
  },
  {
    method: 'PUT',
    path: '/api/reports/{id}/status',
    description: 'Approve, reject, or otherwise update report status.',
    auth: 'Roles: Admin, Manager',
    body: 'ReportStatusUpdate { status }'
  }
];

const ReportsApis: React.FC = () => (
  <EndpointTable title="ReportsController" endpoints={endpoints} />
);

export default ReportsApis;
