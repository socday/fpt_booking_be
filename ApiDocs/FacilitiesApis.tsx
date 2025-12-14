import React from 'react';
import EndpointTable, { ApiEndpoint } from './components/EndpointTable';

const endpoints: ApiEndpoint[] = [
  {
    method: 'GET',
    path: '/api/facilities',
    description: 'Return facilities filtered by optional name, campus, or type.',
    auth: 'Any authenticated user',
    query: 'name?, campusId?, typeId?'
  },
  {
    method: 'POST',
    path: '/api/facilities',
    description: 'Create a new facility.',
    auth: 'Any authenticated user with creation rights',
    body: 'FacilityCreateRequest'
  },
  {
    method: 'PUT',
    path: '/api/facilities/{id}',
    description: 'Update facility metadata and capacity.',
    auth: 'Any authenticated user with edit rights',
    body: 'FacilityUpdateRequest'
  },
  {
    method: 'DELETE',
    path: '/api/facilities/{id}',
    description: 'Soft-delete or deactivate a facility.',
    auth: 'Any authenticated user with delete rights'
  }
];

const FacilitiesApis: React.FC = () => (
  <EndpointTable title="FacilitiesController" endpoints={endpoints} />
);

export default FacilitiesApis;
