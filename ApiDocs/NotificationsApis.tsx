import React from 'react';
import EndpointTable, { ApiEndpoint } from './components/EndpointTable';

const endpoints: ApiEndpoint[] = [
  {
    method: 'GET',
    path: '/api/notifications',
    description: 'Return notifications belonging to the authenticated user.',
    auth: 'Any authenticated user'
  },
  {
    method: 'POST',
    path: '/api/notifications/create',
    description: 'Create a system notification (used internally for fan-out).',
    auth: 'AllowAnonymous',
    body: 'CreateNotiRequest { userId, title, message }'
  }
];

const NotificationsApis: React.FC = () => (
  <EndpointTable title="NotificationsController" endpoints={endpoints} />
);

export default NotificationsApis;
