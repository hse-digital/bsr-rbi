import { Component } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { environment } from '../../../../../environments/environment';
import { PageComponent } from '../../../../helpers/page.component';
import { FieldValidations } from '../../../../helpers/validators/fieldvalidations';
import { ApplicationService, ApplicationStatus, BuildingProfessionalModel } from '../../../../services/application.service';
import { ApplicantAddressComponent } from '../applicant-address/applicant-address.component';
import { takeLast } from 'rxjs';
import { ApplicationTaskListComponent } from '../../task-list/task-list.component';
import { NavigationService } from 'src/app/services/navigation.service';
import { PersonalDetailRoutes, PersonalDetailRouter } from '../PersonalDetailRoutes'
import { AddressModel } from '../../../../services/address.service';

@Component({
  selector: 'hse-applicant-summary',
  templateUrl: './applicant-summary.component.html',
})
export class ApplicantSummaryComponent extends PageComponent<string> {
  PersonalDetailRoutes = PersonalDetailRoutes;

  public static route: string = PersonalDetailRoutes.SUMMARY;
  static title: string = "Personal details - Register as a building inspector - GOV.UK";
  production: boolean = environment.production;
  modelValid: boolean = false;
  photoHasErrors = false;
  private personalDetailRouter: PersonalDetailRouter;
  override model?: string;

  constructor(
    activatedRoute: ActivatedRoute,
    applicationService: ApplicationService,
    personalDetailRouter: PersonalDetailRouter) {
    super(activatedRoute);
    this.personalDetailRouter = personalDetailRouter;
    this.updateOnSave = true;
  //  this.SetupTestModel();
  }
  /// <summary>
  /// Sets up a test model for the applicant summary page. Just used during Development
  /// </summary>
  private SetupTestModel() {
    this.applicationService.model = new BuildingProfessionalModel();
    if (this.applicationService.model.PersonalDetails === undefined) {
      this.applicationService.model.PersonalDetails = {};
    }
    this.applicationService.model.ApplicationStatus = ApplicationStatus.PersonalDetailsComplete;
    this.applicationService.model.id = "1234567890";
    this.applicationService.model.PersonalDetails.ApplicantDateOfBirth = {};
    this.applicationService.model.PersonalDetails.ApplicantDateOfBirth.Year = "1967";
    this.applicationService.model.PersonalDetails.ApplicantDateOfBirth.Month = "01";
    this.applicationService.model.PersonalDetails.ApplicantDateOfBirth.Day = "17";
    this.applicationService.model.PersonalDetails.ApplicantAddress = new AddressModel();
    this.applicationService.model.PersonalDetails.ApplicantAddress.Address = "37 The Old Trunk Road";
    this.applicationService.model.PersonalDetails.ApplicantAddress.AddressLineTwo = "Newtown Avenue";
    this.applicationService.model.PersonalDetails.ApplicantAddress.Town = "Newbridge";
    this.applicationService.model.PersonalDetails.ApplicantAddress.AdministrativeArea = "Co. Kildare";
    this.applicationService.model.PersonalDetails.ApplicantAddress.Postcode = "W12 AB97";
    this.applicationService.model.PersonalDetails.ApplicantAlternativeEmail = "N@EMAIL.COM";
    this.applicationService.model.PersonalDetails.ApplicantAlternativePhone = "+44123456789";
    this.applicationService.model.PersonalDetails.ApplicantEmail = "PADDY@CODEC.IE";
    this.applicationService.model.PersonalDetails.ApplicantName = {};
    this.applicationService.model.PersonalDetails.ApplicantName.FirstName = "Paddy";
    this.applicationService.model.PersonalDetails.ApplicantName.LastName = "Wagon";
    this.applicationService.model.PersonalDetails.ApplicantNationalInsuranceNumber = "AB 12 34 56 C";
    this.applicationService.model.PersonalDetails.ApplicantPhone = "+353123456789";

  }

  override onInit(applicationService: ApplicationService): void {
    //this.model = applicationService.model.personalDetails?.applicantPhoto?.toString() ?? '';
  }

  override async onSave(applicationService: ApplicationService): Promise<void> {
    this.applicationService.model.ApplicationStatus = ApplicationStatus.PersonalDetailsComplete;

   }

  override canAccess(applicationService: ApplicationService, routeSnapshot: ActivatedRouteSnapshot): boolean {
    return true;
    //return (FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.firstName) || FieldValidations.IsNotNullOrWhitespace(applicationService.model?.personalDetails?.applicatantName?.lastName));

  }


  override isValid(): boolean {
    return true;
/*     this.phoneNumberHasErrors = !PhoneNumberValidator.isValid(this.model?.toString() ?? '');
    return !this.phoneNumberHasErrors; */

  }

  override navigateNext(): Promise<boolean> {
    return this.personalDetailRouter.navigateTo(this.applicationService.model, PersonalDetailRoutes.TASK_LIST);
    //return this.navigationService.navigateRelative(`../${ApplicationTaskListComponent.route}`, this.activatedRoute);
  }

  public navigateTo(route: string) {
    return this.navigationService.navigateRelative(`${route}`, this.activatedRoute);
  }

  public GetFormattedDateofBirth(): string {
    let year: number = 0;
    let month: number = 0;
    let day: number = 0;
    if (this.applicationService.model.PersonalDetails?.ApplicantDateOfBirth?.Year !== undefined) {
      if (FieldValidations.IsNotNullOrWhitespace(this.applicationService.model.PersonalDetails?.ApplicantDateOfBirth?.Year)) {
        year = parseInt(this.applicationService.model.PersonalDetails?.ApplicantDateOfBirth?.Year);
      }
    }
    if (this.applicationService.model.PersonalDetails?.ApplicantDateOfBirth?.Month !== undefined) {
      if (FieldValidations.IsNotNullOrWhitespace(this.applicationService.model.PersonalDetails?.ApplicantDateOfBirth?.Month)) {
        month = parseInt(this.applicationService.model.PersonalDetails?.ApplicantDateOfBirth?.Month);
      }
    }
    if (this.applicationService.model.PersonalDetails?.ApplicantDateOfBirth?.Day !== undefined) {
      if (FieldValidations.IsNotNullOrWhitespace(this.applicationService.model.PersonalDetails?.ApplicantDateOfBirth?.Day)) {
        day = parseInt(this.applicationService.model.PersonalDetails?.ApplicantDateOfBirth?.Day);
      }
    }

    if (year === 0 || month === 0 || day === 0)
    {
      return "";
    }

    // Don't know why but the month is always one less than it should be.  So we need to subtract 1 from the month
    let date: Date = new Date(year,month-1,day);
    let options: Intl.DateTimeFormatOptions = {
      year: "numeric",
      month: "long",
      day: "numeric"
    };

    let formattedDate = new Intl.DateTimeFormat("en-GB", options).format(date);
    return formattedDate;
  }


}
