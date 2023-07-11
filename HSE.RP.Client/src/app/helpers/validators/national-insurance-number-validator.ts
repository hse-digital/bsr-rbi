export class NationalInsuranceNumberValidator {

  private static _isNationalInsuranceNumberValid(nationalInsuranceNumber: string): boolean {
    return new RegExp(/^[A-Za-z]{2}\s?\d{2}s?\d{2}s?\d{2}\s?[A-Za-z]$/, "gm").test(nationalInsuranceNumber);
  }


  static isValid(value: string): boolean {
    return this._isNationalInsuranceNumberValid(value);
  }
}
