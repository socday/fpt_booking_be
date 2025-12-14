import React from 'react';
import EndpointTable, { ApiEndpoint } from './components/EndpointTable';

const endpoints: ApiEndpoint[] = [
  {
    method: 'GET',
    path: '/api/slots',
    description: 'Return every available time slot used for booking requests.',
    auth: 'Any authenticated user'
  }
];

const SlotsApis: React.FC = () => (
  <EndpointTable title="SlotsController" endpoints={endpoints} />
);

export default SlotsApis;
