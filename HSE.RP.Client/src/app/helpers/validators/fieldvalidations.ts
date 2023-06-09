export class FieldValidations {
  static IsNotNullOrWhitespace: (value: string | undefined) => boolean = (value) => (value?.trim().length ?? 0) > 0;
  static IsGreaterThanZero: (value: number | undefined) => boolean = (value) => value !== undefined && value > 0;
  static IsLessThanOrEqualTo100: (value: number | undefined) => boolean = (value) => value !== undefined && value <= 100;
  static IsAPositiveNumber: (value: number | undefined) => boolean = (value) => value !== undefined && value > -1;
  static IsWholeNumber: (value: number | undefined) => boolean = (value) => value !== undefined && (value % 1 === 0);
  static IsLessThanOrEqualToMaxChar: (value: string | undefined, maxLength: number) => boolean = (value, maxLength) => value !== undefined && value.length <= maxLength;
}
