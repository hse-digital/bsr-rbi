import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { firstValueFrom } from "rxjs";
import { BuildingProfessionalModel } from "../models/building-profession-application.model";

@Injectable({
    providedIn: 'root'
})
export class SearchService {
    constructor(private httpClient: HttpClient) { }

    async SearchRBIInspectorNames(name: string, country: string): Promise<RBIInspectorNamesResponse> {
        return await firstValueFrom(this.httpClient.get<RBIInspectorNamesResponse>(`api/SearchRBIInspectorNames?name=${name.trim()}&country=${country}`));
    }

    async SearchRBICompanyNames(company: string, country: string): Promise<RBIInspectorCompaniesResponse> {
        return await firstValueFrom(this.httpClient.get<RBIInspectorCompaniesResponse>(`api/SearchRBICompanyNames?company=${company.trim()}&country=${country}`));
    }

    async GetRegisterLastUpdated(service: string, country: string): Promise<string> {
        try{
        return await firstValueFrom(this.httpClient.get<string>(`api/GetRegisterLastUpdated?service=${service}&country=${country}`));
        } catch (error: any) {
            return 'Unavailable';
        }
    }

    async SearchRBIRegister(name: string, company: string, country: string): Promise<RBISearchResponse> {
        try {
            return await firstValueFrom(this.httpClient.get<RBISearchResponse>(`api/SearchRBIRegister?name=${name.trim()}&company=${company.trim()}&country=${country}`));
        } catch (error: any) {
            if (error.status === 404) {
                // Handle 404 error here
                return {
                    RBIApplications: [],
                    Results: 0
                };
            } else {
                // Handle other errors
                return {
                    RBIApplications: [],
                    Results: 0
                };
            }
        }
    }


    async GetRBIDetails(Id: string): Promise<BuildingProfessionalModel | undefined> {

        try{
        return await firstValueFrom(this.httpClient.get<BuildingProfessionalModel>(`api/GetRBIDetails/${Id}`));
        } catch (error: any) {
            if (error.status === 404) {
                // Handle 404 error here
                return undefined;
            } else {
                // Handle other errors
                return undefined;
            }
        }
    }

}

export class RBIInspectorCompaniesResponse {
    Companies!: string[];
    Results!: number;
}

export class RBIInspectorNamesResponse {
    Inspectors!: string[];
    Results!: number;
}

export class RBISearchResponse {
    RBIApplications!: BuildingProfessionalModel[];
    Results!: number;
}
