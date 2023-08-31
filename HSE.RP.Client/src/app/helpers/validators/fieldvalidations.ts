export class FieldValidations {


  static PostcodeValidator: (value: string | undefined) => boolean = (value) => (RegExp( /^([Gg][Ii][Rr]\s*0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))\s*[0-9][A-Za-z]{2})$/).test(value ?? ''));
  static IsNotNullOrWhitespace: (value: string | undefined) => boolean = (value) => (value?.trim().length ?? 0) > 0;
  static IsGreaterThanZero: (value: number | undefined) => boolean = (value) => value !== undefined && value > 0;
  static IsLessThanOrEqualTo100: (value: number | undefined) => boolean = (value) => value !== undefined && value <= 100;
  static IsAPositiveNumber: (value: number | undefined) => boolean = (value) => value !== undefined && value > -1;
  static IsWholeNumber: (value: number | undefined) => boolean = (value) => value !== undefined && (value % 1 === 0);
  static IsLessThanOrEqualToMaxChar: (value: string | undefined, maxLength: number) => boolean = (value, maxLength) => value !== undefined && value.length <= maxLength;
  static IsEmpty(value: any | undefined): boolean {
    if (value === undefined || value === null) {
      return true;
    }
    if (typeof value === 'string' && value.trim() === '') {
      return true;
    }
    if (Array.isArray(value) && value.length === 0) {
      return true;
    }
    if (typeof value === 'object' && Object.keys(value).length === 0) {
      return true;
    }
    return false;
  }
}
