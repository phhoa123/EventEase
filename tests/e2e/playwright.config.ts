import { defineConfig, devices } from "@playwright/test";

export default defineConfig({
  testDir: ".",
  fullyParallel: true,
  forbidOnly: Boolean(process.env.CI),
  retries: process.env.CI ? 2 : 0,
  reporter: "list",
  use: {
    baseURL: "http://127.0.0.1:5188",
    trace: "retain-on-failure",
  },
  projects: [
    {
      name: "chromium",
      use: { ...devices["Desktop Chrome"] },
    },
  ],
  webServer: {
    command:
      "dotnet run --project ../../EventEase.csproj --urls http://127.0.0.1:5188",
    url: "http://127.0.0.1:5188/healthz",
    reuseExistingServer: !process.env.CI,
    timeout: 120_000,
  },
});
