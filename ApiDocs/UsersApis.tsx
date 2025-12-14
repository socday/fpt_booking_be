import React from 'react';
import EndpointTable, { ApiEndpoint } from './components/EndpointTable';

const endpoints: ApiEndpoint[] = [
  {
    method: 'GET',
    path: '/api/users',
    description: 'Filter users with optional search criteria.',
    auth: 'Roles: Admin',
    query: 'UserFilterRequest query params'
  },
  {
    method: 'POST',
    path: '/api/users',
    description: 'Create a new user with a default password and role.',
    auth: 'Roles: Admin',
    body: 'CreateUserRequest'
  },
  {
    method: 'PUT',
    path: '/api/users/{id}',
    description: 'Update user profile information.',
    auth: 'Roles: Admin',
    body: 'UpdateUserRequest'
  },
  {
    method: 'PUT',
    path: '/api/users/{id}/role',
    description: 'Change the role assigned to a user.',
    auth: 'Roles: Admin',
    body: 'UpdateRoleRequest { newRoleId }'
  },
  {
    method: 'DELETE',
    path: '/api/users/{id}',
    description: 'Delete a user account (fails if constrained by related data).',
    auth: 'Roles: Admin'
  }
];

const UsersApis: React.FC = () => (
  <EndpointTable title="UsersController" endpoints={endpoints} />
);

export default UsersApis;
