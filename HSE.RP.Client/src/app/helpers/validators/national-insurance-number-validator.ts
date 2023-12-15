export class NationalInsuranceNumberValidator {

  private static _isNationalInsuranceNumberValid(nationalInsuranceNumber: string): boolean {
    return new RegExp(/^(?!BG)(?!GB)(?!NK)(?!KN)(?!TN)(?!NT)(?!ZZ)[A-Z]*[^DFIQUV][A-Z]*[^DFIOQUV][0-9]{6}[A-D]$/, "gmi").test(nationalInsuranceNumber.replace(/\s/g, ''));
  }


  static isValid(value: string): boolean {
    return this._isNationalInsuranceNumberValid(value);
  }
}
