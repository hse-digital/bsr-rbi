export class FieldValidations {
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
