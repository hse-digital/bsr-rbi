import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { PaymentModel } from '../models/payment.model';
import { BuildingProfessionalModel } from '../models/building-professional.model';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  constructor(private httpClient: HttpClient) {}

  async GetPayment(paymentReference: string): Promise<PaymentModel> {
    return await firstValueFrom(
      this.httpClient.get<PaymentModel>(`api/GetPayment/${paymentReference}`)
    );
  }

  async InitialisePayment(
    applicationModel: BuildingProfessionalModel
  ): Promise<PaymentModel> {
    return await firstValueFrom(
      this.httpClient.post<PaymentModel>(
        `api/InitialisePayment/${applicationModel.id}`,
        null
      )
    );
  }
}

export const PaymentReconciliationStatus = {
  PENDING: 760810000,
  SUCCESS: 760810001,
  FAILED_RECONCILIATION: 760810002,
  FAILED_PAYMENT: 760810003,
  REFUNDED: 760810004,
};
