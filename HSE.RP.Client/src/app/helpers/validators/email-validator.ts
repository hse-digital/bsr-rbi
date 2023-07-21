export class EmailValidator {
  private static _isEmailFormatValid(email: string): boolean {
    var EMAIL_REGEXP = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9-]+[.][A-Za-z.]{2,}$/;

    return new RegExp(EMAIL_REGEXP).test(email);
  }

  static isValid(value: string): boolean {
    return this._isEmailFormatValid(value);
  }
}
