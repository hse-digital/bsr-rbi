export class DateFormatHelper {
  static LongMonthFormat(
    yearPassed: number | string | undefined,
    monthPassed: number | string | undefined,
    dayPassed: number | string | undefined): string {
    if (yearPassed === undefined || monthPassed === undefined || dayPassed === undefined) {
      return "";
    }
    let year: number = 0, month: number = 0, day: number = 0;
    if (typeof yearPassed === 'string')
      year = parseInt(yearPassed);
    else
      year = yearPassed;
    if (typeof monthPassed === 'string')
      month = parseInt(monthPassed);
    else
      month = monthPassed;
    if (typeof dayPassed === 'string')
      day = parseInt(dayPassed);
    else
      day = dayPassed;

    if (year === 0 || month === 0 || day === 0) {
      return "";
    }

    // Don't know why but the month is always one less than it should be.  So we need to subtract 1 from the month
    let date: Date = new Date(year, month - 1, day);
    let options: Intl.DateTimeFormatOptions = {
      year: "numeric",
      month: "long",
      day: "numeric"
    };
    let formattedDate = new Intl.DateTimeFormat("en-GB", options).format(date);
    return formattedDate;
  }
}
