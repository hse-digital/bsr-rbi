import { EmploymentType } from "../models/employment-type.enum"

export class ProfessionalActivityHelper {

  static professionalBodyNames: Record<string,string> =
  {
      "RICS": "Royal Institution of Chartered Surveyors (RICS)",
      "CABE": "Chartered Association of Building Engineers (CABE)",
      "CIOB": "Chartered Institute of Building (CIOB)",
      "OTHER": "Other"
  }

  static employmentTypeNames: Record<EmploymentType,string> =
  {
      0: "None",
      1: "Public sector building control body",
      2: "Private sector building control body",
      3: "Other",
      4: "Unemployed"
  }

}
