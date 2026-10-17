# EventEase

EventEase is a Blazor web application for browsing events, viewing event details, registering for events, and tracking demo attendance. The interface is built with Razor components and Bootstrap.

## Features

- Browse upcoming events and open event detail pages.
- Register for an event with name, email, optional phone, and confirmation validation.
- Review registrations and mark attendance on the Attendance page.
- Navigate to a friendly not-found page for unknown routes.
- Run Playwright end-to-end tests for form validation, session state, navigation, health checks, and mobile layout.

## Requirements

- .NET 10 SDK.
- Node.js LTS and npm to run the browser tests.
- Chromium for Playwright tests. Install it with the command below.

## Run the application

Clone the repository and start the Blazor application:

```bash
git clone https://github.com/phhoa123/EventEase.git
cd EventEase
dotnet restore
dotnet run --urls http://localhost:5187
```

Open <http://localhost:5187/events> in a browser. To stop the application, press `Ctrl+C` in the terminal.

### Main routes

| Route | Description |
| --- | --- |
| `/` or `/events` | Event list |
| `/events/{slug}` | Event details |
| `/events/{slug}/register` | Registration form |
| `/attendance` | Current-session attendance tracker |
| `/healthz` | Application health check |

Example event routes:

- `/events/technology-innovation-conference`
- `/events/technology-innovation-conference/register`

## Run the end-to-end tests

From the repository root:

```bash
cd tests/e2e
npm ci
npx playwright install chromium
npm run typecheck
npm test
```

Playwright starts the application on `http://127.0.0.1:5188` and stops it after the tests. Do not start another app instance on that port first.

To watch the tests in a browser:

```bash
npm run test:headed
```

The test suite checks required and malformed registration fields, optional phone validation, registration continuity while navigating to Attendance, the check-in action, the health endpoint, event-list rendering, and mobile horizontal overflow.

## Build and publish

Build the application in Release configuration:

```bash
dotnet build EventEase.csproj -c Release
```

Publish a framework-dependent deployment:

```bash
dotnet publish EventEase.csproj -c Release -o ./publish
```

The target environment must have the .NET 10 ASP.NET Core Runtime installed. Deploy the contents of `publish` to an ASP.NET Core host such as IIS or a Linux service behind a reverse proxy. Configure HTTPS at the host or proxy, and allow WebSocket connections because the application uses Blazor Interactive Server.

The health endpoint is available at `/healthz` and returns HTTP 200 when the application is running.

## Current demo limitations

- Events are held in an in-memory mock catalog.
- Registration and attendance records are scoped to the current Blazor Server circuit. They are not stored in a database and do not survive a lost/restarted session or application process.
- The application does not implement user authentication or authorization.
- “Mark attended” is a demo self check-in action, not a verified attendance process.
- Registration currently displays the attendee's email on the session's Attendance page. Do not use real personal data with this demo.

Before using EventEase with real users, add authentication and authorization, persist registrations in a database, validate check-in operations on the server, protect personal data, and use shared durable storage if deploying multiple application instances. Configure production secrets outside source control and test the deployed environment over HTTPS.

## Project structure

```text
Components/
  Layout/       Shared application layout and navigation
  Models/       Event model and mock event catalog
  Pages/        Event list, details, registration, attendance, and error pages
  Services/     Per-circuit session and attendance state
  Shared/       Reusable event and attendance components
tests/
  e2e/          Playwright browser tests
wwwroot/        Static assets, including the Bootstrap stylesheet
```

## Dependencies

The application uses the ASP.NET Core shared framework and does not declare third-party NuGet packages. Playwright and TypeScript are development dependencies isolated to `tests/e2e`; they are not included in the .NET publish output.
