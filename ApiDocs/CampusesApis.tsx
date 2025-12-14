import React from 'react';
import EndpointTable, { ApiEndpoint } from './components/EndpointTable';

const endpoints: ApiEndpoint[] = [
  {
    method: 'GET',
    path: '/api/campuses',
    description: 'Retrieve every campus with soft-deleted records filtered out.',
    auth: 'Any authenticated user'
  },
  {
    method: 'GET',
    path: '/api/campuses/{id}',
    description: 'Fetch a single campus by identifier.',
    auth: 'Any authenticated user'
  },
  {
    method: 'POST',
    path: '/api/campuses',
    description: 'Create a new campus record.',
    auth: 'Roles: Admin',
    body: 'CampusDto'
  },
  {
    method: 'PUT',
    path: '/api/campuses/{id}',
    description: 'Update campus details.',
    auth: 'Roles: Admin',
    body: 'CampusDto'
  },
  {
    method: 'DELETE',
    path: '/api/campuses/{id}',
    description: 'Soft-delete (hide) a campus.',
    auth: 'Roles: Admin'
  }
];

const CampusesApis: React.FC = () => (
  <EndpointTable title="CampusesController" endpoints={endpoints} />
);

export default CampusesApis;
