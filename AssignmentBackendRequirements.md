# Assignment API Requirements

Purpose
- Backend API to support frontend assignment features: student assignment view, dynamic countdown, submissions, and teacher create/grade flows.
- JSON over HTTPS, RESTful endpoints. Use JWT Bearer auth with role-based authorization.

API Endpoints

- GET /courses
  - Returns array of courses used by the Assignments index.

- GET /courses/:courseId/assignments
  - Returns assignments for a given course (may be empty).

- GET /courses/:courseId/assignments/:assignmentId
  - Returns assignment detail including submissions array (teacher may see full list; student may get only their submission depending on auth rules).

- POST /courses/:courseId/assignments
  - Create assignment (teacher/admin only).
  - Body: { title, instructions (HTML allowed), openingDate (ISO), deadline (ISO) }

- POST /assignments/:assignmentId/submissions
  - Student submits assignment links.
  - Body: { studentId, links: string[] }

- GET /assignments/:assignmentId/submissions
  - Teacher-only: returns list of submissions with status and grade.

- GET /assignments/:assignmentId/submissions/me
  - Student endpoint to fetch their own submission (or use query param/studentId with auth).

- POST /assignments/:assignmentId/submissions/:submissionId/grade
  - Teacher-only grading endpoint. Body: { grade: number, feedback?: string }

Data Models (JSON shapes)

- Course
```
{ "id":"string", "name":"string", "description":"string?" }
```

- Assignment
```
{
  "id":"string",
  "courseId":"string",
  "courseName":"string?",
  "title":"string",
  "instructions":"string (HTML)",
  "openingDate":"ISO8601 string",
  "deadline":"ISO8601 string",
  "submissions":[Submission]? 
}
```

- Submission
```
{
  "id":"string",
  "assignmentId":"string",
  "studentId":"string",
  "links":["https://github.com/...'"],
  "status":"Submitted|Pending|Graded|NoSubmission",
  "grade":number|null,
  "createdAt":"ISO",
  "updatedAt":"ISO"
}
```

Auth & Authorization
- Scheme: `Authorization: Bearer <JWT>` for protected endpoints.
- Roles: `student`, `teacher`, `admin`.
  - `POST /courses/:courseId/assignments` and grading endpoints require `teacher` or `admin`.
  - Submission endpoints require authenticated `student`.
- Public read: `GET /courses` and `GET /courses/:courseId/assignments` can be public or protected; if requested from the browser, ensure CORS is enabled.

Validation rules
- `title`: non-empty, max length (recommend 300 chars).
- `instructions`: sanitize HTML server-side.
- `openingDate`/`deadline`: valid ISO strings, `openingDate < deadline`.
- `links`: each must be a valid URL; reject empty arrays.
- Return `400` for validation errors with JSON: { "error": "message", "details": {...} }.

Responses & Status Codes
- 200: OK (GET), 201: Created (POST), 204: No Content (DELETE)
- 401: Unauthenticated, 403: Forbidden, 400: Validation, 404: Not found, 500: Server error

CORS & Client Considerations
- If frontend calls from browser, enable CORS for the frontend origin (`NEXT_PUBLIC_API_URL` must be reachable by browser).
- Allow headers: `Authorization, Content-Type` and support `OPTIONS` preflight.
- Use HTTPS in production.

Performance & Paging
- For lists implement optional pagination with `?page=&limit=` if datasets can grow.

Error format
```
{ "error": "Short message", "details": { "field": "reason" } }
```

Database schema (suggested)
- users (id, name, email, role, createdAt, updatedAt)
- courses (id, name, description, createdAt)
- assignments (id, courseId, title, instructions, openingDate, deadline, createdBy, createdAt, updatedAt)
- submissions (id, assignmentId, studentId, links JSON, status, grade, feedback, createdAt, updatedAt)

Security & hygiene
- Sanitize `instructions` HTML when rendering.
- Validate and normalize URLs in `links`.
- Rate-limit submission endpoints.
- Audit create/grade actions.

Env & Deployment
- Required env vars: `PORT`, `DATABASE_URL`, `JWT_SECRET`, `CORS_ALLOWED_ORIGINS`.
- Provide the frontend with `NEXT_PUBLIC_API_URL` pointing to the deployed API.

Testing & Deliverables
- Provide OpenAPI/Swagger or Postman collection for all endpoints.
- Provide example credentials or test JWTs for a `teacher` and `student`.
- Confirm CORS settings and provide the API base URL.

Sample Requests / Responses (examples omitted here for brevity — include in API docs)

---

Please tell me if you want this added as an OpenAPI skeleton (YAML/JSON) or a Postman collection; I can generate that next and place it in the repo (`C:\projects\Educationservices`).