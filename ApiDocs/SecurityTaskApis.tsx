import React from 'react';
import EndpointTable, { ApiEndpoint } from './components/EndpointTable';

const endpoints: ApiEndpoint[] = [
  {
    method: 'POST',
    path: '/api/securitytask/assign',
    description: 'Assign a task to a security staff member.',
    auth: 'Roles: Admin, Manager, FacilityAdmin',
    body: 'CreateTaskRequest'
  },
  {
    method: 'GET',
    path: '/api/securitytask/pending',
    description: 'List all pending security tasks.',
    auth: 'Roles: Security, Admin, Manager'
  },
  {
    method: 'PUT',
    path: '/api/securitytask/complete/{taskId}',
    description: 'Mark a task as complete with an optional report note.',
    auth: 'Roles: Security',
    body: 'CompleteTaskRequest { reportNote }'
  }
];

const SecurityTaskApis: React.FC = () => (
  <EndpointTable title="SecurityTaskController" endpoints={endpoints} />
);

export default SecurityTaskApis;
