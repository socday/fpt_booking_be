import React from 'react';
import EndpointTable, { ApiEndpoint } from './components/EndpointTable';

const endpoints: ApiEndpoint[] = [
  {
    method: 'GET',
    path: '/api/facility-types',
    description: 'Return all available facility type definitions.',
    auth: 'Any authenticated user'
  },
  {
    method: 'POST',
    path: '/api/facility-types',
    description: 'Create a facility type.',
    auth: 'Any authenticated user',
    body: 'FacilityTypeDto { typeName }'
  },
  {
    method: 'DELETE',
    path: '/api/facility-types/{id}',
    description: 'Delete a facility type.',
    auth: 'Any authenticated user'
  }
];

const FacilityTypesApis: React.FC = () => (
  <EndpointTable title="FacilityTypesController" endpoints={endpoints} />
);

export default FacilityTypesApis;
