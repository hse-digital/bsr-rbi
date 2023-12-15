export class NationalInsuranceNumberValidator {

  private static _isNationalInsuranceNumberValid(nationalInsuranceNumber: string): boolean {
    return new RegExp(/^[A-Z][A-Z][0-9]{6}[A-Z]$/, "gmi").test(nationalInsuranceNumber.replace(/\s/g, ''));
  }


  static isValid(value: string): boolean {
    return this._isNationalInsuranceNumberValid(value);
  }
}
