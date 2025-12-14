import React from 'react';

export interface ApiEndpoint {
  method: string;
  path: string;
  description: string;
  auth?: string;
  body?: string;
  query?: string;
  notes?: string;
}

interface EndpointTableProps {
  title: string;
  endpoints: ApiEndpoint[];
}

const methodColors: Record<string, string> = {
  GET: '#2563eb',
  POST: '#16a34a',
  PUT: '#d97706',
  DELETE: '#dc2626',
  PATCH: '#7c3aed'
};

const containerStyle: React.CSSProperties = {
  border: '1px solid #e5e7eb',
  borderRadius: 16,
  padding: 24,
  marginBottom: 32,
  background: 'linear-gradient(135deg, #f8fafc, #ffffff)'
};

const gridStyle: React.CSSProperties = {
  display: 'grid',
  gridTemplateColumns: 'repeat(auto-fit, minmax(280px, 1fr))',
  gap: 16
};

const EndpointTable: React.FC<EndpointTableProps> = ({ title, endpoints }) => (
  <section style={containerStyle}>
    <h2 style={{ fontSize: 20, fontWeight: 600, marginBottom: 16 }}>{title}</h2>
    <div style={gridStyle}>
      {endpoints.map((endpoint) => (
        <article
          key={`${endpoint.method}-${endpoint.path}-${endpoint.description}`}
          style={{
            border: '1px solid #e2e8f0',
            borderRadius: 12,
            padding: 16,
            backgroundColor: '#ffffff',
            boxShadow: '0 10px 25px rgba(15, 23, 42, 0.05)'
          }}
        >
          <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 8 }}>
            <span
              style={{
                fontSize: 12,
                fontWeight: 700,
                color: '#ffffff',
                backgroundColor: methodColors[endpoint.method] ?? '#0f172a',
                padding: '4px 10px',
                borderRadius: 999
              }}
            >
              {endpoint.method}
            </span>
            <code style={{ fontSize: 12, color: '#475569' }}>{endpoint.path}</code>
          </div>
          <p style={{ fontSize: 14, color: '#334155', marginBottom: 12 }}>{endpoint.description}</p>
          {endpoint.auth && (
            <p style={{ fontSize: 12, color: '#475569', marginBottom: 4 }}>
              <strong>Auth:</strong> {endpoint.auth}
            </p>
          )}
          {endpoint.body && (
            <p style={{ fontSize: 12, color: '#475569', marginBottom: 4 }}>
              <strong>Body:</strong> {endpoint.body}
            </p>
          )}
          {endpoint.query && (
            <p style={{ fontSize: 12, color: '#475569', marginBottom: 4 }}>
              <strong>Query:</strong> {endpoint.query}
            </p>
          )}
          {endpoint.notes && (
            <p style={{ fontSize: 12, color: '#475569' }}>
              <strong>Notes:</strong> {endpoint.notes}
            </p>
          )}
        </article>
      ))}
    </div>
  </section>
);

export default EndpointTable;
