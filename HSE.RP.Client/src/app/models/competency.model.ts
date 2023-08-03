import { CompetencyDateOfAssessment } from './competency-date-of-assessment.model';
import { NoCompetencyAssessment } from './no-competency-assessment.model';
import { CompetencyAssessmentCertificateNumber} from './competency-assessment-certificate-number.model';
import { CompetencyIndependentAssessmentStatus } from './competency-independent-assessment-status.model';
import { CompetencyAssessmentOrganisation } from './competency-assessment-organisation.model';

export class Competency {
  CompetencyIndependentAssessmentStatus?: CompetencyIndependentAssessmentStatus = new CompetencyIndependentAssessmentStatus();
  CompetencyAssessmentOrganisation?: CompetencyAssessmentOrganisation = new CompetencyAssessmentOrganisation();
  CompetencyDateOfAssessment?: CompetencyDateOfAssessment = new CompetencyDateOfAssessment();
  NoCompetencyAssessment?: NoCompetencyAssessment = { Declaration: false };
  CompetencyAssessmentCertificateNumber?: CompetencyAssessmentCertificateNumber = new CompetencyAssessmentCertificateNumber();
}
