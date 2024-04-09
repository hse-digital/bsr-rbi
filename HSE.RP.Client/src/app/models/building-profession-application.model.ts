export class BuildingProfessionalModel {

    public id?: string;
    public buildingProfessionType?: string;
    public validFrom?: Date;
    public validTo?: Date;
    public decisionCondition?: string;
    public decisionConditionReason?: string;
    public countries?: string[];
    public creationDate?: Date;
    public applicant?: Applicant;
    public employer?: Employer;
    public classes?: string[];
    public activities?: Activity[];
}

export class Activity {
    public activityName?: string;
    public categories?: Category[];
}

export class Applicant {
    public applicantName?: string;
}

export class Category {
    public categoryName?: string;
    public categoryDescription?: string;
}

export class Employer {
    public employerName?: string;
    public employerAddress?: string;
    public employmentType?: string;
}
