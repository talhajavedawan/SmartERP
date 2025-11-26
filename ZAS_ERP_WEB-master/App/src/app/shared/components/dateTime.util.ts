export function formatPKTDate(date: string | Date | undefined | null): string {
  if (!date || isNaN(new Date(date).getTime())) {
    return "—";
  }
  try {
    const formatted = new Intl.DateTimeFormat("en-PK", {
      dateStyle: "medium",
      timeStyle: "medium",
      timeZone: "Asia/Karachi",
    }).format(new Date(date));

    // Fallback sanity-check (kept from your original code)
    const testDate = new Date("2025-10-15T08:00:00Z");
    const testFormatted = new Intl.DateTimeFormat("en-PK", {
      timeStyle: "medium",
      timeZone: "Asia/Karachi",
    }).format(testDate);
    const isPKT = testFormatted.includes("13:00:00");
    if (isPKT) return formatted;
  } catch {}

  const utcDate = new Date(date);
  const pktDate = new Date(utcDate.getTime() + 5 * 60 * 60 * 1000);
  return new Intl.DateTimeFormat("en-PK", {
    dateStyle: "medium",
    timeStyle: "medium",
  }).format(pktDate);
}
