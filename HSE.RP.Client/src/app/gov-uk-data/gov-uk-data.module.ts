import { RouterModule, Routes } from '@angular/router';
import { GovUKDataComponent } from './gov-uk-data.component';
import { NgModule } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GovUKDataService } from './gov-uk-data.service';

const routes: Routes = [
  { path: GovUKDataComponent.route, component: GovUKDataComponent },
];

@NgModule({
  declarations: [GovUKDataComponent],
  imports: [RouterModule.forChild(routes)],
  providers: [GovUKDataService],
})
export class GovUKDataModule {}
