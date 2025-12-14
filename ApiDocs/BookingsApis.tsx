import React from 'react';
import EndpointTable, { ApiEndpoint } from './components/EndpointTable';

const endpoints: ApiEndpoint[] = [
  {
    method: 'POST',
    path: '/api/bookings',
    description: 'Create a booking request on behalf of the authenticated user.',
    auth: 'Any authenticated user',
    body: 'BookingCreateRequest'
  },
  {
    method: 'PUT',
    path: '/api/bookings/{id}/status',
    description: 'Approve or reject a pending booking.',
    auth: 'Roles: Admin, Manager',
    body: 'BookingStatusUpdate { status: Approved|Rejected, rejectionReason? }'
  },
  {
    method: 'GET',
    path: '/api/bookings/availability',
    description: 'List booked slot IDs for a facility on a given day.',
    auth: 'Any authenticated user',
    query: 'facilityId (int), date (YYYY-MM-DD)'
  },
  {
    method: 'PUT',
    path: '/api/bookings/{id}/cancel',
    description: 'Cancel a booking belonging to the current user.',
    auth: 'Any authenticated user'
  },
  {
    method: 'GET',
    path: '/api/bookings/schedule-today',
    description: 'Return the daily security schedule for a campus.',
    auth: 'Any authenticated user (typically Security roles)',
    query: 'campusId (int)'
  },
  {
    method: 'POST',
    path: '/api/bookings/recurring',
    description: 'Create a recurring booking pattern.',
    auth: 'Any authenticated user',
    body: 'BookingRecurringRequest'
  },
  {
    method: 'PUT',
    path: '/api/bookings/recurring/{recurrenceId}/status',
    description: 'Approve or reject a recurring booking batch.',
    auth: 'Any authenticated user with proper permissions',
    body: 'BookingStatusUpdate { status }'
  },
  {
    method: 'PUT',
    path: '/api/bookings/staff-cancel/{id}',
    description: 'Allow staff to cancel a booking and notify the requester.',
    auth: 'Staff-level authenticated user',
    body: 'StaffCancelRequest { staffId, reason }'
  },
  {
    method: 'GET',
    path: '/api/bookings',
    description: 'Filter bookings (respects role and user-level visibility rules).',
    auth: 'Any authenticated user',
    query: 'BookingFilterRequest query params'
  }
];

const BookingsApis: React.FC = () => (
  <EndpointTable title="BookingsController" endpoints={endpoints} />
);

export default BookingsApis;
