import { CompetencyDateOfAssessment } from './competency-date-of-assessment.model';
import { NoCompetencyAssessment } from './no-competency-assessment.model';
import { CompetenceyAssessmentCertificateNumber } from './competency-assessment-certificate-number.model';
import { IndependentAssessmentStatus } from './independent-assessment-status.model';
import { CompetencyAssesesmentOrganisation } from './competency-assesesment-organisation.model';

export class Competency {
  IndependentAssessmentStatus?: IndependentAssessmentStatus =
    new IndependentAssessmentStatus();
  CompetencyAssesesmentOrganisation?: CompetencyAssesesmentOrganisation =
    new CompetencyAssesesmentOrganisation();
  CompetencyDateOfAssessment?: CompetencyDateOfAssessment = new CompetencyDateOfAssessment();
  NoCompetencyAssessment?: NoCompetencyAssessment = { Declaration: false };
  CompetencyAssessmentCertificateNumber?: CompetenceyAssessmentCertificateNumber =
    new CompetenceyAssessmentCertificateNumber();
}
