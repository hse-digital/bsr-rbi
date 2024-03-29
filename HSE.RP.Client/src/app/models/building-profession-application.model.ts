export class BuildingProfessionalModel {

    public Id?: string;
    public ApplicationNumber?: string;
    public BuildingProfessionType?: string;
    public ValidFrom?: Date;
    public ValidTo?: Date;
    public DecisionCondition?: string;
    public DecisionConditionReason?: string;
    public Countries?: string[];
    public CreationDate?: Date;
    public Applicant?: Applicant;
    public Employer?: Employer;
    public Classes?: string[];
    public Activities?: Activity[];
}

export class Activity {
    public ActivityName?: string;
    public Categories?: Category[];
}

export class Applicant {
    public ApplicantName?: string;
}

export class Category {
    public CategoryName?: string;
    public CategoryDescription?: string;
}

export class Employer {
    public EmployerName?: string;
    public EmployerAddress?: string;
    public EmploymentType?: string;
}
