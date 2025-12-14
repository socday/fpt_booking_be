import React from 'react';
import EndpointTable, { ApiEndpoint } from './components/EndpointTable';

const endpoints: ApiEndpoint[] = [
  {
    method: 'GET',
    path: '/api/facilityasset/facility/{facilityId}',
    description: 'List all monitored assets for a specific facility.',
    auth: 'Open (no attribute-level authorization declared)'
  },
  {
    method: 'PUT',
    path: '/api/facilityasset/update-condition',
    description: 'Update the condition/quantity of an asset inside a room.',
    auth: 'Open (no attribute-level authorization declared)',
    body: 'UpdateConditionRequest { id, condition, quantity }'
  }
];

const FacilityAssetApis: React.FC = () => (
  <EndpointTable title="FacilityAssetController" endpoints={endpoints} />
);

export default FacilityAssetApis;
