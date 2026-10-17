import { expect, test } from "@playwright/test";

const registrationUrl =
  "/events/technology-innovation-conference/register";

async function gotoInteractivePage(
  page: import("@playwright/test").Page,
  path: string,
) {
  await page.goto(path);
  await page.waitForLoadState("networkidle");
}

test("registration validates required name and email fields", async ({
  page,
}) => {
  await gotoInteractivePage(page, registrationUrl);
  await page.getByRole("button", { name: "Complete registration" }).click();

  await expect(
    page.getByText("Please enter your full name.").first(),
  ).toBeVisible();
  await expect(
    page.getByText("Please enter your email address.").first(),
  ).toBeVisible();
  await expect(
    page.getByText("Please confirm your registration details.").first(),
  ).toBeVisible();

  await page.getByLabel("Full name").fill("Jordan Lee");
  await page.getByLabel("Email address").fill("not-an-email");
  await page
    .getByLabel("I confirm that my registration details are correct.")
    .check();
  await page.getByRole("button", { name: "Complete registration" }).click();

  await expect(page.getByText("Enter a valid email address.").first()).toBeVisible();
  await expect(
    page.getByRole("heading", { name: "Registration received" }),
  ).toHaveCount(0);
});

test("optional phone accepts blank and rejects invalid phone input", async ({
  page,
}) => {
  await gotoInteractivePage(page, registrationUrl);
  await page.getByLabel("Full name").fill("Jordan Lee");
  await page.getByLabel("Email address").fill("jordan@example.com");
  await page.getByLabel("Phone number (optional)").fill("not-a-phone");
  await page
    .getByLabel("I confirm that my registration details are correct.")
    .check();
  await page.getByRole("button", { name: "Complete registration" }).click();

  await expect(
    page.getByText("Enter a phone number with 7 to 15 digits.").first(),
  ).toBeVisible();
  await expect(
    page.getByRole("heading", { name: "Registration received" }),
  ).toHaveCount(0);

  await page.getByLabel("Phone number (optional)").fill("");
  await page.getByRole("button", { name: "Complete registration" }).click();
  await expect(
    page.getByRole("heading", { name: "Registration received" }),
  ).toBeVisible();
});

test("registration state survives client-side navigation and check-in", async ({
  page,
}) => {
  await gotoInteractivePage(page, registrationUrl);
  await page.getByLabel("Full name").fill("Jordan Lee");
  await page.getByLabel("Email address").fill("jordan@example.com");
  await page
    .getByLabel("I confirm that my registration details are correct.")
    .check();
  await page.getByRole("button", { name: "Complete registration" }).click();

  await expect(page.getByText("Current session: 1 event registration(s).")).toBeVisible();
  await page.getByRole("link", { name: "Attendance" }).click();
  await expect(page).toHaveURL(/\/attendance$/);
  await expect(
    page.getByRole("heading", {
      name: "Technology & Innovation Conference",
      exact: true,
    }),
  ).toBeVisible();
  await expect(page.getByText("0 of 1 event(s) checked in")).toBeVisible();

  await page.getByRole("button", { name: "Mark attended" }).click();
  await expect(page.getByText("1 of 1 event(s) checked in")).toBeVisible();
  await expect(page.getByText("Attended")).toBeVisible();
});

test("health endpoint responds and events page stays responsive", async ({
  page,
  request,
}) => {
  const healthResponse = await request.get("/healthz");
  expect(healthResponse.status()).toBe(200);

  const pageErrors: string[] = [];
  page.on("pageerror", (error) => pageErrors.push(error.message));

  const startedAt = Date.now();
  const response = await page.goto("/events");
  await page.waitForLoadState("networkidle");
  expect(response?.status()).toBe(200);
  await expect(page.getByRole("heading", { name: "Events", exact: true })).toBeVisible();
  await expect(page.getByRole("link", { name: "View Details" })).toHaveCount(4);
  expect(Date.now() - startedAt).toBeLessThan(5_000);

  await page.setViewportSize({ width: 375, height: 812 });
  const hasHorizontalOverflow = await page.evaluate(
    () => document.documentElement.scrollWidth > document.documentElement.clientWidth,
  );
  expect(hasHorizontalOverflow).toBe(false);

  await page.getByRole("link", { name: "View Details" }).first().click();
  await expect(
    page.getByRole("heading", {
      name: "Technology & Innovation Conference",
      exact: true,
    }),
  ).toBeVisible();
  expect(pageErrors).toEqual([]);
});
