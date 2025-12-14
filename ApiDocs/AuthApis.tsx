import React from 'react';
import EndpointTable, { ApiEndpoint } from './components/EndpointTable';

const endpoints: ApiEndpoint[] = [
  {
    method: 'POST',
    path: '/api/auth/login',
    description: 'Authenticate a user with email/password and issue a JWT.',
    body: 'LoginRequest { email, password }'
  }
];

const AuthApis: React.FC = () => (
  <EndpointTable title="AuthController" endpoints={endpoints} />
);

export default AuthApis;
